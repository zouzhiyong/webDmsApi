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

        public HttpResponseMessage FindRightTable1(dynamic obj)
        {
            int TemplateID = obj.TemplateID;
            var list = db.Sys_TemplateControl.Where<Sys_TemplateControl>(x => x.TemplateID == TemplateID).Select(v => new
            {
                TemplateID = v.TemplateID,
                ControlID=v.ControlID,
                FindUrl = v.FindUrl,
                FormUrl = v.FormUrl,
                DeleteUrl = v.DeleteUrl,
                SaveUrl = v.SaveUrl,
                TemplateName = db.Sys_Templates.Where<Sys_Templates>(w1 => w1.TemplateID == TemplateID).Select(a => a.TemplateName).FirstOrDefault(),
                ControlName = db.Sys_Controls.Where<Sys_Controls>(w2 => w2.ControlID == v.ControlID).Select(a => a.ControlName).FirstOrDefault()
            }).ToList();

            return Json(true, "", list);
        }

        public HttpResponseMessage FindRightTable2(dynamic obj)
        {
            int TemplateID = obj.TemplateID;
            int ControlID = obj.ControlID;
            var list = db.Sys_SubControls.Where<Sys_SubControls>(x => (x.TemplateID == TemplateID && x.ControlID==ControlID)).ToList();

            return Json(true, "", list);
        }
    }
}
