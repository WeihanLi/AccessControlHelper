using System;
using System.Collections.Generic;

#if NET45
using System.Web.Mvc;
using System.Web.Mvc.Html;
#else

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

#endif

namespace WeihanLi.AspNetMvc.AccessControlHelper
{
    public static class HtmlHelperExtension
    {
        private static IControlAccessStrategy _accessStrategy;

        static HtmlHelperExtension()
        {
            if (_accessStrategy == null)
            {
                _accessStrategy = WeihanLi.Common.DependencyResolver.Current.GetService<IControlAccessStrategy>();
                if (_accessStrategy == null)
                {
                    throw new ArgumentException("Control显示策略未初始化，请注册显示策略", nameof(_accessStrategy));
                }
            }
        }

#if NET45
        /// <summary>
        /// SparkButton
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="innerHtml">buttonText</param>
        /// <param name="attributes">attribute</param>
        /// <param name="accessKey">accessKey</param>        
        /// <returns></returns>
        public static MvcHtmlString SparkButton(this HtmlHelper helper, string innerHtml, object attributes = null, string accessKey = "")
        {
            if (_accessStrategy.IsControlCanAccess(accessKey))
            {
                TagBuilder tagBuilder = new TagBuilder("button");
                tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
                tagBuilder.MergeAttribute("type", "button");
                tagBuilder.InnerHtml = innerHtml;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// SparkLink
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="innerHtml">innerHtml</param>
        /// <param name="linkUrl">linkUrl</param>
        /// <param name="attributes">attribute</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static MvcHtmlString SparkLink(this HtmlHelper helper, string innerHtml, string linkUrl, object attributes = null, string accessKey = "")
        {
            if (_accessStrategy.IsControlCanAccess(accessKey))
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
                tagBuilder.MergeAttribute("href", linkUrl);
                tagBuilder.InnerHtml = innerHtml;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// SparkActionLink
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="linkText">linkText</param>
        /// <param name="actionName">actionName</param>
        /// <param name="controllerName">controllerName</param>
        /// <param name="routeValues">routeValues</param>
        /// <param name="htmlAttributes">htmlAttributes</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static MvcHtmlString SparkActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName = "", object routeValues = null, object htmlAttributes = null, string accessKey = "")
        {
            if (_accessStrategy.IsControlCanAccess(accessKey))
            {
                return string.IsNullOrWhiteSpace(controllerName) ? helper.ActionLink(linkText, actionName, routeValues, htmlAttributes) : helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// SparkContainer
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="tagName">标签名称</param>
        /// <param name="attributes">htmlAttributes</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static SparkContainer SparkContainer(this HtmlHelper helper, string tagName, object attributes = null, string accessKey = "")
        => SparkContainerHelper(helper, tagName, HtmlHelper.AnonymousObjectToHtmlAttributes(attributes), _accessStrategy.IsControlCanAccess(accessKey));

        private static SparkContainer SparkContainerHelper(this HtmlHelper helper, string tagName,
            IDictionary<string, object> attributes = null, bool canAccess = true)
        {
            TagBuilder tagBuilder = new TagBuilder(tagName);
            if (canAccess)
            {
                tagBuilder.MergeAttributes(attributes);
                helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            }
            return new SparkContainer(helper.ViewContext, tagName, canAccess);
        }
#else

        /// <summary>
        /// SparkActionLink
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="linkText">linkText</param>
        /// <param name="actionName">actionName</param>
        /// <param name="controllerName">controllerName</param>
        /// <param name="routeValues">routeValues</param>
        /// <param name="htmlAttributes">htmlAttributes</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static IHtmlContent SparkActionLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName = "", object routeValues = null, object htmlAttributes = null, string accessKey = "")
        {
            if (_accessStrategy.IsControlCanAccess(accessKey))
            {
                if (String.IsNullOrEmpty(controllerName))
                {
                    return helper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
                }
                else
                {
                    return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
                }
            }
            return HtmlString.Empty;
        }

        /// <summary>
        /// SparkContainer
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="tagName">标签名称</param>
        /// <param name="attributes">htmlAttributes</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static SparkContainer SparkContainer(this IHtmlHelper helper, string tagName, object attributes = null, string accessKey = "")
        => SparkContainerHelper(helper, tagName, HtmlHelper.AnonymousObjectToHtmlAttributes(attributes), _accessStrategy.IsControlCanAccess(accessKey));

        private static SparkContainer SparkContainerHelper(IHtmlHelper helper, string tagName,
            IDictionary<string, object> attributes = null, bool canAccess = true)
        {
            TagBuilder tagBuilder = new TagBuilder(tagName);
            if (canAccess)
            {
                tagBuilder.MergeAttributes(attributes);
                tagBuilder.TagRenderMode = TagRenderMode.StartTag;
                helper.ViewContext.Writer.Write(tagBuilder);
            }
            return new SparkContainer(helper.ViewContext, tagName, canAccess);
        }

#endif
    }
}