using System;
using Patterns.Objects.Aggregated;
using Patterns.Objects.Entities;

namespace Patterns.Objects.DataInterfaces
{
    public interface ILikesRepository: IBaseRepository<Like>
    {
        ApiLike GetApiLike(Guid userId, Guid patternId);
        Like GetLike(Guid userId, Guid patternId);
        void Delete(Like like);
    }
}
