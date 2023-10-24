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
    public class StudentGrade
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Students")]
        public int StudentId { get; set; }

        public string? StudentName { get; set; }

        public List<DisciplineGrade> DisciplineGrade { get; set; } = new List<DisciplineGrade>();

    }
    public class DisciplineGrade
    {
        public string? DisciplineName { get; set; }
        public int Grade { get; set; }
    }

}
