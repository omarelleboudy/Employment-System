using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class VacancyApplication : IEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Resume { get; set; }
        public string CoverLetter { get; set; }
        public int Status { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign keys
        public string? ApplicantId { get; set; }
        public string? VacancyId { get; set; }

        // Navigation properties
        public virtual ApplicationUser? Applicant { get; set; }
        public virtual Vacancy? Vacancy { get; set; }

        public VacancyApplication() 
        { 
            Id = Guid.NewGuid().ToString();
            Status = 1;
            Name = string.Empty;
            Email = string.Empty;
            Resume = string.Empty;
            CoverLetter = string.Empty;
            AdditionalInformation = string.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
