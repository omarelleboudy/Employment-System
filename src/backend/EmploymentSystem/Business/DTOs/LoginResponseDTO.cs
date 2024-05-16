using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public LoginResponseDTO()
        {
            UserId = string.Empty;
            Name = string.Empty;
            Role = string.Empty;
            Token = string.Empty;
        }
    }
}
