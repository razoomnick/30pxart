using Patterns.Objects.Aggregated;

namespace Patterns.Web.Models
{
    public class BaseModel
    {
        public ApiUser CurrentUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
    }
}