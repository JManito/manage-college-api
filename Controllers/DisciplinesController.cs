using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
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
       
        private readonly IDisciplinesRepository disciplinesRepository;

        public DisciplinesController(IDisciplinesRepository disciplinesRepository)
        {
            this.disciplinesRepository = disciplinesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscipline(CreateDisciplineRequestDTO request)
        {
            //Map DTO to Domain Model
            var discipline = new Disciplines
            {
                DisciplineName = request.DisciplineName,
                ProfessorId = request.ProfessorId
            };

            await disciplinesRepository.Createasync(discipline);


            //Domain model to DTO
            var response = new DisciplineDTO
            {
                DisciplineId = discipline.DisciplineId,
                DisciplineName = discipline.DisciplineName,
                ProfessorId = discipline.ProfessorId
            };

            return Ok(response);

        }
        [HttpGet]
        public async Task<IActionResult> GetDiscipline()
        {

            var disciplines = await disciplinesRepository.Getasync();

            return Ok(disciplines);

        }


        [HttpPut]
        public async Task<IActionResult> EditCourse(DisciplineDTO request, int id)
        {
            //Map DTO to Domain Model
            var discipline = new DisciplineDTO
            {
                DisciplineId = request.DisciplineId,
                DisciplineName = request.DisciplineName,
                ProfessorId = request.ProfessorId
            };
            return null;
            /*
            await disciplinesRepository.Putasync(request);


            return RedirectToAction("Details", new { id = incoming.DinnerID });*/
        }
    }
}

