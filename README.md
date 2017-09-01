## AccessControlHelper

### Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/ht69a1o8b9ss9v8a?svg=true)](https://ci.appveyor.com/project/WeihanLi/accesscontroldemo)

[![Build Status](https://travis-ci.org/WeihanLi/AccessControlHelper.svg?branch=master)](https://travis-ci.org/WeihanLi/AccessControlHelper)

### Intro
AccessControlHelper 是基于 ASP.NET MVC 和 ASP.NET Core 实现的对 `Action` 的访问控制以及页面元素的权限控制。

权限访问控制实现机制：

- Action的访问控制是基于 `ActionFilter` 来实现的
- 页面元素访问控制是基于通过自己封装的 `HtmlHelper` 扩展方法来实现的

### GetStarted
1. 实现自己的权限控制显示策略类

    - 实现页面元素显示策略接口 `IControlAccessStrategy`
    - 实现 `Action` 访问显示策略接口 `IActionAccessStrategy`

    示例代码：
    
    - ASP.NET Mvc
   
   <https://github.com/WeihanLi/AccessControlHelper/blob/master/PowerControlDemo/Helper/AccessControlDisplayStrategy.cs>

    - ASP.NET Core

   <https://github.com/WeihanLi/AccessControlHelper/blob/master/AccessControlDemo/Startup.cs#L60>


1. 程序启动时注册自己的显示策略

    - asp.net core

    在 `Startup` 文件中注册显示策略
    ``` csharp
    app.UseAccessControlHelper(new AccessControlHelperOptions
        {
            ActionAccessStrategy = new ActionAccessStrategy(),
            ControlAccessStrategy = new ControlAccessStrategy()
        });
    ```
    
    - asp.net mvc

    在 `Global` 文件中注册显示策略
    ``` csharp
    AccessControlHelperExtensions.RegisterAccessStragety(new AccessControlHelperOptions
        {
            ActionAccessStrategy = new ActionAccessStrategy(),
            ControlAccessStrategy = new ControlAccessStrategy()
        });
    ```

1. 控制 `Action` 的方法权限

    通过 `AccessControl` 和 `NoAccessControl` Filter 来控制 `Action` 的访问权限

1. 控制页面元素的显示

    通过 `HtmlHelper` 扩展方法来实现权限控制

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

    <span class="custome_p111"></span>
    ```

### Contact

如果您在使用中遇到了问题，欢迎随时与我联系。

Conact me: <weihanli@outlook.com>
