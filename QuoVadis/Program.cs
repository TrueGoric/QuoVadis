using Microsoft.AspNetCore.Authentication.Cookies;
using QuoVadis.Common;
using QuoVadis.Common.Services;
using QuoVadis.Extensions;
using QuoVadis.RpcServices;
using QuoVadis.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();

    siloBuilder.AddReminders();
    siloBuilder.AddStreaming();
    siloBuilder.UseTransactions();

    siloBuilder.Services.AddTransient<IAreaResolverService, StaticAreaResolverService>();

    // the following settings shouldn't be used in a production-ready application
    siloBuilder.AddMemoryGrainStorage(Constants.RegularStorage);
    siloBuilder.AddMemoryGrainStorage(Constants.SecureStorage);
    siloBuilder.AddMemoryGrainStorage(Constants.TransactionalStorage);
    siloBuilder.AddMemoryGrainStorage("PubSubStore");

    siloBuilder.AddMemoryStreams(Constants.MemoryStreamProvider);
    siloBuilder.UseInMemoryReminderService();
});

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options => options.DeactivateCookieChallenge());
builder.Services.AddAuthorization();

builder.Services.AddHostedService<MockDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseStaticFiles();
app.UseRouting();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<MonitorService>();
app.MapGrpcService<UserService>();
app.MapGrpcService<RentService>();
app.MapGrpcService<VehicleService>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
