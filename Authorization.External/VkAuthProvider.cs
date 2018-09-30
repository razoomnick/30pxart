using System;

namespace Authentication.External
{
    public class VkAuthProvider: BaseAuthProvider, IAuthProvider
    {
        private const string loginUrlTemplate = "https://oauth.vk.com/authorize?client_id={client_id}&scope=&redirect_uri={redirect_url}&display=page&v=5.2&response_type=code";
        private const string accessTokenUrlTemplate = "https://oauth.vk.com/access_token?client_id={client_id}&client_secret={client_secret}&code={code}&redirect_uri={redirect_url}";

        public VkAuthProvider(string clientId, string clientSecret, string redirectUrl) : base(clientId, clientSecret, redirectUrl, loginUrlTemplate, accessTokenUrlTemplate, ProviderName, o => o.user_id.ToString())
        {
        }

        public static String ProviderName { get { return "vk"; } }
    }
}
