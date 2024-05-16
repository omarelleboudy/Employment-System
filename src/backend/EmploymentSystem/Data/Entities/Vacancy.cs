using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data.Entities
{
    public class Vacancy : IEntity<string>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxApplicants { get; set; }
        public int CurrentApplicants { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        // Foreign Keys
        public string? EmployerId { get; set; }

        // Navigations
        public virtual ApplicationUser? Employer { get; set; }
        public ICollection<VacancyApplication> VacancyApplications { get; } = new List<VacancyApplication>(); 


        public Vacancy()
        {
            Id = Guid.NewGuid().ToString();
            Title = string.Empty;
            Description = string.Empty;
            MaxApplicants = 0;
            CurrentApplicants = 0;
            IsActive = true;
            IsArchived = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ExpiredAt = DateTime.UtcNow.AddDays(1);
        }
    }
}
