using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.JwtAuth
{
    public class JwtGenerator
    {
        public JwtGenerator(string jwtKey)
        {
            JwtKey = jwtKey;
        }

        public string JwtKey { get; }
        [DefaultValue(true)]
        public bool UseJti { get; set; }

        public string Generate()
        {
            var claims = new List<Claim>()
            {
                //new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Username),
                //new Claim(ClaimTypes.Name, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(JwtKey));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            JwtSecurityTokenHandler tokenHandler = new();
            string tokenJwt = tokenHandler.WriteToken(token);
            return tokenJwt;
        }
    }
}
