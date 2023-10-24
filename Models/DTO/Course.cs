namespace ManageCollege.Models.DTO
{
    public class Course
    {
        public int Id { get; set; }

        public string? CourseName { get; set; }
    }
    public class CreateCourse
    {
        public string CourseName { get; set; } = "Not Created";
    }
    public class GetCourseRequest
    {
        public int Id { get; set; }
        public string? CourseName { get; set; }

    }
    public class UpdateCourse
    {
        public string CourseName { get; set; } = "Not Updated";
    }
}
