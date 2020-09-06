using PickupAnnouncerLegacy.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Interfaces
{
    public interface IStudentHelper
    {
        Task<IEnumerable<StudentDTO>> GetStudentsForCar(int carId);
    }
}
