using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Bas
{
    public class CustomerController : ApiBaseController
    {
        public HttpResponseMessage FindMoudleRow(dynamic obj)
        {
            webDmsEntities db = new webDmsEntities();

            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;

            var data = db.Bas_Customer.Where<Bas_Customer>(p => 1 == 1).Select(u => new
            {
                CustomerID = u.CustomerID,
                CustomerName = u.CustomerName,
                LinkMan = u.LinkMan,
                LinkManPhone = u.LinkManPhone,
                RegionArr =db.View_Region.Where<View_Region>(t=>t.RegionValue==u.Region).Select(v=>v.Name).FirstOrDefault(),
                IsValid = (u.IsValid == null || u.IsValid == 1 ? "正常" : "关门")
            }).ToList();

            var rows = data
                .OrderBy(a => a.CustomerID)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);


            return Json(rows, currentPage, pageSize, data.Count);
        }

        public HttpResponseMessage FindMoudleForm(dynamic obj)
        {
            webDmsEntities db = new webDmsEntities();

            int CustomerID = obj.CustomerID;
            var list = db.Bas_Customer.Where<Bas_Customer>(p => p.CustomerID == CustomerID).Select(u => new
            {
                CustomerID = u.CustomerID,
                CustomerName = u.CustomerName,
                LinkMan = u.LinkMan,
                LinkManPhone = u.LinkManPhone,
                RegionArr = u.Region,
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
                    children = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == v.RegionNo).Select(v1=>new {
                        RegionNo = v1.RegionNo,
                        label = v1.RegionName,
                        RegionParentNo = v1.RegionParentNo,
                        children = db.Sys_Region.Where<Sys_Region>(p => p.RegionParentNo == v1.RegionNo).Select(v2 => new {
                            RegionNo = v2.RegionNo,
                            label = v2.RegionName,
                            RegionParentNo = v2.RegionParentNo
                        }).ToList()
                    }).ToList()
                })
            });


            return Json(true, "", list);
        }

        public HttpResponseMessage FindMoudleRegion(dynamic obj)
        {
            webDmsEntities db = new webDmsEntities();
            string regionNo = obj.RegionNo;

            var list = db.Sys_Region.Where(p => p.RegionParentNo == regionNo).Select(v=>new {
                RegionNo = v.RegionNo,
                label = v.RegionName,
                children = db.Sys_Region.Where<Sys_Region>(p => 1 == 0).ToList(),
                RegionParentNo = v.RegionParentNo
            });

            return Json(true, "", list);
        }

        public HttpResponseMessage SaveMoudleForm(Bas_Customer obj)
        {
            webDmsEntities db = new webDmsEntities();

            db.Entry<Bas_Customer>(obj).State = EntityState.Modified;

            var result = db.SaveChanges();

            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }

    }
}
