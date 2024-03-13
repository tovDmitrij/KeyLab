using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;



var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("/configurations/proxy.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle());

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "ProtectedPolicy",
        policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("some"));
    options.AddPolicy(
        name: "PublicPolicy",
        policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true));
});



var app = builder.Build();
app.UseCors("PublicPolicy");
app.UseOcelot().Wait();
app.UseAuthorization();
app.Run();