
using Data;
using Business;
using EmploymentSystem.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Logging;
using Serilog;
using EmploymentSystem.Policies;
using Infrastructure.Logging.Core;
using Hangfire;
using Hangfire.Common;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace EmploymentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adding Logging
            #region Configure serilog
            builder.Logging.ClearProviders();
            Log.Logger = InfrastructureLogger.CreateSerilogLogger(builder.Configuration);

            builder.Logging.AddSerilog(Log.Logger);
            builder.Host.UseSerilog();
            #endregion
            // Adding Caching
            builder.Services.AddOutputCache(options =>
            {
                options.AddBasePolicy(builder => builder.Cache());
                options.AddPolicy("OutputCacheWithAuthPolicy", OutputCacheWithAuthPolicy.Instance);
            });
            // Adding Persistence
            builder.Services.AddApplicationPersistence(builder.Configuration);
            // Adding Correlation Id Middleware
            builder.Services.AddCorrelationIdManager();
            builder.Services.AddHangfireServer();


            // Adding Business
            builder.Services.AddApplicationBusiness();

            // Adding Authentication  
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClientCORS",
                    corsBuilder =>
                    {
                        corsBuilder.WithOrigins(builder.Configuration.GetValue<string>("ClientAppUrl")?? "http://localhost:3000")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });

            var app = builder.Build();
            app.UseCors("AllowClientCORS");

            app.UseOutputCache();   
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHangfireDashboard("/hangfire");
            app.RegisterHangfireJobs();

            app.UseHttpsRedirection();
            // Injecting Logging Middleware after HTTPS redirection because it requires HTTPContext.
            app.UseLoggingMiddleware();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}