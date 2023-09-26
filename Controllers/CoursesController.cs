using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;
        public CoursesController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseRequestDTO request)
        {
            //Map DTO to Domain Model
            var course = new Courses
            {
                CourseName = request.CourseName
            };

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();


            //Domain model to DTO
            var response = new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetCourse(GetCourseRequestDTO request)
        {
          
            //Map DTO to Domain Model.. see GetCourseRequestDTO 
            var course = new Courses
            {
                Id = request.Id,
                CourseName = request.CourseName
            };

            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();


            //Domain model to DTO
            var response = new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName
            };

            return Ok(response);

        }


    }
}
