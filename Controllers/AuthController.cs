using IntegracionDesarrollo3.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntegracionDesarrollo3.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "auth/";

        public class ErrorType
        {
            public dynamic Message { get; set; }
            public int StatusCode { get; set; }
        }

        public class AuthToken
        {
            public string AccessToken;
        }

        public AuthController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {

            var response = await _http.PostAsJsonAsync("login", dto);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var token = JsonConvert.DeserializeObject<AuthToken>(content);
                return new JsonResult(new
                {
                   token!.AccessToken
                });
            }
            try
            {
                var error = JsonConvert.DeserializeObject<ErrorType>(content);
                return BadRequest(new
                {
                    error!.Message,
                    error.StatusCode
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Something went very wrong: " + ex.Message);
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUpDTO dto)
        {
            var response = await _http.PostAsJsonAsync("register", dto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new
                {
                    Message = "El usuario ha sido registrado satisfactoriamente."
                });
            }
            var error = JsonConvert.DeserializeObject<ErrorType>(content);
            return BadRequest(new
            {
                error!.Message,
                error.StatusCode
            });
        }

        [HttpPost("close")]
        public async Task<JsonResult> Close()
        {
            return new JsonResult(new { });
        }
    }
}
