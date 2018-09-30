using System.Web;

namespace Authentication.External
{
    public class FbAuthProvider: BaseAuthProvider, IAuthProvider
    {
        private const string loginUrlTemplate = "https://www.facebook.com/dialog/oauth?client_id={client_id}&redirect_uri={redirect_url}&response_type=code";
        private const string accessTokenUrlTemplate = "https://graph.facebook.com/oauth/access_token?client_id={client_id}&client_secret={client_secret}&code={code}&redirect_uri={redirect_url}";
        private const string uidUrlTemplate = "https://graph.facebook.com/me?fields=id&access_token={access_token}";

        public FbAuthProvider(string clientId, string clientSecret, string redirectUrl) : base(clientId, clientSecret, redirectUrl, loginUrlTemplate, accessTokenUrlTemplate, ProviderName)
        {
        }

        public static string ProviderName { get { return "fb"; } }

        public override ExternalUser Authenticate(string code)
        {
            var accessTokenUrl = GetAccessTokenUrl(code);
            var accessToken = GetValueFromUrl(accessTokenUrl, "access_token");
            ExternalUser externalUser = null;
            if (!string.IsNullOrEmpty(accessToken))
            {
                var uidUrl = uidUrlTemplate.Replace("{access_token}", accessToken);
                var uid = GetObjectFromUrl(uidUrl).id.ToString();
                externalUser = new ExternalUser() { ExternalId = uid, Provider = "fb" };
            }
            return externalUser;
        }

        private string GetValueFromUrl(string url, string key)
        {
            var uidResponse = GetBodyFromUrl(url);
            var values = HttpUtility.ParseQueryString(uidResponse ?? "");
            var value = values[key];
            return value;
        }
    }
}
