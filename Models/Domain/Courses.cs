using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageCollege.Models.Domain
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }

        public string? CourseName { get; set; }

    }
    public class CoursesGet
    {
        public string? CourseName { get; set; }
    }
    public class CoursesProfessors
    {
        [Key]
        public int Id { get; set; }

        public string? CourseName { get; set; }

        public int CourseDisciplineID { get; set; }
        public string? CourseDisciplineName { get; set; }
        public int CourseProfessorID { get; set; }
        public string? CourseProfessor { get; set; }
    }
    public class CourseStudentAverage
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public decimal Average { get; set; }
    }
    public class CourseInfo
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public int ProfessorNr { get; set; }

        public List<CourseStudentAverage> CourseStudentAverages { get; set; }
    }
}
