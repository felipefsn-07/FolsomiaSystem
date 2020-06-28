using System;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using FolsomiaSystem.Application.Interfaces.Providers;
using FolsomiaSystem.Application.Interfaces.Repositories;

namespace FolsomiaSystem.Infra.Data.Providers
{
    public class SqlConnectionStringProvider : ISqlConnectionStringProvider
    {
        private readonly ICredentialSafeRepository _credentialSafeRepository;

        public SqlConnectionStringProvider(ICredentialSafeRepository credentialSafeRepository)
        {
            _credentialSafeRepository = credentialSafeRepository;
        }

        public async Task<string> GetByNameAsync(string name)
        {
            return await GetByNameAsync(name, false);
        }

        public async Task<string> GetByNameAsync(string name, bool invalidateCache)
        {
            var _credential = await _credentialSafeRepository.GetByNameAsync(name, invalidateCache);

            var _cnnBuilder = new MySqlConnectionStringBuilder();

            var _instance = $"{_credential.Hostname}.{_credential.Domain}\\{_credential.Aditional}";
            _cnnBuilder.Server = _instance;
            _cnnBuilder.Database = _credential.Name;
            _cnnBuilder.UserID = _credential.Username;
            _cnnBuilder.Password = _credential.Password;
            _cnnBuilder.UserID = _credential.Username;
            _cnnBuilder.PersistSecurityInfo = false;

            return _cnnBuilder.ConnectionString;
        }
    }
}
