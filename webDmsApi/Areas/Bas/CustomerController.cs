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
    public class CustomerController : ApiBaseController
    {
        webDmsEntities db = new webDmsEntities();

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

        public HttpResponseMessage FindMoudleForm(Bas_Customer obj)
        {
            int CustomerID = obj == null ? 0 : obj.CustomerID;

            var newData = new
            {
                CustomerID = 0,
                CustomerName = "",
                LinkMan = "",
                LinkManPhone = "",
                RegionArr = "",
                IsValid = 1,
                IsValidList = (
                                        new object[] {
                                    new {label = "有效", value = 1 },
                                    new {label="无效",value=0 }
                                        }).ToList(),
                RegionArrList = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == "0").Select(v => new
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
                })
            };

            var list = db.Bas_Customer.Where<Bas_Customer>(p => p.CustomerID == CustomerID).Select(u => new
            {
                CustomerID =  u.CustomerID,
                CustomerName = u.CustomerName,
                LinkMan =  u.LinkMan,
                LinkManPhone = u.LinkManPhone,
                RegionArr =u.Region,
                IsValid = (u.IsValid == null ? 1 : u.IsValid),
                IsValidList = (
                                        new object[] {
                                    new {label = "有效", value = 1 },
                                    new {label="无效",value=0 }
                                        }).ToList(),
                RegionArrList = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == "0").Select(v => new
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
                })
            }).FirstOrDefault();

            if (CustomerID == 0)
            {
                return Json(true, "", newData);
            }else
            {
                return Json(true, "", list);
            }
            
        }

        public HttpResponseMessage SaveMoudleForm(Bas_Customer obj)
        {
            DBHelper<Bas_Customer> dbhelp = new DBHelper<Bas_Customer>();
            var result = obj.CustomerID == 0 ? dbhelp.Add(obj) : dbhelp.Update(obj);
            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }

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

        [HttpPost]
        public HttpResponseMessage DeleteMoudleRow(Bas_Customer obj)
        {
            var result = new DBHelper<Bas_Customer>().Remove(obj);

            return Json(true, result == 1 ? "删除成功！" : "删除失败");
        }

    }
}
