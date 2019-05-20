using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Infrastructure;
using OAuth.WebAPI.Auth.Data;
using OAuth.WebAPI.Models;
using System;
using System.Threading.Tasks;

namespace OAuth.WebAPI.Auth.Provider
{
    public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public const string ClientId = "mobile_app";

        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).RunSynchronously();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");

            ApplicationDbContext dbContext = context.OwinContext.Get<ApplicationDbContext>();
            using (AuthRepository _repo = new AuthRepository(dbContext))
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = ClientId,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).RunSynchronously();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "http://localhost" });

            string hashedTokenId = Helper.GetHash(context.Token);

            ApplicationDbContext dbContext = context.OwinContext.Get<ApplicationDbContext>();
            using (AuthRepository _repo = new AuthRepository(dbContext))
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    context.Ticket.Properties.IssuedUtc = DateTime.Now;
                    context.Ticket.Properties.ExpiresUtc = DateTime.Now.AddHours(5);
                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }
    }
}