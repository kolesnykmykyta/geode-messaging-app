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
using System.Runtime.CompilerServices;
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
                return await GenerateTokenPairAsync(existingUser, true);
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

            return await GenerateTokenPairAsync(userFromToken, false);
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

        private async Task<TokenDto> GenerateTokenPairAsync(User user, bool changeRefreshTokenExpDate)
        {
            string refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            if (changeRefreshTokenExpDate)
            {
                user.RefreshTokenExpirationDate = DateTime.Now.AddDays(7);
            }

            await _userManager.UpdateAsync(user);

            string bearerToken = await GenerateBearerToken(user);

            TokenDto tokenPair = new TokenDto
            {
                AccessToken = bearerToken,
                RefreshToken = refreshToken,
            };

            return tokenPair;
        }

        private async Task<string> GenerateBearerToken(User user)
        {
            List<Claim> userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!)
            };

            foreach(string role in await _userManager.GetRolesAsync(user))
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            SecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
            SigningCredentials credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddHours(1),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Issuer").Value,
                signingCredentials: credentials
                );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return accessToken;
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
                ValidateLifetime = false, // Expired access tokens can (false) or can't (true) use refresh tokens
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
