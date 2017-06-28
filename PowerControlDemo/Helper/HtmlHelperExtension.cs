using PowerControlDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PowerControlDemo
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// ShopButton
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="buttonText">buttonText</param>
        /// <param name="classNames">class名称</param>
        /// <param name="attributes">attribute</param>
        /// <returns></returns>
        public static MvcHtmlString ShopButton(this HtmlHelper helper,string buttonText, string classNames = null, Dictionary<string,object> attributes = null,string accessKey = "")
        {
            var user = HttpContext.Current.User.Identity.Name;
            var role = CommonHelper.GetUserRoleInfo(user);
            TagBuilder tagBuilder = new TagBuilder("button");
            if (role.Any(r => r.RoleName.Contains("超级管理员")))
            {
                tagBuilder.MergeAttributes(attributes);
                tagBuilder.MergeAttribute("type", "button");
                if (!String.IsNullOrEmpty(classNames))
                {
                    tagBuilder.MergeAttribute("class", classNames);
                }
                tagBuilder.InnerHtml = buttonText;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            if (String.IsNullOrEmpty(accessKey))
            {
                if (role.Any(r=>r.RoleName.StartsWith("门店")))
                {
                    tagBuilder.MergeAttributes(attributes);
                    tagBuilder.MergeAttribute("type","button");
                    if (!String.IsNullOrEmpty(classNames))
                    {
                        tagBuilder.MergeAttribute("class",classNames);
                    }
                    tagBuilder.InnerHtml = buttonText;
                    return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
                }
            }
            else
            {
                var accessList = Helper.CommonHelper.GetPowerList(HttpContext.Current.User.Identity.Name);
                if (accessList != null && accessList.Any(a=>a.AccessKey == Guid.Parse(accessKey)))
                {
                    tagBuilder.MergeAttributes(attributes);
                    tagBuilder.MergeAttribute("type", "button");
                    if (!String.IsNullOrEmpty(classNames))
                    {
                        tagBuilder.MergeAttribute("class", classNames);
                    }
                    tagBuilder.SetInnerText(buttonText);
                    return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
                }
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
        public static MvcHtmlString ShopLink(this HtmlHelper helper,string innerHtml,string linkUrl,string classNames = null, Dictionary<string, string> attributes = null)
        {
            var user = HttpContext.Current.User.Identity.Name;
            var role = CommonHelper.GetUserRoleInfo(user);
            if (role.Any(r => r.RoleName.Contains("超级管理员")))
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttributes(attributes);
                tagBuilder.MergeAttribute("href", linkUrl);
                if (!String.IsNullOrEmpty(classNames))
                {
                    tagBuilder.MergeAttribute("class", classNames);
                }
                tagBuilder.InnerHtml = innerHtml;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            if (role.Any(r => r.RoleName.StartsWith("门店")))
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttributes(attributes);
                tagBuilder.MergeAttribute("href",linkUrl);
                if (!String.IsNullOrEmpty(classNames))
                {
                    tagBuilder.MergeAttribute("class", classNames);
                }
                tagBuilder.InnerHtml = innerHtml;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Empty;
        }
    }

    public static class ShopContainerExtensions
    {
        public static ShopContainer ShopContainer(this HtmlHelper helper, string tagName,string id="", Dictionary<string, object> attributes = null, string accessKey = "")
        {
            var user = HttpContext.Current.User.Identity.Name;
            var role = CommonHelper.GetUserRoleInfo(user);
            if (role.Any(r => r.RoleName.Contains("超级管理员")))
            {
                return ShopContainerHelper(helper, tagName, id, attributes);
            }
            if (String.IsNullOrEmpty(accessKey))
            {
                if (role.Any(r => r.RoleName.StartsWith("门店")))
                {
                    return ShopContainerHelper(helper, tagName, id, attributes);
                }
            }
            else
            {
                var accessList = Helper.CommonHelper.GetPowerList(HttpContext.Current.User.Identity.Name);
                if (accessList != null && accessList.Any(a => a.AccessKey == Guid.Parse(accessKey)))
                {
                    return ShopContainerHelper(helper, tagName, id, attributes);
                }
            }
            return ShopContainerHelper(helper, tagName,id, attributes,false);
        }

        private static ShopContainer ShopContainerHelper(this HtmlHelper helper, string tagName, string id,
            Dictionary<string, object> attributes = null,bool canAccess= true)
        {
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
            else
            {
                if (!String.IsNullOrEmpty(id))
                {
                    tagBuilder.MergeAttribute("id", id);
                }
                tagBuilder.MergeAttribute("style","display:none;");
                tagBuilder.MergeAttribute("class","tuhu-hidden");
                //注释开始，注释会被转义
                //helper.ViewContext.Writer.Write("@*");
                helper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));                
            }
            ShopContainer container = new ShopContainer(helper.ViewContext, tagName,canAccess);
            return container;
        }

        public static void EndShopContainer(this HtmlHelper helper,string tagName, bool canAccess = true)
        {
            EndShopContainer(helper.ViewContext, tagName, canAccess);
        }

        public static void EndShopContainer(ViewContext viewContext, string tagName,bool canAccess)
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

    public class ShopContainer : IDisposable
    {
        private readonly string _tagName;
        private readonly ViewContext _viewContext;
        private readonly bool _canAccess;
        private bool _disposed;

        public ShopContainer(ViewContext viewContext,string tagName,bool canAccess =true)
        {
            _viewContext = viewContext;
            _tagName = tagName;
            _canAccess = canAccess;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                EndShopContainer();
            }
        }

        public void EndShopContainer()
        {
            if (_canAccess)
            {
                _viewContext.Writer.Write("</{0}>", _tagName);
            }
            else
            {
                _viewContext.Writer.Write("</{0}>", _tagName);
                // 注释结束，会被转义
                // _viewContext.Writer.Write("*@");
            }
        }
    }
}