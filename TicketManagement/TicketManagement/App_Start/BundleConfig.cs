using System.Web.Optimization;

namespace TicketManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* -------------------- SCRIPTS -------------------- */
            /* ------------------------------------------------- */

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_datepicker_js").Include(
                      "~/Scripts/bootstrap-datepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_toggle_js").Include(
                      "~/Scripts/bootstrap-toggle.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_tree_js").Include(
                      "~/Scripts/bootstrap-tree.min.js"));


            /* -------------------- STYLES -------------------- */
            /* ------------------------------------------------ */

            bundles.Add(new StyleBundle("~/bundles/bootstrap_datepicker_css").Include(
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/bootstrap-datepicker.standalone.min.css",
                      "~/Content/bootstrap-datepicker3.min.css",
                      "~/Content/bootstrap-datepicker3.standalone.min.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap_toggle_css").Include(
                      "~/Content/Custom/bootstrap-custom-toggle.min.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap_tree_css").Include(
                      "~/Content/Custom/bootstrap-custom-tree.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Custom/bootstrap-custom-buttons.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css"));

            /* -------------------- REFERENCES -------------------- */
            /* ---------------------------------------------------- */

            // bootstrap-datepicker == https://github.com/eternicode/bootstrap-datepicker

            // bootstrap-toggle == http://www.bootstraptoggle.com/

            // bootstrap-tree == https://github.com/jhfrench/bootstrap-tree

        }
    }
}
