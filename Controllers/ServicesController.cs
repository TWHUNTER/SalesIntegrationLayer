using IntegracionDesarrollo3;
using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using SalesIntegrationLayer.Dtos;
using System.Net.Http.Headers;
using System.Text;
using static IntegracionDesarrollo3.Dtos.ServicesDTO;

namespace IntegracionDesarrollo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "services/";

        public ServicesController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
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
        public async Task<ActionResult> CreateServices(CreateServicesDto dto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            var response = await _http.PostAsJsonAsync("create", dto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var servicesCreated = JsonConvert.DeserializeObject<ServicesModel>(content);
                return new JsonResult(servicesCreated);
            }
            return BadRequest(new
            {
                Message = "Failed to create services. Try again"
            });
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateServices([FromBody] UpdateServicesDto dto)
        {

            Utils.RequestNeedsAuthentication(Request, _http);

            var updateContent = new
            {
                service_name = dto.service_name,
                service_description = dto.service_description,
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
