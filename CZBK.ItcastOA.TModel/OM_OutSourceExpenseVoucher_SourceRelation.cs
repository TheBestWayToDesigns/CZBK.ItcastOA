//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CZBK.ItcastOA.TModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class OM_OutSourceExpenseVoucher_SourceRelation
    {
        public int id { get; set; }
        public byte[] ts { get; set; }
        public Nullable<int> VoucherId { get; set; }
        public Nullable<int> SourceVoucherId { get; set; }
        public Nullable<int> SourceVoucherDetailId { get; set; }
        public Nullable<int> IdOutSourceExpenseVoucherDetailDTO { get; set; }
        public Nullable<int> IdsourceVoucherType { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitExchangeRate { get; set; }
        public Nullable<decimal> Quantity2 { get; set; }
        public Nullable<decimal> BaseQuantity { get; set; }
        public Nullable<int> idunit { get; set; }
        public Nullable<int> idbaseUnit { get; set; }
        public Nullable<int> idunit2 { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<decimal> SubQuantity { get; set; }
        public Nullable<int> idsubUnit { get; set; }
        public string SourceVoucherTs { get; set; }
        public string SourceVoucherDetailTs { get; set; }
    }
}