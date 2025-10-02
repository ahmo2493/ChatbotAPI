using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChatbotAPI.Services; // your service
using Microsoft.OpenApi.Models;
using ChatbotAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// 🔐 Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.Cookie.Name = "auth";
        opt.Cookie.SameSite = SameSiteMode.None;      // cross-site for Vite dev
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        opt.SlidingExpiration = true;
    });

// CORS for Vite (http + https) and allow credentials
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Your services
builder.Services.AddSingleton<OpenAIService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Chatbot API",
        Version = "v1",
        Description = "API for custom business chatbot with OpenAI integration"
    });
});

builder.Services.AddDbContext<ChatbotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // ⬅️ must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
