using Business.Base;
using Business.DTOs;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IAccountService
    {
        Task<IResult> Register(RegistrationModel model);
        Task<IResult<LoginResponseDTO>> Login(LoginModel model);
    }
}
