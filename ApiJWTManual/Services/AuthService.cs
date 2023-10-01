using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using ApiJWTManual.Dtos;
using ApiJWTManual.Dtos.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Microsoft.VisualBasic;

namespace ApiJWTManual.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApiJWTManualContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApiJWTManualContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateToken(string IdUser){
            var key = _configuration.GetValue<string>("JWTSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IdUser));

            var credentialToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credentialToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreated = tokenHandler.WriteToken(tokenConfig);

            return tokenCreated;
        }

        public async Task<AuthResponse> ReturnToken(AuthRequest auth)
        {
            var userFound = _context.Users.FirstOrDefault(x =>
                x.Username == auth.Username &&
                x.Password == auth.Password
            );

            if(userFound == null){
                return await Task.FromResult<AuthResponse>(null);
            }

            string tokenCreated = GenerateToken(userFound.Id.ToString());

            return new AuthResponse(){Token = tokenCreated, Result = true, Msg = "OK"};
        }
    }
}