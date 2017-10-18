using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Sys
{
    public class MenuController : ApiBaseController
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMenu()
        {
            webDmsEntities db = new webDmsEntities();

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
                                             DeleteUrl=t3.DeleteUrl,
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
            webDmsEntities db = new webDmsEntities();

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
            webDmsEntities db = new webDmsEntities();

            int MenuID = obj.MenuID;
            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;

            var data = (from t1 in db.Sys_Menu
                        join t2 in db.Sys_DictionaryData
                        on t1.ApplicationNo equals t2.DictdataValue
                        where (t2.DictdataTable == "Sys_Menu" && t2.DictdataField == "ApplicationNo" && t1.MenuParentID == MenuID)
                        select new
                        {
                            MenuID = t1.MenuID,
                            MenuName = t1.MenuName,
                            MenuUrl = t1.MenuUrl,
                            MenuIcon = t1.MenuIcon,
                            IsValid = t1.IsValid == null || t1.IsValid == 1 ? "有效" : "无效",
                            ApplicationNo = t2.DictdataName
                        }).ToList();
            var rows = data
                .OrderBy(a => a.MenuID)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);

            return Json(rows, currentPage, pageSize, data.Count);
        }

        public HttpResponseMessage FindSysMoudleForm(dynamic obj)
        {
            webDmsEntities db = new webDmsEntities();
            int MenuID = obj == null ? 0 : obj.MenuID;

            var _list = db.Sys_Menu.Where<Sys_Menu>(p => p.MenuID == MenuID);

            var newData = new
            {
                MenuID = 0,
                MenuName = "",
                MenuParentID = 0,
                MenuUrl = "",
                IsValid = 1,
                MenuIcon = "",
                ApplicationNo = "",
                Xh = "",

                IsValidList = (
                                new object[] {
                                    new {label = "有效", value = 1 },
                                    new {label="无效",value=0 }
                                }).ToList(),
                ApplicationNoList = db.Sys_Application.Select(a => new
                {
                    label = a.ApplicationName,
                    value = a.ApplicationNo
                }),
                MenuParentIDList = new object[] { new { label = "未对应上级", value = 0 } }.
                            Concat(
                                db.Sys_Menu.Where<Sys_Menu>(a => a.MenuParentID == 0 && a.IsValid != 0).Select(a => new
                                {
                                    label = a.MenuName,
                                    value = a.MenuID
                                }))
            };

            var list = (from t1 in _list
                        select new
                        {
                            MenuID = t1.MenuID,
                            MenuName = t1.MenuName,
                            MenuParentID = t1.MenuParentID,
                            MenuUrl = t1.MenuUrl,
                            IsValid = t1.IsValid == null ? 1 : t1.IsValid,
                            MenuIcon = t1.MenuIcon,
                            ApplicationNo = t1.ApplicationNo,
                            Xh = t1.Xh,

                            IsValidList = (
                                new object[] {
                                    new {label = "有效", value = 1 },
                                    new {label="无效",value=0 }
                                }).ToList(),
                            ApplicationNoList = db.Sys_Application.Select(a => new
                            {
                                label = a.ApplicationName,
                                value = a.ApplicationNo
                            }),
                            MenuParentIDList = new object[] { new { label = "未对应上级", value = 0 } }.
                            Concat(
                                db.Sys_Menu.Where<Sys_Menu>(a => a.MenuParentID == 0 && a.IsValid != 0).Select(a => new
                                {
                                    label = a.MenuName,
                                    value = a.MenuID
                                }))
                        }).FirstOrDefault();

            if (MenuID == 0)
            {
                return Json(true, "", newData);
            }
            else
            {
                return Json(true, "", list);
            }
        }

        public HttpResponseMessage SaveSysMoudleForm(Sys_Menu menu)
        {
            webDmsEntities db = new webDmsEntities();

            db.Entry<Sys_Menu>(menu).State = menu.MenuID == 0 ? EntityState.Added : EntityState.Modified;

            var result = db.SaveChanges();

            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }
    }
}
