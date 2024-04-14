using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesIntegrationLayer.Dtos;
using static System.Net.WebRequestMethods;

namespace IntegracionDesarrollo3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "clients/";

        public UsersController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
        }

        [HttpGet("get")]
        public async Task<ActionResult> GetAllUsers()
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync("get");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(content);
                return Ok(users);
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

        [HttpGet("get/{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync($"get/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var users = JsonConvert.DeserializeObject<UserModel>(content);
                return Ok(users);
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
    }
}
