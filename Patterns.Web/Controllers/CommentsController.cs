using System;
using System.Web.Mvc;
using Patterns.Logic;
using Patterns.Web.Authorization;

namespace Patterns.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly CommentsLogic commentsLogic = new CommentsLogic();

        [MvcAuthorize(AllowAnonymous = true)]
        public ActionResult Index(Guid id)
        {
            var apiComment = commentsLogic.GetApiComment(id);
            return View("CommentTile", apiComment);
        }
    }
}
