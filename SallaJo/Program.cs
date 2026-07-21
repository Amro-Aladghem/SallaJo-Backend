using Application.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SallaJo.MiddleWares;
using System.Text;
using FluentValidation;
using System.Reflection;
using Application.Services;
using Infrastructure.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Infrastructure.ExternalServices;


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
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<BlobStorageUploadService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ImageProductService>();
builder.Services.AddScoped<DiscountService>();

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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<GlobalExeptionMidlleWare>();

app.UseHttpsRedirection();

app.UseMiddleware<JwtFromCookieMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
