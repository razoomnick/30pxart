using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Patterns.Data.Linq;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public ApiUser GetApiUser(Guid id)
        {
            var apiUser = Context
                .Users
                .Where(u => u.Id == id)
                .SelectApiUser()
                .FirstOrDefault();
            return apiUser;
        }

        public ApiUser GetApiUserByExternalIdAndProvider(string externalId, string provider)
        {
            var apiUser = Context
                .Users
                .Where(u => u.ExternalId == externalId && u.Provider == provider)
                .SelectApiUser()
                .FirstOrDefault();
            return apiUser;
        }

        public User GetUser(Guid id)
        {
            var user = Context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public ApiUser GetApiUserByName(string name)
        {
            var apiUser = Context
                .Users
                .Where(u => u.Name == name)
                .SelectApiUser()
                .FirstOrDefault();
            return apiUser;
        }

        public ApiUser GetApiUserByToken(Guid tokenId)
        {
            var apiUser = Context
                .Tokens
                .Where(t => t.Id == tokenId)
                .Where(t => t.ExpirationTime >= DateTime.UtcNow)
                .Select(t => t.User)
                .SelectApiUser()
                .FirstOrDefault();
            return apiUser;
        }

        public IList<ApiUser> GetApiUsersWhoLikePattern(Guid patternId)
        {
            var apiUsers = Context
                .Likes
                .Where(l => l.Pattern.Id == patternId)
                .OrderByDescending(l => l.CreationTime)
                .Take(100)
                .Select(l => l.User)
                .SelectApiUser()
                .ToList();
            return apiUsers;
        }

        protected override DbSet<User> Collection
        {
            get { return Context.Users; }
        }
    }
}
