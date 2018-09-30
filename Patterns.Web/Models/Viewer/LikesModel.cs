using System.Collections.Generic;
using Patterns.Objects.Aggregated;

namespace Patterns.Web.Models.Viewer
{
    public class LikesModel
    {
        public LikesModel()
        {
            UsersWhoLike = new List<ApiUser>();
        }

        public IList<ApiUser> UsersWhoLike { get; set; }
    }
}