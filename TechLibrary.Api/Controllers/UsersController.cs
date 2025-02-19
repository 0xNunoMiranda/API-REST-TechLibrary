using Microsoft.AspNetCore.Mvc;

namespace TechLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        public IActionResult Get()
        {
            return Ok("Get all users");
        }


    }
}
