using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class ApplicationDTO
    {
        public string ApplicationId { get; set; }
        public string ApplicantId { get; set; }
        public string VacancyId { get; set; }
        public string VacancyName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CoverLetter { get; set; }
        public string AdditionalInformation { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ResumeContentType { get; set; }

        public string ResumeAsBase64 { get; set; }


        public ApplicationDTO()
        {
            ApplicationId = string.Empty;
            ApplicantId = string.Empty;
            VacancyId = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            ResumeAsBase64 = string.Empty;
            CoverLetter = string.Empty;
            AdditionalInformation = string.Empty;
            Status = 1;
            CreatedAt = DateTime.Now;
            ResumeContentType = string.Empty;
            VacancyName = string.Empty;
        }
    }
}
