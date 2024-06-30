using System.Security.Cryptography;

namespace jobquest.Application.Commands.Authentication;

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    public string GenerateToken(string email)
    {
        var hmac = new HMACSHA256();
        var key = Convert.ToBase64String(hmac.Key);
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)); // Replace with your secret key
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5142",  // Replace with your issuer
            audience: "http://localhost:4200",  // Replace with your audience
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),  // Token expiration, adjust to your needs
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}