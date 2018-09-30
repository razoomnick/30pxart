using System.Web;
using System.Web.Http;
using Patterns.Objects.Aggregated;
using Patterns.Web.Settings;

namespace Patterns.Web.Api
{
    public class BaseApiController: ApiController
    {
        private readonly Configuration config = Settings.Configuration.GetConfiguration();

        protected Configuration Config
        {
            get
            {
                return config;
            }
        }

        protected ApiUser CurrentUser
        {
            get { return HttpContext.Current.Items["User"] as ApiUser; }
        }
    }
}