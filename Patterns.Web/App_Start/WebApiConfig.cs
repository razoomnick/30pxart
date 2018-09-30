using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;

namespace Patterns.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "LikesApi",
                "api/patterns/{patternId}/likes",
                new { Controller = "Likes" }
            );

            config.Routes.MapHttpRoute(
                "CommentsApi",
                "api/patterns/{patternId}/comments",
                new { Controller = "Comments" }
            );

            config.Routes.MapHttpRoute(
                "FollowersApi",
                "api/users/{userName}/followers",
                new { Controller = "Followers" }
            );

            config.Routes.MapHttpRoute(
                "PatternsPreviewApi",
                "api/patterns/base64",
                new { Controller = "Patterns", Action = "ConvertToBase64" }
            );

            config.Routes.MapHttpRoute(
                "PatternSave",
                "api/patterns",
                new { Controller = "Patterns", Action = "Index" }
            );

            config.Routes.MapHttpRoute(
                "PatternsBestApi",
                "api/patterns/best",
                new { Controller = "Patterns", Action = "Best" }
            );

            config.Routes.MapHttpRoute(
                "PatternsMostCommentedApi",
                "api/patterns/mostcommented",
                new { Controller = "Patterns", Action = "MostCommented" }
            );

            config.Routes.MapHttpRoute(
                "PatternsRecentUnregisteredApi",
                "api/patterns/recentunregistered",
                new { Controller = "Patterns", Action = "RecentUnregistered" }
            );

            config.Routes.MapHttpRoute(
                "PatternsBestUnregisteredApi",
                "api/patterns/bestunregistered",
                new { Controller = "Patterns", Action = "BestUnregistered" }
            );

            config.Routes.MapHttpRoute(
                "PatternsFeedApi",
                "api/patterns/feed",
                new { Controller = "Patterns", Action = "Feed" }
            );

            config.Routes.MapHttpRoute(
                "PatternsDraftsApi",
                "api/patterns/drafts",
                new { Controller = "Patterns", Action = "Drafts" }
            );

            config.Routes.MapHttpRoute(
                "PatternApi",
                "api/patterns/{id}",
                new { Controller = "Patterns", action = "Pattern", id = UrlParameter.Optional },
                new { id = @"^[0-9a-fA-F\-]+$" }
            );

            config.Routes.MapHttpRoute(
                "CurrentUserApi",
                "api/users/current",
                new { Controller = "Users", action = "Current" }
            );

            config.Routes.MapHttpRoute(
                "UserApi",
                "api/users/{name}",
                new { Controller = "Users", action = "Index", name = UrlParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                "UserPatternsApi",
                "api/users/{name}/patterns",
                new { Controller = "Users", action = "Patterns", name = UrlParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                "LoginUrlsApi",
                "api/accounts/loginurls",
                new { Controller = "Accounts", action = "LoginUrls" }
            );

            config.Routes.MapHttpRoute(
                "TokensApi",
                "api/accounts/token",
                new { Controller = "Accounts", action = "Token" }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
