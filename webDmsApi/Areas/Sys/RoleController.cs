using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Sys
{
    public class RoleController : ApiBaseController
    {
        public HttpResponseMessage FindSysRoleTree()
        {
            var list = db.Sys_Role.Where<Sys_Role>(p => p.IsValid != 0).Select(s => new
            {
                label = s.RoleName,
                RoleID = s.RoleID
            }).ToList();

            return Json(true, "", list);
        }
        
        public HttpResponseMessage FindSysMenuTable()
        {
            var list = db.Sys_Menu.Where(w => w.IsValid != 0 && w.MenuParentID==0).Select(s => new {
                MenuID=s.MenuID,
                MenuName=s.MenuName,
                MenuIcon=s.MenuIcon,
                label =s.MenuID,
                Comment=s.Comment,
                chilDren =db.Sys_Menu.Where(w1=>w1.IsValid!=0 && w1.MenuParentID == s.MenuID).Select(s1 => new {
                    MenuID = s1.MenuID,
                    MenuName = s1.MenuName,
                    label = s1.MenuID,
                }).ToList()
            }).ToList();

            return Json(true, "", list);
        }
    }
}
