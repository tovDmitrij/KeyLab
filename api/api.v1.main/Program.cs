using api.v1.main.Middlewares;
using api.v1.main.Services.Confirm;
using api.v1.main.Services.User;

using db.v1.main.Contexts;
using db.v1.main.Contexts.Interfaces;
using db.v1.main.Repositories.Confirm;
using db.v1.main.Repositories.User;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using service.v1.configuration;
using service.v1.email;
using service.v1.jwt.Service;
using service.v1.minio;
using service.v1.security.Service;
using service.v1.timestamp;
using service.v1.validation;

using System.Text;



#region Builder

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services.AddAntiforgery(options =>
{
    options.FormFieldName = "KeyboardAntiforgery";
    options.HeaderName = "X-CSRF-TOKEN-KEYBOARD";
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = cfg["JWT:Issuer"],
        ValidateIssuer = true,

        ValidAudience = cfg["JWT:Audience"],
        ValidateAudience = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["JWT:SecretKey"]!)),
        ValidateIssuerSigningKey = true,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "ProtectedPolicy",
        policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("some"));
    options.AddPolicy(
        name: "PublicPolicy",
        policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true));
});

InitServices();
InitRepositories();
InitContexts();

void InitServices()
{
    builder.Services.AddSingleton<IUserService, UserService>();
    builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
    builder.Services.AddSingleton<IEmailService, EmailService>();
    builder.Services.AddSingleton<IJWTService, JWTService>();
    builder.Services.AddSingleton<ISecurityService, SecurityService>();
    builder.Services.AddSingleton<ITimestampService, TimestampService>();
    builder.Services.AddSingleton<IValidationService, ValidationService>();
    builder.Services.AddSingleton<IMinioService, MinioService>();
    builder.Services.AddSingleton<IConfirmService, ConfirmService>();
}

void InitRepositories()
{
    builder.Services.AddSingleton<IUserRepository, UserRepository>();
    builder.Services.AddSingleton<IConfirmRepository, ConfirmRepository>();
}

void InitContexts()
{
    builder.Services.AddDbContext<MainContext>(options => options.UseNpgsql(cfg.GetConnectionString("main_local")), ServiceLifetime.Singleton);
    builder.Services.AddSingleton<IUserContext, MainContext>();
    builder.Services.AddSingleton<IConfirmContext, MainContext>();
}

#endregion



#region App

var app = builder.Build();
app.UseCors("PublicPolicy");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();

#endregion

