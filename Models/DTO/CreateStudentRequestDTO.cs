namespace ManageCollege.Models.DTO
{
    public class CreateStudentRequestDTO
    {
        public string StudentName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int EnrollmentNumber { get; set; }
    }
}
