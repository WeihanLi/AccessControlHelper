using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccessControlHelper
{
    public static class HtmlHelperExtension
    {
        private static IControlDisplayStrategy displayStrategy;

        public static void RegisterDisplayStrategy<TStrategy>(TStrategy strategy) where TStrategy:IControlDisplayStrategy
        {
            displayStrategy = strategy;
        }

        /// <summary>
        /// ShopButton
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="buttonText">buttonText</param>
        /// <param name="classNames">class名称</param>
        /// <param name="attributes">attribute</param>
        /// <returns></returns>
        public static MvcHtmlString ShopButton(this HtmlHelper helper, string buttonText, string classNames = null, object attributes = null, string accessKey = "")
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            displayStrategy.AccessKey = accessKey;
            if (displayStrategy.IsCanDisplay)
            {
                TagBuilder tagBuilder = new TagBuilder("button");
                tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
                tagBuilder.MergeAttribute("type", "button");
                if (!String.IsNullOrEmpty(classNames))
                {
                    tagBuilder.MergeAttribute("class", classNames);
                }
                tagBuilder.InnerHtml = buttonText;
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
        public static MvcHtmlString ShopLink(this HtmlHelper helper, string innerHtml, string linkUrl, string classNames = null, object attributes = null,string accessKey="")
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            displayStrategy.AccessKey = accessKey;
            if (displayStrategy.IsCanDisplay)
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
                tagBuilder.MergeAttribute("href", linkUrl);
                if (!String.IsNullOrEmpty(classNames))
                {
                    tagBuilder.MergeAttribute("class", classNames);
                }
                tagBuilder.InnerHtml = innerHtml;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Empty;
        }

        public static ShopContainer ShopContainer(this HtmlHelper helper, string tagName, string id = "", object attributes = null, string accessKey = "")
        {
            if (displayStrategy == null)
            {
                throw new ArgumentException("Control显示策略未初始化，请使用 HtmlHelperExtension.RegisterDisplayStrategy(IControlDisplayStrategy stragety) 方法注册显示策略", nameof(displayStrategy));
            }
            displayStrategy.AccessKey = accessKey;
            return ShopContainerHelper(helper, tagName, id, HtmlHelper.AnonymousObjectToHtmlAttributes(attributes), displayStrategy.IsCanDisplay);
        }

        private static ShopContainer ShopContainerHelper(this HtmlHelper helper, string tagName, string id,
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
                if (!String.IsNullOrEmpty(id))
                {
                    tagBuilder.MergeAttribute("id", id);
                }
                helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            }
            ShopContainer container = new ShopContainer(helper.ViewContext, tagName, canAccess);
            return container;
        }

        public static void EndShopContainer(this HtmlHelper helper, string tagName, bool canAccess = true)
        {
            EndShopContainer(helper.ViewContext, tagName, canAccess);
        }

        public static void EndShopContainer(ViewContext viewContext, string tagName, bool canAccess)
        {
            if (canAccess)
            {
                viewContext.Writer.Write("</{0}>", tagName);
            }
            else
            {
                viewContext.Writer.Write("</{0}>", tagName);
            }
        }
    }
}
