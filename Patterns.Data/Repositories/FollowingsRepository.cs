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
    public class FollowingsRepository: BaseRepository<Following>, IFollowingsRepository
    {
        public Following GetFollowing(Guid publisherId, Guid subscriberId)
        {
            var following = Context
                .Followings
                .FirstOrDefault(f => f.Publisher.Id == publisherId && f.Subscriber.Id == subscriberId);
            return following;
        }

        public ApiFollowing GetApiFollowing(Guid publisherId, Guid subscriberId)
        {
            var following = Context
                .Followings
                .Where(f => f.Publisher.Id == publisherId && f.Subscriber.Id == subscriberId)
                .SelectApiFollowing()
                .FirstOrDefault();
            return following;
        }

        public List<ApiFollowing> GetFollowers(Guid id)
        {
            var followers = Context
                .Followings
                .Where(f => f.Publisher.Id == id)
                .OrderByDescending(f => f.CreationTime)
                .Take(500)
                .SelectApiFollowing()
                .ToList();
            return followers;
        }

        public List<ApiFollowing> GetFollowings(Guid id)
        {
            var followings = Context
                .Followings
                .Where(f => f.Subscriber.Id == id)
                .OrderByDescending(f => f.CreationTime)
                .Take(500)
                .SelectApiFollowing()
                .ToList();
            return followings;
        }

        public int GetFollowersCount(Guid id)
        {
            var followersCount = Context
                .Followings
                .Count(f => f.Publisher.Id == id);
            return followersCount;
        }

        public int GetFollowingsCount(Guid id)
        {
            var followingsCount = Context
                .Followings
                .Count(f => f.Subscriber.Id == id);
            return followingsCount;
        } 

        public void Delete(Following following)
        {
            Context.Followings.Remove(following);
            Context.SaveChanges();
        }

        protected override DbSet<Following> Collection
        {
            get { return Context.Followings; }
        }
    }
}
