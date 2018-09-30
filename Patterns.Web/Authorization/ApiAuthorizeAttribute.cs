using System.Web;
using System.Web.Http;

namespace Patterns.Web.Authorization
{
    public class ApiAuthorizeAttribute: AuthorizeAttribute
    {
        private readonly AuthorizeAttributeHelper authorizeAttributeHelper = new AuthorizeAttributeHelper();

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            authorizeAttributeHelper.SetUserToConetext();
        }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var user = HttpContext.Current.Items["User"];
            return user != null;
        }
    }
}