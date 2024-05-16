using Microsoft.AspNetCore.Identity;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using Data.Repository.Base;
using Data.Repository;
using Hangfire.SqlServer;
using Hangfire;
using Microsoft.Data.SqlClient;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EmploymentSystemDb")));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));

            services.AddScoped<IVacancyApplicationRepository, VacancyApplicationRepository>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();


            // For Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            // For Hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("HangFireDB")));

            return services;
        }
    }
}
