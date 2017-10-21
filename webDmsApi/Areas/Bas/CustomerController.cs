using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Bas
{
    public class CustomerController : ApiBaseController
    {
        webDmsEntities db = new webDmsEntities();

        partial class customerForm : Bas_Customer
        {
            public IQueryable<object> RegionList { get; set; }
        }
        /// <summary>
        /// 浏览窗口表格数据查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage FindMoudleRows(dynamic obj)
        {
            DBHelper<View_Customer> dbhelp = new DBHelper<View_Customer>();

            string CustomerName = obj.CustomerName;
            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;
            int total = 0;
            string Region = obj.Region;

            var list = dbhelp.FindPagedList(currentPage, pageSize, out total, x => (x.CustomerName.Contains(CustomerName) && x.Region.Contains(Region)), s => s.CustomerID, true);

            return Json(list, currentPage, pageSize, total);

        }
        /// <summary>
        /// 表单窗口数据获取，包括新增和查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage FindMoudleForm(Bas_Customer obj)
        {
            int CustomerID = obj == null ? 0 : obj.CustomerID;

            IQueryable<object> mList = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == "0").Select(v => new
            {
                RegionNo = v.RegionNo,
                label = v.RegionName,
                RegionParentNo = v.RegionParentNo,
                children = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == v.RegionNo).Select(v1 => new
                {
                    RegionNo = v1.RegionNo,
                    label = v1.RegionName,
                    RegionParentNo = v1.RegionParentNo,
                    children = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == v1.RegionNo).Select(v2 => new
                    {
                        RegionNo = v2.RegionNo,
                        label = v2.RegionName,
                        RegionParentNo = v2.RegionParentNo
                    }).ToList()
                }).ToList()
            });

            customerForm customerform = new customerForm();
            customerform.RegionList = mList;
            customerform.IsValid = 1;

            if (CustomerID != 0)
            {
                DBHelper<Bas_Customer> dbhelp = new DBHelper<Bas_Customer>();
                var list = dbhelp.FindList(p => p.CustomerID == CustomerID).FirstOrDefault();

                PropertyInfo[] properties1 = customerform.GetType().GetProperties();
                PropertyInfo[] properties2 = list.GetType().GetProperties();

                foreach (PropertyInfo item1 in properties1)
                {
                    foreach (PropertyInfo item2 in properties2)
                    {
                        if(item1.Name==item2.Name && item1.PropertyType == item2.PropertyType)
                        {
                            item1.SetValue(customerform, item2.GetValue(list,null),null);
                            break;
                        }                        
                    }
                }
            }
            return Json(true, "", customerform);
        }
        /// <summary>
        /// 表单数据保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage SaveMoudleForm(Bas_Customer obj)
        {
            DBHelper<Bas_Customer> dbhelp = new DBHelper<Bas_Customer>();
            var result = obj.CustomerID == 0 ? dbhelp.Add(obj) : dbhelp.Update(obj);
            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }
        /// <summary>
        /// 查询条件中销售区域获取
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage FindMoudleRegionList()
        {
            var list = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == "0").Select(v => new
            {
                RegionNo = v.RegionNo,
                label = v.RegionName,
                RegionParentNo = v.RegionParentNo,
                children = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == v.RegionNo).Select(v1 => new
                {
                    RegionNo = v1.RegionNo,
                    label = v1.RegionName,
                    RegionParentNo = v1.RegionParentNo,
                    children = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == v1.RegionNo).Select(v2 => new
                    {
                        RegionNo = v2.RegionNo,
                        label = v2.RegionName,
                        RegionParentNo = v2.RegionParentNo
                    }).ToList()
                }).ToList()
            }).ToList();

            return Json(true, "", list);
        }
        /// <summary>
        /// 浏览窗口删除数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteMoudleRow(Bas_Customer obj)
        {
            var result = new DBHelper<Bas_Customer>().Remove(obj);

            return Json(true, result == 1 ? "删除成功！" : "删除失败");
        }

    }
}
