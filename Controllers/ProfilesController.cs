using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SalesIntegrationLayer.Dtos;

namespace IntegracionDesarrollo3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "profiles/";
        private readonly IntegrationDatabase _integration;

        public ProfilesController(IConfiguration cfg, IHttpClientFactory factory,IntegrationDatabase integration)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
            _integration = integration;
        }

        [HttpGet("get")]
        public async Task<ActionResult> GetAllProfiles()
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync("get");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var users = JsonConvert.DeserializeObject<IEnumerable<Profile>>(content);
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

        [HttpGet("get/{id}")]
        public async Task<ActionResult> GetProfileById(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync($"get/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var users = JsonConvert.DeserializeObject<Profile>(content);
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

        [HttpPost("create")]
        public async Task<ActionResult> CreateProfile(CreateProfileDTO dto)
        {

            bool invoicesExists = await _integration.profiles.AnyAsync(profile => profile.profile_role == dto.profile_role);

            if (invoicesExists)
            {
                return BadRequest(new
                {
                    Message = "Ese Prefil ya exite, intente con otro nombre."
                });
            }

            var newProfile = new Models.Profile
            {
                profile_role = dto.profile_role,
                role_description = dto.role_description,
            };
            _integration.profiles.Add(newProfile);

            await _integration.SaveChangesAsync();

            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.PostAsJsonAsync("create", dto);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new
                {
                    Message = "El rol ha sido creado"
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

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProfile(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.DeleteAsync($"delete/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new
                {
                    Message = "El rol ha sido eliminado"
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
