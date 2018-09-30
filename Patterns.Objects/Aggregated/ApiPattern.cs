using System;
using System.Collections.Generic;

namespace Patterns.Objects.Aggregated
{
    public class ApiPattern
    {
        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
        public string Name { get; set; }
        public int LikesCount { get; set; }
        public bool Liked { get; set; }
        public string Url { get; set; }
        public string ImageCdnUrl { get; set; }
        public ApiUser Author { get; set; }
        public List<string> Canvases { get; set; }
        public List<Boolean> LayersVisibility { get; set; }
        public int Scale { get; set; }
        public String FilterName { get; set; }
        public Boolean IsDraft { get; set; }
        public DateTime CreationTime { get; set; }
        public int CommentsCount { get; set; }
        public IEnumerable<ApiComment> Comments { get; set; }
        public Guid OriginalImageId { get; set; }
    }
}
