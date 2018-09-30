using System;
using Patterns.Data.Repositories;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Logic
{
    public class FollowingsLogic
    {
        private readonly IFollowingsRepository followingsRepository = new FollowingsRepository();
        private readonly IUsersRepository usersRepository = new UsersRepository();

        public ApiFollowing Follow(Guid publisherId, Guid subscriberId)
        {
            var apiFollowing = followingsRepository.GetApiFollowing(publisherId, subscriberId);
            if (apiFollowing == null)
            {
                var publisher = usersRepository.GetById(publisherId);
                var subscriber = usersRepository.GetById(subscriberId);
                if (publisher != null && subscriber != null)
                {
                    var following = new Following()
                    {
                        Publisher = publisher,
                        Subscriber = subscriber
                    };
                    followingsRepository.Save(following);
                    apiFollowing = followingsRepository.GetApiFollowing(publisherId, subscriberId);
                }
            }
            return apiFollowing;
        }

        public void Unfollow(Guid publisherId, Guid subscriberId)
        {
            var following = followingsRepository.GetFollowing(publisherId, subscriberId);
            if (following != null)
            {
                followingsRepository.Delete(following);
            }
        }
    }
}
