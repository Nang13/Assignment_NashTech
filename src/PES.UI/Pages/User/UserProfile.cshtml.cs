using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.Domain.DTOs.User;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PES.UI.Pages.User
{
    public class UserProfileModel : PageModel
    {
        [BindProperty]
        public UserProfile profile { get; set; }

        static HttpClient _httpClient = new HttpClient();
        public async Task OnGet()
        {
            await GetProfile();
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

    }
}
