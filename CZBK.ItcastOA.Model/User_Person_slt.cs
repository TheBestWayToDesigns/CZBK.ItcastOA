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
    using System;
    using System.Collections.Generic;
    
    public partial class User_Person_slt
    {
        public User_Person_slt()
        {
            this.T_jxzztjb = new HashSet<T_jxzztjb>();
            this.T_jgzztjb = new HashSet<T_jgzztjb>();
        }
    
        public long ID { get; set; }
        public int UserID { get; set; }
        public Nullable<decimal> Wage_slt { get; set; }
        public string Job_Name { get; set; }
        public Nullable<decimal> HoursWage { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
    
        public virtual ICollection<T_jxzztjb> T_jxzztjb { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<T_jgzztjb> T_jgzztjb { get; set; }
    }
}
