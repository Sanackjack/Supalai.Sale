using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using Microsoft.AspNetCore.Http.Headers;
using Novell.Directory.Ldap;
using System.DirectoryServices;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private readonly IConfiguration _configuration;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
    }

    

    // public void GetByName(string alias)
   //      {
   //          int ldapPort = LdapConnection.DefaultPort;
   //          string ldapHost = _appSettings.HrsaLdapHost;    // ourOrgName.gov
   //          string loginDn = _appSettings.AdUser;
   //          string password = _appSettings.AdPassword;
   //
   //          string searchBase = _appSettings.HrsaAdSearchBase;
   //          string searchFilter = $"(samaccountname=*{alias}*)";
   //          string[] attributes = new string[] { "cn", "userPrincipalName", "st", "givenname", "samaccountname",
   //              "description", "telephonenumber", "department", "displayname", "name", "mail", "givenName", "sn" };
   //
   //          // List<UserProfileModel> users = new List<UserProfileModel>();
   //          //
   //          // if (alias == null || alias.Trim().Equals(""))
   //          // {
   //          //     return users;
   //          // }
   //
   //          try
   //          {
   //              using (var con = new LdapConnection())
   //              {
   //                  con.Connect(ldapHost, ldapPort);
   //                  con.Bind(loginDn, password);
   //
   //                  LdapSearchQueue queue = con.Search(
   //                      searchBase,
   //                      LdapConnection.ScopeSub,
   //                      searchFilter,
   //                      attributes,
   //                      false,
   //                      (LdapSearchQueue)null,
   //                      (LdapSearchConstraints)null);
   //
   //                  LdapMessage message;
   //
   //                  while ((message = queue.getResponse()) != null)
   //                  {
   //                      if (message is LdapSearchResult)
   //                      {
   //                          LdapEntry entry = ((LdapSearchResult)message).Entry;
   //
   //                          LdapAttributeSet attributeSet = entry.getAttributeSet();
   //
   //                          // users.Add(new UserProfileModel
   //                          // {
   //                          //
   //                          //     Cn = attributeSet.getAttribute("cn")?.StringValue,
   //                          //     UserPrincipalName = attributeSet.getAttribute("userPrincipalName")?.StringValue,
   //                          //     St = attributeSet.getAttribute("st")?.StringValue,
   //                          //     Givenname = attributeSet.getAttribute("givenname")?.StringValue,
   //                          //     Samaccountname = attributeSet.getAttribute("samaccountname")?.StringValue,
   //                          //     Description = attributeSet.getAttribute("description")?.StringValue,
   //                          //     Telephonenumber = attributeSet.getAttribute("telephonenumber")?.StringValue,
   //                          //     Department = attributeSet.getAttribute("department")?.StringValue,
   //                          //     Displayname = attributeSet.getAttribute("displayname")?.StringValue,
   //                          //     Name = attributeSet.getAttribute("name")?.StringValue,
   //                          //     Mail = attributeSet.getAttribute("mail")?.StringValue,
   //                          //     GivenName = attributeSet.getAttribute("givenName")?.StringValue,
   //                          //     Sn = attributeSet.getAttribute("sn")?.StringValue
   //                          // });
   //                      }
   //                  }
   //              }
   //
   //             // return users;
   //          }
   //          catch(Exception ex)
   //          {
   //              Console.WriteLine("Connecting to Error");
   //          }
   //
   //      }

   public BaseResponse Ldap(string username , string pwd)
    {
        
        // use connection = new LdapConnection();
        // connection.Connect(credentials.host, LdapConnection.DefaultPort);
        // connection.Bind($"{credentials.domain}\{credentials.username}", credentials.password);
        // match connection.Connected with
        //     | true ->   
        //         let schema = connection.FetchSchema((connection.GetSchemaDn()));
        // let filter = $"(SAMAccountName={credentials.username})"
        // let searcher = connection.Search(String.Empty, LdapConnection.ScopeBase, filter, null, false);
        // return (searcher |> Some, String.Empty)
        //
        //     | false -> 
        //     raise (Exception()) 
        // return (None, $"Cannot connect to domain {credentials.domain} with user {credentials.username}")
        
       // connection.Bind($"{credentials.domain}\{credentials.username}", credentials.password);
         try
            {
                string ldapHost = "192.168.2.11";
                int ldapPort = 389;
                 // string name = "atgt.sv";
                 // string password = "AfURYBodEimO5kj";
                 string name = username;
                 string password = pwd;
                //string loginDN = "uid=admin,ou=system";
                //string password = "secret";
                //string loginDN = "cn = admin,dc = ramhlocal,dc = com";
                //string password = "admin_pass";
            
                //string loginDN = @"DC=dinesh,DC=com,cn="+name;
                //string loginDN = @"supalai.com\\"+name;
                string loginDN = name+"@supalai.com";
            // new DirectoryEntry("LDAP://192.168.2.11/DC=supalai,DC=com", name, password))
                //string searchBase = "ou=users,o=Company";
                //string searchBase = "ou=supalai";
                //string searchFilter = "objectClass=inetOrgPerson";
                string searchBase = "ou=supalai,dc=ramhlocal,dc=com";
                string searchFilter = "SAMAccountName="+name;
            
                LdapConnection conn = new LdapConnection();
                Console.WriteLine("Connecting to " + ldapHost);
                conn.Connect(ldapHost, LdapConnection.DefaultPort);
                conn.Bind(loginDN, password);
            
                //Search (string @base, int scope, string filter, string[] attrs, bool typesOnly)
                // string[] requiredAttributes = { "cn", "sn", "uid" , "givenName" };
                // LdapSearchResults lsc = (LdapSearchResults)conn.Search(searchBase,
                //                     LdapConnection.ScopeSub,
                //                     searchFilter,
                //                     requiredAttributes,
                //                     false);
                //
                // while (lsc.HasMore())
                // {
                //     LdapEntry nextEntry = null;
                //     try
                //     {
                //         nextEntry = lsc.Next();
                //
                //     }
                //     catch (LdapException e)
                //     {
                //         Console.WriteLine("Error : " + e.LdapErrorMessage);
                //         continue;
                //     }
                //     Console.WriteLine("\n" + nextEntry.Dn);
                //     LdapAttributeSet attributeSet = nextEntry.GetAttributeSet();
                //     System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                //     while (ienum.MoveNext())
                //     {
                //         LdapAttribute attribute = (LdapAttribute)ienum.Current;
                //         string attributeName = attribute.Name;
                //         string attributeVal = attribute.StringValue;
                //         //  Console.WriteLine("\t" + attributeName + "\tvalue  = \t" + attributeVal);
                //        // data = " " +data + "Name  = " + attributeName + ",value  = " + attributeVal +"|";
                //     }
                // }
                // conn.Disconnect();
                
                Console.WriteLine("Ldappppp ok" );
            }
            catch (LdapException e)
            {
                Console.WriteLine("Error :" + e.LdapErrorMessage);
               // return "LdapException";
            }
            catch (Exception e)
            {
                Console.WriteLine("Error :" + e.Message);
              //  return "Exception";
            }
            finally {
            
               // conn.Disconnect();
            }

         return null;
         //return data;
    }
        
  //  }

    // public BaseResponse Ldap()
    // {
    //     // bool userOk = false;
    //     // string realName = string.Empty;
    //     //
    //     // string name = "sriphothong";
    //     // string password = "vt1A36bzQ6";
    //     // try
    //     // {
    //     //
    //     //     using (DirectoryEntry directoryEntry = 
    //     //            new DirectoryEntry("LDAP://192.168.2.11/DC=supalai,DC=com", name, password))
    //     //     {
    //     //         using (DirectorySearcher searcher = new DirectorySearcher(directoryEntry))
    //     //         {
    //     //             searcher.Filter = "(SAMAccountName=" + name + ")";
    //     //             searcher.PropertiesToLoad.Add("displayname");
    //     //
    //     //             SearchResult adsSearchResult = searcher.FindOne();
    //     //
    //     //             if (adsSearchResult != null)
    //     //             {
    //     //                 // if (adsSearchResult.Properties["displayname"].Count == 1)
    //     //                 // {   
    //     //                 //     realName = (string)adsSearchResult.Properties["displayname"][0];
    //     //                 // }
    //     //                 // userOk = true;
    //     //                 Console.WriteLine("Connecti ok");
    //     //             }
    //     //             else
    //     //             {
    //     //                 Console.WriteLine("Connecting to Error");
    //     //             }
    //     //         }
    //     //     }
    //     // }
    //     // catch (Exception e)
    //     // {
    //     //     Console.WriteLine(e);
    //     //     throw;
    //     // }
    //     //
    //     //
    //      return null;
    //     
  


    public BaseResponse Login(LoginRequest login)
    {
        // validate account LDAP
        // Query Data SPLDB Get info and role
        // build token
       // string token = BuildToken();
        
     //  var token=_jwtUtils.GenerateJwtToken("supacjai");
       //terminate session
        //
        // LoginResponse response = new LoginResponse();
        // response.token = token;
        // response.refresh_token = token;
        //
        // UserInfo userInfo = new UserInfo();
        // userInfo.firstname = "test";
        // userInfo.lastname = "test";
        // userInfo.email = "email";
        // userInfo.user_id = "id";
        // userInfo.username = "username";
        // userInfo.role_name = new string[]{"admin","user"};
        // response.user_info = userInfo;
        
       ///*** return new BaseResponse(new StatusResponse(), token);
       return new BaseResponse(new StatusResponse(), "");
    }

    public BaseResponse RefreshToken(string userId)
    {
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(userId),
            refresh_token = _jwtUtils.GenerateRefreshToken(userId)
        };
        return new BaseResponse(new StatusResponse(), response);
    }
}