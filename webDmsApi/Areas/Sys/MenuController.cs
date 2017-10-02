using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
