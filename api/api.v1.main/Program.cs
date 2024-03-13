using System.Text;
using System.Globalization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;

using MassTransit;

using api.v1.main.Middlewares;
using api.v1.main.Services.Verification;
using api.v1.main.Services.User;
using api.v1.main.Services.Keyboard;
using api.v1.main.Services.Switch;
using api.v1.main.Services.Profile;
using api.v1.main.Services.Box;

using db.v1.main.Contexts;
using db.v1.main.Contexts.Interfaces;
using db.v1.main.Repositories.Verification;
using db.v1.main.Repositories.User;
using db.v1.main.Repositories.Keyboard;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.Switch;

using helper.v1.jwt.Helper;
using helper.v1.security.Helper;
using helper.v1.time;
using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.configuration;
using helper.v1.regex.Interfaces;
using helper.v1.regex;
using helper.v1.localization.Helper;
using helper.v1.file;
using helper.v1.messageBroker;
using helper.v1.cache.Implements;



#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/configurations/db.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("/configurations/file.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("/configurations/redis.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("/configurations/rabbitmq.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("/configurations/jwt.json", optional: false, reloadOnChange: true);

var cfg = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = cfg["RedisMain:Configuration"];
});

var rabbitHost = cfg["RabbitMQ:Host"];
var rabbitUsername = cfg["RabbitMQ:Username"];
var rabbitPassword = cfg["RabbitMQ:Password"];
builder.Services.AddMassTransit(options =>
{
    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, host =>
        {
            host.Username(rabbitUsername);
            host.Password(rabbitPassword);
        });
    });
});

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

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new List<CultureInfo>
    {
        new("ru-RU")
    };
    options.DefaultRequestCulture = new RequestCulture("ru-RU", "ru-RU");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
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

InitContexts();
InitRepositories();
InitHelpers();
InitServices();

void InitContexts()
{
    builder.Services.AddDbContext<MainContext>(options => options.UseNpgsql(cfg["PostgreSQL:Main"]), ServiceLifetime.Scoped);
    builder.Services.AddScoped<IUserContext, MainContext>();
    builder.Services.AddScoped<IVerificationContext, MainContext>();
    builder.Services.AddScoped<IKeyboardContext, MainContext>();
    builder.Services.AddScoped<ISwitchContext, MainContext>();
    builder.Services.AddScoped<IBoxContext, MainContext>();
}

void InitRepositories()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();
    builder.Services.AddScoped<IKeyboardRepository, KeyboardRepository>();
    builder.Services.AddScoped<ISwitchRepository, SwitchRepository>();
    builder.Services.AddScoped<IBoxRepository, BoxRepository>();
}

void InitHelpers()
{
    builder.Services.AddSingleton<IJWTHelper, JWTHelper>();
    builder.Services.AddSingleton<IFileHelper, FileHelper>();
    builder.Services.AddSingleton<ISecurityHelper, SecurityHelper>();
    builder.Services.AddSingleton<ITimeHelper, TimeHelper>();
    builder.Services.AddSingleton<ICacheHelper, RedisCacheHelper>();
    builder.Services.AddSingleton<ILocalizationHelper, LocalizationHelper>();
    builder.Services.AddSingleton<IMessageBrokerHelper, RabbitMQHelper>();

    builder.Services.AddSingleton<IJWTConfigurationHelper, ConfigurationHelper>();
    builder.Services.AddSingleton<IFileConfigurationHelper, ConfigurationHelper>();
    builder.Services.AddSingleton<ICacheConfigurationHelper, ConfigurationHelper>();

    builder.Services.AddSingleton<IUserRegexHelper, RegexHelper>();
    builder.Services.AddSingleton<IVerificationRegexHelper, RegexHelper>();
    builder.Services.AddSingleton<IKeyboardRegexHelper, RegexHelper>();
    builder.Services.AddSingleton<IBoxRegexHelper, RegexHelper>();
}

void InitServices()
{
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IVerificationService, VerificationService>();
    builder.Services.AddScoped<IKeyboardService, KeyboardService>();
    builder.Services.AddScoped<ISwitchService, SwitchService>();
    builder.Services.AddScoped<IProfileService, ProfileService>();
    builder.Services.AddScoped<IBoxService, BoxService>();
}

#endregion



#region App

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseCors("PublicPolicy");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();

#endregion

