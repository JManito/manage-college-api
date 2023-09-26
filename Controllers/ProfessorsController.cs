using ManageCollege.Data;
using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;
        public ProfessorsController(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfessor(CreateProfessorRequestDTO request)
        {
            //Map DTO to Domain Model so that the response doesn't return the final user the model structure
            var professor = new Professors
            {
                ProfessorName = request.ProfessorName,
                DateOfBirth = request.DateOfBirth,
                Salary = request.Salary
            };



            await dbContext.Professors.AddAsync(professor);
            await dbContext.SaveChangesAsync();


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
    }
}
