using System;
using System.Globalization;

namespace GabrielBertasso.BackendIntegration.DTOs
{
    [Serializable]
    public class AuthenticationResponse
    {
        public string name;
        public string email;
        public string token;
        public string expiration;
        public string refreshToken;
        public string refreshTokenExpiration;

        public DateTime ExpirationUtc => DateTime.Parse(expiration, null, DateTimeStyles.AdjustToUniversal);

        public AuthenticationResponse() { }

        public AuthenticationResponse(string name, string email, string token, string expiration, string refreshToken, string refreshTokenExpiration)
        {
            this.name = name;
            this.email = email;
            this.token = token;
            this.expiration = expiration;
            this.refreshToken = refreshToken;
            this.refreshTokenExpiration = refreshTokenExpiration;
        }
    }
}
