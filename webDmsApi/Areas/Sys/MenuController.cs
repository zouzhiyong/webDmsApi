using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Sys
{
    public class MenuController : ApiBaseController
    {
        webDmsEntities db = new webDmsEntities();
        partial class sysMenuForm : Sys_Menu
        {
            public IQueryable<object> ApplicationNoList { get; set; }
            public IEnumerable<object> MenuParentIDList { get; set; }
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMenu()
        {
            string userId = HttpContext.Current.Session["userId"].ToString();

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId).OrderBy(o => o.Xh).ThenBy(o => o.MenuID);

            var _list = (from t in list
                         select new
                         {
                             MenuID = t.MenuID,
                             MenuParentID = t.MenuParentID,
                             MenuName = t.MenuName,
                             MenuUrl = t.MenuUrl,
                             MenuIcon = t.MenuIcon,
                             IsValid = t.IsValid,
                             ApplicationNo = t.ApplicationNo,
                             Controls = (from t2 in db.Sys_Templates
                                         join t3 in db.Sys_TemplateControl
                                         on t2.TemplateID equals t3.TemplateID
                                         join t4 in db.Sys_Controls
                                         on t3.ControlID equals t4.ControlID
                                         where t2.TemplateName == t.MenuUrl
                                         select new
                                         {
                                             ControlName = t4.ControlName,
                                             ControlID = t3.ControlID,
                                             FindUrl = t3.FindUrl,
                                             DeleteUrl = t3.DeleteUrl,
                                             FormUrl = t3.FormUrl,
                                             SaveUrl = t3.SaveUrl,
                                             SubControls = (from t5 in db.Sys_SubControls
                                                            where (t5.ControlID == t3.ControlID && t5.TemplateID == t3.TemplateID)
                                                            select t5).ToList()
                                         }).ToList()
                         }).ToList();

            return Json(true, "", _list);
        }

        /// <summary>
        /// 菜单编辑窗口左边树
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMoudle()
        {
            string userId = HttpContext.Current.Session["userId"].ToString();

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId);

            var treeList = new object[]{
                new
                {
                    MenuID = 0,
                    label = "所有模块",
                    MenuName = "所有模块",
                    MenuParentID = 0,
                    children = list.Where<View_menu>(p => p.MenuParentID == 0).Select(t1 => new {
                        MenuID = t1.MenuID,
                        label = t1.MenuName,
                        MenuName = t1.MenuName,
                        MenuParentID = t1.MenuParentID,
                        children = list.Where<View_menu>(p => p.MenuParentID == t1.MenuID).Select(t2 => new {
                            MenuID = t2.MenuID,
                            label = t2.MenuName,
                            MenuName = t2.MenuName,
                            MenuParentID = t2.MenuParentID
                        }).ToList()
                    }).ToList()
                }
            };

            return Json(true, "", new { rows = list, tree = treeList });
        }

        /// <summary>
        /// 获取菜单编辑窗口右边表格
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMoudleRow(dynamic obj)
        {
            DBHelper<View_menu> dbhelp = new DBHelper<View_menu>();

            int MenuID = obj.MenuID;
            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;
            int total = 0;

            var list = dbhelp.FindPagedList(currentPage, pageSize, out total, x => x.MenuParentID == MenuID, s => s.MenuID, true);

            return Json(list, currentPage, pageSize, total);
        }
        /// <summary>
        /// 表单窗口数据获取
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage FindSysMoudleForm(Sys_Menu obj)
        {
            int MenuID = obj == null ? 0 : obj.MenuID;

            sysMenuForm sysmenuform = new sysMenuForm();
            sysmenuform.ApplicationNoList = db.Sys_Application.Select(a => new
            {
                label = a.ApplicationName,
                value = a.ApplicationNo
            });
            sysmenuform.MenuParentIDList = new object[] { new { label = "未对应上级", value = 0 } }.
                        Concat(
                            db.Sys_Menu.Where<Sys_Menu>(a => a.MenuParentID == 0 && a.IsValid != 0).Select(a => new
                            {
                                label = a.MenuName,
                                value = a.MenuID
                            }));
            sysmenuform.IsValid = 1;

            if (MenuID != 0)
            {
                DBHelper<Sys_Menu> dbhelp = new DBHelper<Sys_Menu>();
                var list = dbhelp.FindList(p => p.MenuID == MenuID).FirstOrDefault();

                PropertyInfo[] properties1 = sysmenuform.GetType().GetProperties();
                PropertyInfo[] properties2 = list.GetType().GetProperties();

                foreach (PropertyInfo item1 in properties1)
                {
                    foreach (PropertyInfo item2 in properties2)
                    {
                        if (item1.Name == item2.Name && item1.PropertyType == item2.PropertyType)
                        {
                            item1.SetValue(sysmenuform, item2.GetValue(list, null), null);
                            break;
                        }
                    }
                }
            }
            return Json(true, "", sysmenuform);






            //webDmsEntities db = new webDmsEntities();
            //int MenuID = obj == null ? 0 : obj.MenuID;

            //var _list = db.Sys_Menu.Where<Sys_Menu>(p => p.MenuID == MenuID);

            //var newData = new
            //{
            //    MenuID = 0,
            //    MenuName = "",
            //    MenuParentID = 0,
            //    MenuUrl = "",
            //    IsValid = 1,
            //    MenuIcon = "",
            //    ApplicationNo = "",
            //    Xh = "",

            //    IsValidList = (
            //                    new object[] {
            //                        new {label = "有效", value = 1 },
            //                        new {label="无效",value=0 }
            //                    }).ToList(),
            //    ApplicationNoList = db.Sys_Application.Select(a => new
            //    {
            //        label = a.ApplicationName,
            //        value = a.ApplicationNo
            //    }),
            //    MenuParentIDList = new object[] { new { label = "未对应上级", value = 0 } }.
            //                Concat(
            //                    db.Sys_Menu.Where<Sys_Menu>(a => a.MenuParentID == 0 && a.IsValid != 0).Select(a => new
            //                    {
            //                        label = a.MenuName,
            //                        value = a.MenuID
            //                    }))
            //};

            //var list = (from t1 in _list
            //            select new
            //            {
            //                MenuID = t1.MenuID,
            //                MenuName = t1.MenuName,
            //                MenuParentID = t1.MenuParentID,
            //                MenuUrl = t1.MenuUrl,
            //                IsValid = t1.IsValid,
            //                MenuIcon = t1.MenuIcon,
            //                ApplicationNo = t1.ApplicationNo,
            //                Xh = t1.Xh,
            //                ApplicationNoList = db.Sys_Application.Select(a => new
            //                {
            //                    label = a.ApplicationName,
            //                    value = a.ApplicationNo
            //                }),
            //                MenuParentIDList = new object[] { new { label = "未对应上级", value = 0 } }.
            //                Concat(
            //                    db.Sys_Menu.Where<Sys_Menu>(a => a.MenuParentID == 0 && a.IsValid != 0).Select(a => new
            //                    {
            //                        label = a.MenuName,
            //                        value = a.MenuID
            //                    }))
            //            }).FirstOrDefault();

            //if (MenuID == 0)
            //{
            //    return Json(true, "", newData);
            //}
            //else
            //{
            //    return Json(true, "", list);
            //}
        }
        /// <summary>
        /// 表单数据保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage SaveSysMoudleForm(Sys_Menu obj)
        {
            DBHelper<Sys_Menu> dbhelp = new DBHelper<Sys_Menu>();
            var result = obj.MenuID == 0 ? dbhelp.Add(obj) : dbhelp.Update(obj);

            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }

    }
}
