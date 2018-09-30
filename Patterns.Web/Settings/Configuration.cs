using System.Configuration;
using System.Web;

namespace Patterns.Web.Settings
{
    public class Configuration
    {
        public int PageSize { get; private set; }
        public string VkClientId { get; private set; }
        public string VkClientSecret { get; private set; }
        public string FbClientId { get; private set; }
        public string FbClientSecret { get; private set; }
        public string InstagramClientId { get; private set; }
        public string InstagramClientSecret { get; private set; }

        public static Configuration GetConfiguration()
        {
            var strPageSize = ConfigurationManager.AppSettings["pageSize"];
            int pageSize;
            if (!int.TryParse(strPageSize ?? "20", out pageSize))
            {
                pageSize = 20;
            }
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                pageSize = 12;
            }

            var configuration = new Configuration()
            {
                PageSize = pageSize,
                FbClientId = ConfigurationManager.AppSettings["fbClientId"],
                FbClientSecret = ConfigurationManager.AppSettings["fbClientSecret"],
                VkClientId = ConfigurationManager.AppSettings["vkClientId"],
                VkClientSecret = ConfigurationManager.AppSettings["vkClientSecret"],
                InstagramClientId = ConfigurationManager.AppSettings["instagramClientId"],
                InstagramClientSecret = ConfigurationManager.AppSettings["instagramClientSecret"]
            };
            return configuration;
        }
    }
}