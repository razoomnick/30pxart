using System;
using System.Web.Http;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Authorization;

namespace Patterns.Web.Api
{
    [ApiAuthorize]
    public class LikesController : BaseApiController
    {
        private readonly LikesLogic likesLogic = new LikesLogic();

        [HttpPut]
        public ApiLike Put(Guid patternId)
        {
            var apiLike = likesLogic.Like(CurrentUser.Id, patternId);
            return apiLike;
        }

        [HttpDelete]
        public void Delete(Guid patternId)
        {
            likesLogic.Dislike(CurrentUser.Id, patternId);
        }
    }
}