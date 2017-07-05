using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using Users.Infrastructure;

namespace Users
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);

            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);  //на базе юзеров
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);  //на базе ролей

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                ClientId = "142161729771-knfisofc81t7gr55h79m96gq9q6vlkus.apps.googleusercontent.com",
                ClientSecret = "2bmhba9gFBORpEHUMIwVQXfH",
                Caption = "Авторизаціия через Google+",
                CallbackPath = new PathString("/GoogleLoginCallback"),
                AuthenticationMode = AuthenticationMode.Passive,
                SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType(),
                BackchannelTimeout = TimeSpan.FromSeconds(60),
                BackchannelHttpHandler = new System.Net.Http.WebRequestHandler(),
                BackchannelCertificateValidator = null,
                Provider = new GoogleOAuth2AuthenticationProvider()
            }
            );
        }
    }
}