using Microsoft.AspNetCore.Mvc;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models.Requests;
using System;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly IDbHelper _dbHelper;

        public SiteController(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SettingsUpdateRequest request)
        {
            try
            {
                await _dbHelper.SetNumberOfCones(request.NumberOfCones);
                return new OkResult();
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
