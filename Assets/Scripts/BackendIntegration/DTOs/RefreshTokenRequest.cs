using System;

namespace GabrielBertasso.BackendIntegration.DTOs
{
    [Serializable]
    public class RefreshTokenRequest
    {
        public string Token;
        public string RefreshToken;

        public RefreshTokenRequest() { }

        public RefreshTokenRequest(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
