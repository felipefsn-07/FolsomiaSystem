using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.Exceptions;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Domain.Enums;
using FolsomiaSystem.Infra.ES.CredentialSafe.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FolsomiaSystem.Infra.ES.CredentialSafe
{
    public class CredentialSafeExternalService : ICredentialSafeExternalService
    {

        #region Private Classes

        private class SecurePasswordConfig
        {

            public int DefaultCacheExpirationInMinutes { get; set; }
            public string TokenSecret { get; set; }

        }

        #endregion

        #region Constants

        const string CONFIG_SECTION_NAME = "SecurePassword";
        const string CONFIG_DEFAULT_CACHE_EXPIRATION_IN_MINUTES = "DefaultCacheExpirationInMinutes";
        const string CONFIG_TOKEN_SECRET = "TokenSecret";
        private readonly IAdminUserRepository _adminUserRepository;


        #endregion

        #region Variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public CredentialSafeExternalService(IConfiguration configuration, IAdminUserRepository adminUserRepository)
        {
            _configuration = configuration;
            _adminUserRepository = adminUserRepository;
        }

        #endregion

        #region ICredentialSafeConfiguration Implementation

        public Task<CredentialSafeConfig> Login(AdminUserInputs user)
        {
            
            return Task.FromResult(Authenticate(user));
        }

        #endregion

        #region Private Methods

        private string GenerateToken (string secret_token, int expired)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret_token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "admin")
                }),
                Expires = DateTime.UtcNow.AddMinutes(expired),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
       }


        private CredentialSafeConfig Authenticate(AdminUserInputs adminUser)
        {
            var _config = _configuration.GetSection(CONFIG_SECTION_NAME).Get<SecurePasswordConfig>();
            Validate(_config);

            CredentialSafeConfig credential = new CredentialSafeConfig();

            var logar =  _adminUserRepository.GetFirstAsync();
            if (logar.Result != null)
            {
                if (logar.Result.UserName == adminUser.Username && logar.Result.Password == MD5Hash(adminUser.Password))
                {
                    var token = GenerateToken(_config.TokenSecret, _config.DefaultCacheExpirationInMinutes);
                    adminUser.Password = "";
                    credential.AuditLog = null;
                    credential.DefaultCacheExpirationInMinutes = _config.DefaultCacheExpirationInMinutes;
                    credential.Token = token;
                }
                else
                {
                    credential.AuditLog = new AuditLog
                    {
                        DateLog = DateTime.UtcNow,
                        MessageLog = "User or password incorrect",
                        StatusLog = StatusLog.fail,
                        OperationLog = OperationLog.Login

                    };
                }

            }
            return credential;
        }

        private string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }


        private void Validate(SecurePasswordConfig config)
        {

            if (config.DefaultCacheExpirationInMinutes.Equals(default))
                throw new ConfigurationMissingException($"Property \"{CONFIG_DEFAULT_CACHE_EXPIRATION_IN_MINUTES}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");

            if (config.TokenSecret == null)
                throw new ConfigurationMissingException($"The \"{CONFIG_TOKEN_SECRET}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");
        }

        public Task<CredentialSafeConfig> AlterPassword(AlterAdminUserInputs alterAdminUserInputs)
        {
            throw new NotImplementedException();
        }



        #endregion

    }
}