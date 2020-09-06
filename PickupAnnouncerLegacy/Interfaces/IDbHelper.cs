using PickupAnnouncerLegacy.Models;
using PickupAnnouncerLegacy.Models.DAO.Config;
using PickupAnnouncerLegacy.Models.DAO.Data;
using PickupAnnouncerLegacy.Models.DAO.Staging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Interfaces
{
    public interface IDbHelper
    {
        Task<bool> AddStudentRegistrations(IEnumerable<RegistrationDetailsDAO> registrationDetails);

        Task<bool> DeleteStudentRegistrations();

        Task<IEnumerable<StudentDAO>> GetStudentsForRegistrationId(int registrationId);
        Task<Stream> GetRegistrationDetailsStream();
        Task<int> GetNumberOfCones();
        Task AddPickupLog(PickupNotice pickupNotice);
        Task<IEnumerable<PickupNotice>> GetPickupNotices(DateTimeOffset startOfDay);
        Task SetNumberOfCones(int numberOfCones);
        Task<IEnumerable<GradeLevel>> GetGradeLevelConfig();
        Task<IDictionary<string, GradeLevel>> GetGradeLevelConfig(IEnumerable<string> gradeLevels);
        Task<bool> DeleteGradeLevelConfig(int id);
        Task<bool> AuthenticateUser(string userName, string password);
        Task<bool> UpdateGradeLevel(GradeLevel gradeLevel);
        Task<bool> InsertGradeLevel(GradeLevel gradeLevel);
    }
}