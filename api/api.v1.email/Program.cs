using MassTransit;

using helper.v1.configuration.Interfaces;
using helper.v1.configuration;

using api.v1.email.Service;
using api.v1.email.Consumers;



#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("/configurations/api.json", optional: false, reloadOnChange: true);

var cfg = builder.Configuration;

var rabbitHost = cfg["RabbitMQ:Host"];
var rabbitUsername = cfg["RabbitMQ:Username"];
var rabbitPassword = cfg["RabbitMQ:Password"];
builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<EmailConsumer>();
    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, host =>
        {
            host.Username(rabbitUsername);
            host.Password(rabbitPassword);
        });
        cfg.ReceiveEndpoint("email",e =>
        {
            e.Consumer<EmailConsumer>(context);
        });
    });
});

builder.Services.AddAntiforgery(options =>
{
    options.FormFieldName = "KeylabAntiforgery";
    options.HeaderName = "X-CSRF-TOKEN-KEYLAB";
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "PublicPolicy",
        policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true));
});

builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddSingleton<IEmailConfigurationHelper, ConfigurationHelper>();

#endregion



#region App

var app = builder.Build();
app.UseCors("PublicPolicy");
app.Run();

#endregion


