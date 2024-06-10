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
        static SignInModel _signInModel = new SignInModel();
        [BindProperty]
        public List<CartItem> Items { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Items = TempDataHelper.Get<List<CartItem>>(TempData, "cart") as List<CartItem>;
            return Page();

        }

        public async Task<IActionResult> OnGetProceedToCheckout()
        {

            string testCase = "https://localhost:7187/api/v1/Cart";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7187/api/v1/Cart");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"Bearer {Request.Cookies["AccessToken"]}");

                HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
                HttpContent contentHihi = responseMessage.Content;
                string message = await contentHihi.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(message);
                JArray items = responseObject["items"];
                Items = items.Select(item => item.ToObject<CartItem>()).ToList();
                //?
                List<OrderDetailRequest> data = Items.Select(x => new OrderDetailRequest { Price = x.Price, ProductId = x.Id, Quantity = x.Quantity }).ToList();
                var payload = new
                {
                    orderDetails = data
                };
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
               var token =   $"Bearer {Request.Cookies["AccessToken"]}";
                // Add Bearer token to the request
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

                var response = await httpClient.PostAsync("https://localhost:7187/api/v1/Order", content);

                HttpContent content1 = response.Content;
                string messageKhoNoi = await content1.ReadAsStringAsync();
                Console.WriteLine("The output from thirdparty is: {0}", messageKhoNoi);
                return RedirectToPage("HomePage");
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred. {0}", exception.Message);
            }

            return RedirectToPage("HomePage");
        }


        }


    }
