﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class webDmsEntities : DbContext
    {
        public webDmsEntities()
            : base("name=webDmsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Sys_Application> Sys_Application { get; set; }
        public virtual DbSet<Sys_Button> Sys_Button { get; set; }
        public virtual DbSet<Sys_Role> Sys_Role { get; set; }
        public virtual DbSet<Sys_RoleMenu> Sys_RoleMenu { get; set; }
        public virtual DbSet<Sys_User> Sys_User { get; set; }
        public virtual DbSet<Sys_UserRole> Sys_UserRole { get; set; }
        public virtual DbSet<View_menu> View_menu { get; set; }
        public virtual DbSet<Sys_dictionarydata> Sys_dictionarydata { get; set; }
        public virtual DbSet<Sys_ControlDetail> Sys_ControlDetail { get; set; }
        public virtual DbSet<Sys_Control> Sys_Control { get; set; }
        public virtual DbSet<Sys_Menu> Sys_Menu { get; set; }
        public virtual DbSet<Global_Condition> Global_Condition { get; set; }
    }
}
