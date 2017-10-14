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
                             MenuNo = t.MenuNo,
                             MenuParentNo = t.MenuParentNo,
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
                                          select new {
                                              ControlName = t4.ControlName,
                                              ControlID = t3.ControlID,
                                              FindUrl = t3.FindUrl,
                                              FormUrl = t3.FormUrl,
                                              SaveUrl = t3.SaveUrl,
                                              SubControls = (from t5 in db.Sys_SubControls
                                                             where (t5.ControlID == t3.ControlID)
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

            Menu item = new Menu
            {
                MenuID = 0,
                MenuNo = "0",
                label = "所有模块",
                MenuName = "所有模块",
                MenuParentNo = "",
                children = new Menu[] { }
            };

            object[] arr = new object[] { item };

            BindTreeView(item, list, "0");
            return Json(true, "", new { rows = list, tree = arr });
        }

        private void BindTreeView(Menu item, IQueryable<View_menu> list, string parent)
        {
            List<Menu> _list = item.children.ToList();

            var childs = list.Where(p => p.MenuParentNo == parent);

            foreach (View_menu cc in childs)
            {
                Menu replyNode = new Menu();
                replyNode.MenuID = cc.MenuID;
                replyNode.MenuNo = cc.MenuNo;
                replyNode.label = cc.MenuName;
                replyNode.MenuName = cc.MenuName;
                replyNode.MenuParentNo = cc.MenuParentNo;
                replyNode.children = new Menu[] { };

                _list.Add(replyNode);
                item.children = _list.ToArray();

                BindTreeView(replyNode, list, cc.MenuNo);

            }

        }

        public partial class Menu
        {
            public int MenuID { get; set; }
            public string MenuNo { get; set; }
            public string label { get; set; }
            public string MenuParentNo { get; set; }
            public string MenuName { get; set; }
            public Menu[] children { get; set; }
        }

       
        /// <summary>
        /// 获取菜单编辑窗口右边表格
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMoudleRow(dynamic obj)
        {
            webDmsEntities db = new webDmsEntities();

            string MenuNo = obj.MenuNo;
            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;

            //var list = db.Sys_Menu.Where<Sys_Menu>(p => p.MenuParentNo == parentNo);

            var data = (from t1 in db.Sys_Menu
                        join t2 in db.Sys_dictionarydata
                        on t1.ApplicationNo equals t2.dictdata_Value
                        where (t2.dictdata_Table == "Sys_Menu" && t2.dictdata_Field == "ApplicationNo" && t1.MenuParentNo == MenuNo)
                        select new
                        {
                            MenuID = t1.MenuID,
                            MenuName = t1.MenuName,
                            MenuUrl = t1.MenuUrl,
                            MenuIcon = t1.MenuIcon,
                            IsValid = t1.IsValid == null || t1.IsValid == 1 ? "有效" : "无效",
                            ApplicationNo = t2.dictdata_Name
                        }).ToList();
            var rows = data
                .OrderBy(a => a.MenuID)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);


            //object[] arr = new object[]
            //    {
            //    new  { type="index",prop = "", label = "", width = "50" },
            //    new  { type="",prop = "MenuName", label = "模块名称", width = "200" },
            //    new  { type="",prop = "MenuUrl", label = "模块路径", width = "250" },
            //    new  { type="",prop = "MenuIcon", label = "图标", width = "100" },
            //    new  { type="",prop = "IsValid", label = "有效否", width = "100" },
            //    new  { type="",prop = "ApplicationID", label = "应用平台", width = "250" },
            //    };


            return Json(rows, currentPage, pageSize, data.Count);
        }

        public HttpResponseMessage FindSysMoudleForm(dynamic obj)
        {

            webDmsEntities db = new webDmsEntities();
            int MenuID = obj.MenuID;

            var _list = db.Sys_Menu.Where<Sys_Menu>(p => p.MenuID == MenuID);

            var list = (from t1 in _list
                        select new
                        {
                            MenuID = t1.MenuID,
                            MenuName = t1.MenuName,
                            MenuNo = t1.MenuNo,
                            MenuParentNo = t1.MenuParentNo,
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
                            MenuParentNoList = new object[] { new { label = "未对应上级", value = "0" } }.
                            Concat(
                                db.Sys_Menu.Where<Sys_Menu>(a => a.MenuParentNo == "0" && a.IsValid != 0).Select(a => new
                                {
                                    label = a.MenuName,
                                    value = a.MenuNo
                                }))
                        });

            return Json(true, "", list);
        }

        public HttpResponseMessage SaveSysMoudleForm(Sys_Menu menu)
        {
            webDmsEntities db = new webDmsEntities();

            db.Entry<Sys_Menu>(menu).State = EntityState.Modified;

            var result = db.SaveChanges();

            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }
    }
}
