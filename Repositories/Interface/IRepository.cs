using ManageCollege.Models.Domain;

namespace ManageCollege.Repositories.Interface
{
    public interface IRepository
    {
        //Tasks for Auth
        Task<Authentication> GetAuthentication();
        Task<Authentication> SetAuthentication(Authentication auth, int id);

        //Tasks for Courses
        Task<Courses> CreateCourseAsync(Courses course);
        Task<Enrollment> Enroll(Enrollment enroll, int id, int studentId);
        Task<List<CoursesProfessors>> GetCoursesProfessorsAsync();
        Task<List<CourseInfo>> GetCoursesInfoAsync(int? id);
        Task<List<Courses>> GetCoursesAsync();
        Task<Courses> GetCourseAsync(int id);
        Task<List<Disciplines>> GetCourseDisciplinesAsync(int id);
        Task<Courses> EditCourseAsync(Courses grade, int id);
        Task<Courses> DeleteCourseAsync(int id);

        //Tasks for Disciplines
        Task<Disciplines> CreateDisciplineAsync(Disciplines disciplines);
        Task<List<Disciplines>> GetDisciplinesAsync();
        Task<List<DisciplineInfo>> GetDisciplineInfoAsync(int? id);
        Task<Disciplines> GetDisciplineAsync(int id);
        Task<Disciplines> EditDisciplineAsync(Disciplines disciplines, int id);
        Task<Disciplines> DeleteDisciplineAsync(int id);
        //Tasks for Professors
        Task<Professors> CreateProfessorAsync(Professors professor);
        Task<List<Professors>> GetProfessorsAsync();
        Task<Professors> GetProfessorAsync(int id);
        Task<Professors> EditProfessorAsync(Professors professor, int id);
        Task<Professors> DeleteProfessorAsync(int id);
        //Tasks for Students
        Task<Students> CreateStudentAsync(Students student);
        Task<List<Students>> GetStudentsAsync();
        Task<Students> GetStudentAsync(int id);
        Task<Students> EditStudentAsync(Students student, int id);
        Task<Students> DeleteStudentAsync(int id);
        //Tasks for Grades
        Task<Grades> CreateGradesAsync(Grades grade);
        Task<List<Grades>> GetGradesAsync();
        Task<List<StudentGrade>> GetStudentGradeAsync(int? id);
        Task<Grades> GetGradeAsync(int id);
        Task<Grades> EditGradeAsync(Grades grade, int id);
        Task<Grades> DeleteGradeAsync(int id);

    }
}
