namespace ManageCollege.Models.DTO
{
    public class CreateGradeRequestDTO
    {
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Grade { get; set; }
    }
}
