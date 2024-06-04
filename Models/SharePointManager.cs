using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;

namespace HSEMS.Models
{
    public class SharePointManager
    {

        public static ClientContext ConnectToSharePoint(string siteUrl, string username, string password)
        {
            ClientContext context = new ClientContext(siteUrl);
            context.AuthenticationMode = ClientAuthenticationMode.Default;
            context.Credentials = new SharePointOnlineCredentials(username, ConvertToSecureString(password));
            return context;
        }
        private static SecureString ConvertToSecureString(string password)
        {
            SecureString securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            securePassword.MakeReadOnly();
            return securePassword;
        }

    }
}