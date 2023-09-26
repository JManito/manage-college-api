using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}

