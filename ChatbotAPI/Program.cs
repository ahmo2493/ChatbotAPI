using ChatbotAPI.Services; // make sure this matches your namespace
using Microsoft.OpenApi.Models;
using ChatbotAPI.Data;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddControllers();

// ✅ OpenAI + your custom services
builder.Services.AddSingleton<OpenAIService>();

// ✅ Swagger config (with OpenAPI info)
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // exact frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ChatbotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
