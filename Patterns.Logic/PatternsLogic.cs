using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Patterns.Cloud.Gcs;
using Patterns.Data.Repositories;
using Patterns.Objects.Aggregated;
using Patterns.Objects.DataInterfaces;
using Patterns.Objects.Entities;

namespace Patterns.Logic
{
    public class PatternsLogic
    {
        private readonly IPatternsRepository patternsRepository = new PatternsRepository();
        private readonly IPatternImagesRepository patternImagesRepository = new PatternImagesRepository();
        private readonly IUsersRepository usersRepository = new UsersRepository();
        private readonly ImagesLogic imagesLogic = new ImagesLogic();

        public ApiPattern CreateOrUpdate(ApiPattern apiPattern, Guid authorId, int scale, String filterName)
        {
            var patternImage = imagesLogic.CanvasesToPatternImage(apiPattern, scale, filterName);            
            imagesLogic.SaveImage(patternImage);
            var jsonRawData = JsonConvert.SerializeObject(apiPattern.Canvases);
            var originalRawData = Encoding.UTF8.GetBytes(jsonRawData);
            var originalImage = new PatternImage()
                {
                    RawData = originalRawData 
                };
            patternImagesRepository.Save(originalImage);
            var user = usersRepository.GetById(authorId);
            var pattern = patternsRepository.GetById(apiPattern.Id);
            if (pattern != null && pattern.Author != null && pattern.Author.Id == authorId)
            {
                ApplyChanges(apiPattern, pattern, patternImage, originalImage, user);
                patternsRepository.SaveChanges();
            }
            if (pattern == null)
            {
                pattern = new Pattern();
                ApplyChanges(apiPattern, pattern, patternImage, originalImage, user);
                patternsRepository.Save(pattern);
            }
            var savedApiPattern = patternsRepository.GetApiPattern(pattern.Id, authorId);
            return savedApiPattern;
        }

        private static void ApplyChanges(ApiPattern apiPattern, Pattern pattern, PatternImage patternImage,
                                         PatternImage originalImage, User user)
        {
            pattern.PatternImage = patternImage;
            pattern.OriginalImage = originalImage;
            pattern.Author = user;
            pattern.Name = apiPattern.Name ?? "";
            pattern.IsDraft = apiPattern.IsDraft;
            pattern.FilterName = apiPattern.FilterName;
        }

        public IList<ApiPattern> GetTopPatterns(Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetTopApiPatterns(currentUserId, skip, count);
            return patterns;
        }

        public IList<ApiPattern> GetUsersPatterns(Guid userId, Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetUsersApiPatterns(userId, currentUserId, skip, count);
            return patterns;
        }

        public IList<ApiPattern> GetUsersFeedPatterns(Guid userId, Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetUsersFeedPatterns(userId, currentUserId, skip, count);
            return patterns;
        }

        public ApiPattern Get(Guid id, Guid currentUserId)
        {
            var apiPattern = patternsRepository.GetApiPattern(id, currentUserId);
            return apiPattern;
        }

        public IList<ApiPattern> GetRecentPatterns(Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetRecentApiPatterns(currentUserId, skip, count);
            return patterns;
        }

        public IList<ApiPattern> GetMostCommentedPatterns(Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetMostCommentedApiPatterns(currentUserId, skip, count);
            return patterns;
        }

        public IList<ApiPattern> GetRecentUnregisteredPatterns(Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetRecentUnregisteredApiPatterns(currentUserId, skip, count);
            return patterns;
        }

        public IList<ApiPattern> GetTopUnregisteredPatterns(Guid currentUserId, int skip, int count)
        {
            var patterns = patternsRepository.GetTopUnregisteredApiPatterns(currentUserId, skip, count);
            return patterns;
        }

        public IList<ApiPattern> GetUsersDraftPatterns(Guid userId, int skip, int count)
        {
            var patterns = patternsRepository.GetUsersDraftPatterns(userId, skip, count);
            return patterns;
        }

        public ApiPattern GetWithState(Guid id, Guid currentUserId)
        {
            var pattern = Get(id, currentUserId);
            if (pattern != null)
            {
                var image = patternImagesRepository.GetById(pattern.OriginalImageId);
                if (image != null)
                {
                    if (String.IsNullOrEmpty(image.ContentType))
                    {
                        var jsonRawData = Encoding.UTF8.GetString(image.RawData);
                        var canvases = JsonConvert.DeserializeObject<List<String>>(jsonRawData);
                        pattern.Canvases = canvases;
                    }
                    else
                    {
                        var strRawData = Convert.ToBase64String(image.RawData);
                        pattern.Canvases = new List<string>(){strRawData};
                    }
                }
            }
            return pattern;
        }

        public IList<ApiUser> GetUsersWhoLike(Guid id)
        {
            var apiUsers = usersRepository.GetApiUsersWhoLikePattern(id);
            return apiUsers;
        }

        public void ChangeDraftState(Guid patternId, ApiUser currentUser)
        {
            if (currentUser != null)
            {
                var apiPattern = patternsRepository.GetApiPattern(patternId, currentUser.Id);
                var pattern = patternsRepository.GetById(patternId);
                if (pattern != null && apiPattern != null &&
                    (apiPattern.Author != null && apiPattern.Author.Id == currentUser.Id || currentUser.Name == "admin"))
                {
                    pattern.IsDraft = !pattern.IsDraft;
                    patternsRepository.SaveChanges();
                }
            }
        }
    }
}
