//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace webDmsApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sys_ControlDetail
    {
        public int ID { get; set; }
        public Nullable<int> ControlID { get; set; }
        public string type { get; set; }
        public string prop { get; set; }
        public string label { get; set; }
        public string width { get; set; }
        public string align { get; set; }
        public Nullable<int> isTemplate { get; set; }
        public string templateIcon { get; set; }
        public string templateLabel { get; set; }
        public Nullable<int> IsValid { get; set; }
    }
}
