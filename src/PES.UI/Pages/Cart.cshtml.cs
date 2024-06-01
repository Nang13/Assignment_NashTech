using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.Product;
using PES.Domain.Entities.Model;
using System.Net.Http;
using PES.Domain.DTOs.Cart;
using PES.Domain.Enum;
using System.Text;
using PES.UI.Pages.Shared;
using System.Net.Http.Headers;

namespace PES.UI.Pages
{
    public class CartModel : PageModel
    {
        static HttpClient _httpClient = new HttpClient();
        [BindProperty]
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
        public async Task OnGetAsync()
        {

            string testCase = "http://localhost:5046/api/v1/Cart";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5046/api/v1/Cart");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", UserData.AccessToken); // Replace with your actual access token

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(message);
                    JArray items = responseObject.items;
                    CartItems = items.Select(item => item.ToObject<CartItem>()).ToList();
                    Total = responseObject.totalPrice;
                }
                else
                {
                   Console.WriteLine($"Error fetching cart data: {response.StatusCode}");
                }

                Page();
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred. {0}", exception.Message);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            //Dictionary<string, string> dictCars = new Dictionary<string, string> { { "passedObject",  } };
            //    return RedirectToPage("Checkout", dictCars);
            
            return Redirect("Checkout");

        }

        public async Task<IActionResult> OnGetDecreaseQuantity(string id)
        {
            var payload = new
            {
                productId = id,
                quantity = 1,
                cartActionType = 2
            };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5046/api/v1/Cart", content);
            response.Headers.Add("Authorization", $"Bearer {UserData.AccessToken}");
            HttpContent content1 = response.Content;
            string message = await content1.ReadAsStringAsync();
            Console.WriteLine("The output from thirdparty is: {0}", message);

            return RedirectToPage();
        }


        public async Task<IActionResult> OnGetIncreaseQuantity(string id)
        {
            var payload = new
            {
                productId = id,
                quantity = 1,
                cartActionType = 1
            };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5046/api/v1/Cart", content);

            HttpContent content1 = response.Content;
            string message = await content1.ReadAsStringAsync();
            Console.WriteLine("The output from thirdparty is: {0}", message);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetSubmit()
        {
            return RedirectToPage();
        }
    }
}
