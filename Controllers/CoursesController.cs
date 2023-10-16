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
           if(courses != null)
            {
                return Ok(courses);
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
