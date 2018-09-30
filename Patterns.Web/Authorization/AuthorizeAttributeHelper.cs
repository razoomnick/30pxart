using System;
using System.Web;
using Patterns.Logic;

namespace Patterns.Web.Authorization
{
    internal class AuthorizeAttributeHelper
    {
        private readonly UsersLogic usersLogic = new UsersLogic();

        public void SetUserToConetext()
        {
            var tokenCookie = HttpContext.Current.Request.Cookies["Token"];
            var token = tokenCookie != null && !String.IsNullOrEmpty(tokenCookie.Value)
                            ? tokenCookie.Value
                            : HttpContext.Current.Request.Headers["Authorization"];
            if (!String.IsNullOrEmpty(token))
            {
                Guid tokenId;
                if (Guid.TryParse(token, out tokenId))
                {
                    var apiUser = usersLogic.GetApiUserByToken(tokenId);
                    HttpContext.Current.Items["User"] = apiUser;
                }
            }
        }
    }
}