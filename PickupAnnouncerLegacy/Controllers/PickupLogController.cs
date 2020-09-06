using Microsoft.AspNetCore.Mvc;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models;
using PickupAnnouncerLegacy.Models.DTO;
using PickupAnnouncerLegacy.Models.Requests;
using PickupAnnouncerLegacy.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickupLogController : ControllerBase
    {
        private readonly IDbHelper _dbHelper;

        public PickupLogController(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpPost]
        public async Task<PickupLogResponse> Post([FromBody] PickupLogRequest request)
        {
            var updatedAnnouncements = new List<PickupNotice>();
            var pickupNotices = await _dbHelper.GetPickupNotices(request.StartOfDayUTC);
            if (pickupNotices.Any())
            {
                var gradeLevels = pickupNotices.SelectMany(x => x.Students.Select(y => y.GradeLevel)).Distinct();
                var gradeLevelConfigs = await _dbHelper.GetGradeLevelConfig(gradeLevels);
                foreach (var pickupNotice in pickupNotices)
                {
                    var updatedStudents = new List<StudentDTO>();
                    foreach (var student in pickupNotice.Students)
                    {
                        if (gradeLevelConfigs.TryGetValue(student.GradeLevel, out var gradeLevelConfig))
                        {
                            student.BackgroundColor = gradeLevelConfig.BackgroundColor;
                            student.TextColor = gradeLevelConfig.TextColor;
                        }
                        updatedStudents.Add(student);
                    }
                    updatedAnnouncements.Add(new PickupNotice()
                    {
                        Car = pickupNotice.Car,
                        Cone = pickupNotice.Cone,
                        PickupTimeUTC = pickupNotice.PickupTimeUTC,
                        Students = updatedStudents
                    });
                }
            }
            return new PickupLogResponse()
            {
                Announcements = updatedAnnouncements.OrderBy(x => x.PickupTimeUTC)
            };
        }
    }
}
