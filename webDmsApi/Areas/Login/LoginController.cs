using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Login
{
    public class LoginController : ApiController
    {
        ////[Authorize]
        //public HttpResponseMessage Login(getLogin gl)
        //{
        //    return Json(gl);
        //}
        public class getLogin
        {
            public string strUser { get; set; }
            public string strPwd { get; set; }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>

        public object Login(getLogin loginData)
        {
            if (!ValidateUser(loginData.strUser, loginData.strPwd))
            {
                return new { bRes = false, message="账号或密码不正确！" };
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, loginData.strUser, DateTime.Now,
                            DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", loginData.strUser, loginData.strPwd),
                            FormsAuthentication.FormsCookiePath);
            //返回登录结果、用户信息、用户验证票据信息
            var oUser = new UserInfo { bRes = true, UserName = loginData.strUser, Password = loginData.strPwd, Ticket = FormsAuthentication.Encrypt(ticket) };
            //将身份信息保存在session中，验证当前请求是否是有效请求
            HttpContext.Current.Session[loginData.strUser] = oUser;            
            return oUser;
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateUser(string strUser, string strPwd)
        {
            webDmsEntities db = new webDmsEntities();
            var list = db.Sys_User.FirstOrDefault(p => p.LoginName == strUser && p.LoginPassword==strPwd);

            if (list!=null)
            {
                HttpContext.Current.Session["userId"] = list.UserID;
                return true;
            }
            else
            {
                return false;
            }
        }

        public class UserInfo
        {
            public bool bRes { get; set; }

            public string UserName { get; set; }

            public string Password { get; set; }

            public string Ticket { get; set; }
        }
    }
}

