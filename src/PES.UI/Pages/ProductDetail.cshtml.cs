using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.Product;
using PES.Domain.Entities.Model;
using System.Net.Http;
using PES.Domain.DTOs.Category;

namespace PES.UI.Pages
{
    public class ProductDetailModel : PageModel
    {
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        public Guid? ProductId { get; set; }
        [BindProperty]
        public NutrionInfo NutrionInfo { get; set; }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; }
        [BindProperty]
        public ImportantInfo importantInfo { get; set; }

        
        static HttpClient httpClient = new HttpClient();
        public async Task<IActionResult> OnGet(string id)
        {
            string url = $"http://localhost:5046/api/v1/Product/{id}";
            
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                HttpContent content = responseMessage.Content;
                string message = await content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(message);
               

                JToken  nutrionObject = responseObject["nutrionInfo"];
                JToken imporatantObject = responseObject["importantInfo"];
                JToken categoryObject = responseObject["productCategory"];

                ProductName = responseObject["productName"].ToString();
                id = responseObject["id"];
                Price = responseObject["price"];
                NutrionInfo = nutrionObject.ToObject<NutrionInfo>();
                ProductCategory = categoryObject.ToObject<ProductCategory>();
                importantInfo = imporatantObject.ToObject<ImportantInfo>();


                RedirectToPage();
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred. {0}", exception.Message);
            }
            return Page();
        }
    }
}
