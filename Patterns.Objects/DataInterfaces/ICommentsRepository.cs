using System;
using Patterns.Objects.Aggregated;
using Patterns.Objects.Entities;

namespace Patterns.Objects.DataInterfaces
{
    public interface ICommentsRepository: IBaseRepository<Comment>
    {
        ApiComment GetApiComment(Guid id);
    }
}
