using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    /// <summary>
    /// AccessControlTagHelper
    /// add support for tagHelper
    /// https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-2.1#condition-tag-helper
    /// </summary>
    [HtmlTargetElement(Attributes = "asp-access")]
    public sealed class AccessControlTagHelper : TagHelper
    {
        private readonly IControlAccessStrategy _controlAccessStrategy;

        public AccessControlTagHelper(IControlAccessStrategy controlAccessStrategy)
        {
            _controlAccessStrategy = controlAccessStrategy;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.AllAttributes.TryGetAttribute("asp-access-key", out var accessKey);
            if (!_controlAccessStrategy.IsControlCanAccess(accessKey?.Value.ToString()))
            {
                output.SuppressOutput();
            }
            else
            {
                if (accessKey != null)
                {
                    output.Attributes.Remove(accessKey);
                }
            }
        }
    }
}
