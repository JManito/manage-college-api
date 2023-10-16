namespace ManageCollege.Models.DTO
{
    public class CreateGrade
    {
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Grade { get; set; }
    }
}
