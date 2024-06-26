using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Services.Implementations;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Back_End_TPI_PSS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Obtener configuraci�n JWT
            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
            var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();

            // Configuraci�n de autenticaci�n JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            // Configuraci�n de Swagger
            builder.Services.AddSwaggerGen(setupAction =>
            {
                setupAction.AddSecurityDefinition("BackendPPS-API-BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Ingresar el token para autenticarse."
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BackendPPS-API-BearerAuth"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            // Configuraci�n de controladores y JSON
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Explorador de Endpoints API
            builder.Services.AddEndpointsApiExplorer();

            // Configuraci�n de DbContext
            builder.Services.AddDbContext<PPSContext>(dbContextOptions =>
                dbContextOptions.UseSqlite(builder.Configuration["DB:ConnectionString"])
            );

            // Inyecci�n de dependencias
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IMercadoPagoPayment, MercadoPagoPayment>();

            // Configuraci�n de la aplicaci�n
            var app = builder.Build();

            // Configuraci�n de CORS
            app.UseCors(corsBuilder =>
            {
                corsBuilder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
            });

            // Configuraci�n del pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
