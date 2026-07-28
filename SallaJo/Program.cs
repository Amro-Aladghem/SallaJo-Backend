using Application.Common.Models;
using Application.Services;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SallaJo.Extentions;
using SallaJo.MiddleWares;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var jwtOption = builder.Configuration.GetSection("JWT").Get<JwtOption>();

builder.Services.AddSingleton(jwtOption);

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOption!.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOption!.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SigningKey))
        };
    });

builder.Services.AddValidatorsFromAssembly(typeof(Application.Validators.PersonAuthValidator).Assembly);

builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<BlobStorageUploadService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ImageProductService>();
builder.Services.AddScoped<DiscountService>();
builder.Services.AddScoped<OfferService>();
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<OfferProductService>();
builder.Services.AddScoped<ActivationCodeService>();
builder.Services.AddScoped<StoreDeliveryService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["SQL_CONNECTION_STRING"]);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PersonRole", policy =>
    {
        policy.RequireRole("person", "Person");
    });

    options.AddPolicy("SellerRole", policy =>
    {
        policy.RequireRole("seller", "Seller");
    });
});


builder.Services.SetRateLimiters();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
           policy =>
           {
               policy.WithOrigins("http://localhost:5173")
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials();
           });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseMiddleware<GlobalExeptionMidlleWare>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMiddleware<JwtFromCookieMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
