namespace Application.Dtos.Auth
{
    using System.Collections.Generic;

    public class AuthenticationResponse
    {
        public string AuthenticationResponseId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

    }
}
