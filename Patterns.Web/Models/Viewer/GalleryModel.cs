using System;
using System.Collections.Generic;
using Patterns.Objects.Aggregated;

namespace Patterns.Web.Models.Viewer
{
    public class GalleryModel: BaseModel
    {
        public GalleryModel()
        {
            Patterns = new List<ApiPattern>();
        }

        public IList<ApiPattern> Patterns { get; set; }
        public int MoreSkip { get; set; }
        public Boolean ShowDescription { get; set; }
    }
}