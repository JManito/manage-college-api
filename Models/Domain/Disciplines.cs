using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.Domain
{
    public class Disciplines
    {
        [Key]
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        [ForeignKey("Professors")]
        public int ProfessorId { get; set; }
        [ForeignKey("Courses")]
        public int CourseId { get; set; }
        
    }
}
