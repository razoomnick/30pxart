using System;
using System.Web.Http;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Authorization;

namespace Patterns.Web.Api
{
    [ApiAuthorize]
    public class CommentsController : BaseApiController
    {
        private readonly CommentsLogic commentsLogic = new CommentsLogic();

        [HttpPut]
        public ApiComment Put(Guid patternId, ApiComment comment)
        {
            var apiComment = commentsLogic.AddComment(patternId, CurrentUser.Id, comment.Text);
            return apiComment;
        }
    }
}
