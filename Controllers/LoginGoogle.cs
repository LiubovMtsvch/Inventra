using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
namespace CourseProjectitr.Controllers
{
    
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");


        }
        [Route("Login/Login")]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });

            return new EmptyResult();

        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var name = result.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Name);
            var email = result.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);

            HttpContext.Session.SetString("CurrentUserName", name ?? email ?? "Unknown");

            //
            // var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(c => new { c.Type, c.Value });
            // return Json(claims);

            return RedirectToAction("Profile", "User", new { area = "" });
        }

    }
}
