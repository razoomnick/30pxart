using System;
using System.IO;
using System.Linq;
using Patterns.Data.Repositories;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Logic
{
    public class UsersLogic
    {
        readonly IUsersRepository usersRepository = new UsersRepository();
        readonly ImagesLogic imagesLogic = new ImagesLogic();
        readonly IFollowingsRepository followingsRepository = new FollowingsRepository();
        readonly ITokensRepository tokensRepository = new TokensRepository();

        public ApiUser GetApiUserById(Guid id)
        {
            var apiUser = usersRepository.GetApiUser(id);
            return apiUser;
        }

        public ApiUser GetApiUserByExternalIdAndProvider(String externalId, String provider)
        {
            var apiUser = usersRepository.GetApiUserByExternalIdAndProvider(externalId, provider);
            return apiUser;
        }

        public ApiUser GetApiUserByName(String name)
        {
            var apiUser = usersRepository.GetApiUserByName(name);
            return apiUser;
        }

        public ApiUser GetApiUserByNameWithFollowings(String name, Guid currentUserId)
        {
            var apiUser = usersRepository.GetApiUserByName(name);
            if (apiUser != null)
            {
                apiUser.IsFollowed = followingsRepository.GetFollowing(apiUser.Id, currentUserId) != null;
                apiUser.FollowersCount = followingsRepository.GetFollowersCount(apiUser.Id);
                apiUser.FollowingCount = followingsRepository.GetFollowingsCount(apiUser.Id);
                apiUser.Followers = followingsRepository.GetFollowers(apiUser.Id).OrderBy(f => f.Subscriber.Id != currentUserId).ToList();
                apiUser.Followings = followingsRepository.GetFollowings(apiUser.Id).OrderBy(f => f.Publisher.Id != currentUserId).ToList();
            }
            return apiUser;
        }

        public ApiToken CreateTokenForApiUser(ApiUser apiUser)
        {
            var user = usersRepository.GetById(apiUser.Id);
            ApiToken result = null;
            if (user != null)
            {
                var token = new Token() {User = user};
                tokensRepository.Save(token);
                result = new ApiToken()
                {
                    CreationTime = token.CreationTime,
                    ExpirationTime = token.ExpirationTime,
                    Id = token.Id,
                    User = apiUser
                };
            }
            return result;
        }

        public ApiUser GetApiUserByToken(Guid tokenId)
        {
            var apiUser = usersRepository.GetApiUserByToken(tokenId);
            return apiUser;
        }

        public ApiUser CreateUser(string externalId, string provider)
        {
            var user = new User()
                {
                    ExternalId = externalId,
                    Provider = provider
                };
            usersRepository.Save(user);
            var apiUser = usersRepository.GetApiUser(user.Id);
            return apiUser;
        }

        public void UpdateUser(ApiUser apiUser)
        {
            var user = usersRepository.GetById(apiUser.Id);
            if (user != null)
            {
                user.Name = apiUser.Name;
                usersRepository.SaveChanges();
            }
        }

        public void InvalidateToken(Guid tokenId)
        {
            var token = tokensRepository.GetById(tokenId);
            if (token != null)
            {
                token.ExpirationTime = DateTime.UtcNow;
                tokensRepository.SaveChanges();
            }
        }

        public void ChangeAvatar(ApiUser apiUser, Stream fileStream)
        {
            var user = usersRepository.GetById(apiUser.Id);
            if (user != null)
            {
                var pngRawData = ImagesLogic.CreateAvatar(fileStream);
                var patternImage = new PatternImage()
                {
                    RawData = pngRawData,
                    ContentType = "image/png"
                };
                imagesLogic.SaveImage(patternImage);
                user.Avatar = patternImage;
                usersRepository.SaveChanges();
            }
        }
    }
}
