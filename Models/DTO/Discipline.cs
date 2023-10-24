using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.DTO
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string? DisciplineName { get; set; }
        public int ProfessorId { get; set; }
        public int CourseId { get; set; }
    }

    public class CreateDiscipline
    {
        public string DisciplineName { get; set; } = string.Empty;
        public int ProfessorId { get; set; }
        public int CourseId { get; set; }
    }
}

