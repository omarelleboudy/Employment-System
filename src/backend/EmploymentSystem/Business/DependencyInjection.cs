using Business.Services;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationBusiness(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IVacancyApplicationService, VacancyApplicationService>();
            services.AddScoped<IVacancyService, VacancyService>();           
           
            return services;
        }

        public static void RegisterHangfireJobs(this IApplicationBuilder builder)
        {
            // Creating up the Hangfire Job for archiving expired vacancies
            var jobManager = new RecurringJobManager();
            jobManager.AddOrUpdate("ArchiveExpiredVacanciesJobId",
                Job.FromExpression<IVacancyService>(v => v.ArchiveExpiredVacanciesAsync()),
                Cron.Minutely(), // Setting it to run every minute for testing purposes
                new RecurringJobOptions());
        }
    }
}
