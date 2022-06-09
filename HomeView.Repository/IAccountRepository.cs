using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeView.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace HomeView.Repository
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> CreateAsync(UserIdentity user,
            CancellationToken cancellationToken);

        public Task<UserIdentity> GetByUsernameAsync(string normalizedUsername,
            CancellationToken cancellationToken);

        public Task<int> UpdatePictureAsync(int userId, int photoId);

        public Task<string> GetUsernameByIdAsync(int userId);
    }
}
