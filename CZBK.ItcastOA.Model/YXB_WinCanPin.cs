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
    
    public partial class YXB_WinCanPin
    {
        public long ID { get; set; }
        public long TCanpinID { get; set; }
        public long TXingHao { get; set; }
        public Nullable<short> Del { get; set; }
    
        public virtual T_ChanPinName T_ChanPinName { get; set; }
        public virtual T_ChanPinName T_ChanPinName1 { get; set; }
    }
}