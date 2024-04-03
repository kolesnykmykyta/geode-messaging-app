using Auth.Dtos;
using Auth.Services.Interfaces;
using DataAccess.Entities;
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

namespace Auth.Services
{
    public class AuthService : IAuthService 
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            User? existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser == null)
            {
                return null;
            }

            bool isCorrectPassword = await _userManager.CheckPasswordAsync(existingUser, dto.Password);
            if (isCorrectPassword)
            {
                return this.GenerateBearerToken(existingUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<RegisterResultDto> RegisterAsync(RegisterDto dto)
        {
            User newUser = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
            };

            var registerResult = await _userManager.CreateAsync(newUser, dto.Password);

            if (registerResult.Succeeded)
            {
                return new RegisterResultDto(true);
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (IdentityError error in registerResult.Errors)
                {
                    errors.Add(error.Description);
                }

                return new RegisterResultDto(false, errors);
            }
        }

        private string GenerateBearerToken(User user)
        {
            IEnumerable<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };


            SecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
            SigningCredentials credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddHours(12),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Issuer").Value,
                signingCredentials: credentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
