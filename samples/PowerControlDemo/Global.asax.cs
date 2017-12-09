using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using PowerControlDemo.Helper;
using WeihanLi.AspNetMvc.AccessControlHelper;
using WeihanLi.Common;

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

            var builder = new ContainerBuilder();
            // etc..
            // register accesss control
            AccessControlHelper.RegisterAccessControlHelper<ActionAccessStrategy, ControlAccessStrategy>(() =>
            {
                builder.RegisterType<ActionAccessStrategy>().As<IActionAccessStrategy>();
                builder.RegisterType<ControlAccessStrategy>().As<IControlAccessStrategy>();
                return new AutofacDependencyResolver(builder.Build());
            });
        }
    }
}