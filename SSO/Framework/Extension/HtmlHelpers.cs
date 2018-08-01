using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SSOManger;

namespace Framework.Extension
{
    public static class HtmlHelpers
    {
        private static SSOManager _sso = new SSOManager();

        public static MvcHtmlString MyExtButton(this HtmlHelper htmlHelper, string id, string name, string cssClass, string controller, string action, string value = "查询", object atttibution = null)
        {
            string host = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.Host;
            string url = $"{host}/{controller}/{action}";
            UserSSOInfo user = _sso._userinfo;
            if (user.RoleList.Any(o => o.MenuList.Any(i => i.Menu_Url == url)))
            {
                //设置隐藏属性为true;
            }
            TagBuilder tag = new TagBuilder("button");
            tag.GenerateId(id);
            tag.AddCssClass(cssClass);
            tag.SetInnerText(value);
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}