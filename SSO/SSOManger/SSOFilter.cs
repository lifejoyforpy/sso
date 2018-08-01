using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using RedisService.Services;
using System.Configuration;
using Newtonsoft.Json;

namespace SSOManger
{
    public class SSOFilteraAttribute : AuthorizeAttribute
    {
        private static string ssoip;
        public static string noauth_url = "/Home/Error";

        private SSOFilteraAttribute()
        { }

        public SSOFilteraAttribute(string ssourl, string noauth_ur = "/Home/Error")
        {
            ssoip = ssourl;
            noauth_url = noauth_ur;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {   

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {

                // 判断ajax 如果在系统配置了ajax权限且 是按钮，
                //if (_userSSOInfo.RoleList.Any(r => r.MenuList.Any(m => m.Menu_Url == filterContext.HttpContext.Request.Url.AbsoluteUri && m.Menu_Type != 2)))
                //{
                //    filterContext.Result = new JsonResult
                //    {
                //        Data = new
                //        {
                //            status = 408,
                //            msg = noauth_url
                //        }

                //    };
                //}
                return;

            }


            if (skipAuthorization)
            {
                return;
            }

            var obj = HttpContext.Current.Request.QueryString["token"];

            //如果是打令牌操作，返回json数据。
            if (!string.IsNullOrEmpty(obj))
            {
                HttpContext.Current.Session["token"] = HttpContext.Current.Request.QueryString["token"].ToString();
                //获取jsonp的参数
                var callback = HttpContext.Current.Request["callback"];
                ContentResult cr = new ContentResult();
                cr.Content = callback + "([{msg: \"hello world! \"}])";

                filterContext.Result = cr;
            }
            else
            {    
                
                if (filterContext.HttpContext.Session["token"] != null)
                {  
                    UserSSOInfo _userSSOInfo = new SSOManager().SSO_Token_IsExist();

                    if (_userSSOInfo != null)
                    {
                        //token信息存在
                        //判断是否有当前访问url的权限;
      
                  
                        if (filterContext.HttpContext.Request.Url.AbsoluteUri.Contains("?flag="))
                        {
                            if (!_userSSOInfo.RoleList.Any(o => o.MenuList.Any(i => i.Menu_Url.Split('&')[0].TrimEnd() == filterContext.HttpContext.Request.Url.AbsoluteUri.Split('&')[0].TrimEnd())))
                            {
                                //没有权限
                                filterContext.Result = new RedirectResult(noauth_url);
                            }

                        }
                        else {

                            if (!_userSSOInfo.RoleList.Any(o => o.MenuList.Any(i => i.Menu_Url.Split('?')[0].TrimEnd() == filterContext.HttpContext.Request.Url.AbsoluteUri.Split('?')[0].TrimEnd())))
                            {
                                //没有权限
                                filterContext.Result = new RedirectResult(noauth_url);
                            }
                        }
                       
                    }
                    else
                    {   
                        //认证中心没有记录，因此需要跳登陆，同时清楚session
                        filterContext.HttpContext.Session.Abandon();
                        //filterContext.Result = new RedirectResult(noauth_url);
                        filterContext.Result = new RedirectResult($"{ssoip}?ReturnUrl=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri));
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult($"{ssoip}?ReturnUrl=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri));
                }
            }
        }
    }
}