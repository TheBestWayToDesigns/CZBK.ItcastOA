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
    
    public partial class Seb_Number
    {
        public Seb_Number()
        {
            this.T_jxzztjb = new HashSet<T_jxzztjb>();
            this.T_jxzztjb1 = new HashSet<T_jxzztjb>();
        }
    
        public long ID { get; set; }
        public string Ttext { get; set; }
        public double Del { get; set; }
        public Nullable<int> Items { get; set; }
        public string Bak { get; set; }
    
        public virtual ICollection<T_jxzztjb> T_jxzztjb { get; set; }
        public virtual ICollection<T_jxzztjb> T_jxzztjb1 { get; set; }
    }
}
