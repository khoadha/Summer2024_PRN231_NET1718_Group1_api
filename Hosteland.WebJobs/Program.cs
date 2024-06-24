using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder();
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