using MRMS.Framework.Dapper;
using SSO.Models;
using SSOManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSO.Service
{
    public class Compare<T> : IEqualityComparer<T>
    {
        public delegate bool EuqalsCompare<F>(F x, F y);

        private EuqalsCompare<T> _compare;

        public Compare(EuqalsCompare<T> euqalsCompare)
        {
            this._compare = euqalsCompare;
        }

        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            if (_compare != null)
            {
                return _compare(x, y);
            }
            else
            {
                return false;
            }
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    public class UserLogin
    {
        public static Response<UserSSOInfo> Login(string username, string password)
        {
            Response<UserSSOInfo> rsp = new Response<UserSSOInfo>();
            UserSSOInfo info = new UserSSOInfo();
            User user = new User();
            List<UserEntity> list = new List<UserEntity>();
            List<App> applist = new List<App>();
            List<Role> rolelist = new List<Role>();
            List<MainMenu> mainlist = new List<MainMenu>();
            string sql = @"select a.*  ,b.Menu_Url Main_Url ,b.Menu_Name Main_Menu  from
              (select a.UserId ,a.UserName ,a.UserJobNo ,a.Password , a.Factory, e.APP_Id,e.App_Name,  f.Role_Id,f.Role_Name ,
                           d.ParentId, d.Menu_Id,d.Menu_Name ,d.Menu_Type,d.Menu_Url,d.Sort ,d.Menu_Script,d.Menu_Img
                           from ucenter_member a ,ucenter_user_role b ,ucenter_role_menu c ,ucenter.ucenter_menu d,ucenter_appinfo e ,ucenter_role f
                           where  a.UserId=b.UserId and b.Role_Id=c.Role_Id and f.App_Id=e.APP_Id and b.Role_Id=f.Role_Id
                               and c.Menu_Id=d.Menu_Id and e.UseStatus=1 and d.DataFlag=1 and f.DataFlag=1
                              and  a.UserJobNo=@username  and a.Password=@password and a.UseStatus=1
                              order by e.APP_Id ,f.Role_Id ,d.ParentId,d.Menu_Id,d.Menu_Type ASC) a LEFT JOIN
                       (select  * from ucenter_menu where ParentId=0 and  DataFlag=1 )b   on a.APP_Id=b.App_Id

";
            try
            {
                list = DapperRepository.Query<UserEntity>(sql, new { username = username, password = password });
                if (list == null || list.Count == 0)
                {
                    rsp.status = 0;
                    rsp.msg = "用户不存或密码错误";
                    return rsp;
                }
                applist = list.Select(x => new App
                {
                    APP_Id = x.APP_Id,
                    App_Name = x.App_Name
                }).Distinct(new Compare<App>((p1, p2) => (p1.APP_Id == p2.APP_Id))).ToList();
                rolelist = list.Select(x => new Role
                {
                    Role_Id = x.Role_Id,
                    Role_Name = x.Role_Name,
                    App_Id = x.APP_Id,
                    MenuList = list.Where(o => o.Role_Id == x.Role_Id && o.APP_Id == x.APP_Id).ToList().Select(t => new Menu
                    {
                        App_Id = x.APP_Id,
                        Menu_Id = t.Menu_Id,
                        Role_Id = t.Role_Id,
                        Menu_Name = t.Menu_Name,
                        Menu_Script = t.Menu_Script,
                        Menu_Img = t.Menu_Img,
                        Menu_Type = t.Menu_Type,
                        Menu_Title = t.Menu_Title,
                        Menu_Url = t.Menu_Url,
                        ParentId = t.ParentId,
                        Main_Url =t.Main_Url,
                        Main_Menu=t.Main_Menu,         
                        Sort = t.Sort
                    }).Distinct(new Compare<Menu>((p1, p2) => (p1.Menu_Id == p2.Menu_Id))).ToList()
                }).Distinct(new Compare<Role>((p1, p2) => (p1.Role_Id == p2.Role_Id))).ToList();
                user = list.Select(x => new User
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Password = x.Password,
                    Factory= x.Factory,
                    UserJobNo =x.UserJobNo

                }).FirstOrDefault();
                mainlist=list.Select(x=>new MainMenu {
                    App_Id =x.APP_Id,
                    Menu_Name = x.Main_Menu,
                    Menu_Url=x.Main_Url,
                }).Distinct(new Compare<MainMenu>((p1, p2) => (p1.App_Id == p2.App_Id))).ToList();
                info.User = user;
                info.AppList = applist;
                info.RoleList = rolelist;
                info.Mainlist = mainlist;
                rsp.entity = info;
                return rsp;
            }
            catch (Exception ex)
            {
                rsp.status = 0;
                rsp.msg = "网络链接异常";
                return rsp;
            }
        }


    }
}