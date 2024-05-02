using api.v1.stats.Consumers;
using api.v1.stats.Services.Activity;
using api.v1.stats.Services.History;
using api.v1.stats.Services.Interval;
using api.v1.stats.Services.Stat;

using component.v1.middlewares;

using db.v1.stats.Contexts;
using db.v1.stats.Contexts.Interfaces;
using db.v1.stats.Repositories.Activity;
using db.v1.stats.Repositories.History;
using db.v1.stats.Repositories.Interval;

using helper.v1.cache;
using helper.v1.configuration;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;
using helper.v1.time;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Globalization;
using System.Text;



#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/configurations/api.json", optional: false, reloadOnChange: true);

var cfg = builder.Configuration;

var rabbitHost = cfg["RabbitMQ:Host"];
var rabbitUsername = cfg["RabbitMQ:Username"];
var rabbitPassword = cfg["RabbitMQ:Password"];
builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<ActivityConsumer>();
    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, host =>
        {
            host.Username(rabbitUsername);
            host.Password(rabbitPassword);
        });
        cfg.ReceiveEndpoint("activity", e =>
        {
            e.Consumer<ActivityConsumer>(context);
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = cfg["Redis:Stats"];
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

builder.Services.AddDbContext<StatContext>(options => options.UseNpgsql(cfg["PostgreSQL:Stats"]), ServiceLifetime.Transient);
builder.Services.AddTransient<IIntervalContext, StatContext>();
builder.Services.AddTransient<IActivityContext, StatContext>();
builder.Services.AddTransient<IHistoryContext, StatContext>();

builder.Services.AddTransient<IIntervalRepository, IntervalRepository>();
builder.Services.AddTransient<IActivityRepository, ActivityRepository>();
builder.Services.AddTransient<IHistoryRepository, HistoryRepository>();

builder.Services.AddSingleton<ICacheConfigurationHelper, ConfigurationHelper>();
builder.Services.AddSingleton<IStatConfigurationHelper, ConfigurationHelper>();

builder.Services.AddSingleton<ILocalizationHelper, LocalizationHelper>();
builder.Services.AddSingleton<ICacheHelper, RedisCacheHelper>();
builder.Services.AddSingleton<ITimeHelper, TimeHelper>();

builder.Services.AddTransient<IIntervalService, IntervalService>();
builder.Services.AddTransient<IActivityService, ActivityService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();
builder.Services.AddTransient<IStatService, StatService>();

#endregion



#region App

var app = builder.Build();
app.UseCors("PublicPolicy");
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<StatisticMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();
app.MapControllers();
app.Run();

#endregion


