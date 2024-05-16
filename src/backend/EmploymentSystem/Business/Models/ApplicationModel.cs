using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ApplicationModel
    {
        public string VacancyId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CoverLetter { get; set; }
        public string AdditionalInformation { get; set; }
        public IFormFile? Resume { get; set; }

        public ApplicationModel()
        {
            VacancyId = string.Empty;
            FullName = string.Empty;
            Email = string.Empty;
            CoverLetter = string.Empty;
            AdditionalInformation = string.Empty;
        }
        public List<string> Errors()
        {
            var errors = new List<string>();
            if(string.IsNullOrEmpty(VacancyId))
            {
                errors.Add("VacancyId is required.");
            }
            if (string.IsNullOrEmpty(FullName))
            {
                errors.Add("FullName is required.");
            }
            if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
            {
                errors.Add("Email is required and must be a valid Email Address.");
            }
            if(string.IsNullOrEmpty(CoverLetter))
            {
                errors.Add("CoverLetter is required.");
            }   
            if(Resume == null || Resume.ContentType != "application/pdf")
            {
                errors.Add("Resume is required and must be a PDF file.");
            }
            return errors;
        }
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
