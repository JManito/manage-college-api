using Azure.Core;
using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Implementation;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Web.Http.Controllers;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
       
        private readonly IRepository disciplinesRepository;

        public DisciplinesController(IRepository disciplinesRepository)
        {
            this.disciplinesRepository = disciplinesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscipline(CreateDiscipline request)
        {
            //Map DTO to Domain Model
            var discipline = new Disciplines
            {
                DisciplineName = request.DisciplineName,
                ProfessorId = request.ProfessorId,
                CourseId = request.CourseId,
            };

            await disciplinesRepository.CreateDisciplineAsync(discipline);


            //Domain model to DTO
            var response = new DisciplineDTO
            {
                DisciplineId = discipline.DisciplineId,
                DisciplineName = discipline.DisciplineName,
                ProfessorId = discipline.ProfessorId,
                CourseId = discipline.CourseId
            };

            return Ok(response);

        }
        [HttpGet]
        public async Task<IActionResult> GetDisciplines()
        { 

            var disciplines = await disciplinesRepository.GetDisciplinesAsync();

            return Ok(disciplines);

        }
        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> GetDisciplineInfoAsync([FromBody] int? id)
        {
            var disciplineInfo = await disciplinesRepository.GetDisciplineInfoAsync(id);

            return Ok(disciplineInfo);

        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDiscipline([FromRoute] int id)
        {

            var discipline = await disciplinesRepository.GetDisciplineAsync(id);

            if (discipline == null)
            {
                return NotFound();
            }

            return Ok(discipline);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateDiscipline([FromRoute] int id, Disciplines request) 
        {

            var discipline = await disciplinesRepository.EditDisciplineAsync(request, id);

            if (discipline == null)
            {
                return NotFound();
            }

            return Ok(discipline);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDiscipline([FromRoute] int id)
        {
            var discipline = await disciplinesRepository.DeleteDisciplineAsync(id);

            if (discipline != null)
            {
                return Ok(discipline);

            }
            return NotFound("Doesn't Exist or has already been deleted");

        }

    }
}

