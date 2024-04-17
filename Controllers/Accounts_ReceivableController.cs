using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System.Net.Http.Headers;
using System.Text;
using static IntegracionDesarrollo3.Dtos.AccountReceivableDTO;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Accounts_ReceivableController : ControllerBase
    {

        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "accounts/";

        public Accounts_ReceivableController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AccountsReceivableModel>>> GetAccountReceivable()
        {

            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync("get");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var accountRe = JsonConvert.DeserializeObject<IEnumerable<AccountsReceivableModel>>(content);
                return Ok(accountRe);
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
        public async Task<ActionResult<AccountsReceivableModel>> GetAccountRById(int id)
        {
            Utils.RequestNeedsAuthentication(Request, _http);
            var response = await _http.GetAsync("get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var accountRJson = await response.Content.ReadAsStringAsync();
                var accountR = JsonConvert.DeserializeObject<List<AccountsReceivableModel>>(accountRJson);
                return new JsonResult(accountR);
            }
            return StatusCode((int)response.StatusCode, new { Message = $"Failed to get accountR by ID: {id}." });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccountR([FromBody] UpdateAccountsReceivableDto accountRedto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            var updateContent = new
            {
                account_id = accountRedto.account_id,
                invoice_id = accountRedto.invoice_id,
                amount = accountRedto.amount,
                due_date = accountRedto.due_date,
                status = accountRedto.status
            };

            var json = JsonConvert.SerializeObject(updateContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Envío de la solicitud PUT
            var response = await _http.PutAsync("update", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Cuenta actualizado exitosamente." });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { Message = errorContent, StatusCode = (int)response.StatusCode });
            }

            // Envío de la solicitud PUT
            //var response = await _http.PutAsync("update", stringContent);
            //if (response.IsSuccessStatusCode)
            //{
            //    return Ok(new { Message = "Account actualizado exitosamente." });
            //}
            //else
            //{
            //    var errorContent = await response.Content.ReadAsStringAsync();
            //    return StatusCode((int)response.StatusCode, new { Message = errorContent, StatusCode = (int)response.StatusCode });
            //}
        }
    }
}
