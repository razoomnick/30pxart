using System;
using System.Collections.Generic;
using Patterns.Objects.Aggregated;
using Patterns.Objects.Entities;

namespace Patterns.Objects.DataInterfaces
{
    public interface IPatternsRepository: IBaseRepository<Pattern>
    {
        IList<ApiPattern> GetTopApiPatterns(Guid currentUserId, int skip = 0, int count = 20);
        IList<ApiPattern> GetUsersApiPatterns(Guid userId, Guid currentUserId, int skip = 0, int count = 20);
        IList<ApiPattern> GetUsersFeedPatterns(Guid userId, Guid currentUserId, int skip, int count);
        ApiPattern GetApiPattern(Guid id, Guid currentUserId);
        IList<ApiPattern> GetRecentApiPatterns(Guid currentUserId, int skip, int count);
        IList<ApiPattern> GetUsersDraftPatterns(Guid userId, int skip, int count);
        IList<ApiPattern> GetMostCommentedApiPatterns(Guid currentUserId, int skip, int count);
        IList<ApiPattern> GetRecentUnregisteredApiPatterns(Guid currentUserId, int skip, int count);
        IList<ApiPattern> GetTopUnregisteredApiPatterns(Guid currentUserId, int skip, int count);
    }
}