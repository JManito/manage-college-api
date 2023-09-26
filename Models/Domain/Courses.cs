using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.Domain
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }

        public string CourseName { get; set; }

    }
}
