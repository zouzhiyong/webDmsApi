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
    
    public partial class Sys_Dept
    {
        public int DeptID { get; set; }
        public Nullable<int> WSID { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsValid { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifyUserID { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public string Comment { get; set; }
    }
}
