using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using BusinessObjects.Entities;
using HostelandOData.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ConfigureControllers(); // Include OData setup
builder.Services.ConfigureDILifeTime();
builder.Services.ConfigureCors();
builder.Services.ConfigureSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddHttpClient();
builder.Services.Configure<FormOptions>(options => {
    options.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment()) {
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseCookiePolicy();
app.MapControllers();

app.Run();