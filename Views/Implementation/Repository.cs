using Azure.Core;
using ManageCollege.Data;
using System.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Diagnostics.Metrics;
using Microsoft.IdentityModel.Abstractions;
using System.Collections.Generic;

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
        //---------------------ALL THE AUTH METHODS--------------------------
        //-------------------------------------------------------------------

        public async Task<Authentication> GetAuthentication()
        {
            var auth = await dbContext.Auth.FirstAsync();

            if (auth != null)
            {
                return auth;
            }

            return null;

        }
        public async Task<Authentication> SetAuthentication(Authentication auth, int id)
        {

            var authed = await dbContext.Auth.FindAsync(id);


            if (authed != null)
            {

                authed.authAs = auth.authAs;
                authed.isAuth = auth.isAuth;

                await dbContext.SaveChangesAsync();

                return authed;

            }
            return null;
        }
        //-------------------------------------------------------------------
        //------------------ALL THE COURSES METHODS--------------------------
        //-------------------------------------------------------------------

        public async Task<Courses> CreateCourseAsync(Courses course)
        {
            //Prepare string to be inserted in DB

            if(course.CourseName != null)
            {
                course.CourseName = course.CourseName.Trim().Replace("\"", "");

                //Check if string already in DB

                var exists = dbContext.Courses
                .Select(x => new Courses
                {
                    CourseName = x.CourseName,
                }).Where(x => x.CourseName == course.CourseName).ToList();

                //Validate if entry is already in DB or is invalid

                foreach (var selectcourse in exists)
                {
                    if (selectcourse != null && selectcourse.CourseName == course.CourseName || course.CourseName.IsNullOrEmpty() || Regex.Replace(course.CourseName, "[^0-9]", "").Equals(course.CourseName) || selectcourse.CourseName.ToLower() == course.CourseName.ToLower() || Regex.IsMatch(course.CourseName, "/ ^[a - z,.'-]*$/i") || course.CourseName == "string")
                    {
                        return null;
                    }
                }


                //Post new entry
                await dbContext.Courses.AddAsync(course);
                await dbContext.SaveChangesAsync();

                return course;
            }

            return null;
        }
        public async Task<Enrollment> Enroll(Enrollment enroll, int id, int studentId)
        {
            //get the student with the ID to change enrollmentnumber
            var studentsselected = await dbContext.Students.FindAsync(studentId);

            var exists = dbContext.Enrollment
                        .Select(x => new Enrollment
                        {
                            Id = x.Id,
                            StudentId = x.StudentId,
                        }).Where(x => x.StudentId == studentId).ToList();

            //Validate if student is already enrolled in DB or is invalid

            foreach (var existingEnrollments in exists)
            {
                if (existingEnrollments != null && (existingEnrollments.Id == id && existingEnrollments.StudentId == studentId))
                {
                    return null;
                }
            }
            //Post new entry if not enrolled
            await dbContext.Enrollment.AddAsync(enroll);
            await dbContext.SaveChangesAsync();



            //Edit Student Table In DB
            var enrollmentNr = await dbContext.Enrollment.FindAsync(enroll.Id);
            if (studentsselected != null && studentsselected.EnrollmentNumber == 0)
            {
                studentsselected.EnrollmentNumber = enroll.Id;
                await dbContext.SaveChangesAsync();

            }

            return enroll;

        }
        public async Task<List<Courses>> GetCoursesAsync()
        {
            var courses = await dbContext.Courses.ToListAsync();

            return courses;

        }

        public async Task<List<CoursesProfessors>> GetCoursesProfessorsAsync()
        {
            var courses = await dbContext.Courses.ToListAsync();
            var professorcounter = 0;
            var professorName = new String("");
            var courseName = new String("");
            var CoursesProfessors = new List<CoursesProfessors>();
            var newProfessor = new Professors();
            var newCourse = new Courses();

            //Quantos cursos
            foreach (var dcourse in courses)
            {
                var CourseProfessor = new CoursesProfessors();


                if (dcourse != null)
                {
                    var courseDisciplines = dbContext.Disciplines
                       .Select(x => new Disciplines
                       {
                           DisciplineId = x.DisciplineId,
                           DisciplineName = x.DisciplineName,
                           ProfessorId = x.ProfessorId,
                           CourseId = x.CourseId
                       }).Where(x => x.CourseId == dcourse.Id).ToList();

                    //Quantas disciplinas e sprof
                    foreach (var disciplinecountermodule in courseDisciplines)
                    {
                        CourseProfessor = new CoursesProfessors();

                        if (disciplinecountermodule != null && disciplinecountermodule.ProfessorId != 0)
                        {
                            CourseProfessor.Id = professorcounter;
                            CourseProfessor.CourseProfessorID = disciplinecountermodule.ProfessorId;
                            //Ir buscar o professor name com o outro getbyid()
                                newProfessor = await dbContext.Professors.FindAsync(disciplinecountermodule.ProfessorId);
                                if (newProfessor != null) { professorName = newProfessor.ProfessorName ;}
                           
                            CourseProfessor.CourseProfessor = professorName;


                            CourseProfessor.CourseDisciplineID = disciplinecountermodule.DisciplineId;
                            CourseProfessor.CourseDisciplineName = disciplinecountermodule.DisciplineName;

                            //Ir buscar o course name com o outro getbyid()
                                newCourse = await dbContext.Courses.FindAsync(disciplinecountermodule.CourseId);
                                if (newCourse != null) { courseName = newCourse.CourseName; }
                            CourseProfessor.CourseName = courseName;

                            //Next Professor
                            professorcounter++;

                            //Add Professor to the List
                            CoursesProfessors.Add(CourseProfessor);
                        }
                    }
                }
            }
             


            return CoursesProfessors;
        }


        public async Task<List<CourseInfo>> GetCoursesInfoAsync(int? id)
        {
            var selectedCourse = new Courses();
            var courses = await dbContext.Courses.ToListAsync();
            var enrollments = await dbContext.Enrollment.ToListAsync();
            var professorName = new String("");
            var courseName = new String("");
            var coursesInfo = new List<CourseInfo>();
            var courseInfo = new CourseInfo();
            var courseStudentAverages = new List<CourseStudentAverage>();
            var courseStudentAverage = new CourseStudentAverage();
            var courseTotal = new Decimal();
            var alumnicounter = 0;
            var courseCounter = 0;

            if (id != null)
            {
                selectedCourse = await dbContext.Courses.FindAsync(id);
                //Para cada curso criamos 1 modelo de informação
                courseInfo = new CourseInfo();
                //Criaçao nova lista de medias por curso
                courseStudentAverages = new List<CourseStudentAverage>();
                if (selectedCourse != null)
                {
                    //Alocar info do curso ao modelo de resposta
                    courseInfo.Id = courseCounter;
                    courseInfo.CourseId = selectedCourse.Id;
                    courseInfo.CourseName = selectedCourse.CourseName;

                    //Valida inscriçoes de aluno por curso
                    //1 inscrição = curso info + studentinfo
                    foreach (var enroll in enrollments)
                    {

                        //Todas as inscrições no curso
                        if (enroll != null && enroll.CourseId == selectedCourse.Id)
                        {
                            //Alocar valores courseStudentAverage a adicionar posteriormente na lista de modelos
                            courseStudentAverage.StudentId = enroll.StudentId;
                            var newStudent = await dbContext.Students.FindAsync(enroll.StudentId);
                            if (newStudent != null) courseStudentAverage.StudentName = newStudent.StudentName;

                            //Ir buscar as notas deste aluno ^^^^^^^^^^^^

                            //Reset do total de aluno do curso
                            courseTotal = 0;
                            alumnicounter = 0;

                            //Info do estudante inscrito

                            //Ir buscar as disciplinas do curso 
                            var courseDisciplines = dbContext.Disciplines.Where(x => x.CourseId == selectedCourse.Id).ToList();
                            //Contar professores 
                            var professorcounter = dbContext.Disciplines.Where(x => x.CourseId == selectedCourse.Id).Distinct().Count();

                            courseInfo.ProfessorNr = professorcounter;

                            //Percorrer cada disciplina do curso e recolher nota do aluno
                            foreach (var disciplinecountermodule in courseDisciplines)
                            {
                                if (disciplinecountermodule != null && newStudent != null)
                                {
                                    var disciplinegrade = dbContext.Grades.Where(x => x.DisciplineId == disciplinecountermodule.DisciplineId && x.StudentId == newStudent.StudentId).Distinct().ToList();
                                    foreach (var grade in disciplinegrade)
                                    {
                                        //Soma do total do curso
                                        courseTotal = courseTotal + grade.Grade;
                                    }

                                    //Adicionar aluno ao contador
                                    alumnicounter++;
                                }
                            }

                            //Se houver notas e alunos calcula-se a media
                            if (courseTotal != 0 && alumnicounter != 0)
                            {

                                courseStudentAverage.Average = courseTotal / alumnicounter;
                                courseStudentAverage.Average = Math.Round(courseStudentAverage.Average, 2);

                            }


                            //Media de 1 aluno com 1 inscrição no curso 
                            courseStudentAverages.Add(courseStudentAverage);
                            //Nova media de aluno
                            courseStudentAverage = new CourseStudentAverage();

                        }
                    }
                    //Adicionar mais 1 curso ao counter
                    courseCounter++;
                    //Adicionar lista de medias de alunos à info de curso
                    courseInfo.CourseStudentAverages = courseStudentAverages;
                    //Caso as medias venham vazias/ nao hajam notas, devolve-se array vazio
                    if (courseInfo.ProfessorNr == 0) courseInfo.CourseStudentAverages = new List<CourseStudentAverage>();
                    //Adicionar info de curso à lista 
                    coursesInfo.Add(courseInfo);
                }
                

            }
            else
            {
                //Quantos cursos
                foreach (var dcourse in courses)
                {
                    //Para cada curso criamos 1 modelo de informação
                    courseInfo = new CourseInfo();
                    //Criaçao nova lista de medias por curso
                    courseStudentAverages = new List<CourseStudentAverage>();
                    if (dcourse != null)
                    {
                        //Alocar info do curso ao modelo de resposta
                        courseInfo.Id = courseCounter;
                        courseInfo.CourseId = dcourse.Id;
                        courseInfo.CourseName = dcourse.CourseName;

                        //Valida inscriçoes de aluno por curso
                        //1 inscrição = curso info + studentinfo
                        foreach (var enroll in enrollments)
                        {

                            //Todas as inscrições no curso
                            if (enroll != null && enroll.CourseId == dcourse.Id)
                            {
                                //Alocar valores courseStudentAverage a adicionar posteriormente na lista de modelos
                                courseStudentAverage.StudentId = enroll.StudentId;
                                var newStudent = await dbContext.Students.FindAsync(enroll.StudentId);
                                if (newStudent != null) courseStudentAverage.StudentName = newStudent.StudentName;

                                //Ir buscar as notas deste aluno ^^^^^^^^^^^^

                                //Reset do total de aluno do curso
                                courseTotal = 0;
                                alumnicounter = 0;

                                //Info do estudante inscrito

                                //Ir buscar as disciplinas do curso 
                                var courseDisciplines = dbContext.Disciplines.Where(x => x.CourseId == dcourse.Id).ToList();
                                //Contar professores 
                                var professorcounter = dbContext.Disciplines.Where(x => x.CourseId == dcourse.Id).Distinct().Count();

                                courseInfo.ProfessorNr = professorcounter;

                                //Percorrer cada disciplina do curso e recolher nota do aluno
                                foreach (var disciplinecountermodule in courseDisciplines)
                                {
                                    if (disciplinecountermodule != null && newStudent != null)
                                    {
                                        var disciplinegrade = dbContext.Grades.Where(x => x.DisciplineId == disciplinecountermodule.DisciplineId && x.StudentId == newStudent.StudentId).Distinct().ToList();
                                        foreach (var grade in disciplinegrade)
                                        {
                                            //Soma do total do curso
                                            courseTotal = courseTotal + grade.Grade;
                                        }

                                        //Adicionar aluno ao contador
                                        alumnicounter++;
                                    }
                                }

                                //Se houver notas e alunos calcula-se a media
                                if (courseTotal != 0 && alumnicounter != 0)
                                {

                                    courseStudentAverage.Average = courseTotal / alumnicounter;
                                    courseStudentAverage.Average = Math.Round(courseStudentAverage.Average, 2);

                                }


                                //Media de 1 aluno com 1 inscrição no curso 
                                courseStudentAverages.Add(courseStudentAverage);
                                //Nova media de aluno
                                courseStudentAverage = new CourseStudentAverage();

                            }
                        }
                        //Adicionar mais 1 curso ao counter
                        courseCounter++;
                        //Adicionar lista de medias de alunos à info de curso
                        courseInfo.CourseStudentAverages = courseStudentAverages;
                        //Caso as medias venham vazias/ nao hajam notas, devolve-se array vazio
                        if (courseInfo.ProfessorNr == 0) courseInfo.CourseStudentAverages = new List<CourseStudentAverage>();
                        //Adicionar info de curso à lista 
                        coursesInfo.Add(courseInfo);
                    }
                }



            }

            
            
            return coursesInfo;
        }

        public async Task<Courses> GetCourseAsync(int id)
        {
            var course = await dbContext.Courses.FindAsync(id);

            if (course != null)
            {
                return course;
            }
            return null;

        }
        public async Task<Courses> EditCourseAsync(Courses courses, int id)
        {

            var course = await dbContext.Courses.FindAsync(id);


            if (course != null)
            {

                 course.CourseName= courses.CourseName;

                await dbContext.SaveChangesAsync();

                return course;

            }
            return null;

        }
        public async Task<Courses> DeleteCourseAsync(int id)
        {

            var course = await dbContext.Courses.FindAsync(id);

            if (course != null)
            {

                dbContext.Remove(course);

                await dbContext.SaveChangesAsync();

                return course;

            }
            return null;

        }

        public async Task<List<Disciplines>> GetCourseDisciplinesAsync(int id)
        {
            var course = await dbContext.Courses.FindAsync(id);
           
            if (course != null) {

                // var courseDisciplines = from x in dbContext.Disciplines.Where(p => p.CourseId == course.Id).ToList();

                var courseDisciplines2 =  dbContext.Disciplines
                .Select(x => new Disciplines
                {
                    DisciplineId = x.DisciplineId,
                    DisciplineName = x.DisciplineName,
                    ProfessorId = x.ProfessorId,
                    CourseId = x.CourseId
                }).Where(x => x.CourseId == course.Id).ToList();

                if (courseDisciplines2 != null)
                {
                    return courseDisciplines2;

                }
                return null;
            }

            
            return null;
            


            

        }

        //-------------------------------------------------------------------
        //------------------ALL THE DISCIPLINE METHODS-----------------------
        //-------------------------------------------------------------------

        public async Task<Disciplines> CreateDisciplineAsync(Disciplines disciplines)
        {

            disciplines.DisciplineName = disciplines.DisciplineName.Trim().Replace("\"", "");

            //Check if string already in DB

            var exists = dbContext.Disciplines
            .Select(x => new Disciplines
            {
                DisciplineName = x.DisciplineName,
            }).Where(x => x.DisciplineName == disciplines.DisciplineName).ToList();
            
            //Validate if entry is already in DB or is invalid
            foreach (var selectdis in exists)
            {
                if (selectdis != null && selectdis.DisciplineName == disciplines.DisciplineName || disciplines.DisciplineName.IsNullOrEmpty() || Regex.Replace(disciplines.DisciplineName, "[^0-9]", "").Equals(disciplines.DisciplineName) || selectdis.DisciplineName.ToLower() == disciplines.DisciplineName.ToLower() || Regex.IsMatch(disciplines.DisciplineName, "/ ^[a - z,.'-]*$/i") || disciplines.DisciplineName == "string")
                {
                    return null;
                }
            }


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

        public async Task<List<DisciplineInfo>> GetDisciplineInfoAsync(int? id)
        {


            var disciplines = await dbContext.Disciplines.ToListAsync();
            var selectedDiscipline = new Disciplines();
            var grades = await dbContext.Grades.ToListAsync();
            var professorName = new String("");
            var disciplineName = new String("");
            var DisciplineInfo = new List<DisciplineInfo>();
            var studentGrade= new Grades();
            var studentCounter = 0;
            var disciplineCounter = 0;

            if(id!= null)
            {
                selectedDiscipline = await dbContext.Disciplines.FindAsync(id);
                if (selectedDiscipline != null)
                {
                    var oneDisciplineInfo = new DisciplineInfo();
                    oneDisciplineInfo.DisciplineStudent = new List<DisciplineStudent>();
                    oneDisciplineInfo.DisciplineProfessor = new DisciplineProfessor();
                    var disciplineProfessor = dbContext.Professors
                       .Select(x => new Professors
                       {
                           ProfessorId = x.ProfessorId,
                           ProfessorName = x.ProfessorName,
                           DateOfBirth = x.DateOfBirth,
                           Salary = x.Salary
                       }).Where(x => x.ProfessorId == selectedDiscipline.ProfessorId).ToList();
                    //Info de cada prof por disciplina
                    foreach (var professor in disciplineProfessor)
                    {


                        if (professor != null)
                        {
                            var oneDisciplineProfessor = new DisciplineProfessor();

                            oneDisciplineInfo.Id = disciplineCounter;
                            oneDisciplineProfessor.ProfessorId = selectedDiscipline.ProfessorId;
                            oneDisciplineProfessor.ProfessorName = professor.ProfessorName;
                            oneDisciplineProfessor.DateOfBirth = professor.DateOfBirth;
                            oneDisciplineProfessor.Salary = professor.Salary;
                            oneDisciplineInfo.DisciplineId = selectedDiscipline.DisciplineId;
                            oneDisciplineInfo.DisciplineName = selectedDiscipline.DisciplineName;

                            oneDisciplineInfo.DisciplineProfessor = oneDisciplineProfessor;


                        }
                    }
                    studentCounter = 0;
                    foreach (var grade in grades)
                    {
                        if (grade != null && grade.DisciplineId == selectedDiscipline.DisciplineId)
                        {
                            var student = dbContext.Students
                              .Select(x => new Students
                              {
                                  StudentId = x.StudentId,
                                  StudentName = x.StudentName,
                              }).Where(x => x.StudentId == grade.StudentId).FirstOrDefault();
                            if (student != null)
                            {
                                var oneDisciplineStudent = new DisciplineStudent();

                                studentCounter++;
                                oneDisciplineStudent.StudentName = student.StudentName;
                                oneDisciplineStudent.Grade = grade.Grade;

                                //Adicionar aluno à disciplina
                                oneDisciplineInfo.DisciplineStudent.Add(oneDisciplineStudent);


                            }

                        }
                    }
                    //Add size of class number
                    oneDisciplineInfo.ClassNumber = studentCounter;
                    //Add number of disciplines to counter
                    disciplineCounter++;

                    //Add Discipline Info to the List
                    DisciplineInfo.Add(oneDisciplineInfo);
                }
            }
            else
            {
                //Quantas disciplinas e info
                foreach (var discipline in disciplines)
                {

                    if (discipline != null)
                    {
                        var oneDisciplineInfo = new DisciplineInfo();
                        oneDisciplineInfo.DisciplineStudent = new List<DisciplineStudent>();
                        oneDisciplineInfo.DisciplineProfessor = new DisciplineProfessor();
                        var disciplineProfessor = dbContext.Professors
                           .Select(x => new Professors
                           {
                               ProfessorId = x.ProfessorId,
                               ProfessorName = x.ProfessorName,
                               DateOfBirth = x.DateOfBirth,
                               Salary = x.Salary
                           }).Where(x => x.ProfessorId == discipline.ProfessorId).ToList();

                        //Info de cada prof por disciplina
                        foreach (var professor in disciplineProfessor)
                        {


                            if (professor != null)
                            {
                                var oneDisciplineProfessor = new DisciplineProfessor();

                                oneDisciplineInfo.Id = disciplineCounter;
                                oneDisciplineProfessor.ProfessorId = discipline.ProfessorId;
                                oneDisciplineProfessor.ProfessorName = professor.ProfessorName;
                                oneDisciplineProfessor.DateOfBirth = professor.DateOfBirth;
                                oneDisciplineProfessor.Salary = professor.Salary;
                                oneDisciplineInfo.DisciplineId = discipline.DisciplineId;
                                oneDisciplineInfo.DisciplineName = discipline.DisciplineName;

                                oneDisciplineInfo.DisciplineProfessor = oneDisciplineProfessor;


                            }
                        }

                        studentCounter = 0;
                        foreach (var grade in grades)
                        {
                            if (grade != null && grade.DisciplineId == discipline.DisciplineId)
                            {
                                var student = dbContext.Students
                                  .Select(x => new Students
                                  {
                                      StudentId = x.StudentId,
                                      StudentName = x.StudentName,
                                  }).Where(x => x.StudentId == grade.StudentId).FirstOrDefault();
                                if (student != null)
                                {
                                    var oneDisciplineStudent = new DisciplineStudent();

                                    studentCounter++;
                                    oneDisciplineStudent.StudentName = student.StudentName;
                                    oneDisciplineStudent.Grade = grade.Grade;

                                    //Adicionar aluno à disciplina
                                    oneDisciplineInfo.DisciplineStudent.Add(oneDisciplineStudent);


                                }

                            }
                        }
                        //Add size of class number
                        oneDisciplineInfo.ClassNumber = studentCounter;
                        //Add number of disciplines to counter
                        disciplineCounter++;

                        //Add Discipline Info to the List
                        DisciplineInfo.Add(oneDisciplineInfo);


                    }
                }
            }

            return DisciplineInfo;


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

        //TODO
        public async Task<List<StudentGrade>> GetStudentGradeAsync(int? id)
        {

            var disciplines = await dbContext.Disciplines.ToListAsync();
            var grades = await dbContext.Grades.ToListAsync();
            var students = await dbContext.Students.ToListAsync();
            var selectedStudent = await dbContext.Students.FindAsync(id);
            var studentName = new String("");
            var disciplineName = new String("");
            var studentCounter = 0;
            var studentGrades = new List<StudentGrade>();
            if(selectedStudent != null)
            {
                var studentGrade = new StudentGrade();
                studentGrade.Id = studentCounter;
                studentGrade.StudentId = selectedStudent.StudentId;
                studentGrade.StudentName = selectedStudent.StudentName;
                studentGrade.DisciplineGrade = new List<DisciplineGrade>();

                //Quantas disciplinas e info
                foreach (var discipline in disciplines)
                {
                    if (discipline != null)
                    {
                        //Info de cada nota por disciplina
                        foreach (var grade in grades)
                        {
                            var studentDisciplineGrade = new DisciplineGrade();
                            if (grade != null && grade.DisciplineId == discipline.DisciplineId && selectedStudent.StudentId == grade.StudentId)
                            {
                                studentDisciplineGrade.DisciplineName = discipline.DisciplineName;
                                studentDisciplineGrade.Grade = grade.Grade;

                                studentGrade.DisciplineGrade.Add(studentDisciplineGrade);
                            }
                        }
                    }
                }
                //Add student grade to list
                studentGrades.Add(studentGrade);

            } else
            {
                foreach (var student in students)
                {

                    if (student != null)
                    {
                        studentCounter++;
                        var studentGrade = new StudentGrade();
                        studentGrade.Id = studentCounter;
                        studentGrade.StudentId = student.StudentId;
                        studentGrade.StudentName = student.StudentName;
                        studentGrade.DisciplineGrade = new List<DisciplineGrade>();

                        //Quantas disciplinas e info
                        foreach (var discipline in disciplines)
                        {
                            if (discipline != null)
                            {
                                //Info de cada nota por disciplina
                                foreach (var grade in grades)
                                {
                                    var studentDisciplineGrade = new DisciplineGrade();
                                    if (grade != null && grade.DisciplineId == discipline.DisciplineId && student.StudentId == grade.StudentId)
                                    {
                                        studentDisciplineGrade.DisciplineName = discipline.DisciplineName;
                                        studentDisciplineGrade.Grade = grade.Grade;

                                        studentGrade.DisciplineGrade.Add(studentDisciplineGrade);
                                    }
                                }
                            }
                        }
                        //Add student grade to list
                        studentGrades.Add(studentGrade);

                    }

                }

            }

            return studentGrades;

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


            if (grade != null && gradeToEdit!= null)
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
