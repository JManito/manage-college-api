using ManageCollege.Controllers;
using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;
        public StudentsController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentRequestDTO request)
        {
            //Map DTO to Domain Model
            var student = new Students
            {
                StudentName = request.StudentName,
                DateOfBirth = request.DateOfBirth,
                EnrollmentNumber = request.EnrollmentNumber
            };



            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();


            //Domain model to DTO
            var response = new StudentDTO
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                DateOfBirth = student.DateOfBirth,
                EnrollmentNumber = student.EnrollmentNumber
            };

            return Ok(response);

        }
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await dbContext.Students.ToListAsync();

            return Ok(students);

        }
    }
}



