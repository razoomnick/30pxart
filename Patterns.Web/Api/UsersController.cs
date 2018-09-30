using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Patterns.Logic;
using Patterns.Objects.Aggregated;
using Patterns.Web.Authorization;

namespace Patterns.Web.Api
{
    public class UsersController : BaseApiController
    {
        private readonly UsersLogic usersLogic = new UsersLogic();
        private readonly PatternsLogic patternsLogic = new PatternsLogic();

        [HttpGet]
        [ApiAuthorize]        
        public ApiUser Index(String name)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var apiUser = usersLogic.GetApiUserByNameWithFollowings(name, currentUserId);
            if (apiUser == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return apiUser;
        }

        [HttpGet]
        [ApiAuthorize]
        public IList<ApiPattern> Patterns(String name, int skip = 0)
        {
            var currentUserId = CurrentUser != null ? CurrentUser.Id : new Guid();
            var apiUser = usersLogic.GetApiUserByName(name);
            var patterns = apiUser != null
                               ? patternsLogic.GetUsersPatterns(apiUser.Id, currentUserId, skip, Config.PageSize)
                               : new List<ApiPattern>();
            return patterns;
        }

        [ApiAuthorize]
        [HttpGet]
        public ApiUser Current()
        {
            return CurrentUser;
        }

        [ApiAuthorize]
        [HttpPost]
        public Object Current(HttpRequestMessage request, ApiUser user)
        {
            object result = request.CreateResponse(HttpStatusCode.NoContent, "");
            if (user != null)
            {
                var currentUser = usersLogic.GetApiUserById(CurrentUser.Id);
                if (currentUser != null)
                {
                    var existingUser = usersLogic.GetApiUserByName(user.Name);
                    if (existingUser == null || existingUser.Id == currentUser.Id)
                    {
                        currentUser.Name = user.Name;
                        usersLogic.UpdateUser(currentUser);
                    }
                    else
                    {
                        result = request.CreateResponse(HttpStatusCode.Conflict, "User with the same name already exists");
                    }
                }
                else
                {
                    result = request.CreateResponse(HttpStatusCode.NotFound, "User with such token was not found");
                }
            }
            else
            {
                result = request.CreateResponse(HttpStatusCode.BadRequest, "Error deserializing user");
            }

            return result;
        }
    }
}
