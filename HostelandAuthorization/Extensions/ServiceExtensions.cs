using HostelandAuthorization.Services.ApplicationUserService;
using HostelandAuthorization.Services.EmailService;
using HostelandAuthorization.Services.FurnitureService;
using HostelandAuthorization.Services.RoomCategoryService;
using HostelandAuthorization.Services.OrderService;
using HostelandAuthorization.Services.RoomService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.ApplicationUserRepositories;
using Repositories.FurnitureRepository;
using Repositories.RoomCategoryRepository;
using Repositories.OrderRepository;
using Repositories.RoomRepository;
using HostelandAuthorization.Context;
using HostelandAuthorization.Services.ServiceService;
using Repositories.ServiceRepository;

namespace HostelandAuthorization.Extensions
{
    public static class ServiceExtensions {

        public static void ConfigureDILifeTime(this IServiceCollection services) {
            // SERVICE
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IUserContext, UserContext>();

            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomCategoryService, RoomCategoryService>();
            services.AddScoped<IFurnitureService, FurnitureService>();
            services.AddScoped<IServiceService, ServiceService>();

            // REPOSITORY
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
            services.AddScoped<IFurnitureRepository, FurnitureRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
        }

        public static void ConfigureCors(this IServiceCollection services) {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                         .WithOrigins(new string[] { "https://localhost:4200", "http://localhost:4200" })
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials());
            });
        }

        public static void ConfigureSwaggerGen(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hosteland Authorization Server", Version = "v1" });
                //c.EnableAnnotations();
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, byte[] key) {
            var tokenValidationParams = new TokenValidationParameters() {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            services.AddSingleton(tokenValidationParams);
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(jwt => {
                        jwt.SaveToken = true;
                        jwt.RequireHttpsMetadata = true;
                        jwt.TokenValidationParameters = tokenValidationParams;
                    });
        }

    }
}
