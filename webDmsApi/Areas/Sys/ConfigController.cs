using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webDmsApi.Controllers;
using webDmsApi.Models;

namespace webDmsApi.Areas.Sys
{
    public class ConfigController : ApiBaseController
    {
        webDmsEntities db = new webDmsEntities();

        public HttpResponseMessage FindLeftMenu()
        {
            var list = db.Sys_Templates.Where<Sys_Templates>(x => x.IsValid != 0).ToList();

            return Json(true, "", list);
        }

        public HttpResponseMessage SaveTemplate(Sys_Templates obj)
        {
            DBHelper<Sys_Templates> dbhelp = new DBHelper<Sys_Templates>();
            var result = obj.TemplateID == 0 ? dbhelp.Add(obj) : dbhelp.Update(obj);
            return Json(true, result == 1 ? "保存成功！" : "保存失败");
        }
    }
}
