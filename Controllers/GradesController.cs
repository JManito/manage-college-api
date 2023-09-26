using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;
        public GradesController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost]
        public async Task<IActionResult> CreateGrade(CreateGradeRequestDTO request)
        {
            //Map DTO to Domain Model
            var grade = new Grades
            {
                StudentId = request.StudentId,
                DisciplineId = request.DisciplineId,
                Grade = request.Grade
            };

            await dbContext.Grades.AddAsync(grade);
            await dbContext.SaveChangesAsync();


            //Domain model to DTO
            var response = new GradeDTO
            {
                StudentId = grade.StudentId,
                DisciplineId = grade.DisciplineId,
                Grade = grade.Grade
            };

            return Ok(response);

        }
    }
}