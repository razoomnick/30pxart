using Patterns.Objects.Aggregated;

namespace Patterns.Web.Models.Viewer
{
    public class RegisterModel: BaseModel
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
    }
}