using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.Domain.DTOs.CategoryDTO;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.UI.Pages.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PES.UI.Pages
{
    public class ShopModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        static SignInModel _signInModel = new SignInModel();
        public List<CategoryResponse> Categories { get; set; }
        public List<ProductsResponse> Products { get; set; }
        public List<ProductsResponse> ProductsMostView { get; set; }

    
        public async Task<IActionResult> OnGet(string? searchQuery = null, string? category = null)
        {
            await GetCategoryData();

            await GetProductDefault(searchQuery, category);
            await MostViewProduct(); 

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
            string urlCategory = "https://localhost:7187/api/v1/Category";
            HttpResponseMessage responseMessage = await httpClient.GetAsync(urlCategory);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(message);
            JArray items = responseObject;
            Categories = items.Select(item => item.ToObject<CategoryResponse>()).ToList();

        }
    
        public async Task MostViewProduct()
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync("https://localhost:7187/api/v1/Product?PopularProduct=June&pageNumber=0&pageSize=10");
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(message);
            JArray items = responseObject["items"];
            ProductsMostView = items.Select(item => item.ToObject<ProductsResponse>()).ToList();
        }
        
        public async Task<IActionResult> OnPostAddToCart(string id)
        {
            var payload = new
            {
                productId = id,
                quantity = 1,
                cartActionType = 0
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7187/api/v1/Cart");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $" {Request.Cookies["AccessToken"]}");

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error: {0}", errorMessage);
                return RedirectToPage(); // or handle the error appropriately
            }

            string message = await response.Content.ReadAsStringAsync();
            Console.WriteLine("The output from thirdparty is: {0}", message);

            return RedirectToPage();

        }
            
    }
}
