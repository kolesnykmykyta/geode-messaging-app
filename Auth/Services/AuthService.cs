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
using System.Security.Cryptography;
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

        public async Task<TokenDto?> LoginAsync(LoginDto dto)
        {
            User? existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser == null)
            {
                return null;
            }

            bool isCorrectPassword = await _userManager.CheckPasswordAsync(existingUser, dto.Password);
            if (isCorrectPassword)
            {
                return await GenerateBearerTokenAsync(existingUser, true);
            }
            else
            {
                return null;
            }
        }

        public async Task<TokenDto?> RefreshAsync(TokenDto dto)
        {
            ClaimsPrincipal principal = GetPrincipalFromExistingToken(dto.AccessToken);

            User? userFromToken = await _userManager.FindByNameAsync(principal.Identity!.Name!);

            if (userFromToken == null || userFromToken.RefreshToken != dto.RefreshToken || userFromToken.RefreshTokenExpirationDate <= DateTime.Now)
            {
                return null;
            }

            return await GenerateBearerTokenAsync(userFromToken, false);
        }

        public async Task<RegisterResultDto> RegisterAsync(RegisterDto dto)
        {
            User newUser = new User
            {
                UserName = dto.Username,
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

        private async Task<TokenDto> GenerateBearerTokenAsync(User user, bool populateExp)
        {
            IEnumerable<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
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

            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpirationDate = DateTime.Now.AddDays(7);
            }

            await _userManager.UpdateAsync(user);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            TokenDto dto = new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return dto;
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExistingToken(string token)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                // ValidateLifetime = true, // Expired access tokens can (false) or can't (true) use refresh tokens
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config.GetSection("Jwt:Issuer").Value,
                ValidAudience = _config.GetSection("Jwt:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!))
            };

            JwtSecurityTokenHandler tokenHanler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = tokenHanler.ValidateToken(token, validationParameters, out securityToken);
            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
