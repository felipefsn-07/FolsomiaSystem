using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FolsomiaSystem.Application.Exceptions;
using FolsomiaSystem.Application.Interfaces.Providers;
using FolsomiaSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace FolsomiaSystem.Infra.Data.Providers
{
    public class CredentialSafeConfigProvider : ICredentialSafeConfigProvider
    {
        #region Private Classes

        private class SenhaSeguraConfig
        {

            public string BaseUrl { get; set; }
            public int DefaultCacheExpirationInMinutes { get; set; }
            public IList<CredentialSafeConfig> Senhas { get; set; }

        }

        #endregion

        #region Constants

        const string CONFIG_SECTION_NAME = "SenhaSegura";
        const string CONFIG_BASE_URL = "BaseUrl";
        const string CONFIG_DEFAULT_CACHE_EXPIRATION_IN_MINUTES = "DefaultCacheExpirationInMinutes";
        const string CONFIG_SENHAS = "Senhas";

        #endregion

        #region Variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public CredentialSafeConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region ICredentialSafeConfigProvider Implementation
        
        public Task<CredentialSafeConfig> GetByNameAsync(string name)
        {
            return Task.FromResult(GetByName(name));
        }

        #endregion

        #region Private Methods

        private CredentialSafeConfig GetByName(string name)
        {
            var _config = _configuration.GetSection(CONFIG_SECTION_NAME).Get<SenhaSeguraConfig>();
            
            Validate(_config);

            var _senhaItem = _config.Senhas.FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (_senhaItem == null)
                throw new ArgumentException($"Name \"{name}\" not found in list \"{CONFIG_SENHAS}\" of section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");

            _senhaItem.BaseUrl = _config.BaseUrl;
            _senhaItem.DefaultCacheExpirationInMinutes = _config.DefaultCacheExpirationInMinutes;

            return _senhaItem;
        }

        private void Validate(SenhaSeguraConfig config)
        {
            if (string.IsNullOrEmpty(config.BaseUrl))
                throw new ConfigurationMissingException($"Property \"{CONFIG_BASE_URL}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");

            if (config.DefaultCacheExpirationInMinutes.Equals(default(int)))
                throw new ConfigurationMissingException($"Property \"{CONFIG_DEFAULT_CACHE_EXPIRATION_IN_MINUTES}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");

            if (config.Senhas == null)
                throw new ConfigurationMissingException($"List \"{CONFIG_SENHAS}\" not found in the section \"{CONFIG_SECTION_NAME}\" of appsettings.json file");
        }

        #endregion
    }
}