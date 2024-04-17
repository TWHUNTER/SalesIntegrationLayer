using IntegracionDesarrollo3.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace IntegracionDesarrollo3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "auth/";

        public class AuthToken
        {
            public string AccessToken;
        }

        private readonly IntegrationDatabase _integration;

        public AuthController(IConfiguration cfg, IHttpClientFactory factory, IntegrationDatabase integration)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
            _integration = integration;
        }

        /*[HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            //var users = await _integration.Users.ToListAsync();

            var response = await _http.PostAsJsonAsync("login", dto);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var token = JsonConvert.DeserializeObject<AuthToken>(content);
                return new JsonResult(new
                {
                    token!.AccessToken,
                    //users
                });
            }
            try
            {
                var error = JsonConvert.DeserializeObject<CoreApiError>(content);
                return BadRequest(new
                {
                    error!.Message,
                    error.StatusCode,
                    //users
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Something went very wrong: " + ex.Message);
            }
        }*/

        /*[HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            bool isValidUser = await _integration.Users.AnyAsync(u => u.username == dto.username && u.user_password == dto.user_password);
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.PostAsJsonAsync("login", dto);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode||isValidUser)
            {
                return new JsonResult(new
                {
                    Message = "Ha ingresado correctamente",
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = content
                });
            };
        }*/

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.PostAsJsonAsync("login", dto);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new
                {
                    Message = "Ha ingresado correctamente",
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = content
                });
            };
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUpDTO dto)
        {
            bool userExists = await _integration.Users.AnyAsync(user => user.username == dto.username);

            if (userExists)
            {
                return BadRequest(new
                {
                    Message = "Ese usuario ya exite, intente con otro nombre."
                });
            }


            var newUser = new Models.UserModel
            {
                client_FullName = dto.full_name,
                username = dto.username,
                user_password = dto.user_password,
                Email = dto.email,
                PhoneNumber = dto.phone_number,
                CreatedAt = DateTime.Now
            };
            _integration.Users.Add(newUser);

            var newClient = new Models.ClientModel
            {
                client_fullname = dto.full_name,
                email = dto.email,
                phone_number = dto.phone_number,
                createdAt = DateTime.Now
            };
            object value = _integration.Clients.Add(newClient);

            await _integration.SaveChangesAsync();

            return new JsonResult(new
            {
                Message = "El usuario ha sido registrado satisfactoriamente."
            });
        }


        [HttpPost("close")]
        public async Task<ActionResult> Close()
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.PostAsJsonAsync("close", new { });
            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new
                {
                    Message = "Se ha cerrado su sesión",
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "No has iniciado sesión"
                });
            }
        }
    }
}
