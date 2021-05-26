using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PdfConversionFunctionApp
{
    public class AuthenticationService
    {
        public static async Task<string> GetAccessTokenAsync(ApiConfig _apiConfig)
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _apiConfig.ClientId),
                new KeyValuePair<string, string>("client_secret", _apiConfig.ClientSecret),
                new KeyValuePair<string, string>("scope", _apiConfig.Scope),
                new KeyValuePair<string, string>("grant_type", _apiConfig.GrantType),
                new KeyValuePair<string, string>("resource", _apiConfig.Resource)
            };
            var client = new HttpClient();
            var requestUrl = $"{_apiConfig.Endpoint}{_apiConfig.TenantId}/oauth2/token";
            var requestContent = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(requestUrl, requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonConvert.DeserializeObject(responseBody);
            return tokenResponse?.access_token;
        }
    }
}
