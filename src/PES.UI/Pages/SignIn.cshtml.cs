using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PES.UI.Pages.Shared;
using System.Net.Http;
using System.Text;

namespace PES.UI.Pages
{
    public class SignInModel : PageModel
    {
        static HttpClient httpClient = new HttpClient();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogin(string email, string password)
        {
          
            string data =  await Login(email, password);
            var json = JObject.Parse(data);
            UserData.AccessToken = json["token"]["accessToken"].ToString();

            UserData.UserName = json["name"].ToString();
            Console.WriteLine(UserData.AccessToken);
            return RedirectToPage("/Shop");
        }

        public async Task<IActionResult> OnPostRegister(string email, string password, string name, string confirmPassword)
        {
            await Register(email, password, name, confirmPassword);
            return RedirectToPage("/Shop");
        }

        public async Task<string> Login(string email, string password)
        {
            var requestData = new
            {
                email = email,
                password = password
            };

            var json = JsonConvert.SerializeObject(requestData);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("http://localhost:5046/api/v1/Auth/login", requestBody);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();

            return message;

        }

        public async Task<string> Register(string email, string password, string name, string confirmPassword)
        {
            var requestData = new
            {
                email = email,
                password = password,
                name = name,
                confirmPassword = confirmPassword
            };

            var json = JsonConvert.SerializeObject(requestData);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("http://localhost:5046/api/v1/Auth/register", requestBody);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();

            return message;

        }
    }
}
