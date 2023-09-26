using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.DTO
{
    public class GradeDTO
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Grade { get; set; }
    }
}
