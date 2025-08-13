using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectitr.Controllers
{
    public class LoginGitHubController : Controller
    {
        [Route("LoginGitHub/signin")]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.ChallengeAsync("GitHub", new AuthenticationProperties
            {
                RedirectUri = Url.Action("GitHubResponse")
            });

            return new EmptyResult();
        }

        public async Task<IActionResult> GitHubResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return Redirect("/index.html");
            }

            var name = result.Principal?.Identities?.FirstOrDefault()?.FindFirst("urn:github:login")?.Value;
            HttpContext.Session.SetString("GitHubUser", name ?? "Unknown");

            return RedirectToAction("Profile", "User");
        }
    }
}
