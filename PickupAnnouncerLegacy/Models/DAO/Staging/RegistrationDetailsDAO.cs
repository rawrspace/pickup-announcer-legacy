using CsvHelper.Configuration.Attributes;
using Dapper;

namespace PickupAnnouncerLegacy.Models.DAO.Staging
{
    [Table("RegistrationDetails", Schema = "Staging")]
    public class RegistrationDetailsDAO
    {
        [Key]
        [Required]
        [Name("Registration Number")]
        public int RegistrationId { get; set; }
        [Name("Student First Name")]
        public string FirstName { get; set; }
        [Name("Student Last Name")]
        public string LastName { get; set; }
        [Name("Teacher Name")]
        public string Teacher { get; set; }
        [Name("Grade Level")]
        public string GradeLevel { get; set; }
    }
}