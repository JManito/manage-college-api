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
    public class CourseList
    {
        [Key]
        public int Id { get; private set; }

        public string CourseName { get; private set; }

        public int CourseDisciplineID { get; private set; }
        public int CourseDisciplineName { get; private set; }
        public int CourseProfessorID { get; private set; }
        public string CourseProfessor { get; private set; }
    }
}
