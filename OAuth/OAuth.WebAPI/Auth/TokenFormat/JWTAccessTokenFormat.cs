namespace OAuth.WebAPI.Auth.TokenFormat
{
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataHandler.Encoder;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    public class JwtAccessTokenFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly TimeSpan expireTimeSpan;
        private readonly string audienceId = "http:///localhost";
        private readonly string issuer = "self";
        private readonly string signKey;

        public JwtAccessTokenFormat(TimeSpan expireTimeSpan, string signKey)
        {
            this.expireTimeSpan = expireTimeSpan;
            this.signKey = signKey;
        }

        public string SignatureAlgorithm
        {
            get { return "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256"; }
        }

        public string DigestAlgorithm
        {
            get { return "http://www.w3.org/2001/04/xmlenc#sha256"; }
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null) throw new ArgumentNullException("data");

            var key = Convert.FromBase64String(signKey);
            DateTime dateTimeNow = DateTime.UtcNow;
            DateTime expires = dateTimeNow.Add(this.expireTimeSpan);
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SignatureAlgorithm, DigestAlgorithm);
            JwtSecurityToken token = new JwtSecurityToken(issuer, audienceId, data.Identity.Claims, dateTimeNow, expires, signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            string symmetricKeyAsBase64 = this.signKey;

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = audienceId,
                ValidIssuer = issuer,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = null;

            var pt = handler.ReadJwtToken(protectedText);
            string t = pt.RawData;

            ClaimsPrincipal claimsPrincipal = handler.ValidateToken(t, tokenValidationParameters, out token);
            if (claimsPrincipal == null)
            {
                return null;
            }

            var identity = claimsPrincipal.Identities.First();

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                IssuedUtc = token.ValidFrom,
                ExpiresUtc = token.ValidTo
            };

            AuthenticationTicket authenticationTicket = new AuthenticationTicket(identity, new AuthenticationProperties());
            return authenticationTicket;
        }
    }
}