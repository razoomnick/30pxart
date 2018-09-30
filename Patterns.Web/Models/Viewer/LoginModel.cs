namespace Patterns.Web.Models.Viewer
{
    public class LoginModel: BaseModel
    {
        public string VkLoginUrl { get; set; }
        public string FbLoginUrl { get; set; }
        public string InstagramLoginUrl { get; set; }
    }
}