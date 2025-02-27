using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.Cart;
using System.Net.Http;
using PES.Domain.DTOs.OrderDTO;
using PES.UI.Pages.Shared;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace PES.UI.Pages
{
    public class OrderModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        static SignInModel _signInModel = new SignInModel();
        private readonly IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        public List<OrderResponse> orders { get; set; }
        public decimal Total { get; set; }

        
        public async Task<IActionResult> OnGet()
        {
            var accessToken = httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7187/api/v1/Order?pageNumber=0&pageSize=10");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var response = await httpClient.SendAsync(request);
               // HttpContent content = responseMessage.Content;
                string message = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(message);
                JArray items = responseObject["items"];
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