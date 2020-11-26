using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using Binus.SampleWebAPI.Services.Base;
using Binus.WebAPI.Cryptography;
//using Binus.ResourcesCenter.Services.Apps.Common;

namespace Binus.SampleWebAPI.WebAPI.App_Start.JWT.LDAP
{
    public class LDAPCustomOAuthProvider : OAuthAuthorizationServerProvider
    {
      

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
           
            var LDAPService = new LDAPService();
           
            UserPrincipal UserAD = LDAPService.LDAPUserCheck(Crypto.Decrypt(context.UserName.Trim()), Crypto.Decrypt(context.Password.Trim()));
            if (UserAD != null)
            {
               
                    var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {
                             "client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                        }
                    });
                       
                        var ticket = new AuthenticationTicket(SetClaimsIdentity(context), props);
                        context.Validated(ticket);
               

            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is incorrect" + context.UserName.Trim() + " - " + context.Password.Trim());
                //context.Rejected();
            }
           
            return Task.FromResult<object>(null);
        }
     
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
           
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            context.Validated();
            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            

           
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
           

            return identity;
        }
    }
}