using Application.Utils.Automapper;
using Auth.Services;
using Auth.Services.Interfaces;
using Azure.Storage.Blobs;
using BlobStorageAccess.Services;
using BlobStorageAccess.Services.Interfaces;
using DataAccess.DbContext;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using Geode.API.Extensions;
using Geode.API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Geode.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureAzureKeyVault();

            // Services
            builder.Services.AddHealthChecks();

            builder.Services.AddSignalR();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string? dbConnectionString = builder.Configuration
                .GetConnectionString(builder.Environment.IsDevelopment() ? "Development" : "Default");
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(dbConnectionString));

            builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

            builder.Services.AddAuthorization();

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!))
                    };
                });

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddCqrsServices();

            builder.Services.AddHelpers();

            var app = builder.Build();

            // Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHealthChecks("/health");

            app.MapControllers();

            app.MapHub<ChatHub>("/chathub");
            app.MapHub<WebRtcHub>("/webrtc");

            app.Run();
        }
    }
}
