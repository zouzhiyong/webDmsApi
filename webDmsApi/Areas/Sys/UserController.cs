﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;
using System.Data.Entity;
using System.Transactions;

namespace webDmsApi.Areas.Sys
{
    public class UserController : ApiBaseController
    {
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysDeptTree()
        {
            var list = db.Sys_Dept.Where<Sys_Dept>(p => p.IsValid != 0).Select(s => new
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
                    DeptID = DBNull.Value,
                    RealName = "",
                    Phone = "",
                    UserTypeID = DBNull.Value,
                    BirthDate = "",
                    CreateDate = "",
                    LastLoginDate = "",
                    Email = "",
                    IMEICode = "",
                    RoleID = "",
                    WorkingPlace = "",
                    Avatar = "",
                    Comment = "",
                    DeptIDList = db.Sys_Dept.Select(s1 => new
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
                var list = db.Sys_User.Where(w => w.UserID == UserID).Select(s => new
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
                    RoleID = db.Sys_UserRole.Where(w1 => w1.UserID == s.UserID).Select(s1 => s1.RoleID).FirstOrDefault(),
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
                    DeptIDList = db.Sys_Dept.Select(s1 => new
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

        public HttpResponseMessage SaveSysUserForm(dynamic obj)
        {
            var destination = new Sys_User()
            {
                UserID=obj.UserID,
                LoginName = obj.LoginName,
                LoginPassword = "",
                DeptID = obj.DeptID,
                RealName = obj.RealName,
                Phone = obj.Phone,
                UserTypeID = obj.UserTypeID,
                BirthDate = DateTime.Now,
                CreateDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Email = obj.Email,
                IMEICode = obj.IMEICode,
                IsValid = obj.IsValid,
                WorkingPlace = obj.WorkingPlace,
                Avatar = obj.Avatar,
                Comment = obj.Comment,
                SessionId = Guid.NewGuid(),
                RecTimeStamp = DateTime.Now,
                Sys_UserRole = new List<Sys_UserRole>
                    {
                        new Sys_UserRole {
                            RoleID =obj.RoleID,
                            UserID=obj.UserID,
                            CreateUserID=0,
                            CreateDate=DateTime.Now,
                            ModifyUserID=obj.UserID,
                            ModifyDate=DateTime.Now,
                            RecTimeStamp=DateTime.Now
                        }
                    }
            };


            DBHelper<Sys_User> dbhelp = new DBHelper<Sys_User>();

            var result = obj.UserID == 0 ? dbhelp.Add(destination) : dbhelp.Update(destination);

            return Json(true, result >0 ? "保存成功！" : "保存失败");
        }


        [HttpPost]
        public HttpResponseMessage DeleteSysUserRow(Sys_User obj)
        {
            //var sysUser = new Sys_User();


            //var result = new DBHelper<Sys_User>().Remove(obj);

            var sysUser = (from t in db.Sys_User
                          where t.UserID==obj.UserID
                          select t).Single();

            foreach (var sysUserRole in sysUser.Sys_UserRole.ToList())
            {
                db.Sys_UserRole.Remove(sysUserRole);   //先标记相关的从表数据为删除状态
            }
            db.Sys_User.Remove(sysUser);    //再标记主表数据为删除装填
            var result=db.SaveChanges();   //执行上面的所有标记


            return Json(true, result >0 ? "删除成功！" : "删除失败");
        }
    }
}
