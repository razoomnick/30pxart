using System;
using Patterns.Data.Repositories;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Logic
{
    public class CommentsLogic
    {
        private IUsersRepository usersRepository = new UsersRepository();
        private IPatternsRepository patternsRepository = new PatternsRepository();
        private ICommentsRepository commentsRepository = new CommentsRepository();

        public ApiComment AddComment(Guid patternId, Guid authorId, string text)
        {
            ApiComment apiComment = null;
            var user = usersRepository.GetById(authorId);
            var pattern = patternsRepository.GetById(patternId);
            if (user != null && pattern != null)
            {
                var comment = new Comment()
                    {
                        Author = user,
                        Pattern = pattern,
                        Text = text
                    };
                commentsRepository.Save(comment);
                apiComment = commentsRepository.GetApiComment(comment.Id);
            }
            return apiComment;
        }

        public ApiComment GetApiComment(Guid id)
        {
            ApiComment apiComment = commentsRepository.GetApiComment(id);
            return apiComment;
        }
    }
}
