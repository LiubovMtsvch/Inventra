using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            //return Json(claims);
            return RedirectToAction("Profile", "User", new { area = "" });
        }
    }
}
