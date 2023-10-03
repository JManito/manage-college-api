using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.DTO
{
    public class DisciplineDTO
    {
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public int ProfessorId { get; set; }
        public int CourseId { get; set; }
    }
}

