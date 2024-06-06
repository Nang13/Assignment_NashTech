using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PES.Domain.DTOs.Cart;
using static System.Net.WebRequestMethods;
using System.Text;
using System.Text.Json;

namespace PES.UI.Pages.Authen
{

    public class ChangePasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChangePasswordModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostResetPassword(int otp,string password,string confirmpassword)
        {
            string email = TempDataHelper.Get<string>(TempData, "emailResetPassword") as string;
            var client = _httpClientFactory.CreateClient();
            var apiUrl = "http://localhost:5046/api/v1/Auth/${email}/changepassword";

            var payload = new
            {
                password = password,
                confirmPassword = confirmpassword ,
                otp = otp
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // You can redirect to a confirmation page or show a success message here.
                    return RedirectToPage("/Login");
                }
                else
                {
                    // Handle the error response here
                    ModelState.AddModelError(string.Empty, "Failed to change password.");
                    return Page();
                }
            }
            catch (HttpRequestException e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while changing the password.");
                return Page();
            }
            return RedirectToPage();
        }
    }
}
