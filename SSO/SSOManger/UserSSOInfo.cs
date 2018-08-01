﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOManger
{
    public class UserSSOInfo
    {
        public User User { get; set; }
        public List<App> AppList { get; set; }
        public List<Role> RoleList { get; set; }
        public List<MainMenu> Mainlist { get; set; }
    }

    public class MainMenu {

        public int App_Id { get; set; } = 0;
        public string Menu_Url { get; set; }

        public string Menu_Name { get; set; }


    }
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserJobNo { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int DataFlag { get; set; } = 1;
        public string Created_By { get; set; }
        public string Created_By_Id { get; set; }
        public string Created_Time { get; set; }
        public string Last_Modify_By { get; set; }
        public string Last_Modify_By_Id { get; set; }
        public string Last_Modify_Time { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
    }

    public class App
    {
        public int APP_Id { get; set; }
        public string App_Name { get; set; }
        public string App_Description { get; set; }
        public int DataFlag { get; set; } = 1;
        public int CheckStatus { get; set; } = 1;
        public int UseStatus { get; set; } = 1;
        public string Responsible_PersonMobile { get; set; }
        public string Responsible_Person { get; set; }
        public string Created_By { get; set; }
        public string Created_By_Id { get; set; }
        public string Created_Time { get; set; }
        public string Last_Modify_By { get; set; }
        public string Last_Modify_By_Id { get; set; }
        public string Last_Modify_Time { get; set; }
    }

    public class Role
    {
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }
        public int App_Id { get; set; } = 0;
        public int Role_Type { get; set; } = 0;
        public string Role_Description { get; set; }
        public int Sort { get; set; }
        public int DataFlag { get; set; } = 1;
        public string Created_By { get; set; }
        public string Created_By_Id { get; set; }
        public string Created_Time { get; set; }
        public string Last_Modify_By { get; set; }
        public string Last_Modify_By_Id { get; set; }
        public string Last_Modify_Time { get; set; }
        public List<Menu> MenuList { get; set; }
    }

    public class Menu
    {
        public int Menu_Id { get; set; }
        public int Role_Id { get; set; }
        public int ParentId { get; set; } = 0;
        public int Menu_Type { get; set; } = 1;
        public string Menu_Name { get; set; }
        public string Menu_Url { get; set; }
        public string Menu_Title { get; set; }
        public string Menu_Script { get; set; }
        public string Menu_Img { get; set; }

        public string Main_Url { get; set; }

        public string Main_Menu { get; set; }
        public int Sort { get; set; } = 1;
        public int App_Id { get; set; } = 0;
        public int DataFlag { get; set; } = 1;
        public string Created_By { get; set; }
        public string Created_By_Id { get; set; }
        public string Created_Time { get; set; }
        public string Last_Modify_By { get; set; }
        public string Last_Modify_By_Id { get; set; }
        public string Last_Modify_Time { get; set; }
    }

    //接受权限信息的实体
    public class UserEntity
    {
        public string Factory { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserJobNo { get; set; }
        public string Password { get; set; }
        public int APP_Id { get; set; }
        public string App_Name { get; set; }
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }
        public int ParentId { get; set; }
        public int Menu_Id { get; set; }
        public int Menu_Type { get; set; } = 1;
        public string Menu_Name { get; set; }
        public string Menu_Url { get; set; }
        public string Menu_Title { get; set; }
        public string Menu_Script { get; set; }
        public string Menu_Img { get; set; }
        public int Sort { get; set; } = 1;

        public string Main_Url { get; set; }

        public string Main_Menu { get; set; }
    }
}