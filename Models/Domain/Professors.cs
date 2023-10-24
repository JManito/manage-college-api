using System.ComponentModel.DataAnnotations;

namespace ManageCollege.Models.Domain
{
    public class Professors
    {
        [Key]
        public int ProfessorId { get; set; }

        public string ProfessorName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }

    }
}
