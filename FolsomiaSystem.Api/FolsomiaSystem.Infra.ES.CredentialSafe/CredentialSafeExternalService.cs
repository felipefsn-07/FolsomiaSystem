using FolsomiaSystem.Application.Exceptions;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Application.Interfaces.Providers;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Infra.ES.CredentialSafe.DTOs;
using FolsomiaSystem.Infra.ES.CredentialSafe.Mappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FolsomiaSystem.Infra.ES.CredentialSafe
{
    public class CredentialSafeExternalService : ICredentialSafeExternalService
    {
        #region Constants

        private const string _className = "CredentialSafeExternalService";

        #endregion

        #region Variables

        private readonly ILogger<CredentialSafeExternalService> _logger;
        private readonly ICredentialSafeConfigProvider _credentialSafeConfigProvider;
        private readonly HttpClient _httpClient;

        #endregion

        #region Constructors

        public CredentialSafeExternalService(
            ILogger<CredentialSafeExternalService> logger,
            ICredentialSafeConfigProvider credentialSafeConfigProvider,
            IHttpClientFactory httpClientFactory
        )
        {
            _logger = logger;
            _credentialSafeConfigProvider = credentialSafeConfigProvider;
            _httpClient = httpClientFactory?.CreateClient();
        }

        #endregion

        #region ICredentialSafeExternalService Implementation

        public async Task<Credential> GetByNameAsync(string name)
        {
            var _config = await _credentialSafeConfigProvider.GetByNameAsync(name);
            return await CallApi(_config);
        }

        #endregion

        #region Private Methods

        private async Task<Credential> CallApi(CredentialSafeConfig config)
        {
            var _methodName = "CallApi";

            WriteDebug(_methodName, $"Invoking Senha Segura webservice...");

            var _request = CreateRequest(config);
            HttpResponseMessage _response;

            try
            {
                _response = await _httpClient.SendAsync(_request);
            }
            catch(Exception ex)
            {
                throw new CredentialRetrievalFailedException("Failed to retrieve credential from the Credential Safe webservice", ex);
            }

            return await HandleApiResponse(_response, config);
        }

        private async Task<Credential> HandleApiResponse(HttpResponseMessage response, CredentialSafeConfig config)
        {
            if (response.Content == null)
                throw new CredentialRetrievalFailedException("Null response content received from Credential Safe webservice");

            var _contentString = await response.Content.ReadAsStringAsync();
            var _content = JsonConvert.DeserializeObject<SenhaSeguraResponse>(_contentString);

            if (_content.Response == null)
                throw new CredentialRetrievalFailedException("No response details received from Credential Safe webservice");

            var _contentResponseJson = JsonConvert.SerializeObject(_content.Response) ?? "<empty>";
            
            switch(_content.Response.Status)
            {
                case (int)HttpStatusCode.OK:

                    if (_content.Senha == null)
                        throw new CredentialRetrievalFailedException($"No credential detail received. Response: '{_contentResponseJson}'");

                    SetName(_content, config);
                    return SenhaSeguraMapper.DtoToDomain(_content.Senha);

                case (int)HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(_contentResponseJson);

                case (int)HttpStatusCode.NotFound:
                    throw new CredentialNotFoundException($"Credential with Id '{config.Id}' not found");

                case (int)HttpStatusCode.InternalServerError:

                    // O Senha Segura não envia detalhes (json) para erros internos (500)
                    throw new CredentialRetrievalFailedException("InternalServerError (500) invoking Senha Segura webservice. Please contact the Security team to check the execution logs");

                default:
                    throw new CredentialRetrievalFailedException(_contentResponseJson);
            }
        }

        private HttpRequestMessage CreateRequest(CredentialSafeConfig config)
        {
            var _methodName = "CreateRequest";

            var _requestUrl = $"{config.BaseUrl.TrimEnd('/')}/iso/coe/senha/{config.Id}";

           /* var _oauthClient = new OAuthRequest
            {
                Method = "GET",
                Type = OAuthRequestType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ConsumerKey = config.ConsumerKey,
                ConsumerSecret = config.ConsumerSecret,
                Token = config.TokenKey,
                TokenSecret = config.TokenSecret,
                RequestUrl = _requestUrl
            };

            var _authHeader = _oauthClient.GetAuthorizationHeader();*/

            var _request = new HttpRequestMessage(HttpMethod.Get, _requestUrl);

           // WriteDebug(_methodName, $"Auth = {_authHeader}");

           // _request.Headers.Add("Authorization", _authHeader);

            return _request;
        }

        private void SetName(SenhaSeguraResponse response, CredentialSafeConfig config)
        {
            response.Senha.Name = config.Name;
        }

        private void WriteDebug(string methodName, string message)
        {
            _logger.LogDebug($"[{_className}] {methodName} - {message}");
        }

        #endregion
    }
}