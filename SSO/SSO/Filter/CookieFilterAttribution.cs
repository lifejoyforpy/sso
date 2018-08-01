using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Filter
{
    public class CookieFilterAttribution : ActionFilterAttribute
    {
        //
        // 摘要:
        //     在执行操作方法之前由 ASP.NET MVC 框架调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        //
        // 摘要:
        //     在执行操作方法后由 ASP.NET MVC 框架调用。
        //
        // 参数:
        //   filterContext:
        //     筛选器上下文。
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}