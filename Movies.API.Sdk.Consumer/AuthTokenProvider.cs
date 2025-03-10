using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Movies.API.Sdk.Consumer;

public class AuthTokenProvider
{
    private readonly HttpClient _client;
    private string _cachedToken = string.Empty;
    private readonly SemaphoreSlim Lock = new(1, 1);

    public AuthTokenProvider(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken))
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
            var expiryTimeText = jwt.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value;
            var expireDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
            if (expireDateTime > DateTime.Now)
            {
                return _cachedToken;
            }
        }
        await Lock.WaitAsync();

        var response = await _client.PostAsJsonAsync("http://localhost:5003/token", new
        {
            userid = "d8566de3-b1a6-4a9b-b842-8e3887a82e42",
            email = "emran@mhoemran.com",
            customClaims = new Dictionary<string, object>
            {
                { "admin", true },
                { "trusted_member", true }
            }
        });
        
        var newToken = await response.Content.ReadAsStringAsync();
        _cachedToken = newToken;
        Lock.Release();
        return newToken;

    }


    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();    
        return dateTime;;
    }
}