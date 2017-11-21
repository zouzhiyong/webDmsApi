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
            string userId = "3";// HttpContext.Current.Session["userId"].ToString();

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId).Select(s => new
            {
                MenuID = s.MenuID,
                MenuParentID = s.MenuParentID,
                MenuName = s.MenuName,
                MenuUrl = s.MenuUrl,
                MenuPath = s.MenuPath,
                MenuIcon = s.MenuIcon,
                IsValid = s.IsValid,
                ApplicationNo = s.ApplicationNo,
                Xh = s.Xh,
                Controls = (from t in db.Sys_Templates
                            where t.TemplateName == s.MenuUrl
                            select t.Controls).FirstOrDefault()
            }).OrderBy(o => o.Xh).ThenBy(o => o.MenuID).ToList();

            //var _list = (from t in list
            //             select new
            //             {
            //                 MenuID = t.MenuID,
            //                 MenuParentID = t.MenuParentID,
            //                 MenuName = t.MenuName,
            //                 MenuUrl = t.MenuUrl,
            //                 MenuIcon = t.MenuIcon,
            //                 IsValid = t.IsValid,
            //                 ApplicationNo = t.ApplicationNo,
            //                 Controls= JsonConvert.SerializeObject((from t2 in db.Sys_Templates
            //                           where t2.TemplateName==t.MenuUrl select t2).ToList())
            //                 //Controls = (from t2 in db.Sys_Templates
            //                 //            join t3 in db.Sys_TemplateControl
            //                 //            on t2.TemplateID equals t3.TemplateID
            //                 //            join t4 in db.Sys_Controls
            //                 //            on t3.ControlID equals t4.ControlID
            //                 //            where t2.TemplateName == t.MenuUrl
            //                 //            select new
            //                 //            {
            //                 //                ControlName = t4.ControlName,
            //                 //                ControlID = t3.ControlID,
            //                 //                FindUrl = t3.FindUrl,
            //                 //                DeleteUrl = t3.DeleteUrl,
            //                 //                FormUrl = t3.FormUrl,
            //                 //                SaveUrl = t3.SaveUrl,
            //                 //                SubControls = (from t5 in db.Sys_SubControls
            //                 //                               where (t5.ControlID == t3.ControlID && t5.TemplateID == t3.TemplateID)
            //                 //                               select t5).OrderBy(x=>x.Xh).ToList()
            //                 //            }).ToList()
            //             }).ToList();

            return Json(true, "", list);
        }

        /// <summary>
        /// 菜单编辑窗口左边树
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMoudle()
        {
            var list = db.Sys_Menu.ToList();


            // List<Bas_ComoditiesType> treeList = new List<Bas_ComoditiesType>();
            list.Add(new Sys_Menu
            {
                MenuID = 0,
                MenuName = "所有模块",
                MenuParentID = -1
            });

            //List<object> treeList = new List<object>();

            //var tempOjb = new
            //{
            //    MenuID = 0,
            //    label = "所有模块",
            //    MenuName = "所有模块",
            //    MenuParentID = -1,
            //    children = ListToTree(list, 0)
            //};

            //treeList.Add(tempOjb);

            return Json(true, "", new { rows = list, ID = "MenuID", ParentID = "MenuParentID", Label = "MenuName" });
        }

        /// <summary>
        /// 获取菜单编辑窗口右边表格
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindSysMoudleRow(dynamic obj)
        {
            DBHelper<Sys_Menu> dbhelp = new DBHelper<Sys_Menu>();

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
        public HttpResponseMessage FindSysMoudleForm(dynamic obj)
        {
            int MenuID = obj == null ? 0 : obj.MenuID;
            int MenuParentID = obj.MenuParentID;

            sysMenuForm sysmenuform = new sysMenuForm();
            sysmenuform.MenuParentID = MenuParentID;
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


        private void TreeToList(dynamic obj, List<dynamic> list)
        {
            foreach (dynamic item in obj)
            {
                var sys_menu = new
                {
                    MenuID = item.MenuID,
                    MenuParentID = item.MenuParentID,
                    MenuName = item.MenuName,
                    Status = item.Status
                };
                list.Add(sys_menu);
                if (item.children != null)
                {
                    var children = item.children;
                    TreeToList(children, list);
                }
            }
        }


        public HttpResponseMessage FindMenu()
        {
            string userId = "3";// HttpContext.Current.Session["userId"].ToString();

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId && p.MenuParentID==0).Select(s => new
            {
                path="/",
                name = s.MenuName,
                Xh=s.Xh,
                MenuID=s.MenuID,
                iconCls = s.MenuIcon,
                children= db.View_menu.Where<View_menu>(p1 => p1.MenuParentID == s.MenuID).Select(s1=>new {
                    path="/"+s1.MenuPath,
                    name =s1.MenuName,
                    Xh=s1.Xh,
                    MenuID=s1.MenuID
                }).OrderBy(o => o.Xh).ThenBy(o => o.MenuID).ToList()
        }).OrderBy(o => o.Xh).ThenBy(o => o.MenuID).ToList();


            return Json(true, "", list);
        }
    }
}
