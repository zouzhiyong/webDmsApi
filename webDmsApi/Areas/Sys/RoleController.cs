using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public HttpResponseMessage FindSysMenuTable(Sys_RoleMenu obj)
        {
            var list = db.Sys_Menu.Where(w => w.IsValid != 0 && w.MenuParentID == 0).Select(s => new
            {
                MenuID = s.MenuID,
                MenuName = s.MenuName,
                MenuIcon = s.MenuIcon,
                Comment = s.Comment,
                MenuRolesData= db.Sys_Menu.Where(w1 => w1.IsValid != 0 && w1.MenuParentID == s.MenuID && db.Sys_RoleMenu.Where(w0 => w0.RoleID == obj.RoleID && w0.MenuID==w1.MenuID).Select(s0 => s0.MenuID).ToList().Count>0).Select(s1=>s1.MenuID).ToList(),
                chilDren = db.Sys_Menu.Where(w2 => w2.IsValid != 0 && w2.MenuParentID == s.MenuID).Select(s2 => new
                {
                    MenuID = s2.MenuID,
                    MenuName = s2.MenuName
                }).ToList()
            }).ToList();

            return Json(true, "", list);
        }

        public HttpResponseMessage SaveSysRoleForm(dynamic obj)
        {            
            int RoleID = obj.RoleID;
            var arr = obj.MenuID;
            var temp = db.Set<Sys_RoleMenu>().Where(w=>w.RoleID== RoleID);
            foreach (var item in temp)
            {
                db.Entry<Sys_RoleMenu>(item).State = EntityState.Deleted;
            }

            List<Sys_RoleMenu> list = new List<Sys_RoleMenu>();
            

            foreach (int item in arr)
            {
                var tempObj = new Sys_RoleMenu()
                {
                    RoleID = RoleID,
                    MenuID = item,
                    RecTimeStamp = DateTime.Now,
                    CreateUserID = 1,
                    CreateDate = DateTime.Now,
                    ModifyUserID = 1,
                    ModifyDate = DateTime.Now
                };
                list.Add(tempObj);         

            }

            int result = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] == null)
                    continue;
                db.Entry<Sys_RoleMenu>(list[i]).State = EntityState.Added;
            }

            result += db.SaveChanges();

            return Json(true, result>0 ? "保存成功！" : "保存失败");
        }
    }
}
