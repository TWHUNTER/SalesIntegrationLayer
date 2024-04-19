using IntegracionDesarrollo3.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IntegracionDesarrollo3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        private readonly HttpClient _http;
        private static readonly string RESOURCE = "category/";
        private readonly IntegrationDatabase _integration;

        public CategoryController(IConfiguration cfg, IHttpClientFactory factory, IntegrationDatabase integration)
        {
            _cfg = cfg;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri(cfg.GetValue<string>("CoreBaseUrl")! + RESOURCE);
            _integration = integration;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {

            bool categoryExists = await _integration.products_category.AnyAsync(category => category.category_name == categoryDto.category_name);

            if (categoryExists)
            {
                return BadRequest(new
                {
                    Message = "Esa Categoria ya exite, intente con otro nombre."
                });
            }

            var newCategory = new Models.Category
            {
                category_name = categoryDto.category_name
            };
            _integration.products_category.Add(newCategory);

            await _integration.SaveChangesAsync();


            Utils.RequestNeedsAuthentication(Request, _http);

            var json = JsonConvert.SerializeObject(categoryDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("create", content);
            if (response.IsSuccessStatusCode)
            {
                var createdCategoryJson = await response.Content.ReadAsStringAsync();
                var createdCategory = JsonConvert.DeserializeObject<CategoryDTO>(createdCategoryJson);
                return new JsonResult(createdCategory);
            }

            return StatusCode((int)response.StatusCode, new { Message = "Failed to create the category." });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _http.GetAsync("get");

            if (response.IsSuccessStatusCode)
            {
                var categoriesJson = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(categoriesJson);
                return new JsonResult(categories);
            }
            return StatusCode((int)response.StatusCode, new { Message = "Failed to get categories." });
        }



        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _http.GetAsync($"get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var categoryJson = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<CategoryDTO>(categoryJson);
                return new JsonResult(category);
            }
            return StatusCode((int)response.StatusCode, new { Message = $"Failed to get the category by ID: {id}." });
        }

        /*[HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO categoryDto)
        {
            var bearerToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(bearerToken))
            {
                return Unauthorized(new { Message = "Authorization token is missing." });
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var content = new
            {
                ID = categoryDto.ID,
                category_name = categoryDto.category_name
            };

            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"Sending update request for Category ID {categoryDto.ID}");

            var response = await _http.PutAsync("update", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var updatedCategoryJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Update successful: " + updatedCategoryJson);
                return Ok(JsonConvert.DeserializeObject(updatedCategoryJson));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update the category. Status code: {response.StatusCode}, Details: {errorContent}");
                return StatusCode((int)response.StatusCode, new { Message = "Failed to update the category.", Details = errorContent });
            }
        }*/

        [HttpPost("update")] // Aunque el método sea POST, se mantiene la ruta "update".
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO categoryDto)
        {
            Utils.RequestNeedsAuthentication(Request, _http);

            var content = new
            {
                ID = categoryDto.ID,
                category_name = categoryDto.category_name
            };

            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"Sending POST request to update Category ID {categoryDto.ID}");

            // Cambio el método PutAsync por PostAsync.
            var response = await _http.PostAsync("update", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var updatedCategoryJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Update successful: " + updatedCategoryJson);
                return Ok(updatedCategoryJson);
                /*return Ok(JsonConvert.DeserializeObject(updatedCategoryJson));*/
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update the category. Status code: {response.StatusCode}, Details: {errorContent}");
                return StatusCode((int)response.StatusCode, new { Message = "Failed to update the category.", Details = errorContent });
            }
        }

    }
}
