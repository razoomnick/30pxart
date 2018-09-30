using System;
using System.Data.Entity;
using System.Linq;
using Patterns.Data.Linq;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Data.Repositories
{
    public class CommentsRepository: BaseRepository<Comment>, ICommentsRepository
    {
        public ApiComment GetApiComment(Guid id)
        {
            var comment = Context
                .Comments
                .Where(c => c.Id == id)
                .SelectApiComment()
                .FirstOrDefault();
            return comment;
        }

        protected override DbSet<Comment> Collection
        {
            get { return Context.Comments; }
        }
    }
}
