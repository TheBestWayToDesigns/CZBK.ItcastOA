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
    
    public partial class Pu_PurchaseInvoice_Trace
    {
        public string code { get; set; }
        public string sourceVoucherCode { get; set; }
        public byte[] ts { get; set; }
        public int id { get; set; }
        public Nullable<int> sourceVoucherID { get; set; }
        public Nullable<int> sourceVoucherDetailID { get; set; }
        public Nullable<int> voucherID { get; set; }
        public Nullable<int> idPurchaseInvoiceDetailDTO { get; set; }
        public Nullable<int> idsourcevouchertype { get; set; }
    }
}
