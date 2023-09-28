using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Implementation;
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

            //Get all courses
            var courses = await dbContext.Courses.ToListAsync();
            //Declare filters
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            var regexEmpty = new Regex("^(?!$).+");
            //Validate entry so that there are no duplicates
            foreach ( var k in courses)
            {

                if (regexEmpty.IsMatch(request.CourseName) || Regex.Replace(k.CourseName, "[^0-9]", "").Equals(request.CourseName) || k.CourseName.ToLower() == request.CourseName.ToLower() || (regexItem.IsMatch(request.CourseName)))
                {
                    return Ok();
                }

            }
            //Post new entry
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
      

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {

            var course = await dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);

            if(course == null)
            {
                return NotFound();
            }

            return Ok(course);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int id, UpdateCourseRequestDTO updateCourseRequest)
        {
            var course = await dbContext.Courses.FindAsync(id);
            if(course != null) { 
            
                course.CourseName = updateCourseRequest.CourseName;

                await dbContext.SaveChangesAsync();

                return Ok(course);

            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            var course = await dbContext.Courses.FindAsync(id);
            if (course != null)
            {

                dbContext.Remove(course);

                await dbContext.SaveChangesAsync();

                return Ok(course);

            }
            return NotFound();

        }
    }
}
