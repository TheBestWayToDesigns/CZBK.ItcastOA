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
    
    public partial class AM_SplitVoucher
    {
        public string code { get; set; }
        public string docid { get; set; }
        public string memo { get; set; }
        public string maker { get; set; }
        public Nullable<byte> iscarriedforwardout { get; set; }
        public Nullable<byte> iscarriedforwardin { get; set; }
        public Nullable<byte> ismodifiedcode { get; set; }
        public Nullable<byte> issplitbyqty { get; set; }
        public byte[] ts { get; set; }
        public int PrintCount { get; set; }
        public Nullable<int> idhandlereason { get; set; }
        public Nullable<int> idasset { get; set; }
        public int id { get; set; }
        public Nullable<int> voucherstate { get; set; }
        public Nullable<int> makerid { get; set; }
        public Nullable<int> idsplitperiod { get; set; }
        public Nullable<System.DateTime> voucherdate { get; set; }
        public Nullable<System.DateTime> madedate { get; set; }
        public Nullable<System.DateTime> createdtime { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string ExternalCode { get; set; }
    }
}