using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
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
            // Your login logic here
            //if (IsValidUser(email, password))
            //{
            //    // Redirect the user after successful login
            //    return RedirectToPage("/Dashboard");
            //}
            //else
            //{
            //    // Handle invalid login
            //    ModelState.AddModelError(string.Empty, "Invalid email or password");
            //    return Page();
            //}
            await Login(email, password);
            return Page();
        }

        public async Task<IActionResult> OnPostRegister(string email, string password, string name, string confirmPassword)
        {
            await Register(email, password, name, confirmPassword);
            return Page();
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
