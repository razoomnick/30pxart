using System;
using System.Collections.Generic;
using Patterns.Objects.Aggregated;
using Patterns.Resources;

namespace Patterns.Web.Helpers
{
    public static class TagsHelper
    {
        public static List<String> GetTags(ApiPattern pattern)
        {
            var tags = new List<String>()
            {
                Strings.SeamlessPattern
            };
            if (pattern.FilterName == "tiles")
            {
                tags.Add(Strings.TilesTexture);
            }
            if (pattern.FilterName == "knitting")
            {
                tags.Add(Strings.KnittingTexture);
            }
            if (pattern.FilterName == "embroidery")
            {
                tags.Add(Strings.EmbroideryTexture);
            }
            if (String.IsNullOrEmpty(pattern.FilterName) || pattern.FilterName == "none")
            {
                tags.Add(Strings.PixelArt);
            }
            return tags;
        }
    }
}