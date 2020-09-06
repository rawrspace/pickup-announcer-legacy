using CsvHelper;
using Microsoft.Extensions.Logging;
using PickupAnnouncerLegacy.Extensions;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models;
using PickupAnnouncerLegacy.Models.DAO.Config;
using PickupAnnouncerLegacy.Models.DAO.Data;
using PickupAnnouncerLegacy.Models.DAO.Staging;
using PickupAnnouncerLegacy.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Helpers
{
    public class DbHelper : IDbHelper
    {
        private readonly IDbService _dbService;
        private readonly ILogger<DbHelper> _logger;

        public DbHelper(IDbService dbService, ILogger<DbHelper> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }
        public async Task<bool> AddStudentRegistrations(IEnumerable<RegistrationDetailsDAO> registrationDetails)
        {
            try
            {
                var dataTable = new DataTable("Staging.RegistrationDetails");
                dataTable.Columns.Add(new DataColumn("RegistrationId", typeof(string)));
                dataTable.Columns.Add(new DataColumn("FirstName", typeof(string)));
                dataTable.Columns.Add(new DataColumn("LastName", typeof(string)));
                dataTable.Columns.Add(new DataColumn("Teacher", typeof(string)));
                dataTable.Columns.Add(new DataColumn("GradeLevel", typeof(string)));
                foreach (var item in registrationDetails)
                {
                    var row = dataTable.NewRow();
                    row["RegistrationId"] = item.RegistrationId;
                    row["FirstName"] = item.FirstName;
                    row["LastName"] = item.LastName;
                    row["Teacher"] = item.Teacher;
                    row["GradeLevel"] = item.GradeLevel;
                    dataTable.Rows.Add(row);
                }
                //Update Database
                using (var connection = _dbService.GetSqlConnection())
                {
                    var bulkInsert = new SqlBulkCopy(connection);
                    bulkInsert.DestinationTableName = "Staging.RegistrationDetails";
                    bulkInsert.ColumnMappings.Add("RegistrationId", "RegistrationId");
                    bulkInsert.ColumnMappings.Add("FirstName", "FirstName");
                    bulkInsert.ColumnMappings.Add("LastName", "LastName");
                    bulkInsert.ColumnMappings.Add("Teacher", "Teacher");
                    bulkInsert.ColumnMappings.Add("GradeLevel", "GradeLevel");
                    connection.Open();
                    bulkInsert.WriteToServer(dataTable);
                    connection.Close();
                }
                await _dbService.ExecuteStoredProcedure(Sprocs.ProcessStagingRegistrationDetails);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to add registrations");
                return false;
            }
        }

        public async Task<bool> DeleteStudentRegistrations()
        {
            try
            {
                await _dbService.ExecuteStoredProcedure(Sprocs.ClearDatabase);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete registrations.");
                return false;
            }
        }

        public async Task<IEnumerable<StudentDAO>> GetStudentsForRegistrationId(int registrationId)
        {
            var results = await _dbService.ExecuteStoredProcedure(Sprocs.GetStudentsForRegistrationId, new Dictionary<string, object>() { { "RegistrationId", registrationId } });
            return ParseResults<StudentDAO>(results);
        }

        public async Task<Stream> GetRegistrationDetailsStream()
        {
            var results = await _dbService.ExecuteStoredProcedure(Sprocs.ExportRegistrationDetails);
            var registrationDetails = ParseResults<RegistrationDetailsDAO>(results).OrderBy(x => x.RegistrationId);
            var outputStream = new MemoryStream();
            var writer = new StreamWriter(outputStream);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(registrationDetails);
            writer.Flush();
            outputStream.Position = 0;
            return outputStream;
        }

        public async Task AddPickupLog(PickupNotice pickupNotice)
        {
            var pickupLogs = new List<PickupDAO>();
            foreach (var student in pickupNotice.Students)
            {
                pickupLogs.Add(new PickupDAO()
                {
                    Cone = Int32.Parse(pickupNotice.Cone),
                    GradeLevel = student.GradeLevel,
                    Name = student.Name,
                    PickupTimeUTC = pickupNotice.PickupTimeUTC,
                    RegistrationId = Int32.Parse(pickupNotice.Car),
                    Teacher = student.Teacher
                });
            }

            await _dbService.Insert<PickupDAO>(pickupLogs);
        }

        private static List<T> ParseResults<T>(IList<IDictionary<string, object>> results)
        {

            var resultsList = new List<T>();
            if (results != null)
            {
                foreach (var result in results)
                {
                    var dbResponse = result.ToType<object, T>();
                    resultsList.Add(dbResponse);
                }
            }

            return resultsList;
        }

        public async Task<IEnumerable<PickupNotice>> GetPickupNotices(DateTimeOffset startOfDay)
        {
            var pickupLogs = await _dbService.Get<PickupDAO>("WHERE PickupTimeUTC >= @StartOfDay AND PickupTimeUTC < @EndOfDay", new { StartOfDay = startOfDay, EndOfDay = startOfDay.AddDays(1) });
            var pickupNotices = pickupLogs.GroupBy(x => new { x.RegistrationId, x.Cone, x.PickupTimeUTC }).Select(x => new PickupNotice()
            {
                Car = x.Key.RegistrationId.ToString(),
                Cone = x.Key.Cone.ToString(),
                PickupTimeUTC = x.Key.PickupTimeUTC,
                Students = x.Select(y => new StudentDTO()
                {
                    Name = y.Name,
                    Teacher = y.Teacher,
                    GradeLevel = y.GradeLevel
                })
            });
            return pickupNotices;
        }

        public async Task<IEnumerable<GradeLevel>> GetGradeLevelConfig() => await _dbService.Get<GradeLevel>();

        public async Task<IDictionary<string, GradeLevel>> GetGradeLevelConfig(IEnumerable<string> gradeLevels)
        {
            var gradeLevelNames = String.Join("|", gradeLevels);
            var results = await _dbService.ExecuteStoredProcedure(Sprocs.GetGradeLevelConfig, new Dictionary<string, object>() { { "GradeLevelNames", gradeLevelNames } });
            var gradeLevelConfigs = results.Select(x => x.ToType<object, GradeLevel>());
            return gradeLevelConfigs.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Validates if the credentials are authenticated to access the Admin portal. THIS IS NOT A SECURE LOGIN IT IS ONLY TO RESTRICT ACCIDENTAL ACCESS THAT IS WHY IT IS NOT HASHED.
        /// </summary>
        /// <param name="userName">Username to check</param>
        /// <param name="password">Password to authenticate</param>
        /// <returns>Bool indicating if the credentials are allow access</returns>
        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var siteConfig = await _dbService.Get<Site>("WHERE AdminUser = @Username AND AdminPass = @Password", new { Username = username, Password = password });
            return siteConfig.Any();
        }

        /// <summary>
        /// Retrieve the number of cones configured
        /// </summary>
        /// <returns>The number of configured cones from the database and if the value is not present defaults to 8</returns>
        public async Task<int> GetNumberOfCones()
        {
            var siteConfig = (await _dbService.Get<Site>()).FirstOrDefault();
            return siteConfig?.NumberOfCones ?? 8;
        }

        public async Task SetNumberOfCones(int numberOfCones)
        {
            var siteConfig = (await _dbService.Get<Site>()).FirstOrDefault();
            if (siteConfig != null)
            {
                siteConfig.NumberOfCones = numberOfCones;
                await _dbService.Update(siteConfig);
            }
            else
            {
                var errorMessage = "Failed to locate a SiteConfig in the Database";
                _logger.LogError(errorMessage);
                throw new NullReferenceException(errorMessage);
            }
        }

        public async Task<bool> DeleteGradeLevelConfig(int id)
        {
            var results = await _dbService.Delete<GradeLevel>(id);
            return results > 0;
        }

        public async Task<bool> UpdateGradeLevel(GradeLevel gradeLevel)
        {
            var results = await _dbService.Update<GradeLevel>(gradeLevel);
            return results > 0;
        }
        public async Task<bool> InsertGradeLevel(GradeLevel gradeLevel)
        {
            try
            {
                await _dbService.Insert<GradeLevel>(gradeLevel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
