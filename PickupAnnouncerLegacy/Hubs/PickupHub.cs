using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Hubs
{
    public class PickupHub : Hub
    {
        private readonly IStudentHelper _studentDetailsHelper;
        private readonly IDbHelper _dbHelper;
        private readonly ILogger<PickupHub> _logger;

        public PickupHub(IStudentHelper studentDetailsHelper, IDbHelper dbHelper, ILogger<PickupHub> logger)
        {
            _studentDetailsHelper = studentDetailsHelper;
            _dbHelper = dbHelper;
            _logger = logger;
        }

        public async Task AnnouncePickup(ArrivalNotice details)
        {
            var errorMessage = String.Empty;
            if (Int32.TryParse(details.Car, out var registrationId))
            {
                var students = await _studentDetailsHelper.GetStudentsForCar(registrationId);
                if (students.Any())
                {
                    var announcement = new PickupNotice()
                    {
                        Car = details.Car,
                        Cone = details.Cone,
                        Students = students.ToList(),
                        PickupTimeUTC = DateTimeOffset.UtcNow
                    };
                    await _dbHelper.AddPickupLog(announcement);
                    await Clients.All.SendAsync("PickupAnnouncement", JsonConvert.SerializeObject(announcement));
                    await Clients.Caller.SendAsync("SuccessAnnouncement");
                }
                else
                {
                    errorMessage = $"Failed to locate students attached to Registration Id: {registrationId}.";
                }
            }
            else
            {
                errorMessage = $"Failed to convert {details.Car} to number.";
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await Clients.Caller.SendAsync("FailureAnnouncement", errorMessage);
                _logger.LogError(errorMessage);
            }
        }
    }
}
