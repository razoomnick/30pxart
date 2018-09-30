using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Authorization;

namespace Patterns.Web.Api
{
    [ApiAuthorize]
    public class PatternsController : BaseApiController
    {
        private readonly PatternsLogic patternsLogic = new PatternsLogic();
        private readonly ImagesLogic imagesLogic = new ImagesLogic();
        private readonly UsersLogic usersLogic = new UsersLogic();

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> Best(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetTopPatterns(currentUserId, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> Index(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetRecentPatterns(currentUserId, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> MostCommented(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetMostCommentedPatterns(currentUserId, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> RecentUnregistered(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetRecentUnregisteredPatterns(currentUserId, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> BestUnregistered(int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var patterns = patternsLogic.GetTopUnregisteredPatterns(currentUserId, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> Feed(String name, int skip = 0)
        {
            var apiUser = usersLogic.GetApiUserByName(name);
            var patterns = patternsLogic.GetUsersFeedPatterns(apiUser.Id, CurrentUser.Id, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public IList<ApiPattern> Drafts(int skip = 0)
        {
            var patterns = patternsLogic.GetUsersDraftPatterns(CurrentUser.Id, skip, Config.PageSize);
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public ApiPattern Pattern(Guid id)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var apiPattern = patternsLogic.Get(id, currentUserId);
            if (apiPattern == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return apiPattern;
        }

        [HttpPut]
        public ApiPattern Index(ApiPattern apiPattern)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var savedApiPattern = patternsLogic.CreateOrUpdate(apiPattern, currentUserId, apiPattern.Scale, apiPattern.FilterName);
            var url = Url.Link("Pattern", new { Controller = "Patterns", Action = "Pattern", id = savedApiPattern.Id });
            savedApiPattern.Url = url;
            return savedApiPattern;
        }

        [HttpPost]
        public String ConvertToBase64(ApiPattern apiPattern)
        {
            var image = imagesLogic.CanvasesToPatternImage(apiPattern, apiPattern.Scale, apiPattern.FilterName);
            var base64Image = Convert.ToBase64String(image.RawData);
            var result = @"data:" + image.ContentType + ";base64," + base64Image;
            return result;
        }
    }
}