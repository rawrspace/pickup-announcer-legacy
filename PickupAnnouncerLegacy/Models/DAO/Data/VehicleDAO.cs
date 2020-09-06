using Dapper;

namespace PickupAnnouncerLegacy.Models.DAO.Data
{
    [Table("Vehicle", Schema = "Data")]
    public class VehicleDAO : BaseDAO
    {
        public int RegistrationId { get; set; }
    }
}
