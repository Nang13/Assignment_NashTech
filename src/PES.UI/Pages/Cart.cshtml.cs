using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using System.Net.Http;
using PES.Domain.DTOs.Cart;
using PES.Domain.Enum;
using System.Text;
using PES.UI.Pages.Shared;
using System.Net.Http.Headers;
using System.Net;
using Azure.Core;

namespace PES.UI.Pages
{
    public class CartModel : PageModel
    {
        static HttpClient _httpClient = new HttpClient();
        static readonly SignInModel _signIn = new SignInModel();
        private readonly IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        [BindProperty]
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; } = 0;
        public async Task OnGetAsync()
        {


            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                var accessToken = httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7187/api/v1/Cart");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                Request.Cookies.TryGetValue("AccessToken", out string token1);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{Request.Cookies["AccessToken"]}"); // Use the actual access token directly

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(message);
                    JArray items = responseObject.items;
                    CartItems = items.Select(item => item.ToObject<CartItem>()).ToList();
                    Total = responseObject.totalPrice;
                    TempDataHelper.Put(TempData, "totalCart", Total.ToString());
                    TempDataHelper.Put(TempData, "cart", CartItems);
                }
                else
                {
                    Console.WriteLine($"Error fetching cart data: {response.StatusCode}");
                }

                Page();
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred" +
                    ". {0}", exception.Message);
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

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7187/api/v1/Cart");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"Bearer {Request.Cookies["AccessToken"]}");

            var response = await _httpClient.SendAsync(request);

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

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7187/api/v1/Cart");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"Bearer {Request.Cookies["AccessToken"]}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error: {0}", errorMessage);
                ModelState.AddModelError(string.Empty, "Failed to change password.");
                return RedirectToPage(); // or handle the error appropriately
            }

            string message = await response.Content.ReadAsStringAsync();
            Console.WriteLine("The output from thirdparty is: {0}", message);
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteProduct(string id)
        {
            var payload = new
            {
                productId = id,
                quantity = 1,
                cartActionType = 3
            };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7187/api/v1/Cart");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"Bearer {Request.Cookies["AccessToken"]}");

            var response = await _httpClient.SendAsync(request);

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
        public async Task<IActionResult> OnGetSubmit()
        {
            return RedirectToPage();
        }
    }
}
