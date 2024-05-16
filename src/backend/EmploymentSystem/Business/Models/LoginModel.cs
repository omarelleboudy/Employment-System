using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginModel()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public List<string> Errors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Username))
            {
                errors.Add("Username is required.");
            }

            if (string.IsNullOrEmpty(Password))
            {
                errors.Add("Password is required.");
            }

            return errors;
        }
    }
}
