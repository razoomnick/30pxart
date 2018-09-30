using System;
using System.Collections.Generic;

namespace Patterns.Objects.Aggregated
{
    public class ApiUser
    {
        public ApiUser()
        {
            Followers = new List<ApiFollowing>();
            Followings = new List<ApiFollowing>();
        }

        public Guid Id { get; set; }
        public String Name { get; set; }
        public Guid? AvatarImageId { get; set; }
        public String AvatarCdnUrl { get; set; }
        public DateTime RegisteredTime { get; set; }
        public Boolean IsFollowed { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public IList<ApiFollowing> Followers { get; set; }
        public IList<ApiFollowing> Followings { get; set; }
    }
}