using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.Domain.DTOs.Cart;
using PES.Domain.DTOs.OrderDTO;
using PES.Domain.Entities.Model;
using PES.UI.Pages.Shared;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PES.UI.Pages
{
    public class CheckoutModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        private const string BaseUrl = "https://localhost:7187/api/v1";
        static SignInModel _signInModel = new SignInModel();
        private readonly IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        [BindProperty]
        public List<CartItem> Items { get; set; }
        public string Total { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Items = TempDataHelper.Get<List<CartItem>>(TempData, "cart") as List<CartItem>;
            Total = TempDataHelper.Get<string>(TempData, "totalCart");
            return Page();

        }

        public async Task<IActionResult> OnGetProceedToCheckout()
        {

            try
            {
                var accessToken = httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(accessToken))
                {
                    return new UnauthorizedResult(); // Handle unauthorized case
                }

                // Set up the request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Fetch cart data
                var cartResponse = await httpClient.GetAsync($"{BaseUrl}/Cart");
                if (!cartResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error fetching cart: {cartResponse.ReasonPhrase}");
                }

                var cartContent = await cartResponse.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(cartContent);
                JArray itemsArray = responseObject?["items"];


                if (itemsArray == null)
                {
                    throw new InvalidOperationException("Cart data is missing or invalid.");
                }

                var items = itemsArray.Select(item => item.ToObject<CartItem>()).ToList();

                // Prepare order details payload
                var orderDetails = items.Select(x => new OrderDetailRequest
                {
                    Price = x.Price,
                    ProductId = x.Id,
                    Quantity = x.Quantity
                }).ToList();

                var payload = new { orderDetails };
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                if (string.IsNullOrEmpty(accessToken))
                {
                    return new UnauthorizedResult(); // Handle unauthorized case
                }

                // Create the HttpRequestMessage with Bearer token
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/Order")
                {
                    Content = content
                };

                // Add Authorization header with Bearer token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Send order request
                var orderResponse = await httpClient.PostAsync($"{BaseUrl}/Order", content);
                orderResponse.EnsureSuccessStatusCode();

                var orderResult = await orderResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Order Response: {orderResult}");

                return RedirectToPage("HomePage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }


}


