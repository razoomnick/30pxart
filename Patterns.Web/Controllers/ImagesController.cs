using System;
using System.Web.Mvc;
using Patterns.Logic;

namespace Patterns.Web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ImagesLogic imagesLogic = new ImagesLogic();

        [HttpGet]
        [OutputCache(Duration = 600)]
        public ActionResult Index(Guid id)
        {
            var image = imagesLogic.GetImage(id);
            return new FileContentResult(image.RawData, image.ContentType ?? "image/png");
        }
        
        [HttpGet]
        [OutputCache(Duration = 600)]
        public ActionResult Large(Guid id)
        {
            var image = imagesLogic.GetImage(id);
            var largeImagePngRawData = ImagesLogic.RepeatImage(image.RawData, 537, 340);
            return new FileContentResult(largeImagePngRawData, "image/png");
        }
    }
}
