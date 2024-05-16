using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class VacancyDTO
    {
        public string VacancyId { get; set; }
        public string EmployerId { get; set; }
        public string EmployerName { get; set; }
        public string Title { get; set; }
        public int MaxApplicants { get; set; }
        public int CurrentApplicants { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public VacancyDTO()
        {
            VacancyId = string.Empty;
            EmployerId = string.Empty;
            EmployerName = string.Empty;
            Title = string.Empty;
            IsArchived = false;
            IsActive = false;
            Description = string.Empty;
            CreatedAt = DateTime.MinValue;
            UpdatedAt = DateTime.MinValue;
            ExpiredAt = DateTime.MinValue;
            MaxApplicants = 0;
            CurrentApplicants = 0;
        }
    }
}
