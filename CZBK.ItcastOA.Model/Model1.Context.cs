﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CZBK.ItcastOA.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OAEntities : DbContext
    {
        public OAEntities()
            : base("name=OAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ActionInfo> ActionInfo { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<R_UserInfo_ActionInfo> R_UserInfo_ActionInfo { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<BumenInfoSet> BumenInfoSet { get; set; }
        public DbSet<YXB_Kh_list> YXB_Kh_list { get; set; }
        public DbSet<Login_list> Login_list { get; set; }
        public DbSet<SysField> SysFields { get; set; }
        public DbSet<T_YSItems> T_YSItems { get; set; }
        public DbSet<T_CanPan> T_CanPan { get; set; }
        public DbSet<T_SczzItem> T_SczzItem { get; set; }
        public DbSet<T_ShengChanZhiZhaoTopName> T_ShengChanZhiZhaoTopName { get; set; }
        public DbSet<T_SczzDanju> T_SczzDanju { get; set; }
        public DbSet<T_WinBak> T_WinBak { get; set; }
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<T_ChanPinName> T_ChanPinName { get; set; }
        public DbSet<YXB_Baojia> YXB_Baojia { get; set; }
        public DbSet<T_BaoJiaToP> T_BaoJiaToP { get; set; }
        public DbSet<T_BoolItem> T_BoolItem { get; set; }
        public DbSet<T_WinBakFaHuo> T_WinBakFaHuo { get; set; }
        public DbSet<FileItem> FileItems { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleType> ScheduleTypes { get; set; }
        public DbSet<ScheduleUser> ScheduleUsers { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<T_BaoXiaoBill> T_BaoXiaoBill { get; set; }
        public DbSet<T_BaoxiaoItems> T_BaoxiaoItems { get; set; }
        public DbSet<T_JieKuanBill> T_JieKuanBill { get; set; }
        public DbSet<CarNumber> CarNumbers { get; set; }
        public DbSet<GongLuBaoXiaoBill> GongLuBaoXiaoBills { get; set; }
    }
}
