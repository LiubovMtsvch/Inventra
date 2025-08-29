using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!result.Succeeded || result.Principal == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Безопасное извлечение данных из Claims
                var name = result.Principal.FindFirst("urn:github:login")?.Value ?? "Unknown";
                var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? "unknown@example.com";
                var userId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.NewGuid().ToString();

                // Сохраняем в сессию
                HttpContext.Session.SetString("CurrentUserName", name);
                HttpContext.Session.SetString("CurrentUserEmail", email);
                HttpContext.Session.SetString("CurrentUserId", userId);

                // Здесь можно добавить логику создания пользователя в БД, если нужно

                return RedirectToAction("Profile", "User");
            }
            catch (Exception ex)
            {
                var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                var logFile = Path.Combine(logPath, "github-errors.log");

                try
                {
                    if (!Directory.Exists(logPath))
                        Directory.CreateDirectory(logPath);

                    var errorMessage = $"[{DateTime.UtcNow}] Ошибка в GitHubResponse: {ex}\n";
                    await System.IO.File.AppendAllTextAsync(logFile, errorMessage);
                }
                catch
                {
                    // Если даже логирование не удалось — просто молчим
                }

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
