using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using BusinessObjects.ConfigurationModels;
using HostelandAuthorization.Extensions;
using BusinessObjects.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.ConfigureDILifeTime();
builder.Services.ConfigureCors();
builder.Services.ConfigureSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddHttpClient();
builder.Services.Configure<FormOptions>(options => {
    options.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//=====================  DATABASE  ==========================

 builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.User.AllowedUserNameCharacters = null;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<AppDbContext>();


var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value);
builder.Services.ConfigureAuthentication(key);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment()) {
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("v1/swagger.json", "V1 Docs");
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.MapControllers();

app.Run();
