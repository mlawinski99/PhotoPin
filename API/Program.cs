using API.Data;
using API.Data.CommentRepository;
using API.Data.FavouritePostsRepository;
using API.Data.PostRepo;
using API.Data.PostRepository;
using API.Data.UserRepo;
using API.Data.UserRepository;
using API.RabbitMQ;
using IdentityServer4.AccessTokenValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = "https://localhost:44397";
                   options.ApiName = "weatherapi";
                   options.ApiSecret = "ScopeSecret";
               });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddHostedService<EventBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IFavouritePostsRepository, FavouritePostsRepository>();


var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
