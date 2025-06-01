using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using USPFinance.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Load Configuration (appsettings.json)
var configuration = builder.Configuration;

// 🔹 Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7235", "http://localhost:5291")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// 🔹 Configure Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

// 🔹 Add Controllers and Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// 🔹 Configure URL and Port
app.Urls.Clear();
app.Urls.Add("http://localhost:5291");
app.Urls.Add("https://localhost:7235");

// 🔹 Run Application
app.Run();
