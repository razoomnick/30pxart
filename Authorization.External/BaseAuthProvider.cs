using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Authentication.External
{
    public abstract class BaseAuthProvider
    {
        protected readonly string clientId;
        protected readonly string clientSecret;
        protected readonly string redirectUrl;
        protected readonly string loginUrlTemplate;
        protected readonly string accessTokenUrlTemplate;
        protected readonly string providerName;
        private Func<dynamic, String> idGetter;

        protected BaseAuthProvider(String clientId, String clientSecret, String redirectUrl, string loginUrlTemplate, string accessTokenUrlTemplate, string providerName)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUrl = redirectUrl;
            this.loginUrlTemplate = loginUrlTemplate;
            this.accessTokenUrlTemplate = accessTokenUrlTemplate;
            this.providerName = providerName;
        }

        protected BaseAuthProvider(String clientId, String clientSecret, String redirectUrl, string loginUrlTemplate, string accessTokenUrlTemplate, string providerName, Func<dynamic, String> idGetter)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUrl = redirectUrl;
            this.loginUrlTemplate = loginUrlTemplate;
            this.accessTokenUrlTemplate = accessTokenUrlTemplate;
            this.providerName = providerName;
            this.idGetter = idGetter;
        }

        public virtual ExternalUser Authenticate(String code)
        {
            var accessTokenUrl = GetAccessTokenUrl(code);
            var response = GetObjectFromUrl(accessTokenUrl, "POST");
            var uid = GetValue(() => idGetter(response));
            ExternalUser externalUser = null;
            if (!String.IsNullOrEmpty(uid))
            {
                externalUser = new ExternalUser() { ExternalId = uid, Provider = providerName };
            }
            return externalUser;
        }

        protected string GetAccessTokenUrl(string code)
        {
            var accessTokenUrl = accessTokenUrlTemplate
                .Replace("{client_id}", clientId)
                .Replace("{client_secret}", clientSecret)
                .Replace("{code}", code)
                .Replace("{redirect_url}", HttpUtility.UrlEncode(redirectUrl + "?provider=" + providerName));
            return accessTokenUrl;
        }

        public string LoginLink
        {
            get
            {
                return loginUrlTemplate
                    .Replace("{client_id}", clientId)
                    .Replace("{redirect_url}", HttpUtility.UrlEncode(redirectUrl + "?provider=" + providerName));
            }
        }

        protected T GetValue<T>(Func<T> getValueMethod)
        {
            try
            {
                return getValueMethod();
            }
            catch
            {
                return default(T);
            }
        }

        protected dynamic GetObjectFromUrl(String url, String method = "GET")
        {
            var body = GetBodyFromUrl(url, method);
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic obj = body == null ? new {} : serializer.Deserialize(body, typeof(object));
            return obj;
        }

        protected String GetBodyFromUrl(String url, String method = "GET")
        {
            string result = null;
            try
            {
                var body = "";
                if (method == "POST")
                {
                    var chunks = url.Split('?');
                    url = chunks[0];
                    body = chunks[1];
                }
                var request = WebRequest.Create(url);
                request.Method = method;
                if (method == "POST")
                {
                    var buffer = Encoding.UTF8.GetBytes(body);
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    var buffer = new byte[response.ContentLength];
                    stream.Read(buffer, 0, (int) response.ContentLength);
                    result = Encoding.UTF8.GetString(buffer);
                }
            }
            catch(Exception ex){}
            return result;
        }
    }
}
