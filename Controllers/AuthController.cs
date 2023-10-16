using ManageCollege.Models.Domain;
using ManageCollege.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManageCollege.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository authenticationRepository;

        public AuthController(IRepository authenticationRepository)
        {
            this.authenticationRepository = authenticationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuth()
        {

            var auth = await authenticationRepository.GetAuthentication();

            return Ok(auth);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> SetAuth(Authentication request, int id)
        {
            if (request != null) 
            {

                var setAuth = await authenticationRepository.SetAuthentication(request, id);

                if (setAuth == null)
                {
                    return NotFound();
                }
            
                return Ok(setAuth);
            }
            return NotFound();

        }
    }
}
