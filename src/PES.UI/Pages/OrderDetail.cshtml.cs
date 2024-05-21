using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.Order;

namespace PES.UI.Pages
{
    public class OrderDetailModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        [BindProperty]
        public List<OrdererDetailResponse> ordererDetailResponses { get; set; } 
        public decimal? Total {  get; set; }
        public async Task<IActionResult> OnGet(string id)
        {
            string testCase = $"http://localhost:5046/api/v1/Order/{id}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(testCase);
                HttpContent content = responseMessage.Content;
                string message = await content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(message);
                JArray items = responseObject["ordererDetails"];
                ordererDetailResponses = items.Select(item => item.ToObject<OrdererDetailResponse>()).ToList();
                Total = responseObject["totalPrice"];

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
