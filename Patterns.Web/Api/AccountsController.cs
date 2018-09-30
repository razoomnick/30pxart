using System.Collections.Generic;
using System.Web.Http;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Helpers;

namespace Patterns.Web.Api
{
    public class AccountsController : BaseApiController
    {
        private readonly LoginHelper loginHelper = new MvcLoginHelper();
        private readonly UsersLogic usersLogic = new UsersLogic();

        [HttpGet]
        public Dictionary<string, string> LoginUrls()
        {
            var loginUrls = loginHelper.GetLoginUrls();
            return loginUrls;
        }

        [HttpGet]
        public ApiToken Token(string code, string provider)
        {
            var externalUser = loginHelper.GetExternalUser(provider, code);
            ApiToken token = null;
            if (externalUser != null)
            {
                var apiUser = usersLogic.GetApiUserByExternalIdAndProvider(externalUser.ExternalId, externalUser.Provider)
                              ?? usersLogic.CreateUser(externalUser.ExternalId, externalUser.Provider);
                token = usersLogic.CreateTokenForApiUser(apiUser);
            }
            return token;
        }
    }
}
