using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using System.Net.Http;
using PES.Domain.DTOs.Category;
using System.Text;
using PES.UI.Pages.Shared;

namespace PES.UI.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public ProductDetailModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        public Guid? ProductId { get; set; }
        [BindProperty]
        public NutrionInfo NutrionInfo { get; set; }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; }
        [BindProperty]
        public ImportantInfo importantInfo { get; set; }

        [BindProperty]
        public List<ProductImageResponse> productImages { get; set; }


        static HttpClient httpClient = new HttpClient();
        public async Task<IActionResult> OnGet(string id)
        {
            string url = $"http://localhost:5046/api/v1/Product/{id}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
                HttpContent content = responseMessage.Content;
                string message = await content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(message);

                JToken nutrionObject = responseObject["nutrionInfo"];
                JToken imporatantObject = responseObject["importantInfo"];
                JToken categoryObject = responseObject["productCategory"];
                JArray productIma = responseObject["productImages"];

                ProductName = responseObject["productName"].ToString();
                id = responseObject["id"];
                ProductId = Guid.Parse(id);
                Price = responseObject["price"];
                NutrionInfo = nutrionObject.ToObject<NutrionInfo>();
                ProductCategory = categoryObject.ToObject<ProductCategory>();
                importantInfo = imporatantObject.ToObject<ImportantInfo>();
                productImages = productIma.Select(item => item.ToObject<ProductImageResponse>()).ToList();

                RedirectToPage();
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("An HTTP request exception occurred. {0}", exception.Message);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRating(string description, int rating, Guid productId)
        {
            var apiUrl = $"http://localhost:5046/api/v1/Product/{productId}/rate"; // Replace with your API endpoint
            var token = UserData.AccessToken; // Replace with your authorization token

            // Create the request payload
            var requestData = new
            {
                rating = rating,
                comment = description
            };

            // Serialize the request data
            var jsonRequest = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Create HttpClient instance
            var client = _clientFactory.CreateClient();

            // Add authorization header
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Send POST request to API
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Handle success response
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse);
                return RedirectToPage("SuccessPage"); // Redirect to success page
            }
            else
            {
                // Handle error response
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorMessage);
                return Page(); // Stay on the same page
            }


        }
    }
}
