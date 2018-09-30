using System;
using System.Web.Mvc;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Authorization;
using Patterns.Web.Models.Editor;

namespace Patterns.Web.Controllers
{
    public class EditController: BaseController
    {
        private readonly PatternsLogic patternsLogic = new PatternsLogic();

        [MvcAuthorize(AllowAnonymous = true)]
        [OutputCache(Duration = 7, VaryByCustom = "Token")]
        public ActionResult Index(Guid? id = null)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var pattern = new ApiPattern() { };

            if (id != null)
            {
                var savedPattern = patternsLogic.GetWithState(id.Value, currentUserId);
                if (savedPattern != null && savedPattern.Author != null && savedPattern.Author.Id == currentUserId)
                {
                    pattern = savedPattern;
                }
            }

            var model = new IndexModel()
                {
                    Pattern = pattern,
                    CurrentUser = CurrentUser
                };

            return View(model);
        }
    }
}