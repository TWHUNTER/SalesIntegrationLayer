using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionDesarrollo3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "products/";

        public class ErrorType
        {
            public dynamic Message { get; set; }
            public int StatusCode { get; set; }
        }

        public InvoiceController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
        }





    }
}
