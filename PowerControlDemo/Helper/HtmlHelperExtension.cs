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
        public static MvcHtmlString ShopButton(this HtmlHelper helper,string buttonText,string classNames = null,Dictionary<string,string> attributes = null,string accessKey = "")
        {
            var user = HttpContext.Current.User.Identity.Name;
            var role = CommonHelper.GetUserRoleInfo(user);

            StringBuilder sbText = new StringBuilder();
            if (String.IsNullOrEmpty(accessKey))
            {
                if (role.RoleName.StartsWith("门店"))
                {
                    sbText.Append("<button type=\"button\" ");
                    if (classNames != null && classNames.Length > 0)
                    {
                        sbText.Append(" class=\"");
                        var classNames1 = classNames.Split(' ');
                        foreach (var item in classNames1)
                        {
                            sbText.Append(item + " ");
                        }
                        sbText.Append("\"");
                    }
                    if (attributes != null && attributes.Keys.Count > 0)
                    {
                        foreach (var item in attributes)
                        {
                            sbText.AppendFormat(" {0}=\"{1}\"", item.Key, item.Value);
                        }
                    }
                }
            }
            else
            {
                var accessList = Helper.CommonHelper.GetPowerList(HttpContext.Current.User.Identity.Name);
                if (accessList != null && accessList.Any(a=>a.AccessKey == Guid.Parse(accessKey)))
                {
                    sbText.Append("<button type=\"button\" ");
                    if (classNames != null && classNames.Length > 0)
                    {
                        sbText.Append(" class=\"");
                        var classNames1 = classNames.Split(' ');
                        foreach (var item in classNames1)
                        {
                            sbText.Append(item + " ");
                        }
                        sbText.Append("\"");
                    }
                    if (attributes != null && attributes.Keys.Count > 0)
                    {
                        foreach (var item in attributes)
                        {
                            sbText.AppendFormat(" {0}=\"{1}\"", item.Key, item.Value);
                        }
                    }
                }
            }
            return MvcHtmlString.Create(sbText.ToString());
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

            StringBuilder sbText = new StringBuilder();
            if (role.RoleName.StartsWith("门店"))
            {
                sbText.Append("<a ");
                sbText.AppendFormat(" href=\"{0}\" ", linkUrl);
                if (classNames != null && classNames.Length>0)
                {
                    sbText.Append(" class=\"");
                    var classNames1 = classNames.Split(' ');
                    foreach (var item in classNames1)
                    {
                        sbText.Append(item + " ");
                    }
                    sbText.Append("\"");
                }
                if (attributes != null && attributes.Keys.Count > 0)
                {
                    foreach (var item in attributes)
                    {
                        sbText.AppendFormat(" {0}=\"{1}\"", item.Key, item.Value);
                    }
                }
                sbText.AppendFormat(">{0}</a>",innerHtml);
            }
            return MvcHtmlString.Create(sbText.ToString());
        }
    }
}