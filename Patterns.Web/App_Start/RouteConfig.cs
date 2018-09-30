using System.Web.Mvc;
using System.Web.Routing;

namespace Patterns.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "UserAvatar",
                "users/avatar",
                new {Controller = "Users", action = "Avatar", name = UrlParameter.Optional}
                );

            routes.MapRoute(
                "UserProfile",
                "users/{name}",
                new {Controller = "Users", action = "Profile", name = UrlParameter.Optional}
                );

            routes.MapRoute(
                "UserFeed",
                "users/{name}/feed",
                new {Controller = "Patterns", action = "Feed", name = UrlParameter.Optional}
                );

            routes.MapRoute(
                "UserTile",
                "users/{name}/tile",
                new {Controller = "Users", action = "Tile", name = UrlParameter.Optional}
                );

            routes.MapRoute(
                "pattern",
                "patterns/{id}",
                new {Controller = "Patterns", action = "Pattern", id = UrlParameter.Optional},
                new { id = @"^[0-9a-fA-F\-]+$" }
                );

            routes.MapRoute(
                "PatternVector",
                "patterns/{id}/vector",
                new {Controller = "Patterns", action = "Vector", id = UrlParameter.Optional},
                new { id = @"^[0-9a-fA-F\-]+$" }
                );

            routes.MapRoute(
                "comment",
                "comments/{id}",
                new {Controller = "Comments", action = "Index", id = UrlParameter.Optional},
                new { id = @"^[0-9a-fA-F\-]+$" }
                );

            routes.MapRoute(
                "image",
                "images/{id}",
                new { Controller = "Images", action = "Index", id = UrlParameter.Optional },
                new { id = @"^[0-9a-fA-F\-]+$" }
                );

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new {controller = "Patterns", action = "Index"}
            );

            routes.MapRoute(
                "NotFound",
                "{*url}",
                new {controller = "Errors", action = "NotFound"}
                );
        }
    }
}