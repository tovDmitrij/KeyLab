using System.Text;
using System.Globalization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;

using MassTransit;

using api.v1.keyboards.Services.Keyboard;
using api.v1.keyboards.Services.Switch;
using api.v1.keyboards.Services.Box;
using api.v1.keyboards.Services.Kit;
using api.v1.keyboards.Services.Keycap;
using api.v1.keyboards.Consumers;

using db.v1.keyboards.Contexts;
using db.v1.keyboards.Contexts.Interfaces;
using db.v1.keyboards.Repositories.Keyboard;
using db.v1.keyboards.Repositories.Box;
using db.v1.keyboards.Repositories.Switch;
using db.v1.keyboards.Repositories.Keycap;
using db.v1.keyboards.Repositories.Kit;

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

using db.v1.users.Repositories.User;
using db.v1.users.Contexts;
using db.v1.users.Contexts.Interfaces;
using helper.v1.localization.Helper.Interfaces;



#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/configurations/api.json", optional: false, reloadOnChange: true);

var cfg = builder.Configuration;

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = cfg["Redis:Keyboards"];
});

var rabbitHost = cfg["RabbitMQ:Host"];
var rabbitUsername = cfg["RabbitMQ:Username"];
var rabbitPassword = cfg["RabbitMQ:Password"];
builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<UserConsumer>();
    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, host =>
        {
            host.Username(rabbitUsername);
            host.Password(rabbitPassword);
        });
        cfg.ReceiveEndpoint("user", e =>
        {
            e.Consumer<UserConsumer>(context);
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

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
        new("ru-RU"),
        new("en-US")
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

builder.Services.AddDbContext<KeyboardContext>(options => options.UseNpgsql(cfg["PostgreSQL:Keyboards"]), ServiceLifetime.Transient);
builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(cfg["PostgreSQL:Keyboards"]), ServiceLifetime.Transient);
builder.Services.AddTransient<IUserContext, UserContext>();
builder.Services.AddTransient<IKeyboardContext, KeyboardContext>();
builder.Services.AddTransient<ISwitchContext, KeyboardContext>();
builder.Services.AddTransient<IBoxContext, KeyboardContext>();
builder.Services.AddTransient<IKitContext, KeyboardContext>();
builder.Services.AddTransient<IKeycapContext, KeyboardContext>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IKeyboardRepository, KeyboardRepository>();
builder.Services.AddTransient<ISwitchRepository, SwitchRepository>();
builder.Services.AddTransient<IBoxRepository, BoxRepository>();
builder.Services.AddTransient<IKitRepository, KitRepository>();
builder.Services.AddTransient<IKeycapRepository, KeycapRepository>();

builder.Services.AddSingleton<IFileHelper, FileHelper>();
builder.Services.AddSingleton<ITimeHelper, TimeHelper>();
builder.Services.AddSingleton<ICacheHelper, RedisCacheHelper>();
builder.Services.AddSingleton<IFileLocalizationHelper, LocalizationHelper>();
builder.Services.AddSingleton<IMessageBrokerHelper, RabbitMQHelper>();

builder.Services.AddSingleton<IFileConfigurationHelper, ConfigurationHelper>();
builder.Services.AddSingleton<ICacheConfigurationHelper, ConfigurationHelper>();
builder.Services.AddSingleton<IActivityConfigurationHelper, ConfigurationHelper>();

builder.Services.AddSingleton<IKeyboardRegexHelper, RegexHelper>();
builder.Services.AddSingleton<IBoxRegexHelper, RegexHelper>();
builder.Services.AddSingleton<IKitRegexHelper, RegexHelper>();

builder.Services.AddTransient<IKeyboardService, KeyboardService>();
builder.Services.AddTransient<ISwitchService, SwitchService>();
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
app.UseRequestLocalization();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion

