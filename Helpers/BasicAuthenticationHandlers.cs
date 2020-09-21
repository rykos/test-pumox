using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace pumox.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //user credentials hardcoded for brevity
        private readonly string username = "username";
        private readonly string password = "password";

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
         ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        //authentication is too simple, no need for async
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization header");
            }

            AuthenticationHeaderValue headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            byte[] authorizationBytes = Convert.FromBase64String(headerValue.Parameter);
            string creds = Encoding.UTF8.GetString(authorizationBytes);

            if (creds == $"{this.username}:{this.password}")
            {
                //Since user credentials are hardcoded there is no real need to identify users 
                var claims = new[] { new Claim(ClaimTypes.Name, this.username) };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }
            else
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }
        }
    }
}