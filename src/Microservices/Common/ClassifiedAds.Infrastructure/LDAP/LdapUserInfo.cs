namespace ClassifiedAds.Infrastructure.LDAP;

public class LdapUserInfo
{
        public string Cn { get; set; }
        public string UserPrincipalName { get; set; }
        public string Samaccountname { get; set; }
        public string Displayname { get; set; }
        public string GivenName { get; set; }
        public string Sn { get; set; }
}
