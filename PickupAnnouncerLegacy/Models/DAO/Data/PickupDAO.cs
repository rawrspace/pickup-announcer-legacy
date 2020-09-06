using Dapper;
using System;

namespace PickupAnnouncerLegacy.Models.DAO.Data
{
    [Table("Pickup", Schema = "Data")]
    public class PickupDAO : BaseDAO
    {
        public int RegistrationId { get; set; }
        public int Cone { get; set; }
        public DateTimeOffset PickupTimeUTC { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string GradeLevel { get; set; }
    }
}
