using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
namespace ClassifiedAds.Infrastructure.LDAP;

public interface ILDAPUtils
{
    public bool CheckUserLoginLdap(string userName , string password);
    public LdapUserInfo? GetInfoUserLoginLdap(string userName , string password);
}

public class LDAPUtils : ILDAPUtils
{
    private readonly IConfiguration _configuration;

    public LDAPUtils(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public bool CheckUserLoginLdap(string userName, string password)
    {
        bool loginStatus = false;
        string searchBase = _configuration["LDAPSettings:LDAPPath"];
        string searchFilter = $"(samaccountname=*{userName}*)";
        string[] attributes = new string[] { "cn", "userPrincipalName", "givenname", "samaccountname", "displayname", "givenName", "sn" };
        try
        {
            using (var con = new LdapConnection())
            {
                con.Connect(_configuration["LDAPSettings:LDAPHost"], LdapConnection.DefaultPort);
                con.Bind(userName + "@" + _configuration["LDAPSettings:LDAPDomain"], password);
                loginStatus = true;
                Console.WriteLine("Connecting to OK");

                con.Disconnect();
            }
        }
        catch (LdapException ex)
        {Console.WriteLine("Connecting to false");

            loginStatus = false;
            throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
        }
        catch (Exception ex)
        {Console.WriteLine("Connecting to false");
            loginStatus = false;
            throw new AuthenicationErrorException(ResponseData.AUTHENTICATION_FAIL);
        }

        return loginStatus;
    }

    public LdapUserInfo GetInfoUserLoginLdap(string userName, string password)
    {
        LdapUserInfo ldapUserInfo = null;
        string searchBase = _configuration["LDAPSettings:LDAPPath"];
        string searchFilter = $"(samaccountname=*{userName}*)";
        string[] attributes = new string[] { "cn", "userPrincipalName", "givenname", "samaccountname", "displayname", "givenName", "sn" };
        try
        {
           using (var con = new LdapConnection())
           {
               con.Connect(_configuration["LDAPSettings:LDAPHost"], LdapConnection.DefaultPort);
               con.Bind(userName + "@" + _configuration["LDAPSettings:LDAPDomain"], password);

               Console.WriteLine("Connecting to OK");

               LdapSearchQueue queue = con.Search(
                   searchBase,
                   LdapConnection.ScopeSub,
                   searchFilter,
                   attributes,
                   false,
                   (LdapSearchQueue)null, (LdapSearchConstraints)null);

               LdapMessage message;

               while ((message = queue.GetResponse()) != null)
               {
                   if (message is LdapSearchResult)
                   {
                       LdapEntry entry = ((LdapSearchResult)message).Entry;

                       LdapAttributeSet attributeSet = entry.GetAttributeSet();

                       ldapUserInfo = new LdapUserInfo()
                       {
                           Cn = attributeSet.GetAttribute("cn")?.StringValue,
                           UserPrincipalName = attributeSet.GetAttribute("userPrincipalName")?.StringValue,
                           Samaccountname = attributeSet.GetAttribute("samaccountname")?.StringValue,
                           Displayname = attributeSet.GetAttribute("displayname")?.StringValue,
                           GivenName = attributeSet.GetAttribute("givenName")?.StringValue,
                           Sn = attributeSet.GetAttribute("sn")?.StringValue,
                       };
                   }
               }

               con.Disconnect();
           }
        }
        catch (LdapException ex)
        {
            throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
        }
        catch (Exception ex)
        {
            throw new AuthenicationErrorException(ResponseData.AUTHENTICATION_FAIL);
        }

        return ldapUserInfo;
    }
}
