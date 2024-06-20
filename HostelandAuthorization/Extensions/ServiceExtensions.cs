using HostelandAuthorization.Services.ApplicationUserService;
using HostelandAuthorization.Services.EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.ApplicationUserRepositories;
using HostelandAuthorization.Context;
using Hosteland.Services.FurnitureService;
using Hosteland.Services.GlobalRateService;
using Hosteland.Services.OrderService;
using Hosteland.Services.RoomCategoryService;
using Hosteland.Services.RoomService;
using Hosteland.Services.ServiceService;
using Hosteland.Services.VnPayService;
using Repositories.FurnitureRepository;
using Repositories.GlobalRateRepository;
using Repositories.OrderRepository;
using Repositories.PaymentTransactionRepository;
using Repositories.RoomCategoryRepository;
using Repositories.RoomRepository;
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
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IGlobalRateService, GlobalRateService>();

            // REPOSITORY
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
            services.AddScoped<IFurnitureRepository, FurnitureRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            services.AddScoped<IGlobalRateRepository, GlobalRateRepository>();
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hosteland API", Version = "v1" });
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
