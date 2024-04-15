using api.v1.users.Services.Profile;
using api.v1.users.Services.User;
using api.v1.users.Services.Verification;

using component.v1.middlewares;

using db.v1.users.Repositories.User;
using db.v1.users.Repositories.Verification;
using db.v1.users.Contexts;
using db.v1.users.Contexts.Interfaces;

using helper.v1.configuration.Interfaces;
using helper.v1.configuration;
using helper.v1.jwt.Helper;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.regex;
using helper.v1.security.Helper;
using helper.v1.time;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;

using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;



#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/configurations/api.json", optional: false, reloadOnChange: true);

var cfg = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

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

builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(cfg["PostgreSQL:Users"]), ServiceLifetime.Transient);
builder.Services.AddTransient<IUserContext, UserContext>();
builder.Services.AddTransient<IVerificationContext, UserContext>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IVerificationRepository, VerificationRepository>();

builder.Services.AddSingleton<IJWTHelper, JWTHelper>();
builder.Services.AddSingleton<ISecurityHelper, SecurityHelper>();
builder.Services.AddSingleton<ITimeHelper, TimeHelper>();
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
builder.Services.AddTransient<IProfileService, ProfileService>();

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


