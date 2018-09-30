using System;
using Patterns.Data.Repositories;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Logic
{
    public class LikesLogic
    {
        private readonly ILikesRepository likesRepository = new LikesRepository();
        private readonly IUsersRepository usersRepository = new UsersRepository();
        private readonly IPatternsRepository patternsRepository = new PatternsRepository();

        public ApiLike Like(Guid userId, Guid patternId)
        {
            var apiLike = likesRepository.GetApiLike(userId, patternId);
            if (apiLike == null)
            {
                var user = usersRepository.GetById(userId);
                var pattern = patternsRepository.GetById(patternId);
                if (user != null && pattern != null)
                {
                    var like = new Like()
                    {
                        User = user,
                        Pattern = pattern
                    };
                    likesRepository.Save(like);
                    apiLike = likesRepository.GetApiLike(userId, patternId);
                }
            }
            return apiLike;
        }

        public void Dislike(Guid userId, Guid patternId)
        {
            var like = likesRepository.GetLike(userId, patternId);
            if (like != null)
            {
                likesRepository.Delete(like);
            }
        }
    }
}
