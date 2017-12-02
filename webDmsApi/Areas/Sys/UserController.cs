using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;
using System.Data.Entity;

namespace webDmsApi.Areas.Sys
{
    public class UserController : ApiBaseController
    {
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindBasDeptTree()
        {
            var list = db.Bas_Dept.Where<Bas_Dept>(p => p.IsValid != 0).Select(s => new
            {
                label = s.Name,
                DeptID = s.DeptID
            }).ToList();

            return Json(true, "", list);
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage FindSysUserTable(dynamic obj)
        {
            DBHelper<View_user> dbhelp = new DBHelper<View_user>();

            int DeptID = obj.DeptID;
            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;
            int total = 0;

            var list = dbhelp.FindPagedList(currentPage, pageSize, out total, x => DeptID == 0 ? 1 == 1 : (x.DeptID == DeptID), s => s.UserID, true);

            return Json(list, currentPage, pageSize, total);
        }

        public HttpResponseMessage FindSysUserForm(dynamic obj)
        {
            int UserID = obj == null ? 0 : obj.UserID;

            if (UserID == 0)
            {
                var list = new
                {
                    UserID = 0,
                    LoginName = "",
                    DeptID = "",
                    RealName = "",
                    Phone = "",
                    UserTypeID = "",
                    BirthDate = "",
                    CreateDate = "",
                    LastLoginDate = "",
                    Email = "",
                    IMEICode = "",
                    RoleID = "",
                    WorkingPlace = "",
                    Avatar = "",
                    Comment = "",
                    DeptIDList = db.Bas_Dept.Select(s1 => new
                    {
                        value = s1.DeptID,
                        label = s1.Name
                    }).ToList(),
                    UserTypeIDList = db.Sys_UserType.Select(s1 => new
                    {
                        value = s1.UserTypeID,
                        label = s1.UserTypeName
                    }).ToList(),
                    RoleIDList = db.Sys_Role.Select(s1 => new
                    {
                        value = s1.RoleID,
                        label = s1.RoleName
                    }).ToList(),
                    IsValid = 1,
                    IsValidList = new object[] {
                    new { label = "有效", value = 1 },
                    new { label = "无效", value = 0 }
                }.ToList()
                };
                return Json(true, "", list);
            }
            else
            {
                var list = db.Sys_User.Include(u=>u.Sys_UserRole).Where(w => w.UserID == UserID).Select(s => new
                {
                    UserID = s.UserID,
                    LoginName = s.LoginName,
                    DeptID = s.DeptID,
                    RealName = s.RealName,
                    Phone = s.Phone,
                    UserTypeID = s.UserTypeID,
                    BirthDate = s.BirthDate,
                    CreateDate = s.CreateDate,
                    LastLoginDate = s.LastLoginDate,
                    Email = s.Email,
                    IMEICode = s.IMEICode,
                    RoleID = s.Sys_UserRole.Select(s1 => s1.RoleID).FirstOrDefault(),//db.Sys_UserRole.Where(w1 => w1.UserID == s.UserID).Select(s1 => s1.RoleID).FirstOrDefault(),
                    IsValid = s.IsValid == null ? 1 : s.IsValid,
                    WorkingPlace = s.WorkingPlace,
                    Avatar = s.Avatar,
                    Comment = s.Comment,
                    RoleIDList = db.Sys_Role.Select(s1 => new
                    {
                        value = s1.RoleID,
                        label = s1.RoleName
                    }).ToList(),
                    UserTypeIDList = db.Sys_UserType.Select(s1 => new
                    {
                        value = s1.UserTypeID,
                        label = s1.UserTypeName
                    }).ToList(),
                    DeptIDList = db.Bas_Dept.Select(s1 => new
                    {
                        value = s1.DeptID,
                        label = s1.Name
                    }).ToList(),
                    IsValidList = new object[] {
                    new { label = "有效", value = 1 },
                    new { label = "无效", value = 0 }
                }.ToList(),
                }).FirstOrDefault();

                return Json(true, "", list);
            }
        }

        public HttpResponseMessage SaveSysUserForm(Sys_User obj)
        {
            DBHelper<Sys_User> dbhelp = new DBHelper<Sys_User>();

            obj.RecTimeStamp = DateTime.Now;
            obj.SessionId = Guid.NewGuid();
            obj.CreateDate = DateTime.Now;
            

            var result = obj.UserID == 0 ? dbhelp.Add(obj) : dbhelp.Update(obj);

            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }


        [HttpPost]
        public HttpResponseMessage DeleteSysUserRow(Sys_User obj)
        {
            var result = new DBHelper<Sys_User>().Remove(obj);

            return Json(true, result == 1 ? "删除成功！" : "删除失败");
        }
    }
}
