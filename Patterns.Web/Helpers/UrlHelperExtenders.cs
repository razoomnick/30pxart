using System;
using System.Web.Mvc;
using Patterns.Objects.Aggregated;

namespace Patterns.Web.Helpers
{
    public static class UrlHelperExtenders
    {
        public static String ImageUrl(this UrlHelper urlHelper, ApiPattern patternImage)
        {
            return String.IsNullOrEmpty(patternImage.ImageCdnUrl)
                       ? urlHelper.Action("Index", "Images", new {id = patternImage.ImageId})
                       : patternImage.ImageCdnUrl;
        }

        public static String AvatarUrl(this UrlHelper urlHelper, ApiUser apiUser)
        {
            return
                apiUser.AvatarImageId == null
                    ? urlHelper.Content("~/Content/i/avatar.png")
                    : String.IsNullOrEmpty(apiUser.AvatarCdnUrl)
                          ? urlHelper.Action("Index", "Images", new {id = apiUser.AvatarImageId})
                          : apiUser.AvatarCdnUrl;
        }
    }
}