using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.SearchParam
{
    public class Month
    {
        public int ID { get; set; }
        public int WinCount { get; set; }
        public int DaiDingCount { get; set; }
        public int LostCount { get; set; }
        public int SumCount { get; set; }

        public decimal? WinDML { get; set; }
        public decimal? lostDML { get; set; }
        public decimal? DdDml { get; set; }
        public decimal? SumDMLCount { get; set; }

    }
    public class Items {
        public string Text { get; set; }
        public string PName { get; set; }
        public int Count { get; set; }
        public decimal? Wmoney{get;set;}
        public decimal? Lmoney { get; set; }
        public decimal? Dmoney { get; set; }
        public decimal? WPercent{ get; set; }
        public decimal? LPercent { get; set; }
        public decimal? DPercent { get; set; }

    }
}
