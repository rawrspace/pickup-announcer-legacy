using Dapper;

namespace PickupAnnouncerLegacy.Models.DAO.Config
{
    [Table("Site", Schema = "Config")]
    public class Site : BaseDAO
    {
        public int NumberOfCones { get; set; }
        public string AdminUser { get; set; }
        public string AdminPass { get; set; }
    }
}