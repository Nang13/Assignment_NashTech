using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.Cart;
using System.Net.Http;
using PES.Domain.DTOs.Order;

namespace PES.UI.Pages
{
    public class OrderModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        public List<OrderResponse> orders { get; set; }
        public decimal Total { get; set; }

        
        public async Task<IActionResult> OnGet()
        {
            string testCase = "http://localhost:5046/api/v1/Order";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(testCase);
                HttpContent content = responseMessage.Content;
                string message = await content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(message);
                JArray items = responseObject;
                orders = items.Select(item => item.ToObject<OrderResponse>()).ToList();
                //TempData["cart"] = CartItems;

                
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred. {0}", exception.Message);
            }
            return Page();
        }
    }
}