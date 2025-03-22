using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using USPEducation.Data;
using USPEducation.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Load Configuration (appsettings.json)
var configuration = builder.Configuration;

// 🔹 Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🔹 Configure Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

// 🔹 Register API Service for External Student System Integration
builder.Services.AddHttpClient<IStudentGradeService, StudentGradeService>(client =>
{
    var apiBaseUrl = configuration["ExternalAPIs:StudentSystemBaseUrl"];
    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new InvalidOperationException("API Base URL is not set in appsettings.json.");
    }
    client.BaseAddress = new Uri(apiBaseUrl);
});

// 🔹 Add Controllers and Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// 🔹 Enable Swagger for API testing (Development mode)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Enable HTTPS Redirection
app.UseHttpsRedirection();

// 🔹 Enable CORS
app.UseCors();

// 🔹 Enable Authorization Middleware
app.UseAuthorization();

// 🔹 Map Controllers
app.MapControllers();

// 🔹 Run Application
app.Run();
