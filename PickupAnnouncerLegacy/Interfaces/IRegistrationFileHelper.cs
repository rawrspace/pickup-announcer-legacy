using Microsoft.AspNetCore.Http;
using PickupAnnouncerLegacy.Models.DAO.Staging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Interfaces
{
    public interface IRegistrationFileHelper
    {
        Task<IEnumerable<RegistrationDetailsDAO>> ProcessFile(IFormFile formFile);
        Task DeleteAll();
    }
}
