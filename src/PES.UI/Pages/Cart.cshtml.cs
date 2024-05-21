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

namespace PES.UI.Pages
{
    public class CartModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        [BindProperty]
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
        public async Task OnGetAsync()
        {

            string testCase = "http://localhost:5046/api/v1/Cart";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(testCase);
                HttpContent content = responseMessage.Content;
                string message = await content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(message);
                JArray items = responseObject["items"];
                CartItems = items.Select(item => item.ToObject<CartItem>()).ToList();
                Total = responseObject["totalPrice"];
                TempDataHelper.Put(TempData, "cart", CartItems);
                //TempData["cart"] = CartItems;


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
            var response = await httpClient.PostAsync("http://localhost:5046/api/v1/Cart", content);

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
            var response = await httpClient.PostAsync("http://localhost:5046/api/v1/Cart", content);

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
