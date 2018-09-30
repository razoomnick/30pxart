using System.Web.Optimization;

namespace Patterns.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var editorBundleJs = new ScriptBundle("~/bundles/editor/js");
            editorBundleJs.IncludeDirectory("~/Content/editor/js/", "*.js");
            editorBundleJs.Include("~/Content/js/api.js");
            bundles.Add(editorBundleJs);

            var viewerBundleJs = new ScriptBundle("~/bundles/viewer/js");
            viewerBundleJs.IncludeDirectory("~/Content/viewer/js/", "*.js");
            viewerBundleJs.Include("~/Content/js/api.js");
            bundles.Add(viewerBundleJs);

            BundleTable.EnableOptimizations = true;
        }
    }
}