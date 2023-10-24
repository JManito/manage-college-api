using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.DTO
{
    public class GradesDTO
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Grade { get; set; }
    }
    public class CreateGrade
    {
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Grade { get; set; }
    }

}
