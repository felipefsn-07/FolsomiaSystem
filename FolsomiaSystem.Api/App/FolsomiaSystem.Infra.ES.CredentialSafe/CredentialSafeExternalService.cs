using AutoMapper;
using FolsomiaSystem.Application.DTOs;
using FolsomiaSystem.Application.Exceptions;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Interfaces.Repositories;
using FolsomiaSystem.Application.Validators;
using FolsomiaSystem.Domain;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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


        #endregion

        #region Variables

        private readonly IConfiguration _configuration;
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IMapper _mapper;
        private readonly IAuditLogExternalService _auditLogExternalService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CredentialSafeConfig _credential;

        #endregion

        #region Constructors

        public CredentialSafeExternalService(IConfiguration configuration, 
                                             IAdminUserRepository adminUserRepository,
                                            IAuditLogExternalService auditLogExternalService,
                                            IMapper mapper,
                                            IUnitOfWork unitOfWork

            )
        {
            _configuration = configuration;
            _adminUserRepository = adminUserRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _auditLogExternalService = auditLogExternalService;
            _credential = new CredentialSafeConfig()
            {
                AuditLog = new AuditLog
                {
                   DateLog = DateTime.UtcNow,
                   StatusLog = StatusLog.success
                }

            };
        }

        #endregion

        #region ICredentialSafeConfiguration Implementation

        public Task<CredentialSafeConfig> Login(AdminUserInputs user)
        {
            
            return Task.FromResult(Authenticate(user));
        }

        public Task<CredentialSafeConfig> AlterPassword(AlterAdminUserInputs alterAdminUserInputs)
        {

            return Task.FromResult(AlterPasswordReturnCredential(alterAdminUserInputs));
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
            if (!ValidateInputsAdminUser(adminUser)) return _credential;
            var logar =  _adminUserRepository.GetFirstAsync();

           
            if (logar.Result != null && logar.Result.UserName == adminUser.UserName && logar.Result.Password == MD5Hash(adminUser.Password))
            {
                var token = GenerateToken(_config.TokenSecret, _config.DefaultCacheExpirationInMinutes);
                adminUser.Password = "";
                _credential.AuditLog = null;
                _credential.DefaultCacheExpirationInMinutes = _config.DefaultCacheExpirationInMinutes;
                _credential.Token = token;
            }
            else
            {
                _credential.AuditLog = new AuditLog
                {
                    DateLog = DateTime.UtcNow,
                    MessageLog = "User or password incorrect!",
                    StatusLog = StatusLog.fail,
                    OperationLog = OperationLog.Login

                };

            }
            return _credential;
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

        private bool ValidateInputsAdminUser(AdminUserInputs adminUser)
        {

            var validator = new AdminUserValidator();
            BaseValidator baseValidator = new BaseValidator();
            var validRes = validator.Validate(adminUser);

            if (!validator.Validate(adminUser).IsValid)
            {
               _credential.AuditLog = new AuditLog
                {
                    DateLog = DateTime.UtcNow,
                    MessageLog = baseValidator.MsgErrorValidator(validRes),
                    StatusLog = StatusLog.fail,
                    OperationLog = OperationLog.Login

                };
                return false;
            }
            return true;
        }

        private bool ValidateInputsAlterPassword(AlterAdminUserInputs pass)
        {

            var validator = new AlterPasswordValidator();
            BaseValidator baseValidator = new BaseValidator();
            var validRes = validator.Validate(pass);

            if (!validator.Validate(pass).IsValid)
            {
                _credential.AuditLog = new AuditLog
                {
                    DateLog = DateTime.UtcNow,
                    MessageLog = baseValidator.MsgErrorValidator(validRes),
                    StatusLog = StatusLog.fail,
                    OperationLog = OperationLog.Login

                };
                return false;
            }
            return true;
        }

        private void Validate(SecurePasswordConfig config)
        {

            if (config.DefaultCacheExpirationInMinutes.Equals(default))
                throw new ConfigurationMissingException($"Property \"{CONFIG_DEFAULT_CACHE_EXPIRATION_IN_MINUTES}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");

            if (config.TokenSecret == null)
                throw new ConfigurationMissingException($"The \"{CONFIG_TOKEN_SECRET}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");
        }

        private CredentialSafeConfig AlterPasswordReturnCredential(AlterAdminUserInputs alterAdminUserInputs)
        {
            var _config = _configuration.GetSection(CONFIG_SECTION_NAME).Get<SecurePasswordConfig>();
            Validate(_config);
            _credential.AuditLog.OperationLog = OperationLog.ChangePassword;
            if (ValidateInputsAlterPassword(alterAdminUserInputs))
            {
                var logar = _adminUserRepository.GetFirstAsync();
                if (logar.Result != null)
                {
                    logar.Result.Password = MD5Hash(alterAdminUserInputs.NewPassword);
                    _adminUserRepository.Update(logar.Result);
                     //_auditLogExternalService.AddNewLogAuditAsync(_credential.AuditLog);
                     _unitOfWork.CommitAsync();
                    var token = GenerateToken(_config.TokenSecret, _config.DefaultCacheExpirationInMinutes);
                    _credential.Token = token;
                }

            }

            return _credential;
        }



        #endregion

    }
}