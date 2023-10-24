using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManageCollege.Models.Domain
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Courses")]
        public int CourseId { get; set; }
        public int StudentId { get; set; }
    }
}
