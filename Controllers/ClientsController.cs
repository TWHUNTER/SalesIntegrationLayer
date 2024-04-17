using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesIntegrationLayer.Dtos;
using System.Net.Http.Headers;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "clients/";
        private readonly IntegrationDatabase _integration;

        public ClientsController(IConfiguration cfg, IHttpClientFactory factory, IntegrationDatabase integration)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
            _integration = integration;
        }

        /*[HttpPost("create")]
        public async Task<ActionResult> CreateClient(CreateClientDTO dto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            var response = await _http.PostAsJsonAsync("create", dto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var userCreated = JsonConvert.DeserializeObject<ClientModel>(content);
                return new JsonResult(userCreated);
            }
            return BadRequest(new
            {
                Message = "Intente nuevamente con otro correo"
            });
        }*/

        [HttpPost("create")]
        public async Task<ActionResult> CreateClient(CreateClientDTO dto)
        {

            bool clientExists = await _integration.Clients.AnyAsync(c => c.email == dto.email);

            if (clientExists)
            {
                return BadRequest(new
                {
                    Message = "Ese usuario ya exite, intente con otro nombre."
                });
            }

            var newClient = new Models.ClientModel
            {
                client_fullname = dto.client_fullname,
                email = dto.email,
                phone_number = dto.phone_number,
                createdAt = DateTime.Now
            };
            object value = _integration.Clients.Add(newClient);

            await _integration.SaveChangesAsync();


            Utils.RequestNeedsAuthentication(Request, _http);

                   var response = await _http.PostAsJsonAsync("create", dto);
                   var content = await response.Content.ReadAsStringAsync();

                   if (response.IsSuccessStatusCode)
                   {
                       var clientCreated = JsonConvert.DeserializeObject<ClientModel>(content);
                       return new JsonResult(clientCreated);
                   }
                   return BadRequest(new
                   {
                       Message = "Intente nuevamente con otro correo"
                   });

        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ClientModel>>> GetClients()
        {
            Utils.RequestNeedsAuthentication(Request, _http);

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
            Utils.RequestNeedsAuthentication(Request, _http);
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