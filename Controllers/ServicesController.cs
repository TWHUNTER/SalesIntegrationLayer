using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using static IntegracionDesarrollo3.Dtos.ServicesDTO;

namespace IntegracionDesarrollo3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "services/";
        private readonly IntegrationDatabase _integration;

        public ServicesController(IConfiguration cfg, IHttpClientFactory factory, IntegrationDatabase integration)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
            _integration = integration;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ServicesModel>>> GetServices()
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync("get");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var services = JsonConvert.DeserializeObject<IEnumerable<ServicesModel>>(content);
                return Ok(services);
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
        public async Task<ActionResult<IEnumerable<ServicesModel>>> GetServicesById(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync("get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var accountRJson = await response.Content.ReadAsStringAsync();
                var accountR = JsonConvert.DeserializeObject<List<ServicesModel>>(accountRJson);
                return new JsonResult(accountR);
            }
            return StatusCode((int)response.StatusCode, new { Message = $"Failed to get services by ID: {id}." });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateServices(CreateServicesDto dto)
        {

            bool servicesExists = await _integration.services.AnyAsync(services => services.service_name == dto.service_name);

            if (servicesExists)
            {
                return BadRequest(new
                {
                    Message = "Ese Servicio ya exite, intente con otro nombre."
                });
            }

            var newServices = new Models.ServicesModel
            {
                service_name = dto.service_name,
                services_description = dto.services_description,
                price = dto.price,
                isVisible = dto.isVisible,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
            _integration.services.Add(newServices);

            await _integration.SaveChangesAsync();

            Utils.RequestNeedsAuthentication(Request, _http);

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("create", content);
            if (response.IsSuccessStatusCode)
            {
                var createdServicesJson = await response.Content.ReadAsStringAsync();
                var createdServices = JsonConvert.DeserializeObject<CreateServicesDto>(createdServicesJson);
                return new JsonResult(createdServices);
            }

            return StatusCode((int)response.StatusCode, new { Message = "Failed to create the services." });
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateServices([FromBody] UpdateServicesDto dto)
        {

            Utils.RequestNeedsAuthentication(Request, _http);

            var updateContent = new
            {
                service_name = dto.service_name,
                service_description = dto.services_description,
                price = dto.price,
                isVisible = dto.isVisible
            };

            var json = JsonConvert.SerializeObject(updateContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Envío de la solicitud PUT
            var response = await _http.PutAsync("update", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Servicio actualizado exitosamente." });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { Message = errorContent, StatusCode = (int)response.StatusCode });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteServices(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.DeleteAsync($"delete/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new
                {
                    Message = "El servicio ha sido eliminado"
                });
            }
            else
            {
                return StatusCode((int)response.StatusCode, new CoreApiError
                {
                    Message = content,
                    StatusCode = (int)response.StatusCode
                });
            }
        }


    }
}
