using System;

namespace GabrielBertasso.BackendIntegration.DTOs
{
    [Serializable]
    public class RegisterRequest
    {
        public string Name;
        public string Email;
        public string Password;
        public string ConfirmPassword;

        public RegisterRequest() { }

        public RegisterRequest(string name, string email, string password, string confirmPassword)
        {
            Name = name;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
