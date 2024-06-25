using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_PRN231_API.Interface.Auth;
using Project_PRN231_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_PRN231_API.Services.Common
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration? _configuration;

        public AuthService(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(User? u)
        {
            var config = _configuration?.GetSection("JwtSettings");
            var secretKey = config["Key"];
            var issuer = config["Issuer"];
            var audience = config["Audience"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, u.Username),
                new Claim(JwtRegisteredClaimNames.Email, u.Password),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
