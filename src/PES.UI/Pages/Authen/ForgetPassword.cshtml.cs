using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PES.UI.Pages.Authen
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ForgetPasswordModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSendEmail(string email)
        {
            TempDataHelper.Put(TempData, "emailResetPassword",email );
            var client = _httpClientFactory.CreateClient();
            var apiUrl = $"http://localhost:5046/api/v1/Auth/{email}/forgetpassword";

            try
            {
                var response = await client.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    // You can redirect to a confirmation page or show a success message here.
                    return RedirectToPage("/Authen/ChangePassword");
                }
                else
                {
                  
                    // Handle the error response here
                    ModelState.AddModelError(string.Empty, "Failed to send password reset email.");
                    return Page();
                }
            }
            catch (HttpRequestException e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while sending the password reset email.");
                return Page();
            }
            
        }
    }
}
