using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Patterns.Objects.Aggregated;
using Patterns.Web.Models.Viewer;
using Patterns.Web.Settings;

namespace Patterns.Web.Controllers
{
    public class BaseController: Controller
    {
        private readonly Configuration configuration = Configuration.GetConfiguration();

        protected Configuration Configuration
        {
            get
            {
                return configuration;
            }
        }

        protected ApiUser CurrentUser
        {
            get { return HttpContext.Items["User"] as ApiUser; }
        }

        protected GalleryModel GetGalleryModel(IList<ApiPattern> patterns, int skipped, Boolean showDescription = false)
        {
            var model = new GalleryModel()
            {
                Patterns = patterns,
                CurrentUser = CurrentUser,
                MoreSkip = patterns.Count == Configuration.PageSize ? skipped + Configuration.PageSize : 0,
                ShowDescription = showDescription
            };
            return model;
        }
    }
}