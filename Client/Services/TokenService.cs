using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<ISConfig> _is4Config;
        private readonly DiscoveryDocumentResponse _discoveryDocumentResponse;

        public TokenService(IOptions<ISConfig> is4Config)
        {
            _is4Config = is4Config;

            using var httpClient = new HttpClient();
            _discoveryDocumentResponse = httpClient.GetDiscoveryDocumentAsync(is4Config.Value.Url).Result;
            if(_discoveryDocumentResponse.IsError)
            {
                throw new Exception("Discovery document error!");
            }
        }
        public async Task<TokenResponse> GetToken(string scope)
        {
            using var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _discoveryDocumentResponse.TokenEndpoint,
                ClientId = _is4Config.Value.ClientName,
                ClientSecret = _is4Config.Value.ClientPassword,
                Scope = scope
            });

            if (tokenResponse.IsError)
            {
                throw new Exception("Token error", tokenResponse.Exception);
            }

            return tokenResponse;
        }
    }
}
