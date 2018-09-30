using System;
using System.Collections.Generic;
using Patterns.Objects.Aggregated;

namespace Patterns.Web.Models.Viewer
{
    public class PatternModel: BaseModel
    {
        public PatternModel()
        {
            Likes = new List<ApiUser>();
        }

        public ApiPattern Pattern { get; set; }
        public Boolean OnePatternOnPage { get; set; }
        public IList<ApiUser> Likes { get; set; }
    }
}