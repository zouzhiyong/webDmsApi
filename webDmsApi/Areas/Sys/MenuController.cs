using Newtonsoft.Json;
using System;
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

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId);

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
                             ApplicationID = t.ApplicationID,
                             Control = (from t2 in db.Sys_Control
                                        select new {
                                            ControlID = t2.ControlID,
                                            MenuID = t2.MenuID,
                                            URL = t2.URL,
                                            ControlName = t2.ControlName,
                                            describe = t2.describe,
                                            columns = (from a1 in db.Sys_ControlDetail
                                                       where a1.ControlID == t2.ControlID
                                                       select a1).ToList()
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
                MenuName= "所有模块",
                MenuParentNo = "",
                children = new Menu[] { }
            };

            object[] arr = new object[] { item };

            BindTreeView(item, list, "0");
            return Json(true, "", new { rows = list, tree = arr, id="MenuID" });
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
        public HttpResponseMessage FindSysMoudleRow(string parentNo)
        {
            webDmsEntities db = new webDmsEntities();

            //var list = db.Sys_Menu.Where<Sys_Menu>(p => p.MenuParentNo == parentNo);

            var list = (from t1 in db.Sys_Menu
                       join t2 in db.Sys_dictionarydata
                       on t1.ApplicationID.ToString() equals t2.dictdata_Value
                       where (t2.dictdata_Table== "Sys_Menu" && t2.dictdata_Field== "ApplicationID" && t1.MenuParentNo== parentNo)
                       select new
                       {
                           MenuID=t1.MenuID,
                           MenuName = t1.MenuName,
                           MenuUrl = t1.MenuUrl,
                           MenuIcon = t1.MenuIcon,
                           IsValid = t1.IsValid==null || t1.IsValid==1 ?"有效":"无效",
                           ApplicationID = t2.dictdata_Name
                       }).ToList();


            //object[] arr = new object[]
            //    {
            //    new  { type="index",prop = "", label = "", width = "50" },
            //    new  { type="",prop = "MenuName", label = "模块名称", width = "200" },
            //    new  { type="",prop = "MenuUrl", label = "模块路径", width = "250" },
            //    new  { type="",prop = "MenuIcon", label = "图标", width = "100" },
            //    new  { type="",prop = "IsValid", label = "有效否", width = "100" },
            //    new  { type="",prop = "ApplicationID", label = "应用平台", width = "250" },
            //    };


            return Json(true, "", list);
        }

    }
}
