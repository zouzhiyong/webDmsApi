using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Bas
{
    public class ComoditiesController : ApiBaseController
    {
        webDmsEntities db = new webDmsEntities();

        /// <summary>
        /// 菜单编辑窗口左边树
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindLeftTreeMoudle()
        {
            var list = db.Bas_ComoditiesType;
            List<object> treeList = new List<object>();

            var tempOjb = new
            {
                TypeID = 0,
                label = "所有类别",
                TypeName = "所有类别",
                ParentID = 0,
                children = ListToTree(list, 0)
            };

            treeList.Add(tempOjb);

            return Json(true, "", new { rows = list, tree = treeList,ID= "TypeID",Label="TypeName" });
        }

        public HttpResponseMessage SaveLeftTreeMoudle(dynamic obj)
        {            
            DBHelper<Bas_ComoditiesType> dbhelp = new DBHelper<Bas_ComoditiesType>();
            int result = 0;
            List<dynamic> treeData = new List<dynamic>();
            TreeToList(obj[0].children, treeData);
            foreach (var item in treeData)
            {
                if (item.TypeID == 0)
                {
                    db.Entry<Bas_ComoditiesType>(item).State = EntityState.Added;
                }
                else
                {
                    db.Entry<Bas_ComoditiesType>(item).State = EntityState.Modified;
                }
            }

            if (treeData.Count() > 0)
                result += db.SaveChanges();

            return Json(true, (result > 0 && result == treeData.Count()) ? "保存成功！" : "保存失败");
        }

        private void TreeToList(dynamic obj, List<dynamic> list)
        {
            foreach (dynamic item in obj)
            {
                Bas_ComoditiesType bas_comoditiesType = new Bas_ComoditiesType()
                {
                    TypeID = item.TypeID,
                    ParentID = item.ParentID,
                    TypeName = item.TypeName,
                    TypeCode = item.TypeCode,
                    xh = item.xh == null ? 0 : item.xh,
                    IsValid = 1,
                    Remark = item.Remark
                };
                var children = item.children;
                list.Add(bas_comoditiesType);
                TreeToList(children, list);
            }
        }

        private List<object> ListToTree(dynamic list, int parentId)
        {
            List<dynamic> tempList = new List<dynamic>();
            foreach (dynamic item in list)
            {
                if (item.ParentID == parentId)
                {
                    var bas_comoditiesType = new
                    {
                        TypeID = item.TypeID,
                        label = item.TypeName,
                        TypeName = item.TypeName,
                        ParentID = item.ParentID,
                        children = ListToTree(list, item.TypeID)
                    };
                    tempList.Add(bas_comoditiesType);
                }
            }
            return tempList;
        }
    }
}
