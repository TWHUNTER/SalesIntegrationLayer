using IntegracionDesarrollo3.Dtos;
using IntegracionDesarrollo3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "products/";

        public ProductsController(IConfiguration cfg, IHttpClientFactory factory)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _http.GetAsync("get");
            if (response.IsSuccessStatusCode)
            {
                var productsJson = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductsDTO>>(productsJson);
                return new JsonResult(products);
            }
            return StatusCode((int)response.StatusCode, new { Message = "Failed to get products." });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _http.GetAsync($"get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ProductsDTO>(productJson);
                return new JsonResult(product);
            }
            return StatusCode((int)response.StatusCode, new { Message = $"Failed to get the product by ID: {id}." });
        }

        [HttpGet("filter/{category}")]
        public async Task<IActionResult> FilterProductsByCategory(string category)
        {
            var response = await _http.GetAsync($"filter/{category}");
            if (response.IsSuccessStatusCode)
            {
                var filteredProductsJson = await response.Content.ReadAsStringAsync();
                var filteredProducts = JsonConvert.DeserializeObject<IEnumerable<ProductsDTO>>(filteredProductsJson);
                return new JsonResult(filteredProducts);
            }
            return StatusCode((int)response.StatusCode, new { Message = $"Failed to filter products by category: {category}." });
        }


        //[HttpPost("create")]
        //public async Task<IActionResult> CreateProduct([FromForm] ProductsDTO dto)
        //{
        //    var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        //    var response = await _http.PostAsJsonAsync("create", dto);
        //    var content = await response.Content.ReadAsStringAsync();

        //    Console.WriteLine(content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var productCreated = JsonConvert.DeserializeObject<ClientModel>(content);
        //        return new JsonResult(productCreated);
        //    }

        //    //return StatusCode((int)response.StatusCode, new { Message = "Failed to create the product." });
        //    return StatusCode((int)response.StatusCode, new { Message = content});
        //}


        /*
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO productDto)
        {
            var json = JsonConvert.SerializeObject(productDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync("update", content);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Product updated successfully." });
            }

            return StatusCode((int)response.StatusCode, new { Message = "Failed to update the product." });
        }
*/
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            var response = await _http.DeleteAsync($"delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Product deleted successfully." });
            }

            return StatusCode((int)response.StatusCode, new { Message = "Failed to delete the product." });
        }

    }

}
