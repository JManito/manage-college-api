namespace ManageCollege.Models.DTO
{
    //We add an extra DTO layer for flat data structures that contain no business logic and carry data between processes in order to reduce the number of methods calls.
    //Enhances security and performance
    public class GetCourseRequestDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; }

    }
}
