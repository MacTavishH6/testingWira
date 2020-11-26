using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using Binus.WebAPI.Model.Common;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;
using Binus.SampleWebAPI.Data.DBContext.Serpong.MSSQL;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MSSQL.Backend;
using Binus.WebAPI.Cryptography;

namespace Binus.SampleWebAPI.WebAPI.App_Start.JWT.BookDB
{
    public class BookDBCustomOAuthProvider : OAuthAuthorizationServerProvider
    {


        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext Context)
        {
            var Param = new[] { new SqlParameter("@Username", Crypto.Decrypt(Context.UserName)), new SqlParameter("@Password", Crypto.Decrypt(Context.Password)) };
            var User = Context.OwinContext.Get<BookDBMSSQLDBContext>().Database
                .SqlQuery<msUser>("bn_BookDB_GetUser @Username,@Password", Param)
                .FirstOrDefault<msUser>();

            if (User != null)
            {
              
                    var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {
                             "client_id", (Context.ClientId == null) ? string.Empty : Context.ClientId
                        }
                    });
                      
                        var ticket = new AuthenticationTicket(SetClaimsIdentity(Context), props);
                Context.Validated(ticket);
                

            }
            else
            {
                Context.SetError("invalid_grant", "The user name or password is incorrect");
               
            }
           
            return Task.FromResult<object>(null);
        }
   
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext Context)
        {
           
            string ClientID = string.Empty;
            string ClientSecret = string.Empty;
            if (!Context.TryGetBasicCredentials(out ClientID, out ClientSecret))
            {
                Context.TryGetFormCredentials(out ClientID, out ClientSecret);
            }
            Context.Validated();
            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext Context)
        {
            var Identity = new ClaimsIdentity("JWT");
            Identity.AddClaim(new Claim(ClaimTypes.Name, Context.UserName));
            Identity.AddClaim(new Claim("sub", Context.UserName));


            /*var userRoles = context.OwinContext.Get<BookUserManager>().GetRoles(user.Id);
            foreach (var role in userRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }*/
            Identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
           

            return Identity;
        }
    }
}