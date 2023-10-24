using Azure.Core;
using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Implementation;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Swagger.Net;
using System.Text.RegularExpressions;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IRepository coursesRepository;

        public CoursesController(IRepository coursesRepository)
        {
            this.coursesRepository = coursesRepository;
        }
        [HttpPost]

        public async Task<IActionResult> CreateCourse(CreateCourse request)
        {
            //Map DTO to Domain Model
            var course = new Courses
            {
                CourseName = request.CourseName
            };

            //Get all courses
           var courses = await coursesRepository.CreateCourseAsync(course);

            if (courses != null)
            {
                return Ok(courses);
            }

            return NotFound("Can't create, the entered name already exists / is invalid!");
        }

        [HttpPost]
        [Route("{id:int}/enroll/{studentId:int}")]
        public async Task<IActionResult> Enroll([FromRoute] int id, [FromBody] int studentId)
        {
            //Map DTO to Domain Model
            var enrollment = new Enrollment
            {
                CourseId = id,
                StudentId = studentId
            };

            var enroll = await coursesRepository.Enroll(enrollment, id, studentId);

            if (enroll != null)
            {
                return Ok(enroll);
            }

            return NotFound("Can't create, the entered name already exists / is invalid!");
        }

        [HttpGet]
        public async Task<IActionResult> GetCourse()
        {
            var courses = await coursesRepository.GetCoursesAsync();

           return Ok(courses);

        }

        [HttpGet]
        [Route("professors")]
        public async Task<IActionResult> GetCourseProfessors()
        {
            var courseProfessors = await coursesRepository.GetCoursesProfessorsAsync();

            return Ok(courseProfessors);

        }

        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> GetCoursesInfoAsync([FromBody] int? id)
        {
            var courseInfo = await coursesRepository.GetCoursesInfoAsync(id);

            return Ok(courseInfo);

        }
        

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {

            var course = await coursesRepository.GetCourseAsync(id);

            if(course == null)
            {
                return NotFound();
            }

            return Ok(course);

        }

        [HttpGet]
        [Route("{id:int}/disciplines")]
        public async Task<IActionResult> GetDisciplinesOfCourse([FromRoute] int id)
        {

            var course = await coursesRepository.GetCourseDisciplinesAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int id, UpdateCourse updateCourseRequest)
        {
            var course = await coursesRepository.GetCourseAsync(id);
            if (course != null) { 
            
                course.CourseName = updateCourseRequest.CourseName;

                await coursesRepository.EditCourseAsync(course, id);

                return Ok(course);

            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            var course = await coursesRepository.GetCourseAsync(id);

            if (course != null)
            {

                await coursesRepository.DeleteCourseAsync(id);

                return Ok(course);

            }
            return NotFound();

        }
    }
}
