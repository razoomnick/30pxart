using System;
using System.Web;
using System.Web.Mvc;
using Patterns.Logic;
using Patterns.Web.Attributes;
using Patterns.Web.Authorization;
using Patterns.Web.Models.Viewer;

namespace Patterns.Web.Controllers
{
    [CompressContent]
    public class UsersController : BaseController
    {
        private readonly UsersLogic usersLogic = new UsersLogic();
        private readonly PatternsLogic patternsLogic = new PatternsLogic();

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult Profile(String name, int skip = 0)
        {
            ActionResult result = new HttpNotFoundResult();
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var apiUser = usersLogic.GetApiUserByNameWithFollowings(name, currentUserId);
            if (apiUser != null)
            {
                var patterns = patternsLogic.GetUsersPatterns(apiUser.Id, currentUserId, skip, Configuration.PageSize);
                var model = new UserModel()
                {
                    Gallery = GetGalleryModel(patterns, skip),
                    User = apiUser,
                    CurrentUser = CurrentUser
                };
                result = Request.IsAjaxRequest()
                             ? View("Gallery", model.Gallery)
                             : View(model);
            }
            return result;
        }

        [MvcAuthorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult Avatar(HttpPostedFileBase avatar)
        {
            usersLogic.ChangeAvatar(CurrentUser, avatar.InputStream);
            return new RedirectResult(Url.Action("Profile", new { name = CurrentUser.Name }));
        }

        [MvcAuthorize(AllowAnonymous = true)]
        public ActionResult Tile(String name)
        {
            var apiUser = usersLogic.GetApiUserByName(name);
            return View("UserTile", apiUser);
        }
    }
}
