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
    
    public partial class eap_reportsolution
    {
        public string name { get; set; }
        public Nullable<byte> isPublic { get; set; }
        public Nullable<byte> isDefault { get; set; }
        public Nullable<byte> isSum { get; set; }
        public Nullable<byte> isUseUrl { get; set; }
        public string userdefined { get; set; }
        public Nullable<byte> isGroupSum { get; set; }
        public int id { get; set; }
        public string SearchName { get; set; }
        public Nullable<byte> IsShowThousandsSeparator { get; set; }
        public Nullable<byte> IsShowZeroValue { get; set; }
        public Nullable<byte> IsRepeatTableHeader { get; set; }
        public Nullable<byte> IsRepeatTableFooter { get; set; }
        public Nullable<byte> IsRepeatPageHeader { get; set; }
        public Nullable<byte> IsRepeatPageFooter { get; set; }
        public string ReportTitle { get; set; }
        public string OriginalReportTitle { get; set; }
        public Nullable<int> ChartPercent { get; set; }
        public string ExpressionName { get; set; }
        public Nullable<int> TFlag { get; set; }
        public byte[] ts { get; set; }
        public string searchPlanName { get; set; }
        public Nullable<int> ideap_reportpageset { get; set; }
        public Nullable<int> idSearchPlan { get; set; }
        public Nullable<int> idFromSolution { get; set; }
        public Nullable<int> ideap_reporttemplate { get; set; }
        public Nullable<int> ideap_Search { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<byte> IsEachPageShowTotal { get; set; }
        public Nullable<int> PageSize { get; set; }
    }
}