using ManageCollege.Controllers;
using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository studentsRepository;

        public StudentsController(IRepository studentsRepository)
        {
            this.studentsRepository = studentsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudent request)
        {
            //Map DTO to Domain Model
            var student = new Students
            {
                StudentName = request.StudentName,
                EnrollmentNumber = request.EnrollmentNumber,
                DateOfBirth = request.DateOfBirth,
            };

            
            await studentsRepository.CreateStudentAsync(student);


            //Domain model to DTO
            var response = new Student
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
            
            var students = await studentsRepository.GetStudentsAsync();

            return Ok(students);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            

            var student = await studentsRepository.GetStudentAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, Students request)
        {
            

            var student = await studentsRepository.EditStudentAsync(request, id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            

            var student = await studentsRepository.DeleteStudentAsync(id);

            if (student != null)
            {
                return Ok(student);

            }
            return NotFound("Doesn't Exist or has already been deleted");

        }

    }
}



