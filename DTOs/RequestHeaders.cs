using Lotest.Enums;

namespace Lotest.DTOs
{
    public class RequestHeaders
    {
        public RequestHeaders(AuthenticationType authenticationType, string target) 
        {
            AuthType = authenticationType;
            Target = target;
        }

        public RequestHeaders(AuthenticationType authenticationType, string target, string token)
        {
            AuthType = authenticationType;
            Target = target;
            Token = token;
        }

        public RequestHeaders(AuthenticationType authenticationType, string target, string username, string password)
        {
            AuthType = authenticationType;
            Target = target;
            UserName = username;
            Password = password;
        }

        public string Target { get; }
        public string Token { get; } = string.Empty;
        public string UserName { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public AuthenticationType AuthType { get; }
    }
}
