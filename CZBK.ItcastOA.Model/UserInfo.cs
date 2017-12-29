//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CZBK.ItcastOA.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class UserInfo
    {
        public UserInfo()
        {
            this.Login_list = new HashSet<Login_list>();
            this.R_UserInfo_ActionInfo = new HashSet<R_UserInfo_ActionInfo>();
            this.T_CanPan = new HashSet<T_CanPan>();
            this.T_CanPan1 = new HashSet<T_CanPan>();
            this.T_SczzDanju = new HashSet<T_SczzDanju>();
            this.T_SczzDanju1 = new HashSet<T_SczzDanju>();
            this.T_SczzDanju2 = new HashSet<T_SczzDanju>();
            this.T_SczzDanju3 = new HashSet<T_SczzDanju>();
            this.T_SczzDanju4 = new HashSet<T_SczzDanju>();
            this.T_SczzItem = new HashSet<T_SczzItem>();
            this.T_ShengChanZhiZhaoTopName = new HashSet<T_ShengChanZhiZhaoTopName>();
            this.YXB_Kh_list = new HashSet<YXB_Kh_list>();
            this.Departments = new HashSet<Department>();
            this.RoleInfoes = new HashSet<RoleInfo>();
            this.T_ChanPinName = new HashSet<T_ChanPinName>();
            this.T_WinBakFaHuo = new HashSet<T_WinBakFaHuo>();
            this.T_WinBakFaHuo1 = new HashSet<T_WinBakFaHuo>();
            this.Schedules = new HashSet<Schedule>();
            this.ScheduleUsers = new HashSet<ScheduleUser>();
            this.Schedules1 = new HashSet<Schedule>();
            this.T_BaoXiaoBill = new HashSet<T_BaoXiaoBill>();
            this.T_JieKuanBill = new HashSet<T_JieKuanBill>();
            this.GongLuBaoXiaoBills = new HashSet<GongLuBaoXiaoBill>();
            this.YXB_BaoJiaEidtMoney = new HashSet<YXB_BaoJiaEidtMoney>();
            this.YXB_Baojia = new HashSet<YXB_Baojia>();
            this.ShareFileOrNotices = new HashSet<ShareFileOrNotice>();
            this.ScheduleUsers1 = new HashSet<ScheduleUser>();
        }
    
        public int ID { get; set; }
        public string UName { get; set; }
        public string UPwd { get; set; }
        public System.DateTime SubTime { get; set; }
        public short DelFlag { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string Remark { get; set; }
        public string Sort { get; set; }
        public Nullable<System.DateTime> OverTime { get; set; }
        public Nullable<int> UserXiaoHao { get; set; }
        public Nullable<int> Click { get; set; }
        public Nullable<bool> ThisMastr { get; set; }
        public Nullable<int> MasterID { get; set; }
        public string Login_now { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<decimal> Umoney { get; set; }
        public string PerSonName { get; set; }
        public Nullable<int> QuXian { get; set; }
        public Nullable<int> BuMenID { get; set; }

        [JsonIgnore]
        public virtual BumenInfoSet BumenInfoSet { get; set; }
        [JsonIgnore]
        public virtual ICollection<Login_list> Login_list { get; set; }
        [JsonIgnore]
        public virtual ICollection<R_UserInfo_ActionInfo> R_UserInfo_ActionInfo { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_CanPan> T_CanPan { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_CanPan> T_CanPan1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SczzDanju> T_SczzDanju { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SczzDanju> T_SczzDanju1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SczzDanju> T_SczzDanju2 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SczzDanju> T_SczzDanju3 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SczzDanju> T_SczzDanju4 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SczzItem> T_SczzItem { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_ShengChanZhiZhaoTopName> T_ShengChanZhiZhaoTopName { get; set; }
        [JsonIgnore]
        public virtual ICollection<YXB_Kh_list> YXB_Kh_list { get; set; }
        [JsonIgnore]
        public virtual ICollection<Department> Departments { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleInfo> RoleInfoes { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_ChanPinName> T_ChanPinName { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_WinBakFaHuo> T_WinBakFaHuo { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_WinBakFaHuo> T_WinBakFaHuo1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; }
        [JsonIgnore]
        public virtual ICollection<ScheduleUser> ScheduleUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_BaoXiaoBill> T_BaoXiaoBill { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_JieKuanBill> T_JieKuanBill { get; set; }
        [JsonIgnore]
        public virtual ICollection<GongLuBaoXiaoBill> GongLuBaoXiaoBills { get; set; }
        [JsonIgnore]
        public virtual ICollection<YXB_BaoJiaEidtMoney> YXB_BaoJiaEidtMoney { get; set; }
        [JsonIgnore]
        public virtual ICollection<YXB_Baojia> YXB_Baojia { get; set; }
        [JsonIgnore]
        public virtual ICollection<ShareFileOrNotice> ShareFileOrNotices { get; set; }
        [JsonIgnore]
        public virtual ICollection<ScheduleUser> ScheduleUsers1 { get; set; }
    }
}
