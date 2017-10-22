using Newtonsoft.Json;

namespace ProjetoPO.Api.Token
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public TokenResponse()
        {
            
        }

        public TokenResponse(string accessToken, int expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}
