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


namespace AccessControlHelper
{
    public static class HtmlHelperExtension
    {
        private static IControlDisplayStrategy displayStrategy;

        public static void RegisterDisplayStrategy<TStrategy>(TStrategy strategy) where TStrategy : IControlDisplayStrategy
        {
            displayStrategy = strategy;
        }

#if NET45
        /// <summary>
        /// ShopButton
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="innerHtml">buttonText</param>
        /// <param name="classNames">class名称</param>
        /// <param name="attributes">attribute</param>
        /// <returns></returns>
        public static MvcHtmlString SparkButton(this HtmlHelper helper, string innerHtml, object attributes = null, string accessKey = "")
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            if (displayStrategy.IsControlCanAccess(accessKey))
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
        /// ShopLink
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="innerHtml">innerHtml</param>
        /// <param name="linkUrl">linkUrl</param>
        /// <param name="classNames">class名称</param>
        /// <param name="attributes">attribute</param>
        /// <returns></returns>
        public static MvcHtmlString SparkLink(this HtmlHelper helper, string innerHtml, string linkUrl, object attributes = null, string accessKey = "")
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            if (displayStrategy.IsControlCanAccess(accessKey))
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
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            if (displayStrategy.IsControlCanAccess(accessKey))
            {
                if (String.IsNullOrEmpty(controllerName))
                {
                    return helper.ActionLink(linkText, actionName, routeValues: routeValues, htmlAttributes: htmlAttributes);
                }
                else
                {
                    return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
                }
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
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            return SparkContainerHelper(helper, tagName, HtmlHelper.AnonymousObjectToHtmlAttributes(attributes), displayStrategy.IsControlCanAccess(accessKey));
        }

        private static SparkContainer SparkContainerHelper(this HtmlHelper helper, string tagName,
            IDictionary<string, object> attributes = null, bool canAccess = true)
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
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
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            if (displayStrategy.IsControlCanAccess(accessKey))
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
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            return SparkContainerHelper(helper, tagName, HtmlHelper.ObjectToDictionary(attributes), displayStrategy.IsControlCanAccess(accessKey));
        }

        private static SparkContainer SparkContainerHelper(this IHtmlHelper helper, string tagName,
            IDictionary<string, object> attributes = null, bool canAccess = true)
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            TagBuilder tagBuilder = new TagBuilder(tagName);
            if (canAccess)
            {
                tagBuilder.MergeAttributes(attributes);
                tagBuilder.TagRenderMode = TagRenderMode.StartTag;
                helper.ViewContext.Writer.Write(tagBuilder.ToString());
            }
            return new SparkContainer(helper.ViewContext, tagName, canAccess);
        }
#endif

    }
}