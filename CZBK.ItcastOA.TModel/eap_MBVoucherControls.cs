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
    
    public partial class eap_MBVoucherControls
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ControlType { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public Nullable<int> CtlIndex { get; set; }
        public string Format { get; set; }
        public string DefaultValue { get; set; }
        public Nullable<short> MaxLength { get; set; }
        public Nullable<short> Precision { get; set; }
        public string EnumName { get; set; }
        public string RefDTOName { get; set; }
        public string RefShowField { get; set; }
        public string RefDataSource { get; set; }
        public Nullable<int> ShowSysField { get; set; }
        public Nullable<int> IsSelected { get; set; }
        public Nullable<decimal> RowWidth { get; set; }
        public Nullable<int> IsNegativeFormat { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string RelationFields { get; set; }
        public string RefWhereName { get; set; }
        public int MBVoucherID { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public byte[] Ts { get; set; }
    }
}
