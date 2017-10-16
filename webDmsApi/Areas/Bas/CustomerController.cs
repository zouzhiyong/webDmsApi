using System;
using System.Collections.Generic;
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

            //string MenuNo = obj.MenuNo;
            int pageSize = obj.pageSize;
            int currentPage = obj.currentPage;

            var data = db.Bas_Customer.Where<Bas_Customer>(p =>1==1).ToList();

            //var data = (from t1 in db.Sys_Menu
            //            join t2 in db.Sys_dictionarydata
            //            on t1.ApplicationNo equals t2.dictdata_Value
            //            where (t2.dictdata_Table == "Sys_Menu" && t2.dictdata_Field == "ApplicationNo" && t1.MenuParentNo == MenuNo)
            //            select new
            //            {
            //                MenuID = t1.MenuID,
            //                MenuName = t1.MenuName,
            //                MenuUrl = t1.MenuUrl,
            //                MenuIcon = t1.MenuIcon,
            //                IsValid = t1.IsValid == null || t1.IsValid == 1 ? "有效" : "无效",
            //                ApplicationNo = t2.dictdata_Name
            //            }).ToList();
            var rows = data
                .OrderBy(a => a.CustomerID)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize);


            return Json(rows, currentPage, pageSize, data.Count);
        }

    }
}
