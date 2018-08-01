using SSOManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
   public static class HtmlEntensions
    {
      public static  UserSSOInfo _userSSOInfo = new SSOManager().SSO_Token_IsExist();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="class">样式</param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="event">vue 事件</param>
        /// <returns></returns>
        public static MvcHtmlString MyExtButton(this HtmlHelper htmlHelper, string id, string name, string cssClass, string controller, string action, string @event, string value = "查询",object atttibution = null)
        {
            string host = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.Host;
            string url = $"{host}/{controller}/{action}";
            bool display = false;
            if (_userSSOInfo.RoleList.Any(o => o.MenuList.Any(i => i.Menu_Url.IndexOf(url, 0) == 0)))
            {
                //设置隐藏属性为true;
                display = true;
            }
            return new MvcHtmlString($"<button type='button' class='{cssClass}' v-show='{display}' v-on:click='{@event}'>{value}</button>");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssClass">样式</param>
        /// <param name="link">href 链接</param>
        /// <param name="url">验证的url</param>
        /// <param name="show">可见性</param>
        /// <param name="value"></param>
        /// <param name="atttibution"></param>
        /// <returns></returns>
        public static MvcHtmlString MyExtA(this HtmlHelper htmlHelper,  string cssClass, string link,string url, string show ,string value = "保存" , object atttibution = null)
        {
            string display = "none";
            if (url.Contains("?flag="))
            {
                url = url.Split('&')[0].TrimEnd();
            }
            if (_userSSOInfo.RoleList.Any(o => o.MenuList.Any(i => i.Menu_Url.IndexOf(url) != -1)))
            {
                //设置隐藏属性为true;
                display = "block";
            }
            return new MvcHtmlString($"<a  v-bind:href=\"{link}\" class='{cssClass}'  style='display:{display}'  v-show=\"{show}\" >{value}</a>" );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssClass"></param>
        /// <param name="link">验证的url</param>
        /// <param name="event">绑定事件</param>
        /// <param name="value"></param>
        /// <param name="atttibution"></param>
        /// <returns></returns>
        public static MvcHtmlString MyExtATag(this HtmlHelper htmlHelper, string cssClass, string link,string @event ,  string value = "保存", object atttibution = null)
        {
            string host = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.Host;
            //string url = $"{host}/{controller}/{action}";
            string display = "none";
           
         
            if (_userSSOInfo.RoleList.Any(o => o.MenuList.Any(i => i.Menu_Url.IndexOf(link) != -1)))
            {
                //设置隐藏属性为true;
                display = "block";
            }
            return new MvcHtmlString($"<a   class='{cssClass}'  style='display:{display}' {@event} {atttibution}>{value}</a>");
        }
    }
}
