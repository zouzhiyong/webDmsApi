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

            var treeList = new object[]{
                new
                {
                    TypeID = 0,
                    label = "所有类别",
                    TypeName = "所有类别",
                    ParentID = 0,
                    children = list.Where<Bas_ComoditiesType>(p => p.ParentID == 0).Select(t1 => new {
                        TypeID = t1.TypeID,
                        label = t1.TypeName,
                        TypeName = t1.TypeName,
                        ParentID = t1.ParentID,
                        children = list.Where<Bas_ComoditiesType>(p => p.ParentID == t1.TypeID).Select(t2 => new {
                            TypeID = t2.TypeID,
                            label = t2.TypeName,
                            TypeName = t2.TypeName,
                            ParentID = t2.ParentID,
                            children = list.Where<Bas_ComoditiesType>(p => p.ParentID == t2.TypeID).Select(t3 => new {
                                TypeID = t3.TypeID,
                                label = t3.TypeName,
                                TypeName = t3.TypeName,
                                ParentID = t3.ParentID,
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }
            };

            return Json(true, "", new { rows = list, tree = treeList });
        }
    }
}
