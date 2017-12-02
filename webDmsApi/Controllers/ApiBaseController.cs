using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using webDmsApi.App_Start;
using webDmsApi.Models;

namespace webDmsApi.Controllers
{
    //[RequestAuthorize]
    [WebApiExceptionFilter]
    //[Authorize]
    /// <summary>
    /// 
    /// </summary>
    public class ApiBaseController : ApiController
    {
        public static HttpResponseMessage Json(Object obj, bool @new = true)
        {
            if (@new)
                obj = new { list = obj };
            var str = JsonConvert.SerializeObject(obj);
            var result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        public HttpResponseMessage Json(bool result, string message)
        {
            var obj = new { result = result, message = message };
            return Json(obj, false);
        }
        public HttpResponseMessage Json(bool result, string message, object data)
        {
            var obj = new { result = result, message = message, data = data };
            return Json(obj, false);
        }
        public HttpResponseMessage Json(object data, int currentPage, int pageSize, int? total)
        {
            var obj = new { rows = data, currentPage = currentPage, pageSize = pageSize, total = total };
            return Json(obj, false);
        }

        public static webDmsEntities db = new webDmsEntities();

    }
}
