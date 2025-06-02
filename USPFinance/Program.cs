using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using USPFinance.Data;
using USPFinance.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Load Configuration (appsettings.json)
var configuration = builder.Configuration;

// 🔹 Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
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

// 🔹 Add HttpClient for StudentFinanceUpdateService
builder.Services.AddHttpClient<StudentFinanceUpdateService>();

// 🔹 Register StudentFinanceUpdateService
builder.Services.AddScoped<StudentFinanceUpdateService>();

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
app.UseCors("AllowAll");

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
