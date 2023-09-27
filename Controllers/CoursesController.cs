using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

            var courses = await dbContext.Courses.ToListAsync();
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            var regexEmpty = new Regex("^(?!$).+");


            foreach ( var k in courses)
            {

                if (regexEmpty.IsMatch(request.CourseName) || Regex.Replace(k.CourseName, "[^0-9]", "").Equals(request.CourseName) || k.CourseName.ToLower() == request.CourseName.ToLower() || (regexItem.IsMatch(request.CourseName)))
                {
                    return Ok();
                }

            }

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
        public async Task<IActionResult> GetCourse()
        {
           var courses = await dbContext.Courses.ToListAsync();

           return Ok(courses);

        }
    }
}
