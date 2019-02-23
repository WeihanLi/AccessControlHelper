using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
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

            var builder = new ContainerBuilder();
            // etc..
            // register access control
            builder.RegisterType<ActionAccessStrategy>().As<IResourceAccessStrategy>();
            builder.RegisterType<ControlAccessStrategy>().As<IControlAccessStrategy>();
            var container = builder.Build();
            AccessControlHelper.RegisterAccessControlHelper<ActionAccessStrategy, ControlAccessStrategy>(type => container.Resolve(type));
        }
    }
}
