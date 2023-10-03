using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {

        private readonly IRepository professorsRepository;

        public ProfessorsController(IRepository professorsRepository)
        {
            this.professorsRepository = professorsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfessor(CreateProfessorRequestDTO request)
        {
            //Map DTO to Domain Model
            var professor = new Professors
            {
                ProfessorName = request.ProfessorName,
                DateOfBirth = request.DateOfBirth,
                Salary = request.Salary
            };

            //---EDIT
            await professorsRepository.CreateProfessorAsync(professor);


            //Domain model to DTO
            var response = new ProfessorDTO
            {
                ProfessorId = professor.ProfessorId,
                ProfessorName = professor.ProfessorName,
                DateOfBirth = professor.DateOfBirth,
                Salary = professor.Salary
            };

            return Ok(response);

        }
        [HttpGet]
        public async Task<IActionResult> Getprofessors()
        {
            //---EDIT
            var professors = await professorsRepository.GetProfessorsAsync();

            return Ok(professors);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProfessor([FromRoute] int id)
        {
            //---EDIT

            var professor = await professorsRepository.GetProfessorAsync(id);

            if (professor == null)
            {
                return NotFound();
            }

            return Ok(professor);

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProfessor([FromRoute] int id, Professors request)
        {
            //---EDIT

            var professor = await professorsRepository.EditProfessorAsync(request, id);

            if (professor == null)
            {
                return NotFound();
            }

            return Ok(professor);

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProfessor([FromRoute] int id)
        {
            //---EDIT

            var professor = await professorsRepository.DeleteProfessorAsync(id);

            if (professor != null)
            {
                return Ok(professor);

            }
            return NotFound("Doesn't Exist or has already been deleted");

        }

    }
}
