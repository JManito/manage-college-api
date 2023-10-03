using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Diagnostics;

namespace ManageCollege.Repositories.Implementation
{
    public class Repository : IRepository
    {
        private readonly ApplicationDBContext dbContext;

        public Repository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //-------------------------------------------------------------------
        //------------------ALL THE DISCIPLINE METHODS-----------------------
        //-------------------------------------------------------------------

        public async Task<Disciplines> CreateDisciplineAsync(Disciplines disciplines)
        {
            await dbContext.Disciplines.AddAsync(disciplines);
            await dbContext.SaveChangesAsync();

            return disciplines;
        }
        public async Task<List<Disciplines>> GetDisciplinesAsync()
        {
            var discipline = await dbContext.Disciplines.ToListAsync();

            return discipline;

        }
        public async Task<Disciplines> GetDisciplineAsync(int id)
        {
            var discipline = await dbContext.Disciplines.FindAsync(id);

            if (discipline != null)
            {
                return discipline;
            }
            return null;

        }
        public async Task<Disciplines> EditDisciplineAsync(Disciplines disciplines, int id)
        {

            var discipline = await dbContext.Disciplines.FindAsync(id);


            if (discipline != null)
            {

                discipline.DisciplineName = disciplines.DisciplineName;
                discipline.ProfessorId= disciplines.ProfessorId;
                discipline.CourseId= disciplines.CourseId;

                await dbContext.SaveChangesAsync();

                return discipline;

            }
            return null;

        }
        public async Task<Disciplines> DeleteDisciplineAsync(int id)
        {

            var discipline = await dbContext.Disciplines.FindAsync(id);

            if (discipline != null)
            {

                dbContext.Remove(discipline);

                await dbContext.SaveChangesAsync();

                return discipline;

            }
            return null;

        }

        //-------------------------------------------------------------------
        //------------------ALL THE PROFESSOR METHODS------------------------
        //-------------------------------------------------------------------

        public async Task<Professors> CreateProfessorAsync(Professors professors)
        {
            await dbContext.Professors.AddAsync(professors);
            await dbContext.SaveChangesAsync();

            return professors;
        }
        public async Task<List<Professors>> GetProfessorsAsync()
        {
            var professors = await dbContext.Professors.ToListAsync();

            return professors;

        }
        public async Task<Professors> GetProfessorAsync(int id)
        {
            var professorselected = await dbContext.Professors.FindAsync(id);
           
            if (professorselected != null)
            {
                return professorselected;
            }
            return null;

        }
        public async Task<Professors> EditProfessorAsync(Professors professor, int id)
        {

            var professorToEdit = await dbContext.Professors.FindAsync(id);


            if (professor != null)
            {

                professorToEdit.ProfessorName = professor.ProfessorName;
                professorToEdit.DateOfBirth = professor.DateOfBirth;
                professorToEdit.Salary = professor.Salary;

                await dbContext.SaveChangesAsync();

                return professor;

            }
            return null;

        }
        public async Task<Professors> DeleteProfessorAsync(int id)
        {

            var professor = await dbContext.Professors.FindAsync(id);

            if (professor != null)
            {

                dbContext.Remove(professor);

                await dbContext.SaveChangesAsync();

                return professor;

            }
            return null;

        }
        
        //-------------------------------------------------------------------
        //------------------ ALL THE STUDENT METHODS ------------------------
        //-------------------------------------------------------------------

        public async Task<Students> CreateStudentAsync(Students students)
        {
            await dbContext.Students.AddAsync(students);
            await dbContext.SaveChangesAsync();

            return students;
        }
        public async Task<List<Students>> GetStudentsAsync()
        {
            var students = await dbContext.Students.ToListAsync();

            return students;

        }
        public async Task<Students> GetStudentAsync(int id)
        {
            var students = await dbContext.Students.ToListAsync();

            var studentsselected = await dbContext.Students.FindAsync(id);
            if (studentsselected != null)
            {
                return studentsselected;
            }
            return null;

        }
        public async Task<Students> EditStudentAsync(Students student, int id)
        {

            var studentToEdit = await dbContext.Students.FindAsync(id);


            if (student != null)
            {

                studentToEdit.StudentName = student.StudentName;
                studentToEdit.DateOfBirth = student.DateOfBirth;
                studentToEdit.EnrollmentNumber = student.EnrollmentNumber;

                await dbContext.SaveChangesAsync();

                return studentToEdit;

            }
            return null;

        }
        public async Task<Students> DeleteStudentAsync(int id)
        {

            var student = await dbContext.Students.FindAsync(id);

            if (student != null)
            {

                dbContext.Remove(student);

                await dbContext.SaveChangesAsync();

                return student;

            }
            return null;

        }
    
        //-------------------------------------------------------------------
        //------------------ ALL THE GRADES METHODS -------------------------
        //-------------------------------------------------------------------

        public async Task<Grades> CreateGradesAsync(Grades grade)
        {
            await dbContext.Grades.AddAsync(grade);
            await dbContext.SaveChangesAsync();

            return grade;
        }
        public async Task<List<Grades>> GetGradesAsync()
        {
            var grades = await dbContext.Grades.ToListAsync();

            return grades;

        }
        public async Task<Grades> GetGradeAsync(int id)
        {
            var students = await dbContext.Grades.ToListAsync();

            var gradeselected = await dbContext.Grades.FindAsync(id);
            if (gradeselected != null)
            {
                return gradeselected;
            }
            return null;

        }
        public async Task<Grades> EditGradeAsync(Grades grade, int id)
        {

            var gradeToEdit = await dbContext.Grades.FindAsync(id);


            if (grade != null)
            {

                gradeToEdit.StudentId = grade.StudentId;
                gradeToEdit.Grade = grade.Grade;
                gradeToEdit.DisciplineId = grade.DisciplineId;

                await dbContext.SaveChangesAsync();

                return gradeToEdit;

            }
            return null;

        }
        public async Task<Grades> DeleteGradeAsync(int id)
        {

            var grade = await dbContext.Grades.FindAsync(id);

            if (grade != null)
            {

                dbContext.Remove(grade);

                await dbContext.SaveChangesAsync();

                return grade;

            }
            return null;

        }
    }
}
