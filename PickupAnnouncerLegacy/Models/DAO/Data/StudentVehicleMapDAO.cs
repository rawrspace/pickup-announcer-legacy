using Dapper;

namespace PickupAnnouncerLegacy.Models.DAO.Data
{
    [Table("StudentVehicleMap", Schema = "Data")]
    public class StudentVehicleMapDAO : BaseDAO
    {
        public int StudentId { get; set; }
        public int VehicleId { get; set; }
    }
}
