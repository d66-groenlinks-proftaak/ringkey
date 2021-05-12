using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;

namespace ringkey.Logic.Accounts
{
    public static class Utility
    {
        public static string GenerateJwtToken(Account account)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            
            byte[] key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("RINGKEY_SECRET") ?? "This should probably not be your key");
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static List<Permissions> GetPermissionsFromRoles(List<Role> roles)
        {
            List < Permissions > permissions = new List<Permissions>();
            foreach(Role role in roles)
            {
                if(role.Permissions != null)
                {
                    foreach(Permission perm in role.Permissions)
                    {
                        if (!permissions.Contains(perm.Perm))
                            permissions.Add(perm.Perm);
                    }
                }
                
            }
            return permissions;
        }

    }
}