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
        //[AcceptVerbs("GET", "POST")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage GetAllSysForms()
        {
            webDmsEntities db = new webDmsEntities();

            string userId = HttpContext.Current.Session["userId"].ToString();

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId);

            return Json(true, "", list);
        }

        //[AcceptVerbs("GET", "POST")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage GetAllSysMoudle()
        {
            webDmsEntities db = new webDmsEntities();

            string userId = HttpContext.Current.Session["userId"].ToString();

            var list = db.View_menu.Where<View_menu>(p => p.UserID.ToString() == userId);

            Menu item = new Menu {
                MenuNo="",MenuName="",MenuParentNo="",children= new Menu[] { }
            };

            BindTreeView(item, list, "0");
            return Json(true, "", new { rows=list,tree= item.children });
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
                replyNode.MenuName = cc.MenuName;
                replyNode.MenuParentNo = cc.MenuParentNo;
                replyNode.children= new Menu[] { };
                
                _list.Add(replyNode);
                item.children = _list.ToArray();

                BindTreeView(replyNode, list, cc.MenuNo);
               
            }
            
        }

        public partial class Menu
        {
            public int MenuID { get; set; }
            public string MenuNo { get; set; }
            public string MenuParentNo { get; set; }
            public string MenuName { get; set; }
            public Menu[] children { get; set; }
        }
    }
}
