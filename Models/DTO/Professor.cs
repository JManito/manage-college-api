


namespace ManageCollege.Models.DTO
{
    public class Professor
    {
        public int ProfessorId { get; set; }

        public string? ProfessorName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }
    }
    public class CreateProfessor
    {
        public string? ProfessorName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }
    }

}
