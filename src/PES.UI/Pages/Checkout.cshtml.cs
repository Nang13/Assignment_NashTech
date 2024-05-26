using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.Domain.DTOs.Cart;
using PES.Domain.DTOs.Order;
using PES.Domain.Entities.Model;
using System.Net.Http;
using System.Text;

namespace PES.UI.Pages
{
    public class CheckoutModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        [BindProperty]
        public List<CartItem> Items { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Items = TempDataHelper.Get<List<CartItem>>(TempData, "cart") as List<CartItem>;
            return Page();

        }

        public async Task<IActionResult> OnGetProceedToCheckout()
        {

            string testCase = "http://localhost:5046/api/v1/Cart";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(testCase);
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
                var response = await httpClient.PostAsync("http://localhost:5046/api/v1/Order", content);

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