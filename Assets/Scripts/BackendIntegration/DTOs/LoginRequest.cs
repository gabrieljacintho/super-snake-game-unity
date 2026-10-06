using System;

namespace GabrielBertasso.BackendIntegration.DTOs
{
    [Serializable]
    public class LoginRequest
    {
        public string Email;
        public string Password;

        public LoginRequest() { }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
