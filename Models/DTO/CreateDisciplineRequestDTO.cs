namespace ManageCollege.Models.DTO
{
    public class CreateDisciplineRequestDTO
    {
        public string DisciplineName { get; set; }
        public int ProfessorId { get; set; }
        public int CourseId { get; set; }
    }
}
