using Hosteland.Services.OrderService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repositories.OrderRepository;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using Microsoft.Extensions.Configuration;

var builder = new HostBuilder();


builder.ConfigureAppConfiguration((context, config) => {
    config.AddJsonFile("Settings.job", optional: true, reloadOnChange: true);
});

builder.ConfigureServices((context, services) => {
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

    // Register other services
    services.AddScoped<IOrderService, OrderService>();
    services.AddScoped<IOrderRepository, OrderRepository>();
});

builder.ConfigureWebJobs(b =>
{
    b.AddAzureStorageCoreServices();
    b.AddAzureStorage();
    b.AddTimers();
});

builder.ConfigureLogging((context, b) =>
{
    b.AddConsole();
});

var host = builder.Build();
using (host) {
    await host.RunAsync();
}