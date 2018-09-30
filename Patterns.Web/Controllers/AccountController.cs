using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Patterns.Logic;
using Patterns.Resources;
using Patterns.Web.Authorization;
using Patterns.Web.Helpers;
using Patterns.Web.Models.Viewer;

namespace Patterns.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UsersLogic usersLogic = new UsersLogic();
        private readonly MvcLoginHelper loginHelper = new MvcLoginHelper();

        [MvcAuthorize(AllowAnonymous = true)]
        public ActionResult Login(String code, String provider, String redirectUrl)
        {
            ActionResult result;
            if (String.IsNullOrEmpty(code))
            {
                result = RedirectToAction("Index", "Patterns");
            }
            else
            {
                result = Authenticate(code, provider, redirectUrl);
            }
            return result;
        }

        [MvcAuthorize]
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [MvcAuthorize]
        public ActionResult Logout()
        {
            var tokenCookie = HttpContext.Request.Cookies["Token"];
            if (tokenCookie != null)
            {
                Guid token;
                if (Guid.TryParse(tokenCookie.Value, out token))
                {
                    usersLogic.InvalidateToken(token);
                }
            }
            return new RedirectResult(Url.Action("Login"));
        }

        [MvcAuthorize]
        [HttpPost]
        public ActionResult Register(String name)
        {
            var errorMessage = "";
            ActionResult result = Register();
            if (!String.IsNullOrEmpty(name))
            {
                var correctNamePattern = new Regex("^[a-zA-Z0-9_]+$");
                var nameIsCorrect = correctNamePattern.IsMatch(name);
                if (nameIsCorrect)
                {
                    var existingUser = usersLogic.GetApiUserByName(name);
                    if (existingUser == null)
                    {
                        CurrentUser.Name = name;
                        usersLogic.UpdateUser(CurrentUser);
                        result = new RedirectResult(Url.Action("Index", "Patterns"));
                    }
                    else
                    {
                        errorMessage = Strings.NameIsAlreadyTaken;
                    }
                }
                else
                {
                    errorMessage = Strings.NameContainsInappropriateSymbols;
                }
            }
            else
            {
                errorMessage = Strings.NameIsEmpty;
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                var model = new RegisterModel() {Name = name, ErrorMessage = errorMessage};
                result = View(model);
            }

            return result;
        }

        private ActionResult Authenticate(string code, string provider, string redirectUrl)
        {
            ActionResult result;
            var externalUser = loginHelper.GetExternalUser(provider, code);
            if (externalUser != null)
            {
                var apiUser = usersLogic.GetApiUserByExternalIdAndProvider(externalUser.ExternalId,
                                                                           externalUser.Provider);
                if (apiUser != null && apiUser.Name != null)
                {
                    if (String.IsNullOrEmpty(redirectUrl))
                    {
                        redirectUrl = Url.Action("Index", "Patterns");
                    }
                    result = new RedirectResult(redirectUrl);
                }
                else
                {
                    apiUser = usersLogic.CreateUser(externalUser.ExternalId, externalUser.Provider);
                    var registrationUrl = Url.Action("Register");
                    result = new RedirectResult(registrationUrl);
                }
                var token = usersLogic.CreateTokenForApiUser(apiUser);
                if (token != null)
                {
                    HttpContext.Response.Cookies.Add(new HttpCookie("Token", token.Id.ToString())
                        {
                            Expires = token.ExpirationTime
                        });
                }
            }
            else
            {
                result = RedirectToAction("Index", "Patterns");
            }

            return result;
        }
    }
}
