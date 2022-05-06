using Atm.Autenticador.Dados.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Atm.Autenticador.Dados.Helpers
{
    public static class CryptographyHelper
    {
        public static string ToHash(string senha)
        {
            string hash = string.Empty;
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] stringBytes = Encoding.UTF8.GetBytes(senha.ToUpper());
                byte[] hashBytes = sha512Hash.ComputeHash(stringBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
            return hash;
        }

        public static string GenerateJwt(Guid id)
        {
            string secret = "d60QQTGeSeZ5UesRf9jH6oL3c8GS49L3U2p62sPCFlYt9LHvFZI8n1agMfyn";
            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = "Marvin-Autenticador",
                Audience = "Marvin",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                }),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
