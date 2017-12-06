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
    public class DeptController : ApiBaseController
    {
        public HttpResponseMessage FindSysDeptTable(dynamic obj)
        {
            DBHelper<View_Dept> dbhelp = new DBHelper<View_Dept>();

            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;
            int total = 0;

            var list = dbhelp.FindPagedList(currentPage, pageSize, out total, x => 1 == 1, s => s.DeptID, true);

            return Json(list, currentPage, pageSize, total);
        }

        [HttpPost]
        public HttpResponseMessage DeleteSysDeptRow(Sys_Dept obj)
        {
            var result = new DBHelper<Sys_Dept>().Remove(obj);

            return Json(true, result == 1 ? "删除成功！" : "删除失败");
        }

        public HttpResponseMessage FindSysDeptForm(dynamic obj)
        {
            int DeptID = obj == null ? 0 : obj.DeptID;


            if (DeptID == 0)
            {
                var list = new
                {
                    DeptID = 0,
                    Name = "",
                    IsValid = 1,
                    IsValidList = new object[] {
                    new { label = "有效", value = 1 },
                    new { label = "无效", value = 0 }
                }.ToList(),
                    ModifyUserID = "",
                    ModifyDate = DateTime.Now,
                    CreateUserID = "",
                    CreateDate = DateTime.Now,
                    Comment = ""
                };
                return Json(true, "", list);
            }
            else
            {
                var list = db.Sys_Dept.Where(w => w.DeptID == DeptID).Select(s => new
                {
                    DeptID = s.DeptID,
                    Name = s.Name,
                    IsValid = s.IsValid == null ? 1 : s.IsValid,
                    IsValidList = new object[] {
                    new { label = "有效", value = 1 },
                    new { label = "无效", value = 0 }
                }.ToList(),
                    ModifyUserID = "",
                    ModifyDate = DateTime.Now,
                    CreateUserID = "",
                    CreateDate = DateTime.Now,
                    Comment = s.Comment
                }).FirstOrDefault();

                return Json(true, "", list);
            }
        }

        public HttpResponseMessage SaveSysDeptForm(Sys_Dept obj)
        {
            DBHelper<Sys_Dept> dbhelp = new DBHelper<Sys_Dept>();
            var result = obj.DeptID == 0 ? dbhelp.Add(obj) : dbhelp.Update(obj);

            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }
    }
}
