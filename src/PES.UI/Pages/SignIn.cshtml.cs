using Azure.Core;
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

            string data = await Login(email, password);
            var json = JObject.Parse(data);

            await SetAccessToken(json["token"]["accessToken"].ToString(), json["userId"].ToString());
            UserData.UserName = json["name"].ToString();
            return RedirectToPage("/Shop");
        }

        public async Task<IActionResult> OnPostRegister(string email, string password, string firstname, string lastname, string confirmPassword, string address)
        {

            string data = await Register(email, password,firstname, lastname, confirmPassword,address);
            var json = JObject.Parse(data);
            UserData.UserName = json["name"].ToString();
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
            var responseMessage = await httpClient.PostAsync("https://localhost:7187/api/v1/Auth/login", requestBody);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();

            return message;

        }

        public async Task<string> Register(string email, string password, string firstname, string lastname, string confirmPassword, string address)
        {
            var requestData = new
            {
                email = email,
                password = password,
                firstName = firstname,
                confirmPassword = confirmPassword,
                lastname = lastname,
                address = address
            };


            var json = JsonConvert.SerializeObject(requestData);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:7187/api/v1/Auth/register", requestBody);
            HttpContent content = responseMessage.Content;
            string message = await content.ReadAsStringAsync();

            return message;

        }

        public async ValueTask SetAccessToken(string accessToken,string userId)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
            };

            Console.Write(accessToken);
            HttpContext.Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
            HttpContext.Response.Cookies.Append("UserId", userId, cookieOptions);
            // Response.Cookies.Append("MyCookie", "value1");
        }

       


        public async ValueTask<string> GetAccesToken()
        {
            var cookieValue = Request.Cookies["AccessToken"];
            //Request.Cookies.TryGetValue("AccessToken", out var token);
            return cookieValue;
        }


    }
}
