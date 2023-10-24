namespace ManageCollege.Models.DTO
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty; 

        public DateTime DateOfBirth { get; set; }

        public int EnrollmentNumber { get; set; }
    }

    public class CreateStudent
    {
        public string StudentName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int EnrollmentNumber { get; set; }
    }


}
