using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Implementation;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IRepository gradesRepository;

        public GradesController(IRepository gradesRepository)
        {
            this.gradesRepository = gradesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGradesAsync(CreateGrade request)
        {
            //Map DTO to Domain Model
            var grade = new Grades
            {
                StudentId = request.StudentId,
                Grade = request.Grade,
                DisciplineId = request.DisciplineId
            };

            //---EDIT
            await gradesRepository.CreateGradesAsync(grade);


            //Domain model to DTO
            var response = new GradeDTO
            {
                StudentId = grade.StudentId,
                Grade = grade.Grade,
                DisciplineId = grade.DisciplineId
            };

            return Ok(response);

        }
        [HttpGet]
        public async Task<IActionResult> GetGrades()
        {

            var grades = await gradesRepository.GetGradesAsync();

            return Ok(grades);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetGrade([FromRoute] int id)
        {


            var grade = await gradesRepository.GetGradeAsync(id);

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateGrade([FromRoute] int id, Grades request)
        {
            //---EDIT

            var grade = await gradesRepository.EditGradeAsync(request, id);

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGrade([FromRoute] int id)
        {
            //---EDIT

            var grade = await gradesRepository.DeleteGradeAsync(id);

            if (grade != null)
            {
                return Ok(grade);

            }
            return NotFound("Doesn't Exist or has already been deleted");

        }

    }
}