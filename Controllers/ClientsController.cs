using IntegracionDesarrollo3.Dtos;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("clients")]
    public class ClientsController : Controller
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "clients/";

        public class ErrorType
        {
            public dynamic Message { get; set; }
            public int StatusCode { get; set; }
        }
        

        public ClientsController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
        }


        [HttpPost("create")]
        public async Task<JsonResult> CreateUser(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("users", dto);
            var content = await response.Content.ReadAsStringAsync();

            
            return new JsonResult(content);
        }

        [HttpGet("get")]
        public async Task<JsonResult> GetAllUsers()
        {
            var response = await _http.GetAsync("users");
            var content = await response.Content.ReadAsStringAsync();

            return new JsonResult(content);
        }

        [HttpGet("get/{id}")]
        public async Task<JsonResult> GetUserById(string id)
        {
            var response = await _http.GetAsync($"users/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return new JsonResult(content);
        }

        [HttpPut("update")]
        public async Task<JsonResult> UpdateUserById(string id, SignUpDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"users/{id}", dto);
            var content = await response.Content.ReadAsStringAsync();

            return new JsonResult(content);
        }
    }
}
