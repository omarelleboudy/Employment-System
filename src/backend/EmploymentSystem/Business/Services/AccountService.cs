using Business.Base;
using Business.DTOs;
using Business.Enums;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }
        public async Task<IResult<LoginResponseDTO>> Login(LoginModel model)
        {
            var result = new Result<LoginResponseDTO>();

            if (model.Errors().Any())
                return result.FailedWithErrors(ErrorCode.InvalidData, model.Errors());

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return result.FailedWithError(new Error(ErrorCode.NotFound, ErrorMessage.USER_NOT_EXIST));
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return result.FailedWithError(new Error(ErrorCode.InvalidData, ErrorMessage.PASSWORD_INCORRECT));

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                
               new Claim(ClaimTypes.Name, user.UserName??""),
               new Claim("UserId", user.Id??""),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };


            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);

            return result.SucceededWithPayload(new LoginResponseDTO
            {
                UserId = user.Id??"",
                Token = token,
                Name = user.Name ?? "",
                Role = userRoles.FirstOrDefault() ?? ""
            });
        }
        public async Task<IResult> Register(RegistrationModel model)
        {
            // init result
            var result = new Result();

            if (model.Errors().Any())
                return result.FailedWithErrors(ErrorCode.InvalidData, model.Errors());

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) return result.FailedWithError(new Error(ErrorCode.AlreadyExists, ErrorMessage.USER_EXIST));

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null) return result.FailedWithError(new Error(ErrorCode.AlreadyExists, ErrorMessage.EMAIL_EXIST));
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name
            };

            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = createUserResult.Errors.Select(e => e.Description).ToList();
                return result.FailedWithErrors(ErrorCode.InvalidData, errors);
            }

            var role = GetRole(model.Role);

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded(message: "User has been created successfully.");
        }
        #region Helpers
        private string GetRole(int roleType)
        {
            if (roleType == (int)RoleEnum.Applicant) return UserRoles.Applicant;
            return UserRoles.Employer;
        }
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(40), // bit long for testing purposes
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
