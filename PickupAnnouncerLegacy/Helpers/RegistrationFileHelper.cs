using CsvHelper;
using Microsoft.AspNetCore.Http;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models.DAO.Staging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Helpers
{
    public class RegistrationFileHelper : IRegistrationFileHelper
    {
        private readonly IDbHelper _dbHelper;

        public RegistrationFileHelper(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task DeleteAll()
        {
            await _dbHelper.DeleteStudentRegistrations();
        }

        public async Task<IEnumerable<RegistrationDetailsDAO>> ProcessFile(IFormFile formFile)
        {
            IEnumerable<RegistrationDetailsDAO> records = null;
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<RegistrationDetailsDAO>().ToList();
                }
            }
            if (records.Any())
            {
                await _dbHelper.AddStudentRegistrations(records);
            }
            return records;
        }
    }
}
