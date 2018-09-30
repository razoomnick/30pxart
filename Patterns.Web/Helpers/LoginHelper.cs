using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.External;
using Patterns.Web.Settings;

namespace Patterns.Web.Helpers
{
    public abstract class LoginHelper
    {
        private readonly Dictionary<string, IAuthProvider> authProviders;

        protected LoginHelper()
        {
            authProviders = InitAuthProviders();
        }

        public string GetLoginUrl(string providerName)
        {
            return authProviders[providerName].LoginLink;
        }

        public Dictionary<String, String> GetLoginUrls()
        {
            return authProviders.ToDictionary(p => p.Key, p => p.Value.LoginLink);
        }

        public ExternalUser GetExternalUser(string provider, string code)
        {
            var externalUser = authProviders[provider].Authenticate(code);
            return externalUser;
        }

        private Dictionary<string, IAuthProvider> InitAuthProviders()
        {
            var configuration = Configuration.GetConfiguration();
            var providers = new Dictionary<string, IAuthProvider>();
            var vkAuthProvider = new VkAuthProvider(configuration.VkClientId, configuration.VkClientSecret, RedirectUrl);
            providers.Add(VkAuthProvider.ProviderName, vkAuthProvider);
            var fbAuthProvider = new FbAuthProvider(configuration.FbClientId, configuration.FbClientSecret, RedirectUrl);
            providers.Add(FbAuthProvider.ProviderName, fbAuthProvider);
            var instagramAuthProvider = new InstagramAuthProvider(configuration.InstagramClientId, configuration.InstagramClientSecret, RedirectUrl);
            providers.Add(InstagramAuthProvider.ProviderName, instagramAuthProvider);
            return providers;
        }

        protected abstract string RedirectUrl { get; }

    }
}