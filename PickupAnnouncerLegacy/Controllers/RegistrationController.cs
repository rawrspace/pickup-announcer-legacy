using Microsoft.AspNetCore.Mvc;
using PickupAnnouncerLegacy.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IStudentHelper _studentHelper;

        public RegistrationController(IStudentHelper studentHelper)
        {
            _studentHelper = studentHelper;
        }

        [HttpGet]
        public async Task<bool> Get(int id)
        {
            return (await _studentHelper.GetStudentsForCar(id)).Any();
        }
    }
}
