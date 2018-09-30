using Patterns.Objects.Aggregated;

namespace Patterns.Web.Models.Viewer
{
    public class UserModel: BaseModel
    {
        public GalleryModel Gallery { get; set; }
        public ApiUser User { get; set; }
    }
}