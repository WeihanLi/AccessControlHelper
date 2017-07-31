## AccessControlDemo

### Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/ht69a1o8b9ss9v8a?svg=true)](https://ci.appveyor.com/project/WeihanLi/accesscontroldemo)

### Intro
AccessControlDemo 是一个基于 ASP.NET MVC 的权限控制系统，这个权限控制系统主要是实现对 `Action` 的访问控制和页面元素的权限控制。

权限访问控制实现机制：

- Action的访问控制是基于 `ActionFilter` 来实现的
- 页面元素访问控制是基于通过自己封装的一些 `HtmlHelper` 扩展方法来实现的

### GetStarted
1. 实现自己的权限控制显示策略类

    - 实现页面元素显示策略接口 `IControlDisplayStrategy`
    - 实现 `Action` 访问显示策略接口 `IActionResultDisplayStrategy`

    示例代码：
   <https://github.com/WeihanLi/AccessControlDemo/blob/master/PowerControlDemo/Helper/AccessControlDisplayStrategy.cs>


1. 程序启动时注册自己的显示策略

    在 `Global` 文件中注册显示策略
    ``` csharp
    // RegisterControlDisplayStrategy
    AccessControlHelper.HtmlHelperExtension.RegisterDisplayStrategy(new AccessControlDisplayStrategy());
    // RegisterActionResultDisplayStrategy
    AccessControlHelper.AccessControlAttribute.RegisterDisplayStrategy(new AccessActionResultDisplayStrategy());
    ```
    

1. 控制 `Action` 的方法权限

    通过 `AccessControl` Filter 来控制 `Action` 的访问权限

1. 控制页面元素的显示

    通过 `HtmlHelper` 扩展方法来实现权限控制

    - 常用元素权限控制
        
        - `HtmlHelper.SparkLink()`
        - `HtmlHelper.SparkButton()`
        - `HtmlHelper.SparkActionLink()`

    - `SparkContainer` 使用
    
    ``` csharp
    @using(HtmlHelper.SparkContainer("div",new { @class="container",custom-attribute = "abcd" }))
    {
        @Html.Raw("1234")
    }

    @using (Html.SparkContainer("span",new { @class = "custom_p111" }, "F7A17FF9-3371-4667-B78E-BD11691CA852"))
    {
        @:12344
    }
    ```

### Contact
Conact me: <weihanli@outlook.com>