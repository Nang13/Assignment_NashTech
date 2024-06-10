using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PES.UI.Pages.Shared;

namespace PES.UI.Pages.Authen
{
    public class SignOutModel : PageModel
    {
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnGetLogout()
        {
            Response.Cookies.Delete("AccessToken");
            UserData.UserName = "NotLogin";
            return RedirectToPage("/SignIn");
        }
    }
}
