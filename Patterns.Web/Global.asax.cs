using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Patterns.Data;
using Patterns.Web.Controllers;
using Scorm.Core.IpLookup;

namespace Patterns.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static GeoIpLookup geoIpLookup = new GeoIpLookup();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContextManager.Init(() => System.Web.HttpContext.Current.Items);
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            var extension = Path.GetExtension(Request.PhysicalPath);
            if (String.IsNullOrEmpty(extension))
            {
                SetLanguage();
            }
        }

        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode >= 400)
            {
                var rd = new RouteData();
                if (Context.Response.StatusCode == 404)
                {
                    rd.Values["action"] = "NotFound";
                }
                else
                {
                    rd.Values["action"] = "Error";
                }
                Response.Clear();
                rd.Values["controller"] = "Errors";
                IController c = new ErrorsController();
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            }
        }

        private void SetLanguage()
        {
            var cultures = new Dictionary<String, String> {{"be", "be-BY"}, {"ru", "ru-RU"}, {"en", "en-US"}};
            var lang = Request.QueryString["lang"];
            if (String.IsNullOrEmpty(lang))
            {
                var langCookie = Request.Cookies.Get("lang");
                if (langCookie != null)
                {
                    lang = langCookie.Value;
                }
            }
            if (String.IsNullOrEmpty(lang))
            {
                var country = geoIpLookup.GetCountryByIp(Request.UserHostAddress);
                if (country == "Belarus")
                {
                    lang = "be";
                }
                if (country == "Russian Federation")
                {
                    lang = "ru";
                }
            }
            if (!String.IsNullOrEmpty(lang) && cultures.ContainsKey(lang))
            {
                var culture = new CultureInfo(cultures[lang]);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                Response.Cookies.Add(new HttpCookie("lang", lang));
            }
        }

        public override string GetVaryByCustomString(System.Web.HttpContext context, string custom)
        {
            String result;
            if (custom == "Token")
            {
                var cookie = context.Request.Cookies.Get("Token");
                result = cookie != null
                             ? cookie.Value
                             : "";

            }
            else
            {
                result = base.GetVaryByCustomString(context, custom);
            }

            return result;
        }
    }
}