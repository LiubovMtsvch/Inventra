using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using CourseProjectitr.Data;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// ✅ Защита данных — ключи сохраняются между перезапусками
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys")))
    .SetApplicationName("CourseProjectitr");

// ✅ Кэш для сессий
builder.Services.AddDistributedMemoryCache();

// ✅ Сессии
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".CourseProjectitr.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// ✅ Аутентификация
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
})
.AddOAuth("GitHub", options =>
{
    options.ClientId = builder.Configuration["GitHub:ClientId"];
    options.ClientSecret = builder.Configuration["GitHub:ClientSecret"];
    options.CallbackPath = new PathString("/signin-github");

    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    options.TokenEndpoint = "https://github.com/login/oauth/access_token";
    options.UserInformationEndpoint = "https://api.github.com/user";
    options.Scope.Add("user:email");

    options.SaveTokens = true;

    options.ClaimActions.MapJsonKey("urn:github:login", "login");
    options.ClaimActions.MapJsonKey("urn:github:name", "name");
    options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

    options.Events.OnCreatingTicket = async context =>
    {
        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
        request.Headers.Add("User-Agent", "CourseProjectitr");

        var response = await context.Backchannel.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var githubId = user.RootElement.GetProperty("id").GetInt64().ToString();
        var login = user.RootElement.GetProperty("login").GetString();
        var name = user.RootElement.TryGetProperty("name", out var nameProp)
            ? nameProp.GetString()
            : login;

        context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, githubId));
        context.Identity.AddClaim(new Claim("urn:github:login", login));
        context.Identity.AddClaim(new Claim(ClaimTypes.Name, name));

        if (user.RootElement.TryGetProperty("email", out var emailProp))
        {
            context.Identity.AddClaim(new Claim(ClaimTypes.Email, emailProp.GetString() ?? ""));
        }
        else
        {
            context.Identity.AddClaim(new Claim(ClaimTypes.Email, "no-email-provided@github.com"));
        }
    };

    // 🔁 Обработка ошибки авторизации
    options.Events.OnRemoteFailure = context =>
    {
        context.Response.Redirect("/User/Profile?error=auth_failed");

        context.HandleResponse(); // предотвращает стандартный редирект
        return Task.CompletedTask;
    };
});


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Проверка подключения и применение миграций
    if (!context.Database.CanConnect())
    {
        context.Database.Migrate();
    }

    // Заполнение начальными категориями
    DbInitializer.SeedCategories(context);
}

//// ✅ Middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//        options.RoutePrefix = "swagger";
//    });
//}

app.UseStaticFiles();


app.UseSession();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,

});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
