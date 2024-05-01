using FAMS_GROUP2.API;
using FAMS_GROUP2.API.Middlewares;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FAMS Web API", Version = "v.1.0" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.IgnoreNullValues = true;
        });

builder.Services.AddScoped<AccountStatusMiddleware>();

// db local

// builder.Services.AddDbContext<FamsDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
// });

// builder.Services.AddControllers()
//         .AddJsonOptions(options =>
//         {
//             options.JsonSerializerOptions.IgnoreNullValues = true;
//         });




// ===================== FOR DEPLOY AZURE =======================

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
  builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
  connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
  connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<FamsDbContext>(options =>
  options.UseSqlServer(connection));

// ==================== NO EDIT OR REMOVE COMMENT =======================

builder.Services.AddCors(options =>
{
    options.AddPolicy("app-cors",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination")
            .AllowAnyMethod();
        });
});

builder.Services.AddWebAPIService();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FAMS API v.01");
});

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();

app.UseHttpsRedirection();

app.UseCors("app-cors");

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<AccountStatusMiddleware>();

app.MapControllers();

app.Run();
