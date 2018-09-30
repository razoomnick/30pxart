using System;
using System.Data.Entity;
using System.Linq;
using Patterns.Data.Linq;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public class LikesRepository: BaseRepository<Like>, ILikesRepository
    {
        public ApiLike GetApiLike(Guid userId, Guid patternId)
        {
            var apiLike = Context
                .Likes
                .Where(l => l.User.Id == userId)
                .Where(l => l.Pattern.Id == patternId)
                .SelectApiLike()
                .FirstOrDefault();
            return apiLike;
        }

        public Like GetLike(Guid userId, Guid patternId)
        {
            var apiLike = Context
                .Likes
                .FirstOrDefault(l => l.User.Id == userId && l.Pattern.Id == patternId);
            return apiLike;
        }

        public void Delete(Like like)
        {
            Context.Likes.Remove(like);
            Context.SaveChanges();
        }

        protected override DbSet<Like> Collection
        {
            get { return Context.Likes; }
        }
    }
}
