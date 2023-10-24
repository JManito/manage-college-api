using System.ComponentModel.DataAnnotations;

namespace ManageCollege.Models.Domain
{
    public class Students
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; } = "";

        public DateTime DateOfBirth { get; set; }

        public int EnrollmentNumber { get; set;}

    }
}
