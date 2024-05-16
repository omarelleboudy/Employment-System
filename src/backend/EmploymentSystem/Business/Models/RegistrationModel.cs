using Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class RegistrationModel
    {
        public string Username { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }

        public RegistrationModel()
        {
            Username = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Role = 1;
        }

        public List<string> Errors()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(Username))
                errors.Add($"Properity {nameof(Username)} is required.");

            if (string.IsNullOrEmpty(Name))
                errors.Add($"Properity {nameof(Name)} is required.");

            if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
                errors.Add($"Properity {nameof(Email)} is required and must be a valid email address.");

            if (string.IsNullOrEmpty(Password))
                errors.Add($"Properity {nameof(Password)} is required.");

            if (Role != (int)RoleEnum.Applicant && Role != (int)RoleEnum.Employer)
                errors.Add($"Properity {nameof(Role)} is invalid. It must be either 1 for Applicant or 2 for Employer.");

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
