# AccessControlHelper

[![WeihanLi.AspNetMvc.AccessControlHelper](https://img.shields.io/nuget/v/WeihanLi.AspNetMvc.AccessControlHelper.svg)](http://www.nuget.org/packages/WeihanLi.AspNetMvc.AccessControlHelper/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WeihanLi.AspNetMvc.AccessControlHelper.svg)](http://www.nuget.org/packages/WeihanLi.AspNetMvc.AccessControlHelper/)

## [中文](README.md)

## Build Status

[![Build Status](https://weihanli.visualstudio.com/Pipelines/_apis/build/status/WeihanLi.AccessControlHelper?branchName=dev)](https://weihanli.visualstudio.com/Pipelines/_build/latest?definitionId=23&branchName=dev)

## Intro

Project based on `.netstandard2.0` and support `net45` also, you can use it in a asp.net mvc project or a asp.net core project.

You can use this to control the permissions to:

- access the `Action`
- access the element of the razor page
- access the all resources included the static files

## GetStarted

1. Nuget Package <https://www.nuget.org/packages/WeihanLi.AspNetMvc.AccessControlHelper/>

   install the package `WeihanLi.AspNetMvc.AccessControlHelper`

   asp.net:

   ``` bash
   Install-Package WeihanLi.AspNetMvc.AccessControlHelper
   ```

   asp.net core:

   ``` bash
   dotnet add package WeihanLi.AspNetMvc.AccessControlHelper
   ```

1. Implement your own custom access strategy

    - implement your element in razor page access strategy `IControlAccessStrategy`
    - implement your resource access stragety `IResourceAccessStrategy`

    Samples:

    - ASP.NET Mvc

         1. [AccessStragety](https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/PowerControlDemo/Helper/AccessStrategy.cs)

    - ASP.NET Core

        1. [ResourceAccessStrategy](https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/AccessControlDemo/Services/ActionAccessStrategy.cs)

        1. [ControlAccessStrategy](https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/AccessControlDemo/Services/ControlAccessStrategy.cs)

1. Register your stragety when your app start

    - asp.net mvc

    Referer to ：<https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/PowerControlDemo/Global.asax.cs#L23>

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

    Register your access stragety in the `Startup` file，refer to: <https://github.com/WeihanLi/AccessControlHelper/blob/master/samples/AccessControlDemo/Startup.cs>

    ``` csharp
    // ConfigureServices
    services.AddAccessControlHelper<ResourceAccessStrategy, ControlAccessStrategy>();

    // ConfigureServices
    services.AddAccessControlHelper<ResourceAccessStrategy, ControlAccessStrategy>();

    // register service on your need, you can register IResourceAccessStrategy only when you need control your resource access, or register IControlAccessStrategy when you need to control view component only
    //services.TryAddScoped<IResourceAccessStrategy, ActionAccessStrategy>();
    //services.TryAddSingleton<IControlAccessStrategy, ControlAccessStrategy>();
    //services.AddAccessControlHelper();

    // custom service lieftime
    // services.AddAccessControlHelper<ActionAccessStrategy, ControlAccessStrategy>(ServiceLifetime.Scoped, ServiceLifetime.Singleton);

    // asp.net core [recommend]
    services.AddAccessControlHelper()
        .AddResourceAccessStrategy<ResourceAccessStrategy>(ServiceLifetime.Scoped)
        .AddControlAccessStrategy<ControlAccessStrategy>()
        ;

    // Configure middleware, optional
    // app.UseAccessControlHelper(); // use this only when you want to have a global access control especially for static files
    ```

1. Control `Action` access permission

    samples:

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

    you can use poliy in asp.net core, `Policy` is equal to `[AccessControl]`

    ``` csharp
    // [Authorize(AccessControlHelperConstants.PolicyName)]
    [Authorize("AccessControl")]
    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";
        return View();
    }
    ```

1. control the element control access

    import the namespace,see Samples below：

    - asp.net mvc

      1. import namespace

            update **web.config** in the `Views` folder , import `WeihanLi.AspNetMvc.AccessControlHelper`

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

      2. use in razor page

            - `SparkContainer`

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

                when you have the permission to access the element control, you will get the follows, if you do not have the permission, you will get nothing:

                ``` html
                <div class="container" custom-attribute="abcd">1234</div>

                <span class="custome_p111">12344</span>
                ```

            - `SparkLink`

                ``` csharp
                @Html.SparkLink("Learn about me &raquo;", "http://weihanli.xyz",new { @class = "btn btn-default" })
                ```

                when you have the permission to access the element control, you will get the follows output:

                ``` html
                <a class="btn btn-default" href="http://weihanli.xyz">Learn about me »</a>
                ```

            - `SparkButton`

                ``` csharp
                @Html.SparkButton("12234", new { @class= "btn btn-primary" })
                ```

                when you have the permission to access the element control, you will get the follows output:

                ``` html
                <button class="btn btn-primary" type="button">12234</button>
                ```

    - asp.net core

      - HtmlHelper extensions

        1. import namespace

            import namespace `WeihanLi.AspNetMvc.AccessControlHelper` in **_ViewImports.cshtml** in you `Views` folder

            ``` csharp
            @using AccessControlDemo
            @using WeihanLi.AspNetMvc.AccessControlHelper// add WeihanLi.AspNetMvc.AccessControlHelper
            @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
            ```

        2. Use it in your razor page, exactly the same with the code in asp.net mvc

      - TagHelper

        1. add TagHelper reference

            add reference `WeihanLi.AspNetMvc.AccessControlHelper` in **_ViewImports.cshtml** in you `Views` folder

            ``` csharp
            @using AccessControlDemo
            @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
            @addTagHelper *, WeihanLi.AspNetMvc.AccessControlHelper // add WeihanLi.AspNetMvc.AccessControlHelper TagHelper
            ```

        2. use in your razor page

            add `asp-access` in the element you wanna control the access permission, config `asp-acess-key` if you want
            for example: `<ul class="list-group" asp-access asp-access-key="12334">...</ul>`

            you will get the `ul` output when you have the permission, if not, nothing will output

## Contact

Contact me if you have any question.

Contact me: <weihanli@outlook.com>
