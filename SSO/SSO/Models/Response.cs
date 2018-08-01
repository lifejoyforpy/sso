using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSO.Models
{
    public class Response<T>
    {
        public int status { get; set; } = 1;

        //1 代表是站点访问returnurl
        public int isSSO { get; set; } = 1;
        public T entity { get; set; }

        public string token { get; set; } = "";

        public string redirect_url { get; set; } = "/Home/Index";

        public string msg { get; set; }
    }
}