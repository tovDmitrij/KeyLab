using api.v1.main.Middlewares;
using api.v1.main.Services.Verification;
using api.v1.main.Services.User;

using db.v1.main.Contexts;
using db.v1.main.Contexts.Interfaces;
using db.v1.main.Repositories.Verification;
using db.v1.main.Repositories.User;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using service.v1.configuration;
using service.v1.configuration.Interfaces;
using service.v1.email;
using service.v1.jwt.Service;
using service.v1.security.Service;
using service.v1.time;
using service.v1.validation;
using service.v1.validation.Interfaces;
using System.Text;
using service.v1.file.File;
using db.v1.main.Repositories.Keyboard;
using api.v1.main.Services.Keyboard;
using service.v1.cache;



#region Builder

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();

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
    builder.Services.AddSingleton<IEmailConfigurationService, ConfigurationService>();
    builder.Services.AddSingleton<IJWTConfigurationService, ConfigurationService>();
    builder.Services.AddSingleton<IMinioConfigurationService, ConfigurationService>();
    builder.Services.AddSingleton<IFileConfigurationService, ConfigurationService>();
    builder.Services.AddSingleton<IKeyboardConfigurationService, ConfigurationService>();
    builder.Services.AddSingleton<IKeyboardCacheConfigurationService, ConfigurationService>();

    builder.Services.AddSingleton<IKeyboardCacheService, MemoryCacheService>();

    builder.Services.AddSingleton<IUserValidationService, ValidationService>();
    builder.Services.AddSingleton<IVerificationValidationService, ValidationService>();
    builder.Services.AddSingleton<IKeyboardValidationService, ValidationService>();

    builder.Services.AddSingleton<IUserService, UserService>();
    builder.Services.AddSingleton<IVerificationService, VerificationService>();
    builder.Services.AddSingleton<IKeyboardService, KeyboardService>();

    builder.Services.AddSingleton<IEmailService, EmailService>();
    builder.Services.AddSingleton<IJWTService, JWTService>();
    builder.Services.AddSingleton<IFileService, FileService>();
    builder.Services.AddSingleton<ISecurityService, SecurityService>();
    builder.Services.AddSingleton<ITimeService, TimeService>();
}

void InitRepositories()
{
    builder.Services.AddSingleton<IUserRepository, UserRepository>();
    builder.Services.AddSingleton<IVerificationRepository, VerificationRepository>();
    builder.Services.AddSingleton<IKeyboardRepository, KeyboardRepository>();
}

void InitContexts()
{
    builder.Services.AddDbContext<MainContext>(options => options.UseNpgsql(cfg.GetConnectionString("main")), ServiceLifetime.Singleton);
    builder.Services.AddSingleton<IUserContext, MainContext>();
    builder.Services.AddSingleton<IVerificationContext, MainContext>();
    builder.Services.AddSingleton<IKeyboardContext, MainContext>();
}

#endregion



#region App

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseCors("PublicPolicy");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();

#endregion

