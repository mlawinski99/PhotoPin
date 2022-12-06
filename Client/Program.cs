using Client.Handlers;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//IConfiguration configuration = builder.Configuration;
//builder.Services.Configure<ISConfig>(configuration.GetSection("IS4Config"));
//builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<BearerTokenHandler>();

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7080/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<BearerTokenHandler>();
// create an HttpClient used for accessing the IDP
builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:44397/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
          .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
          {
              options.AccessDeniedPath = "/Authorization/AccessDenied";
          })
          .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
          {
              options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              options.Authority = "https://localhost:44397/";
              options.ClientId = "photoClient";
              options.ResponseType = "code";
              options.Scope.Add("weatherapi.read");
              options.Scope.Add("offline_access");
              options.ClaimActions.DeleteClaim("sid");
              options.ClaimActions.DeleteClaim("idp");
              options.ClaimActions.DeleteClaim("s_hash");
              options.ClaimActions.DeleteClaim("auth_time");
              options.ClaimActions.MapUniqueJsonKey("role", "role");
              options.SaveTokens = true;
              options.ClientSecret = "SuperSecretPassword";
              options.GetClaimsFromUserInfoEndpoint = true;
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  NameClaimType = JwtClaimTypes.GivenName,
                  RoleClaimType = JwtClaimTypes.Role
              };
          });
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(async context => {
	if (context.HttpContext.Response.StatusCode == 404)
	{
		 context.HttpContext.Response.Redirect("https://localhost:7166/Home/ErrorPage");
	}
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
