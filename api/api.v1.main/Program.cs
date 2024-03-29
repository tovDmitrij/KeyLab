using System.Text;
using System.Globalization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;

using MassTransit;

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
using component.v1.middlewares;
using db.v1.main.Repositories.Kit;
using api.v1.main.Services.Kit;
using db.v1.main.Repositories.Keycap;
using api.v1.main.Services.Keycap;



#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/configurations/api.json", optional: false, reloadOnChange: true);

var cfg = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = cfg["Redis:Main"];
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
    options.FormFieldName = "KeylabAntiforgery";
    options.HeaderName = "X-CSRF-TOKEN-KEYLAB";
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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
        name: "PublicPolicy",
        policy => policy.SetIsOriginAllowed(origin => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

builder.Services.AddDbContext<MainContext>(options => options.UseNpgsql(cfg["PostgreSQL:Main"]), ServiceLifetime.Transient);
builder.Services.AddTransient<IUserContext, MainContext>();
builder.Services.AddTransient<IVerificationContext, MainContext>();
builder.Services.AddTransient<IKeyboardContext, MainContext>();
builder.Services.AddTransient<ISwitchContext, MainContext>();
builder.Services.AddTransient<IBoxContext, MainContext>();
builder.Services.AddTransient<IKitContext, MainContext>();
builder.Services.AddTransient<IKeycapContext, MainContext>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IVerificationRepository, VerificationRepository>();
builder.Services.AddTransient<IKeyboardRepository, KeyboardRepository>();
builder.Services.AddTransient<ISwitchRepository, SwitchRepository>();
builder.Services.AddTransient<IBoxRepository, BoxRepository>();
builder.Services.AddTransient<IKitRepository, KitRepository>();
builder.Services.AddTransient<IKeycapRepository, KeycapRepository>();

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
builder.Services.AddSingleton<IActivityConfigurationHelper, ConfigurationHelper>();

builder.Services.AddSingleton<IUserRegexHelper, RegexHelper>();
builder.Services.AddSingleton<IVerificationRegexHelper, RegexHelper>();
builder.Services.AddSingleton<IKeyboardRegexHelper, RegexHelper>();
builder.Services.AddSingleton<IBoxRegexHelper, RegexHelper>();
builder.Services.AddSingleton<IKitRegexHelper, RegexHelper>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IVerificationService, VerificationService>();
builder.Services.AddTransient<IKeyboardService, KeyboardService>();
builder.Services.AddTransient<ISwitchService, SwitchService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<IBoxService, BoxService>();
builder.Services.AddTransient<IKitService, KitService>();
builder.Services.AddTransient<IKeycapService, KeycapService>();

#endregion



#region App

var app = builder.Build();
app.UseCors("PublicPolicy");
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<StatisticMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion

