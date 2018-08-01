using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedisService;
using SSO.Service;
using SSOManger;
using Newtonsoft.Json;
using SSO.Models;
using Framework.Extension;

namespace SSO.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnURL = ReturnUrl;
            return View();
        }

        [AllowAnonymous]
        public JsonResult SS0Login(string Username, string Password, string ReturnUrl)
        {
            Response<List<MainMenu>> rsp = new Response<List<MainMenu>>();
            Response<UserSSOInfo> usi = new Response<UserSSOInfo>();
            //获取用户信息
            UserSSOInfo ssoinfo = new UserSSOInfo();
            string redirect_url = ReturnUrl;
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                redirect_url = "/Home/Index";
                //rsp.isSSO = true;
            }

            //db查询;
            usi = UserLogin.Login(Username, Md5Helpers.CreateMD5Hash( Password));
            if (usi.status == 0)
            {
                rsp.redirect_url = redirect_url;
                rsp.status = 0;
                rsp.msg = usi.msg;
                return Json(rsp);
            }
            ssoinfo = usi.entity;
            List<Role> rolelist = ssoinfo.RoleList;
            List<Menu> menuelist = new List<Menu>();

            //foreach (var item in rolelist)
            //{
            //    //取父菜单
            //    List<Menu> tempmenue = item.MenuList.Where(x => x.ParentId == 0 && x.Menu_Type == 1).ToList();
            //    //取并集
            //    menuelist = menuelist.Union(tempmenue).ToList();
            //}

            SSOManager sSOManager = new SSOManager();
            string token = sSOManager.SSO_Token(ssoinfo);
            //附加token
            ViewBag.token = token;
            ViewBag.User = Username;
            HttpContext.Session["token"] = token;
            HttpContext.Session["User"] = ssoinfo.User.UserName;
            rsp.entity = ssoinfo.Mainlist;
            rsp.token = token;
            rsp.redirect_url = redirect_url;
            return Json(rsp);
        }

        public ActionResult LogOut()
        {
            SSOManager sSOManager = new SSOManager();
            //string token = HttpContext.Session["User"].ToString();
            //清楚session
            sSOManager.SSO_Token_Clear();
            HttpContext.Session.Remove("token");
            //清楚redis用户权限信息
           
            return Redirect("~/Home/Index");
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}