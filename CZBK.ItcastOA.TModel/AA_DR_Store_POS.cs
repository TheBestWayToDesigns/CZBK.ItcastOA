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
    
    public partial class AA_DR_Store_POS
    {
        public string code { get; set; }
        public string name { get; set; }
        public string memo { get; set; }
        public Nullable<byte> isused { get; set; }
        public string poscode { get; set; }
        public string posname { get; set; }
        public Nullable<int> sequencenumber { get; set; }
        public byte[] ts { get; set; }
        public string updatedBy { get; set; }
        public string cancel { get; set; }
        public Nullable<int> idStoreDTO { get; set; }
        public Nullable<int> encryptionstate { get; set; }
        public int id { get; set; }
        public Nullable<System.DateTime> createdtime { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
    }
}