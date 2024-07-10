using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PES.Domain.DTOs.User;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace PES.UI.Pages.User
{
    public class UpdateProfileModel : PageModel
    {

        static HttpClient _httpClient = new HttpClient();
        [BindProperty]
        public UserProfile profile {  get; set; }
        public async  Task OnGet()
        {
           await  GetProfile();
        }

        public async Task GetProfile()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7187/api/v1/Auth/profile");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["AccessToken"]);


            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(message);
                var userProfile = new UserProfile
                (responseObject["fullName"].ToString(), responseObject["email"].ToString(), responseObject["phoneNumber"].ToString(), responseObject["address"].ToString());
                profile = userProfile;
            }
            else
            {
                Console.WriteLine($"Error fetching profile data: {response.StatusCode}");
            }
        }
  
    
        public async Task<IActionResult> OnPostUpdateProfile(string address, string email, string phone)
        {
            var payload = new
            {
                address = address,
                email = email,
                phone = phone
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7187/api/v1/Auth/update-profile");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["AccessToken"]);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error: {0}", errorMessage);
                return RedirectToPage();
            }

            string message = await response.Content.ReadAsStringAsync();
            Console.WriteLine("The output from thirdparty is: {0}", message);

            return RedirectToPage();
        }


    }
}
