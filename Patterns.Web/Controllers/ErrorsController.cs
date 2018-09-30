using System.Web.Mvc;
using Patterns.Resources;
using Patterns.Web.Authorization;
using Patterns.Web.Models;

namespace Patterns.Web.Controllers
{
    [MvcAuthorize(AllowAnonymous = true)]
    public class ErrorsController : BaseController
    {
        public ActionResult NotFound()
        {
            var model = new BaseModel
                {
                    CurrentUser = CurrentUser,
                    Title = Strings.PageNotFound
                };
            return View(model);
        }

        public ActionResult Error()
        {
            var model = new BaseModel
                {
                    Title = Strings.ErrorOccurred
                };
            return View(model);
        }
    }
}
