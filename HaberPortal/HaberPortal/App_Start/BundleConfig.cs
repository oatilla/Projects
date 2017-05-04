using System.Web;
using System.Web.Optimization;

namespace HaberPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Geliştirme yapmak ve öğrenmek için Modernizr'ın geliştirme sürümünü kullanın. Daha sonra,
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery-2.2.3.min.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/app.min.js",
                      "~/Scripts/summernote.min.js",
                      "~/Scripts/etiket.js",
                      "~/Scripts/bootstrap-tagsinput.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/skin-blue.min.css",
                      "~/Content/adminlte.min.css",
                      "~/Content/summernote.css",
                      "~/Content/adminlte.min.css",
                      "~/Content/etiket.css",
                       "~/Content/bootstrap-tagsinput.css"));
        }
    }
}
