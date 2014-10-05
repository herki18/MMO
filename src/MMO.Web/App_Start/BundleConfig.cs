using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI.WebControls;
using BundleTransformer.Core.Transformers;
using BundleTransformer.Core.Translators;
using BundleTransformer.Less.Translators;

namespace MMO.Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {
            var frontendScript = new ScriptBundle("~/js/frontend");
            frontendScript.Include("~/scripts/jquery-2.1.1.js", "~/scripts/forms.js");
            frontendScript.Transforms.Add(new JsMinify());
            bundles.Add(frontendScript);

            var backendScripts = new ScriptBundle("~/js/backend");
            backendScripts.Include("~/scripts/jquery-2.1.1.js", "~/scripts/forms.js");
            backendScripts.Transforms.Add(new JsMinify());
            bundles.Add(backendScripts);


            var frontedStyles = new StyleBundle("~/styles/frontend");

            frontedStyles.Transforms.Add(new StyleTransformer(
                new List<ITranslator> {
                    new LessTranslator()
                }));

            frontedStyles.Transforms.Add(new CssMinify());
            frontedStyles.Include("~/content/styles/application.less");

            bundles.Add(frontedStyles);
            
            var backendStyles = new StyleBundle("~/styles/backend");

            backendStyles.Transforms.Add(new StyleTransformer(
                new List<ITranslator> {
                    new LessTranslator()
                }));

            backendStyles.Transforms.Add(new CssMinify());
            backendStyles.Include("~/content/styles/application.less");

            bundles.Add(backendStyles);
        }
    }
}