using Client.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

IConfiguration configuration = builder.Configuration;
builder.Services.Configure<ISConfig>(configuration.GetSection("IS4Config"));
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = configuration["InteractiveIS4Config:Url"];
        options.ClientId = configuration["InteractiveIS4Config:ClientId"];
        options.ClientSecret = configuration["InteractiveIS4Config:ClientSecret"];

        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";

        options.Scope.Add(configuration["InteractiveIS4Config:Scopes:0"]);
        options.SaveTokens = true;
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
