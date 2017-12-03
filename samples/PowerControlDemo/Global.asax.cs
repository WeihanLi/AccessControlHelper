using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PowerControlDemo.Helper;
using WeihanLi.AspNetMvc.AccessControlHelper;

namespace PowerControlDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // register accesss control
            AccessControlHelper.RegisterAccessControlHelper<ActionAccessStrategy, ControlAccessStrategy>();
        }
    }
}