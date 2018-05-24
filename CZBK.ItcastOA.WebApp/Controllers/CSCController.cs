using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class CSCController : Controller
    {
        //
        // GET: /CSC/
        IBLL.IT_CSC_CardService T_CSC_CardService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCSCInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int toalcount = 0;
            var temp = T_CSC_CardService.LoadPageEntities(pageIndex, pageSize, out toalcount, x=>x.ID>0, x => x.CdTime, false).DefaultIfEmpty().ToList();
            if(temp!=null && temp[0] != null)
            {
                int TruckWeight = Convert.ToInt32(Request["TruckWeight"]);
                foreach (var a in temp)
                {
                    a.sunint = a.sunint - TruckWeight;
                }
            }
            return Json(new { rows=temp, total = toalcount}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SumInfo()
        {
            var temp1 = T_CSC_CardService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            if (temp1 != null && temp1[0] != null)
            {
                int TruckWeight = Convert.ToInt32(Request["TruckWeight"]);
                foreach (var a in temp1)
                {
                    a.sunint = a.sunint - TruckWeight;
                }
            }
            DateTime dtStart = Convert.ToDateTime(Request["excelYM"]);
            DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1 * (dtStart.Day));
            var temp = temp1.Where(x => x.CdTime >= dtStart && x.CdTime <= dtEnd).DefaultIfEmpty().ToList();
            var aTruckWeight = temp.GroupBy(x => x.CardName).Select(x => new sumInfo { Textname = x.First().CardName, WeightSum = x.Sum(g => g.sunint) }).ToList();
            sumInfo allTruckWeight = new sumInfo();
            allTruckWeight.Textname = "全部车辆总重量";
            allTruckWeight.WeightSum = temp.Sum(x=>x.sunint);
            aTruckWeight.Add(allTruckWeight);
            return Json(aTruckWeight, JsonRequestBehavior.AllowGet);
        }
    }
    public class sumInfo
    {
        public string Textname { get; set; }
        public decimal? WeightSum { get; set; }
    }
}
