namespace OAuth.WebAPI
{
    using System;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using OAuth.WebAPI.Auth.Data;
    using OAuth.WebAPI.Auth.Provider;
    using OAuth.WebAPI.Models.Auth.Provider;
    using Owin;

    public partial class Startup
    {
        public void ConfigureAuthAspNetIdentity(IAppBuilder app)
        {
            //CreatePerOwinContext registers a static callback which your application will use to get back a new instance of a specified type.
            //This callback will be called once per request and will store the object/ objects in OwinContext so that you will be able to use them throughout the application.
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Configure the application for OAuth based flow
            string publicClientId = "self";
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(publicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true,
            };


            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }

        public void ConfigureAuthCustomImplementation(IAppBuilder app)
        {
            //CreatePerOwinContext registers a static callback which your application will use to get back a new instance of a specified type.
            //This callback will be called once per request and will store the object/ objects in OwinContext so that you will be able to use them throughout the application.
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Configure the application for OAuth based flow
            string publicClientId = "self";
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(publicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true,
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
