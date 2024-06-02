using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.Domain.DTOs.ProductDTO;
using static Google.Apis.Requests.BatchRequest;
using static System.Net.WebRequestMethods;

namespace PES.UI.Pages
{
    public class HomePageModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        public List<ProductsResponse> Products { get; set; }
      
        
        public async Task OnGetAsync()
        {

            string testCase = "http://localhost:5046/api/v1/Product?pageNumber=0&pageSize=10";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(testCase);
                HttpContent content = responseMessage.Content;
                string message = await content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(message);
                JArray items = responseObject["items"];
                Products = items.Select(item => item.ToObject<ProductsResponse>()).ToList();
               

                RedirectToPage();
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred. {0}", exception.Message);
            }
        }

        public async Task<IActionResult> OnGetUpdate(string id)
        {
            string testCase = $"http://localhost:5046/api/v1/Product?CategoryId={id}&pageNumber=0&pageSize=10";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(testCase);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();
            dynamic responseObject = JsonConvert.DeserializeObject(message);
            JArray items = responseObject["items"];
            Products = items.Select(item => item.ToObject<ProductsResponse>()).ToList();
           
            return Page();
        }
    }
}
