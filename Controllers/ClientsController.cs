using IntegracionDesarrollo3.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {

        [HttpPost("/create")]
        public async Task<JsonResult> CreteUser(LoginDTO dto)
        {
            return new JsonResult(new
            {

            });
        }

        [HttpGet("/get")]
        public async Task<JsonResult> GetAllUsers(SignUpDTO dto)
        {
            return new JsonResult(new { });
        }

        [HttpGet("/get/{id}")]
        public async Task<JsonResult> GetUserById()
        {
            return new JsonResult(new { });
        }

        [HttpPut("/update/{id}")]
        public async Task<JsonResult> UpdateUserById()
        {
            return new JsonResult(new { });
        }
    }
}
