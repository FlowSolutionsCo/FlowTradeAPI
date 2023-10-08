using Azure.Core.Extensions;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FlowTrade.Authentication.Repositories;
using FlowTrade.Authentication.Services;
using FlowTrade.Infrastructure.Data;
using FlowTrade.Infrastructure.Middleware;
using FlowTrade.Interfaces;
using FlowTrade.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var secretClient = new SecretClient(new Uri(configuration["AzureKeyVault:BaseUrl"]), new DefaultAzureCredential());

var secretKey = secretClient.GetSecret("FlowTrade-Jwt-SecretKey").Value.Value;
var issuer = secretClient.GetSecret("FlowTrade-Jwt-Issuer").Value.Value;
var audience = secretClient.GetSecret("FlowTrade-Jwt-Audience").Value.Value;

// Add services to the container.
builder.Services.AddSingleton(new SecretClient(new Uri("AzureKeyVault:BaseUrl"), new DefaultAzureCredential()));
builder.Services.AddSingleton(new AzureServiceTokenProvider());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddScoped<IProductionPossibilityRepository, ProductionPossibilityRepository>();
builder.Services.AddCustomDbContext(secretClient.GetSecret("FlowTrade-Database-ConnectionString").Value.Value);

// Add Identity services
builder.Services.AddIdentity<UserCompany, IdentityRole>(options =>
{
    // Configure password options
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseErrorHandlingMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();