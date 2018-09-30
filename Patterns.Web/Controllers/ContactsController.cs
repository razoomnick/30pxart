using System.Web.Mvc;
using Patterns.Resources;
using Patterns.Web.Authorization;
using Patterns.Web.Models;

namespace Patterns.Web.Controllers
{
    public class ContactsController : BaseController
    {
        [MvcAuthorize(AllowAnonymous = true)]
        public ActionResult Index()
        {
            var model = new BaseModel()
                {
                    CurrentUser = CurrentUser,
                    Title = Strings.DrawPatternsOnline
                };
            return View(model);
        }
    }
}
