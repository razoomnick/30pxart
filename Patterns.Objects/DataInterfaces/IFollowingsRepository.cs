using System;
using System.Collections.Generic;
using Patterns.Objects.Aggregated;
using Patterns.Objects.Entities;

namespace Patterns.Objects.DataInterfaces
{
    public interface IFollowingsRepository: IBaseRepository<Following>
    {
        Following GetFollowing(Guid publisherId, Guid subscriberId);
        ApiFollowing GetApiFollowing(Guid publisherId, Guid subscriberId);
        void Delete(Following following);
        List<ApiFollowing> GetFollowers(Guid id);
        List<ApiFollowing> GetFollowings(Guid id);
        int GetFollowersCount(Guid id);
        int GetFollowingsCount(Guid id);
    }
}
