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
    
    public partial class DI_SelectInventoryByRRA
    {
        public byte[] ts { get; set; }
        public string name { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public Nullable<int> ReNewGoodSellDays { get; set; }
        public Nullable<decimal> AdviceReplenishQuantity { get; set; }
        public Nullable<decimal> ActualReplenishQuantity { get; set; }
        public Nullable<decimal> SaleQuantity { get; set; }
        public Nullable<decimal> ExistingQuantity { get; set; }
        public Nullable<decimal> AvailableQuantity { get; set; }
        public string freeItem0 { get; set; }
        public string freeItem1 { get; set; }
        public string freeItem2 { get; set; }
        public string freeItem3 { get; set; }
        public string freeItem4 { get; set; }
        public string freeItem5 { get; set; }
        public string freeItem6 { get; set; }
        public string freeItem7 { get; set; }
        public string freeItem8 { get; set; }
        public string freeItem9 { get; set; }
        public int id { get; set; }
        public Nullable<int> idInventory { get; set; }
        public Nullable<int> idWarehouse { get; set; }
        public Nullable<int> ProductInfo { get; set; }
        public Nullable<int> StoreReplenishmentRule { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
        public Nullable<System.DateTime> BeginDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
