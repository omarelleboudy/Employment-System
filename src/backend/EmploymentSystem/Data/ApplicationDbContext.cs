using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Vacancy> Vacancies { get; }
        public DbSet<VacancyApplication> VacancyApplications { get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Vacancies = Set<Vacancy>();
            VacancyApplications = Set<VacancyApplication>();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Base Builder for Identity
            base.OnModelCreating(builder);

            // Map Tables  
            builder.Entity<Vacancy>().ToTable("Vacancy");
            builder.Entity<VacancyApplication>().ToTable("VacancyApplication");
           

            // Map Relations
            builder.Entity<Vacancy>(b =>
            {
                b.HasIndex(e => e.Id).IsUnique();
                b.HasOne<ApplicationUser>(e => e.Employer).WithMany().HasForeignKey(e=>e.EmployerId).OnDelete(DeleteBehavior.NoAction);
                b.HasMany<VacancyApplication>(e => e.VacancyApplications).WithOne(e => e.Vacancy).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<VacancyApplication>(b =>
            {
                b.HasOne<ApplicationUser>(e => e.Applicant).WithMany().HasForeignKey(e=>e.ApplicantId).OnDelete(DeleteBehavior.NoAction);
            });
            
        }

    }
}

