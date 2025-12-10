namespace Employee.WebApi.Controllers.Auth.AuthenticateUser
{
    public class AuthenticateUserResponse
    {
        public string Token { get; set; } = string.Empty;
        public Guid Id { get; set; }
    }
}
