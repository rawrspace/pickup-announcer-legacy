using Dapper;

namespace PickupAnnouncerLegacy.Models.DAO.Data
{
    [Table("Student", Schema = "Data")]
    public class StudentDAO : BaseDAO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //TODO: We could move Teacher and GradeLevel to a separate object normalize this a bit more
        // it would make the import file more complex however and I don't think it is needed at this time.
        public string Teacher { get; set; }
        public string GradeLevel { get; set; }
    }
}
