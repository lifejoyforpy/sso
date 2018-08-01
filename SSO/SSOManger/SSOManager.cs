using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using RedisService.Services;
using ServiceStack.Text;
using System.Web;

namespace SSOManger
{
    public class SSOManager
    {
        private static RedisStringService redis = new RedisStringService();
        private static double session_erpire = double.Parse(ConfigurationManager.AppSettings["RedisSession"].ToString());
        public string _token = "";
        public UserSSOInfo _userinfo = new UserSSOInfo();
        //public SSOManager(UserSSOInfo userSSOInfo)
        //{
        //    _userSSOInfo = userSSOInfo;
        //}

        public SSOManager()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session["token"] != null)
            {
                _token = HttpContext.Current.Session["token"].ToString();
                _userinfo = SSO_Token_IsExist();
            }
            else
            {
                _token = "";
            }
        }

        //SSO登陆产生令牌
        public string SSO_Token(UserSSOInfo userSSOInfo)
        {
            string token = $"{Guid.NewGuid().ToString()}";
            //string value = ServiceStack.Text.JsonSerializer.SerializeToString(userSSOInfo);
            redis.Set<UserSSOInfo>(token, userSSOInfo, DateTime.Now.AddHours(session_erpire));
            _token = token;
            return token;
        }

        //退出清楚令牌
        public bool SSO_Token_Clear()
        {
            return redis.Remove(_token);
        }

        //查询token是否有效(在应用程序中)
        public UserSSOInfo SSO_Token_IsExist()
        {
            UserSSOInfo info = new UserSSOInfo();
            info = redis.Get<UserSSOInfo>(_token);
            //if (string.IsNullOrEmpty(value))
            //    return null;
            ////重置过期时间
            //return ServiceStack.Text.JsonSerializer.DeserializeFromString<UserSSOInfo>(value);
            return info;
        }
    }
}