using SSO.Filter;
using System.Web;
using System.Web.Mvc;

namespace SSO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MyAuthorizationAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}