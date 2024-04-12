using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesIntegrationLayer.Dtos;
using System.Net.Http.Headers;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<ActionResult> CreateClient(CreateClientDTO dto)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            var response = await _http.PostAsJsonAsync("create", dto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var userCreated = JsonConvert.DeserializeObject<ClientModel>(content);
                return new JsonResult(userCreated);
            }
            return BadRequest(new
            {
                Message = "Failed to create user. Try again"
            });
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ClientModel>> GetClientById(string id)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _http.GetAsync($"get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var client = JsonConvert.DeserializeObject<ClientModel>(content);
                return client;
            }
            else
            {
                return StatusCode((int)response.StatusCode, new ErrorType
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateClient(string id, UpdateClientDTO dto)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");   
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _http.PutAsJsonAsync($"update/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)response.StatusCode, new ErrorType
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                });
            }
        }
    }
}
