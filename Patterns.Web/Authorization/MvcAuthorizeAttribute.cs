using System;
using System.Web;
using System.Web.Mvc;
using Patterns.Objects.Aggregated;

namespace Patterns.Web.Authorization
{
    public class MvcAuthorizeAttribute: AuthorizeAttribute
    {
        private AuthorizeAttributeHelper authorizeAttributeHelper = new AuthorizeAttributeHelper();

        public Boolean AllowAnonymous { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var authorized = AuthorizeCore(filterContext.HttpContext);
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            if (!authorized && !AllowAnonymous)
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("Login", "Account"));
            }
            var apiUser = (ApiUser)HttpContext.Current.Items["User"];
            if (apiUser != null && String.IsNullOrEmpty(apiUser.Name) && filterContext.ActionDescriptor.ActionName != "Register")
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("Register", "Account"));
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            authorizeAttributeHelper.SetUserToConetext();
            return HttpContext.Current.Items["User"] != null;
        }
    }
}