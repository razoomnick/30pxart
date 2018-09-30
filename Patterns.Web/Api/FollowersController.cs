using System;
using System.Web.Http;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Authorization;

namespace Patterns.Web.Api
{
    [ApiAuthorize]
    public class FollowersController: BaseApiController
    {
        private readonly FollowingsLogic followingsLogic = new FollowingsLogic();
        private readonly UsersLogic usersLogic = new UsersLogic();

        [HttpPut]
        public ApiFollowing Put(String userName)
        {
            var publisher = usersLogic.GetApiUserByName(userName);
            var apiFollowing = followingsLogic.Follow(publisher.Id, CurrentUser.Id);
            return apiFollowing;
        }

        [HttpDelete]
        public void Delete(String userName)
        {
            var publisher = usersLogic.GetApiUserByName(userName);
            followingsLogic.Unfollow(publisher.Id, CurrentUser.Id);
        }
    }
}