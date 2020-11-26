using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Permissions;

namespace Binus.SampleWebAPI.Services.Base
{
    public enum SearchType { All = 0, One = 1 };
    public enum SearchBy { Name = 0, Username = 1, BinusianID = 2 };
    public interface ILDAPService
    {
        //IEnumerable<User> DoAuth(string UserName = null);
        UserPrincipal LDAPUserCheck(string Username = null, string Password = null);

    }
    [DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    public class LDAPService : ILDAPService
    {


        public LDAPService()
        {

        }

        public dynamic LDAPUserSearch(string Key, SearchType Search, SearchBy SearchBy, string Username, string Password)
        {
            dynamic result = null;
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://10.200.200.200", "binus\\" + Username, Password);
                //de.Path = "LDAP://cn=hendry,DC=binus.local";
                //de.AuthenticationType = AuthenticationTypes.;
                DirectorySearcher Searcher = new DirectorySearcher(de);
                Searcher.PageSize = int.MaxValue;
                if (SearchBy == SearchBy.Username)
                {
                    Searcher.Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + Key.Replace("@binus.edu", "") + "))";
                }
                if (SearchBy == SearchBy.Name)
                {
                    Searcher.Filter = "(&(objectCategory=person)(objectClass=user)(cn=*" + Key + "*))";
                }
                if (SearchBy == SearchBy.BinusianID)
                {
                    Searcher.Filter = "(&(objectCategory=person)(objectClass=user)(extensionattribute10=" + Key + "))";
                }

                DirectorySearcher deSearch = new DirectorySearcher();
                if (Search == SearchType.One)
                {
                    result = Searcher.FindOne();
                }
                else
                {
                    result = Searcher.FindAll();
                }

            }
            catch (Exception Ex)
            {

            }
            return result;
        }

        public UserPrincipal LDAPUserCheck(string Username, string Password)
        {



            using (var Context = new PrincipalContext(ContextType.Domain, "10.200.200.200", "binus\\" + Username, Password))
            {

                //Username and password for authentication.
                if (Context.ValidateCredentials(Username, Password))
                {
                    UserPrincipal User = UserPrincipal.FindByIdentity(Context, IdentityType.SamAccountName, Username);




                    return User;
                }
                else
                {
                    return null;
                }

                //return context.ValidateCredentials(Username, Password); 
            }
        }
    }
}
