using System.Web.Mvc;

namespace MMO.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            var namespaces = new[] { "MMO.Web.Areas.Admin.Controllers" };
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional }, namespaces
            ).DataTokens["UseNamespaceFallback"] = false ;
        }
    }
}