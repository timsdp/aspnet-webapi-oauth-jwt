using Microsoft.Win32.SafeHandles;
using OAuth.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace OAuth.WebAPI.Auth.Data
{
    public class AuthRepository : IDisposable
    {
        private ApplicationDbContext DbContext { get; set; }

        public AuthRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = DbContext.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            DbContext.RefreshTokens.Add(token);

            return await DbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await DbContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                DbContext.RefreshTokens.Remove(refreshToken);
                return await DbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            DbContext.RefreshTokens.Remove(refreshToken);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await DbContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return DbContext.RefreshTokens.ToList();
        }

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
    }
}