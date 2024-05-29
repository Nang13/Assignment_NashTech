using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.Domain.DTOs.Category;
using PES.Domain.DTOs.Product;
using PES.Domain.Entities.Model;
using System;
using System.Text;

namespace PES.UI.Pages
{
    public class ShopModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        public List<CategoryResponse> Categories { get; set; }
        public List<ProductsResponse> Products { get; set; }

    
        public async Task<IActionResult> OnGet(string? searchQuery = null, string? category = null)
        {
            await GetCategoryData();

            await GetProductDefault(searchQuery, category);

            return Page();

        }
       

        public async ValueTask GetProductDefault(string? SearchQuery = null,string? CategoryQuery = null)
        {
            string urlProducts = "";
            if (SearchQuery != null)
            {
                urlProducts = $"http://localhost:5046/api/v1/Product?ProductName={SearchQuery}&pageNumber=0&pageSize=10";
            }
            else if(CategoryQuery != null)
            {
                urlProducts = $"http://localhost:5046/api/v1/Product?CategoryName={CategoryQuery}&pageNumber=0&pageSize=10";
            }
            else
            {
                urlProducts = "http://localhost:5046/api/v1/Product?pageNumber=0&pageSize=9";

            }
            HttpResponseMessage responseMessage = await httpClient.GetAsync(urlProducts);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(message);
            JArray items = responseObject["items"];
            Products = items.Select(item => item.ToObject<ProductsResponse>()).ToList();
        }
        public async ValueTask GetCategoryData()
        {
            string urlCategory = "http://localhost:5046/api/v1/Category";
            HttpResponseMessage responseMessage = await httpClient.GetAsync(urlCategory);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(message);
            JArray items = responseObject;
            Categories = items.Select(item => item.ToObject<CategoryResponse>()).ToList();

        }
    }
}
