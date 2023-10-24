using ManageCollege.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ManageCollege.Models.Domain
{
   
    public class Disciplines
    {
        [Key]
        public int DisciplineId { get; set; }
        public string? DisciplineName { get; set; }
        [ForeignKey("Professors")]
        public int ProfessorId { get; set; }
        [ForeignKey("Courses")]
        public int CourseId { get; set; }
        
    }
    public class DisciplineStudent
    {
        public string? StudentName { get; set; }
        public int Grade { get; set; }
    }

    public class DisciplineProfessor
    {

        public int ProfessorId { get; set; }
        public string? ProfessorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Salary { get; set; }
        
    }

    public class DisciplineInfo
    {
        [Key]
        public int Id { get; set; }
        public int DisciplineId { get; set; }
        public string? DisciplineName { get; set; }
        public DisciplineProfessor? DisciplineProfessor { get; set; }
        public int ClassNumber { get; set; }    
        public List<DisciplineStudent>? DisciplineStudent { get; set; }

    }

}
