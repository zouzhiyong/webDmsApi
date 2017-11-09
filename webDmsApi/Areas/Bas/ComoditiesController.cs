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
            var list = db.Bas_ComoditiesType.ToList();
            // List<Bas_ComoditiesType> treeList = new List<Bas_ComoditiesType>();
            list.Add(new Bas_ComoditiesType
            {
                TypeID=0,
                TypeName= "所有类别",
                ParentID=-1
            });
            //var tempOjb = new
            //{
            //    TypeID = 0,
            //    label = "所有类别",
            //    TypeName = "所有类别",
            //    ParentID = -1,
            //    isEdit=false,
            //    children = ListToTree(list, 0)
            //};

            //treeList.Add(tempOjb);

            return Json(true, "", new { rows = list, ID = "TypeID", ParentID = "ParentID", Label = "TypeName" });
        }

        public HttpResponseMessage SaveLeftTreeMoudle(dynamic obj)
        {
            DBHelper<Bas_ComoditiesType> dbhelp = new DBHelper<Bas_ComoditiesType>();
            int result = 0;

            //新增或修改
            List<dynamic> treeData = new List<dynamic>();
            TreeToList(obj[0].children, treeData);

            List<int> listID = new List<int>();            

            foreach (var item in treeData)
            {
                Bas_ComoditiesType _item = new Bas_ComoditiesType()
                {
                    TypeID = item.TypeID,
                    ParentID = item.ParentID,
                    TypeName = item.TypeName,
                    IsValid = 1,
                };

                listID.Add(item.TypeID);

                //-1为新增
                if (item.Status == -1)
                {
                    db.Entry<Bas_ComoditiesType>(_item).State = EntityState.Added;
                }           
                else
                {
                    db.Entry<Bas_ComoditiesType>(_item).State = EntityState.Modified;
                }
            }

            //删除
            foreach (var item in db.Bas_ComoditiesType.ToList())
            {
                if (listID.BinarySearch(item.TypeID) <0)
                {
                    db.Entry<Bas_ComoditiesType>(item).State = EntityState.Deleted;
                }
            }

            result += db.SaveChanges();


            return Json(true, (result>0) ? "保存成功！" : "保存失败");
        }

        public HttpResponseMessage FindComoditiesRows(dynamic obj)
        {
            DBHelper<Bas_Comodities> dbhelp = new DBHelper<Bas_Comodities>();

            List<dynamic> objTemp = new List<dynamic>();
            objTemp.Add(obj);
            List<dynamic> treeData = new List<dynamic>();
            List<int?> arrID = new List<int?>();
            TreeToList(objTemp, treeData);

            foreach (var item in treeData)
            {
                arrID.Add(item.TypeID);
            }

            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;
            int total = 0;

            var list = dbhelp.FindPagedList(currentPage, pageSize, out total, x => arrID.Contains(x.TypeID), s => s.TypeID, true);

            return Json(list, currentPage, pageSize, total);
        }
        /// <summary>
        /// 树形转数组
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="list">返回生成以后的数组</param>
        private void TreeToList(dynamic obj, List<dynamic> list)
        {
            foreach (dynamic item in obj)
            {
                var bas_comoditiesType = new
                {
                    TypeID = Convert.ToInt32(item.TypeID),
                    ParentID = Convert.ToInt32(item.ParentID),
                    TypeName = Convert.ToString(item.TypeName),
                    IsValid = 1,
                    Status = Convert.ToInt32(item.Status)
                };
                
                list.Add(bas_comoditiesType);
                if (item.children != null)
                {
                    var children = item.children;
                    TreeToList(children, list);
                }
            }
        }

        //private List<object> ListToTree(dynamic list, int parentId)
        //{
        //    List<dynamic> tempList = new List<dynamic>();
        //    foreach (dynamic item in list)
        //    {
        //        int ID = item.TypeID;
        //        if (item.ParentID == parentId)
        //        {
        //            var bas_comoditiesType = new
        //            {
        //                TypeID = item.TypeID,
        //                label = item.TypeName,
        //                isEdit = false,
        //                Status = 0,
        //                isLink = db.Bas_Comodities.Where<Bas_Comodities>(w => w.TypeID == ID).Count() > 0 ? true : false,//是否有对应商品
        //                TypeName = item.TypeName,
        //                ParentID = item.ParentID,
        //                //以下为树形添加字段
        //                isAdd = false,
        //                children = ListToTree(list, item.TypeID)
        //            };
        //            tempList.Add(bas_comoditiesType);
        //        }
        //    }
        //    return tempList;
        //}
    }
}
