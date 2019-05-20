namespace OAuth.WebAPI
{
    using System;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using OAuth.WebAPI.Auth.Data;
    using OAuth.WebAPI.Auth.Provider;
    using OAuth.WebAPI.Auth.TokenFormat;
    using OAuth.WebAPI.Models.Auth.Provider;
    using Owin;

    public partial class Startup
    {
        public const string accessTokenKey = "c2VjcmV0a2V5c3Ryb25ndHlwZQ==";
        public void ConfigureAuthAspNetIdentity(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            string publicClientId = "self";
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(publicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true,
            };

            app.UseOAuthBearerTokens(OAuthOptions);
        }

        public void ConfigureAuthCustomImplementation(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            string publicClientId = "self";
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(publicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true,
                AccessTokenFormat = new JwtAccessTokenFormat(TimeSpan.FromMinutes(60), accessTokenKey),
                RefreshTokenProvider = new CustomRefreshTokenProvider(),
            };

            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
