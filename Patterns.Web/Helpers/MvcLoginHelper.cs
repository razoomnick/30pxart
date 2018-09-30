using System.Web;
using System.Web.Mvc;

namespace Patterns.Web.Helpers
{
    public class MvcLoginHelper : LoginHelper
    {
        protected override string RedirectUrl
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var redirectUrl = urlHelper.Action("Login", "Account", null, HttpContext.Current.Request.Url.Scheme);
                return redirectUrl;
            }
        }
    }
}