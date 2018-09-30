using System;
using System.Collections.Generic;
using Patterns.Objects.Aggregated;
using Patterns.Objects.Entities;

namespace Patterns.Objects.DataInterfaces
{
    public interface IUsersRepository: IBaseRepository<User>
    {
        ApiUser GetApiUser(Guid id);
        ApiUser GetApiUserByExternalIdAndProvider(String externalId, String provider);
        ApiUser GetApiUserByName(string name);
        ApiUser GetApiUserByToken(Guid tokenId);
        IList<ApiUser> GetApiUsersWhoLikePattern(Guid patternId);
    }
}