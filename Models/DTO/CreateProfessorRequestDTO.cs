namespace ManageCollege.Models.DTO
{
    public class CreateProfessorRequestDTO
    {
        public string ProfessorName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }
    }
}
