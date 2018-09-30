using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Resources;
using Patterns.Svg;
using Patterns.Web.Attributes;
using Patterns.Web.Authorization;
using Patterns.Web.Models.Viewer;

namespace Patterns.Web.Controllers
{
    [CompressContent]
    public class PatternsController : BaseController
    {
        private readonly PatternsLogic patternsLogic = new PatternsLogic();
        private readonly UsersLogic usersLogic = new UsersLogic();

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult Best(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetTopPatterns(currentUserId, skip, Configuration.PageSize);
            return GetGallery(patterns, skip);
        }

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult Index(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetRecentPatterns(currentUserId, skip, Configuration.PageSize);
            return GetGallery(patterns, skip, skip == 0);
        }

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult MostCommented(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetMostCommentedPatterns(currentUserId, skip, Configuration.PageSize);
            return GetGallery(patterns, skip);
        }

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult RecentUnregistered(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetRecentUnregisteredPatterns(currentUserId, skip, Configuration.PageSize);
            return GetGallery(patterns, skip);
        }

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult BestUnregistered(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetTopUnregisteredPatterns(currentUserId, skip, Configuration.PageSize);
            return GetGallery(patterns, skip);
        }

        [MvcAuthorize]
        public ActionResult Feed(String name, int skip = 0)
        {
            var apiUser = usersLogic.GetApiUserByName(name);
            var patterns = patternsLogic.GetUsersFeedPatterns(apiUser.Id, CurrentUser.Id, skip, Configuration.PageSize);
            return GetGallery(patterns, skip);
        }

        [MvcAuthorize]
        public ActionResult Drafts(int skip = 0)
        {
            var patterns = patternsLogic.GetUsersDraftPatterns(CurrentUser.Id, skip, Configuration.PageSize);
            return GetGallery(patterns, skip);
        }

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult Pattern(Guid id)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            ActionResult result = new HttpNotFoundResult();
            var apiPattern = patternsLogic.Get(id, currentUserId);
            if (apiPattern != null)
            {
                var model = new PatternModel()
                {
                    Likes = patternsLogic.GetUsersWhoLike(id),
                    Pattern = apiPattern,
                    CurrentUser = CurrentUser,
                    OnePatternOnPage = true,
                    Title = apiPattern.Name,
                    Description = Strings.APatternBy + " " + (apiPattern.Author != null ? apiPattern.Author.Name : "") + ". " + Strings.DrawYourPattern,
                    ImageSrc = @Url.Action("Large", "Images", new { id = apiPattern.ImageId }, Request.Url.Scheme)
                };
                result = View(model);
            }
            return result;
        }

        private ActionResult GetGallery(IList<ApiPattern> patterns, int skipped, Boolean showDescription = false)
        {
            var model = GetGalleryModel(patterns, skipped, showDescription);
            var result = Request.IsAjaxRequest()
                             ? View("Gallery", model)
                             : View("Patterns", model);
            return result;
        }

        [HttpPost]
        [MvcAuthorize]
        public ActionResult DraftState(Guid id)
        {
            patternsLogic.ChangeDraftState(id, CurrentUser);
            return new RedirectResult(Url.Action("Pattern", "Patterns", new {id = id}));
        }

        [MvcAuthorize(AllowAnonymous = true)]
        public ActionResult Vector(Guid id, String filter)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var apiPattern = patternsLogic.GetWithState(id, currentUserId);
            var svg = ImagesLogic.GetVector(apiPattern, filter);
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = id + ".svg",
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", contentDisposition.ToString());
            return new FileStreamResult(svg, "image/svg+xml");
        }
    }
}
