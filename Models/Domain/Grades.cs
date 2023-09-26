using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.Domain
{
    public class Grades
    {
        [Key]
        public int GradeId { get; set; }
        [ForeignKey("Students")]
        public int StudentId { get; set; }
        [ForeignKey("Disciplines")]
        public int DisciplineId { get; set; }
        public int Grade { get; set; }
    }
}
