# AccessControlHelper

[![WeihanLi.AspNetMvc.AccessControlHelper](https://img.shields.io/nuget/v/WeihanLi.AspNetMvc.AccessControlHelper.svg)](http://www.nuget.org/packages/WeihanLi.AspNetMvc.AccessControlHelper/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WeihanLi.AspNetMvc.AccessControlHelper.svg)](http://www.nuget.org/packages/WeihanLi.AspNetMvc.AccessControlHelper/)

## [English](README.en.md)

## Build Status

[![Build status](https://ci.appveyor.com/api/projects/status/ht69a1o8b9ss9v8a?svg=true)](https://ci.appveyor.com/project/WeihanLi/accesscontroldemo)

[![Build Status](https://travis-ci.org/WeihanLi/AccessControlHelper.svg?branch=master)](https://travis-ci.org/WeihanLi/AccessControlHelper)

## Intro

由于项目需要，需要在 基于 Asp.net mvc 的 Web 项目框架中做权限的控制，于是才有了这个权限控制组件。

项目基于 .NETStandard，同时支持 asp.net mvc（.NET faremwork4.5以上） 和 asp.net core 项目（asp.net 2.0以上），基于 ASP.NET MVC 和 ASP.NET Core 实现的对 `Action` 的访问控制以及页面元素的权限控制。

asp.net core 支持的更多一些，asp.net core 可以使用  TagHelper 来控制页面上元素的权限访问，同时支持通过中间件也可以实现对静态资源的访问。

## GetStarted

1. Nuget Package <https://www.nuget.org/packages/WeihanLi.AspNetMvc.AccessControlHelper/>

   安装权限控制组件 `WeihanLi.AspNetMvc.AccessControlHelper`

   asp.net:

   ``` bash
   Install-Package WeihanLi.AspNetMvc.AccessControlHelper
   ```

   asp.net core:

   ``` bash
   dotnet add package WeihanLi.AspNetMvc.AccessControlHelper
   ```

1. 实现自己的权限控制显示策略类

    - 实现页面元素显示策略接口 `IControlAccessStrategy`
    - 实现 `Action` 访问显示策略接口 `IResourceAccessStrategy`

    示例代码：

    - ASP.NET Mvc

         1. [AccessStragety](https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/PowerControlDemo/Helper/AccessStrategy.cs)

    - ASP.NET Core

        1. [ResourceAccessStrategy](https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/AccessControlDemo/Services/ActionAccessStrategy.cs)

        1. [ControlAccessStrategy](https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/AccessControlDemo/Services/ControlAccessStrategy.cs)

1. 程序启动时注册自己的显示策略

    - asp.net mvc

    可基于Autofac实现的依赖注入，在 autofac 的 Ioc Container中注册显示策略，并返回一个可以从Ioc Container中获取对象的委托或者实现 `IServiceProvider` 接口的对象，参考：<https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/PowerControlDemo/Global.asax.cs#L23>

    ``` csharp
    //autofac ContainerBuilder
    var builder = new ContainerBuilder();
    // etc...

    // register accesss control
    builder.RegisterType<ResourceAccessStrategy>().As<IResourceAccessStrategy>();
    builder.RegisterType<ControlAccessStrategy>().As<IControlAccessStrategy>();
    var container = builder.Build();
    // Important
    AccessControlHelper.RegisterAccessControlHelper<ActionAccessStrategy, ControlAccessStrategy>(type => container.Resolve(type));
    ```

    - asp.net core

    在 `Startup` 文件中注册显示策略，参考<https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/AccessControlDemo/Startup.cs>

    ``` csharp
    // ConfigureServices
    services.AddAccessControlHelper<ResourceAccessStrategy, ControlAccessStrategy>();

    // 自己注册服务，如果只用到资源访问，比如只有 API 可以只注册 IResourceAccessStrategy，反之如果只用到视图上的权限控制可以只注册 IControlAccessStrategy
    //services.TryAddScoped<IResourceAccessStrategy, ActionAccessStrategy>();
    //services.TryAddSingleton<IControlAccessStrategy, ControlAccessStrategy>();
    //services.AddAccessControlHelper();

    // 自定义服务生命周期
    // services.AddAccessControlHelper<ActionAccessStrategy, ControlAccessStrategy>(ServiceLifetime.Scoped, ServiceLifetime.Singleton);

    // Configure 中间件，可选，当你需要一个全局的 access control 时使用（会忽略控制器上的 AllowAnonymous）
    // app.UseAccessControlHelper(); // use this only when you want to have a global access control especially for static files
    ```

1. 控制 `Action` 的方法权限

    通过 `AccessControl` 和 `NoAccessControl` Filter 来控制 `Action` 的访问权限，如果Action上定义了 `NoAccessControl` 可以忽略上级定义的 `AccessControl`，另外可以设置 Action 对应的 `AccessKey`

    使用示例：

    ``` csharp
    [NoAccessControl]
    public IActionResult Index()
    {
        return View();
    }

    [AccessControl]
    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";

        return View();
    }

    [AccessControl(AccessKey = "Contact")]
    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";

        return View();
    }
    ```

    在 asp.net core 中你也可以设置 `Policy` 和直接使用 `[AccessControl]` 方法一致

    ``` csharp
    [Authorize("AccessControl")]
    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";
        return View();
    }
    ```

1. 控制页面元素的显示

    为了使用比较方便，建议在页面上导入命名空间，具体方法如下，详见 Samples：

    - asp.net mvc

      1. 添加命名空间引用

            在 项目的 Views 目录下的 **web.config** 文件中添加命名空间 `WeihanLi.AspNetMvc.AccessControlHelper`

            ``` xml
            <system.web.webPages.razor>
                <pages pageBaseType="System.Web.Mvc.WebViewPage">
                    <namespaces>
                        <add namespace="System.Web.Mvc" />
                        <add namespace="System.Web.Mvc.Ajax" />
                        <add namespace="System.Web.Mvc.Html" />
                        <add namespace="System.Web.Optimization"/>
                        <add namespace="System.Web.Routing" />
                        <add namespace="PowerControlDemo" />
                        <add namespace="WeihanLi.AspNetMvc.AccessControlHelper" /><!-- add WeihanLi.AspNetMvc.AccessControlHelper-->
                    </namespaces>
                </pages>
            </system.web.webPages.razor>
            ```

      2. 在 Razor 页面上使用

            - `SparkContainer` 使用

                ``` csharp
                @using(Html.SparkContainer("div",new { @class="container",custom-attribute = "abcd" }))
                {
                    @Html.Raw("1234")
                }

                @using (Html.SparkContainer("span",new { @class = "custom_p111" }, "F7A17FF9-3371-4667-B78E-BD11691CA852"))
                {
                    @:12344
                }
                ```

                没有权限访问就不会渲染到页面上，有权限访问的时候渲染得到的 Html 如下：

                ``` html
                <div class="container" custom-attribute="abcd">1234</div>

                <span class="custome_p111">12344</span>
                ```

            - `SparkLink`

                ``` csharp
                @Html.SparkLink("Learn about me &raquo;", "http://weihanli.xyz",new { @class = "btn btn-default" })
                ```

                有权限访问时渲染出来的 html 如下：

                ``` html
                <a class="btn btn-default" href="http://weihanli.xyz">Learn about me »</a>
                ```

            - `SparkButton`

                ``` csharp
                @Html.SparkButton("12234", new { @class= "btn btn-primary" })
                ```

                有权限访问时渲染出来的 html 如下：

                ``` html
                <button class="btn btn-primary" type="button">12234</button>
                ```

    - asp.net core

      - HtmlHelper 扩展

        1. 添加命名空间引用

            在 Views 目录下的 **_ViewImports.cshtml** 中引用命名空间 `WeihanLi.AspNetMvc.AccessControlHelper`

            ``` csharp
            @using AccessControlDemo
            @using WeihanLi.AspNetMvc.AccessControlHelper// add WeihanLi.AspNetMvc.AccessControlHelper
            @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
            ```

        2. 在 Razor 页面上使用，使用方法与上面的使用方式一样

      - TagHelper

        1. 添加 TagHelper 引用

            在 Views 目录下的 **_ViewImports.cshtml** 中引用 `WeihanLi.AspNetMvc.AccessControlHelper` TagHelper

            ``` csharp
            @using AccessControlDemo
            @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
            @addTagHelper *, WeihanLi.AspNetMvc.AccessControlHelper // add WeihanLi.AspNetMvc.AccessControlHelper TagHelper
            ```

        2. 在 Razor 页面上使用

            在需要权限控制的元素上增加 `asp-access` 即可，如果需要配置 access-key 通过 `asp-accesss-key` 来配置，示例：`<ul class="list-group" asp-access asp-access-key="12334">...</ul>`

            这样有权限的时候就会输出这个 `ul` 的内容，如果没有权限就不会输出，而且出于安全考虑，如果有配置 `asp-access-key` 的话也会把 `asp-access-key` 给移除，不会输出到浏览器中

## Contact

如果您在使用中遇到了问题，欢迎随时与我联系。

Contact me: <weihanli@outlook.com>
