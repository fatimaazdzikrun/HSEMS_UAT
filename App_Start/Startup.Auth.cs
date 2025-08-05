using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;

namespace HSEMS
{
    public class StartupAuth
    {
        private static string myclientId = ConfigurationManager.AppSettings["ClientId"];
        private static string mytenant = ConfigurationManager.AppSettings["Tenant"];
        private static string myaadInstance = ConfigurationManager.AppSettings["AADInstance"];
        private static string mypostLogoutRedirectUri = ConfigurationManager.AppSettings["PostLogoutRedirectUri"];
        private string authority = string.Format(CultureInfo.InvariantCulture, myaadInstance, mytenant);
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(

                            new OpenIdConnectAuthenticationOptions
                            {
                                ClientId = myclientId,
                                Authority = authority,
                                PostLogoutRedirectUri = mypostLogoutRedirectUri,
                                Notifications = new OpenIdConnectAuthenticationNotifications
                                {
                                    AuthenticationFailed = (context) =>
                                    {
                                        context.HandleResponse();
                                        context.OwinContext.Response.Redirect("/Home/Index");
                                        return Task.FromResult(0);
                                    }
                                }
                            });


        }
    }
}