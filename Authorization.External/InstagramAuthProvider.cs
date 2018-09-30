using System;

namespace Authentication.External
{
    public class InstagramAuthProvider: BaseAuthProvider, IAuthProvider
    {
        private const string loginUrlTemplate = "https://api.instagram.com/oauth/authorize/?client_id={client_id}&redirect_uri={redirect_url}&response_type=code";
        private const string accessTokenUrlTemplate = "https://api.instagram.com/oauth/access_token?client_id={client_id}&client_secret={client_secret}&code={code}&redirect_uri={redirect_url}&grant_type=authorization_code";

        public InstagramAuthProvider(string clientId, string clientSecret, string redirectUrl) : base(clientId, clientSecret, redirectUrl, loginUrlTemplate, accessTokenUrlTemplate, ProviderName, o => o.user.id.ToString())
        {
        }

        public static String ProviderName { get { return "instagram"; } }
    }
}
