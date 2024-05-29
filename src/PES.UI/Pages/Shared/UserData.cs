namespace PES.UI.Pages.Shared
{
    public static class UserData
    {
        static string UserName { get; set; } = "NotLogin";
        static string RefreshToken { get; set; } = "Empty";
        static string AccessToken { get; set; } = "Empty"; 

        static async Task SetLogin(string userName)
        {
             UserName = userName;
        }

        static async Task SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
