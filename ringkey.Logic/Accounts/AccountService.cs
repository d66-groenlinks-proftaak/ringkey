using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
using Microsoft.IdentityModel.Tokens;
using ringkey.Common.Models;
using ringkey.Data;
using ringkey.Logic.Messages;

namespace ringkey.Logic.Accounts
{
    public class AccountService
    {
        private UnitOfWork _unitOfWork;
        
        public AccountService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Account GetByToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("RINGKEY_SECRET") ?? "This should probably not be your key");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                return _unitOfWork.Account.GetById(userId);
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public Account Login(AccountLogin account)
        {
            Account temp = _unitOfWork.Account.GetByEmail(account.Email);
            if (temp != null)
            {
                if (Argon2.Verify(temp.Password, account.Password))
                    return temp;
            }
            
            return null;
        }
        
        public AccountError Register(AccountRegister account)
        {
            if (!Messages.Utility.IsValidEmail(account.Email))
                return AccountError.InvalidEmail;

            if (account.FirstName.Length <= 1)
                return AccountError.FirstNameTooShort;
            if (account.FirstName.Length > 50)
                return AccountError.FirstNameTooLong;
            
            if (account.LastName.Length <= 1)
                return AccountError.LastNameTooShort;
            if (account.LastName.Length > 50)
                return AccountError.LastNameTooLong;
            
            if (account.Password.Length <= 1)
                return AccountError.PasswordTooShort;
            if (account.Password.Length > 50)
                return AccountError.PasswordTooLong;

            if (_unitOfWork.Account.GetByEmail(account.Email) != null)
                return AccountError.EmailInUse;
            
            _unitOfWork.Account.Add(new Account()
            {
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Password = Argon2.Hash(account.Password),
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Type = RoleType.Member
                    }
                }
            });

            _unitOfWork.SaveChanges();
            
            return AccountError.NoError;
        }
    }
}