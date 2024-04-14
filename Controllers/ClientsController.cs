using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesIntegrationLayer.Dtos;
using System.Net.Http.Headers;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using System.Text;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "clients/";

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

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ClientModel>>> GetClients()
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return Unauthorized(new { Message = "Authorization token is missing." });
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _http.GetAsync("get");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clients = JsonConvert.DeserializeObject<IEnumerable<ClientModel>>(content);
                return Ok(clients);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                });
            }
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
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateClient([FromBody] UpdateClientDTO dto)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return Unauthorized(new { Message = "Authorization token is missing." });
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            // Asegúrate de que los campos que estás enviando coincidan con los que espera el servidor.
            var updateContent = new
            {
                client_fullname = dto.client_fullname,
                email = dto.email,
                phone_number = dto.phone_number // Este es el campo que se usa para identificar al cliente en la base de datos
            };

            var json = JsonConvert.SerializeObject(updateContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Envío de la solicitud PUT
            var response = await _http.PutAsync("update", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Cliente actualizado exitosamente." });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { Message = errorContent, StatusCode = (int)response.StatusCode });
            }
        }


    }
}
