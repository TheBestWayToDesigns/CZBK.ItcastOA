using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class SCCJController : Controller
    {
        //
        // GET: /SCCJ/

            IBLL.IT_SCCJService T_SCCJService { get; set; }
            IBLL.IT_ChanPinNameService T_ChanPinNameService { get; set; }
            IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
            IBLL.IYXB_WinCanPinService YXB_WinCanPinService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        //获取单据数据
        public ActionResult GetSccjInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int toalcount = 0;
            var temp = T_SCCJService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.Del_f == 0, x => x.Wtime, false).DefaultIfEmpty().ToList();
            var rtmp = from a in temp
                       select new
                       {
                           ID=a.ID,
                           Wtime = a.Wtime,
                           BuMenid = a.BumenInfoSet.Name,
                           ProductNameId = a.T_ChanPinName2.MyTexts,
                           ProductGGId = a.T_ChanPinName.MyTexts,
                           ProductJB = a.T_ChanPinName1.MyTexts,
                           JiaCiPinNum = a.JiaCiPinNum,
                           JiaHeGePinNum = a.JiaHeGePinNum,
                           JiaYiDengPinNum = a.JiaYiDengPinNum,
                           JiaYouDengPinNum = a.JiaYouDengPinNum,
                           JiaFeiPinNum = a.JiaFeiPinNum,
                           YiCiPinNum = a.YiCiPinNum,
                           YiHeGePinNum = a.YiHeGePinNum,
                           YiYiDengPinNum = a.YiYiDengPinNum,
                           YiYouDengPinNum = a.YiYouDengPinNum,
                           YiFeiPinNum=a.YiFeiPinNum
                       };
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        //添加单据数据
        public ActionResult AddorEditSccjInfo(T_SCCJ tsccj)
        {
            tsccj.ProductGGId = tsccj.ProductGGId == 0 ? 533 : tsccj.ProductGGId;
            if (tsccj.ID > 0)//修改
            {
                if (T_SCCJService.EditEntity(tsccj))
                {
                    return Json(new { ret = "ok", msg = "操作成功" }, JsonRequestBehavior.AllowGet);
                }else
                {
                    return Json(new { ret = "no", msg = "修改失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            else//新增
            {
                tsccj.Del_f = 0;
                T_SCCJService.AddEntity(tsccj);
                return Json(new { ret = "ok", msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取要修改的数据
        public ActionResult GetEditInfo()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = T_SCCJService.LoadEntities(x => x.ID==id).FirstOrDefault();
            editInfo ei = new editInfo();
            ei.ID = temp.ID;
            ei.Wtime = temp.Wtime;
            ei.BuMenid = temp.BuMenid;
            ei.ProductNameId = temp.ProductNameId;
            ei.ProductGGId = temp.ProductGGId;
            ei.ProductJB = temp.ProductJB;
            ei.JiaCiPinNum = temp.JiaCiPinNum;
            ei.JiaHeGePinNum = temp.JiaHeGePinNum;
            ei.JiaYiDengPinNum = temp.JiaYiDengPinNum;
            ei.JiaYouDengPinNum = temp.JiaYouDengPinNum;
            ei.JiaFeiPinNum = temp.JiaFeiPinNum;
            ei.YiCiPinNum = temp.YiCiPinNum;
            ei.YiHeGePinNum = temp.YiHeGePinNum;
            ei.YiYiDengPinNum = temp.YiYiDengPinNum;
            ei.YiYouDengPinNum = temp.YiYouDengPinNum;
            ei.YiFeiPinNum = temp.YiFeiPinNum;
            return Json(ei, JsonRequestBehavior.AllowGet);
        }
        //删除单据数据
        public ActionResult DelSccjInfo()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = T_SCCJService.LoadEntities(x=>x.ID==id).FirstOrDefault();
            if (temp != null)
            {
                temp.Del_f = 1;
                T_SCCJService.EditEntity(temp);
                return Json(new { ret="ok",msg="操作成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "no", msg = "数据库中无此条数据" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取部门列表
        public ActionResult GetBuMenList()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.Renark == "1" && x.DelFlag == 0).DefaultIfEmpty();
            var rtmp= from a in temp
                   select new
                   {
                       ID=a.ID,
                       Name = a.Name
                   };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取产品名称
        public ActionResult GetCPNameList()
        {
            var temp = T_ChanPinNameService.LoadEntities( a => a.MyColums == "CPname" && a.Del != 1).DefaultIfEmpty();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           MyTexts = a.MyTexts
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取产品型号
        public ActionResult GetBCPXHList()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = T_ChanPinNameService.LoadEntities(x => x.ID == id).FirstOrDefault();
            var tem = from a in temp.YXB_WinCanPin.DefaultIfEmpty()
                      select new
                      {
                          ID = a.T_ChanPinName1.ID,
                          MyTexts= a.T_ChanPinName1.MyTexts
                      };

            return Json(tem, JsonRequestBehavior.AllowGet);
        }
        //获取产品级别
        public ActionResult GetCPJBList()
        {
            var temp = T_ChanPinNameService.LoadEntities(x => x.MyColums == "CPDengJi").DefaultIfEmpty();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           MyTexts = a.MyTexts
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //按日统计产品完成
        public ActionResult TongJiByMonth()
        {
            int bmID = Convert.ToInt32(Request["bmID"]);
            DateTime dt = Convert.ToDateTime(Request["monthExcel"]);
            DateTime dtStr = new DateTime(dt.Year, dt.Month, 1);
            var temp = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime == dt && x.Del_f == 0).DefaultIfEmpty().ToList();
            var temp2 = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime >= dtStr && x.Wtime <= dt && x.Del_f == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                var groupData = temp.GroupBy(x => x.ProductNameId).ToList();
                List<MonthAnfYearTJclass> lmaf = new List<MonthAnfYearTJclass>();
                int num = 0;
                long id = 0;
                foreach (var a in groupData)
                {
                    var s = a.DefaultIfEmpty();
                    foreach (var b in s)
                    {
                        if (b.ProductNameId != id)
                        {
                            id = b.ProductNameId;
                            num = num == 0 ? 1 : 0;
                        }
                        MonthAnfYearTJclass may = new MonthAnfYearTJclass();
                        may.BuMenid = b.BumenInfoSet.Name;
                        may.ProductGGId = b.T_ChanPinName2.MyTexts + "——" + b.T_ChanPinName.MyTexts + "——" +num;
                        may.ProductJB = b.T_ChanPinName1.MyTexts;
                        may.TrueCLNum = b.JiaCiPinNum + b.JiaFeiPinNum + b.JiaHeGePinNum + b.JiaYiDengPinNum + b.JiaYouDengPinNum + b.YiCiPinNum + b.YiFeiPinNum + b.YiHeGePinNum + b.YiYiDengPinNum + b.YiYouDengPinNum;
                        var data = temp2.Where(x => x.ProductNameId == b.ProductNameId && x.ProductGGId == b.ProductGGId && x.ProductJB == b.ProductJB).ToList();
                        may.SumTrueCL= data.Sum(x => x.JiaCiPinNum) + data.Sum(x => x.JiaFeiPinNum) + data.Sum(x => x.JiaHeGePinNum) + data.Sum(x => x.JiaYiDengPinNum) + data.Sum(x => x.JiaYouDengPinNum) + data.Sum(x => x.YiCiPinNum) + data.Sum(x => x.YiFeiPinNum) + data.Sum(x => x.YiHeGePinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiYouDengPinNum);
                        may.ZhengPinDayNum = b.JiaYouDengPinNum + b.JiaYiDengPinNum + b.JiaHeGePinNum + b.YiYiDengPinNum + b.YiYouDengPinNum + b.YiHeGePinNum;
                        may.ZhengPinMonthNum = data.Sum(x => x.JiaYouDengPinNum) + data.Sum(x => x.JiaYiDengPinNum) + data.Sum(x => x.JiaHeGePinNum) + data.Sum(x => x.YiYouDengPinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiHeGePinNum);
                        may.CiPinDayNum = b.JiaCiPinNum + b.YiCiPinNum;
                        may.CiPinMonthNum = data.Sum(x => x.JiaCiPinNum) + data.Sum(x => x.YiCiPinNum);
                        may.JiaZhengPinNum = b.JiaHeGePinNum+b.JiaYiDengPinNum+b.JiaYouDengPinNum;
                        may.JiaCiPinNum = b.JiaCiPinNum;
                        may.YiZhengPinNum = b.YiHeGePinNum + b.YiYiDengPinNum + b.YiYouDengPinNum;
                        may.YiCiPinNum = b.YiCiPinNum;
                        if (may.SumTrueCL==0&&may.TrueCLNum == 0 && may.ZhengPinDayNum == 0 && may.ZhengPinMonthNum == 0 && may.CiPinDayNum == 0 && may.CiPinMonthNum == 0 && may.JiaZhengPinNum == 0 && may.JiaCiPinNum == 0 && may.YiZhengPinNum == 0 && may.YiCiPinNum == 0)
                        {
                            continue;
                        }else
                        {
                            lmaf.Add(may);
                        }
                    }
                }
                return Json(lmaf, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //按年统计
        public ActionResult TongJiByYear()
        {
            int bmID = Convert.ToInt32(Request["bmID"]);
            DateTime dtStr = Convert.ToDateTime(Request["yearExcel"]);
            DateTime dtEnd = dtStr.AddYears(1).AddDays(-1);
            var temp2 = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime >= dtStr && x.Wtime <= dtEnd && x.Del_f == 0).DefaultIfEmpty().ToList();
            DateTime dt = temp2.Max(x => x.Wtime);
            var temp = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime == dt && x.Del_f == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                var groupData = temp.GroupBy(x => x.ProductNameId).ToList();
                List<MonthAnfYearTJclass> lmaf = new List<MonthAnfYearTJclass>();
                int num = 0;
                long id = 0;
                foreach (var a in groupData)
                {
                    var s = a.DefaultIfEmpty();
                    foreach (var b in s)
                    {
                        if (b.ProductNameId != id)
                        {
                            id = b.ProductNameId;
                            num = num == 0 ? 1 : 0;
                        }
                        MonthAnfYearTJclass may = new MonthAnfYearTJclass();
                        may.BuMenid = b.BumenInfoSet.Name;
                        may.ProductGGId = b.T_ChanPinName2.MyTexts + "——" + b.T_ChanPinName.MyTexts + "——" + num;
                        may.ProductJB = b.T_ChanPinName1.MyTexts;
                        var data = temp2.Where(x => x.ProductNameId == b.ProductNameId && x.ProductGGId == b.ProductGGId && x.ProductJB == b.ProductJB).ToList();
                        may.SumTrueCL = data.Sum(x => x.JiaCiPinNum) + data.Sum(x => x.JiaFeiPinNum) + data.Sum(x => x.JiaHeGePinNum) + data.Sum(x => x.JiaYiDengPinNum) + data.Sum(x => x.JiaYouDengPinNum) + data.Sum(x => x.YiCiPinNum) + data.Sum(x => x.YiFeiPinNum) + data.Sum(x => x.YiHeGePinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiYouDengPinNum);
                        may.ZhengPinMonthNum = data.Sum(x => x.JiaYouDengPinNum) + data.Sum(x => x.JiaYiDengPinNum) + data.Sum(x => x.JiaHeGePinNum) + data.Sum(x => x.YiYouDengPinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiHeGePinNum);
                        may.CiPinMonthNum = data.Sum(x => x.JiaCiPinNum) + data.Sum(x => x.YiCiPinNum);
                        may.JiaZhengPinNum = data.Sum(x => x.JiaHeGePinNum) + data.Sum(x => x.JiaYiDengPinNum) + data.Sum(x => x.JiaYouDengPinNum);
                        may.JiaCiPinNum = data.Sum(x => x.JiaCiPinNum);
                        may.YiZhengPinNum = data.Sum(x => x.YiHeGePinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiYouDengPinNum);
                        may.YiCiPinNum = data.Sum(x => x.YiCiPinNum);
                        if (may.SumTrueCL == 0 && may.ZhengPinMonthNum == 0 && may.CiPinMonthNum == 0 && may.JiaZhengPinNum == 0 && may.JiaCiPinNum == 0 && may.YiZhengPinNum == 0 && may.YiCiPinNum == 0)
                        {
                            continue;
                        }
                        else
                        {
                            lmaf.Add(may);
                        }
                    }
                }
                return Json(lmaf, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TongJiByDay()
        {
            int bmID = Convert.ToInt32(Request["bmID"]);
            DateTime dt= Convert.ToDateTime(Request["dayExcel"]);
            DateTime dtStr = new DateTime(dt.Year, dt.Month, 1);
            var temp = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime == dt && x.Del_f == 0).DefaultIfEmpty().ToList();
            var temp2 = T_SCCJService.LoadEntities(x => x.BuMenid == bmID &&x.Wtime>=dtStr&& x.Wtime<=dt && x.Del_f == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                var groupData = temp.GroupBy(x => x.ProductNameId).ToList();
                List<DayTJclass> ldt = new List<DayTJclass>();
                int num = 0;
                long id = 0;
                foreach (var a in groupData)
                {
                    var s = a.DefaultIfEmpty();
                    foreach (var b in s)
                    {
                        if (b.ProductNameId != id)
                        {
                            id = b.ProductNameId;
                            num = num == 0 ? 1 : 0;
                        }
                        DayTJclass dtj = new DayTJclass();
                        dtj.ProductGGId = b.T_ChanPinName2.MyTexts + "——" + b.T_ChanPinName.MyTexts + "——" + num;
                        dtj.ProductJB = b.T_ChanPinName1.MyTexts;
                        dtj.YSDayNum = b.JiaCiPinNum + b.JiaFeiPinNum + b.JiaHeGePinNum + b.JiaYiDengPinNum + b.JiaYouDengPinNum + b.YiCiPinNum + b.YiFeiPinNum + b.YiHeGePinNum + b.YiYiDengPinNum + b.YiYouDengPinNum;
                        var data = temp2.Where(x => x.ProductNameId == b.ProductNameId && x.ProductGGId == b.ProductGGId && x.ProductJB == b.ProductJB).ToList();
                        dtj.YSMonthNum = data.Sum(x=>x.JiaCiPinNum)+data.Sum(x=>x.JiaFeiPinNum)+data.Sum(x=>x.JiaHeGePinNum)+data.Sum(x=>x.JiaYiDengPinNum)+data.Sum(x=>x.JiaYouDengPinNum)+ data.Sum(x => x.YiCiPinNum) + data.Sum(x => x.YiFeiPinNum) + data.Sum(x => x.YiHeGePinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiYouDengPinNum);
                        dtj.JiaBanDayNum = b.JiaCiPinNum + b.JiaFeiPinNum + b.JiaHeGePinNum + b.JiaYiDengPinNum + b.JiaYouDengPinNum;
                        dtj.JiaBanMonthNum = data.Sum(x => x.JiaCiPinNum) + data.Sum(x => x.JiaFeiPinNum) + data.Sum(x => x.JiaHeGePinNum) + data.Sum(x => x.JiaYiDengPinNum) + data.Sum(x => x.JiaYouDengPinNum);
                        dtj.YiBanDayNum = b.YiCiPinNum + b.YiFeiPinNum + b.YiHeGePinNum + b.YiYiDengPinNum + b.YiYouDengPinNum;
                        dtj.YiBanMonthNum = data.Sum(x => x.YiCiPinNum) + data.Sum(x => x.YiFeiPinNum) + data.Sum(x => x.YiHeGePinNum) + data.Sum(x => x.YiYiDengPinNum) + data.Sum(x => x.YiYouDengPinNum);
                        dtj.JiaYouDayNum = b.JiaYouDengPinNum;
                        dtj.JiaYouMonthNum = data.Sum(x => x.JiaYouDengPinNum);
                        dtj.JiaYiDayNum = b.JiaYiDengPinNum;
                        dtj.JiaYiMonthNum = data.Sum(x => x.JiaYiDengPinNum);
                        dtj.JiaHeDayNum = b.JiaHeGePinNum;
                        dtj.JiaHeMonthNum = data.Sum(x => x.JiaHeGePinNum);
                        dtj.JiaCiDayNum = b.JiaCiPinNum;
                        dtj.JiaCiMonthNum = data.Sum(x => x.JiaCiPinNum);
                        dtj.JiaFeiDayNum = b.JiaFeiPinNum;
                        dtj.JiaFeiMonthNum = data.Sum(x => x.JiaFeiPinNum);
                        dtj.YiYouDayNum = b.YiYouDengPinNum;
                        dtj.YiYouMonthNum = data.Sum(x => x.YiYouDengPinNum);
                        dtj.YiYiDayNum = b.YiYiDengPinNum;
                        dtj.YiYiMonthNum = data.Sum(x => x.YiYiDengPinNum);
                        dtj.YiHeDayNum = b.YiHeGePinNum;
                        dtj.YiHeMonthNum = data.Sum(x => x.YiHeGePinNum);
                        dtj.YiCiDayNum = b.YiCiPinNum;
                        dtj.YiCiMonthNum = data.Sum(x => x.YiCiPinNum);
                        dtj.YiFeiDayNum = b.YiFeiPinNum;
                        dtj.YiFeiMonthNum = data.Sum(x => x.YiFeiPinNum);
                        ldt.Add(dtj);
                    }
                }
                return Json(ldt, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //导出表
        [ValidateInput(false)]
        public void GetExcelTable()
        {
            string excelHtml = Request["OFtable"];
            string name = DateTime.Now.ToString();
            Response.Buffer = true;
            //输出的应用类型 
            Response.ContentType = "application/vnd.ms-excel";
            //设定编码方式，若输出的excel有乱码，可优先从编码方面解决
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //filenames是自定义的文件名
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + name + ".xls");
            //content是步骤1的html，注意是string类型
            Response.Write(excelHtml);
            Response.End();
        }
    }
    public class editInfo
    {
        public long ID { get; set; }
        public System.DateTime Wtime { get; set; }
        public int BuMenid { get; set; }
        public long ProductNameId { get; set; }
        public long ProductGGId { get; set; }
        public long ProductJB { get; set; }
        public string Class { get; set; }
        public string Groups { get; set; }
        public int CiPinNum { get; set; }
        public int HeGePinNum { get; set; }
        public int YiDengPinNum { get; set; }
        public int? YouDengPinNum { get; set; }
        public int? FeiPinNum { get; set; }
        public int? JiaCiPinNum { get; set; }
        public int? JiaHeGePinNum { get; set; }
        public int? JiaYiDengPinNum { get; set; }
        public int? JiaYouDengPinNum  { get; set; }
        public int? JiaFeiPinNum { get; set; }
        public int? YiCiPinNum  { get; set; }
        public int? YiHeGePinNum { get; set; }
        public int? YiYiDengPinNum { get; set; }
        public int? YiYouDengPinNum { get; set; }
        public int? YiFeiPinNum { get; set; }
        public short Del_f { get; set; }
    }
    public class DayTJclass
    {
       public string ProductGGId { get; set; }
       public string ProductJB { get; set; }
       public int? YSDayNum { get; set; }
       public int? YSMonthNum { get; set; }
       public int? JiaBanDayNum { get; set; }
       public int? JiaBanMonthNum { get; set; }
       public int? YiBanDayNum { get; set; }
       public int? YiBanMonthNum { get; set; }
       public int? JiaYouDayNum  { get; set; }
       public int? JiaYouMonthNum  { get; set; }
       public int? JiaYiDayNum  { get; set; }
       public int? JiaYiMonthNum  { get; set; }
       public int? JiaHeDayNum  { get; set; }
       public int? JiaHeMonthNum  { get; set; }
       public int? JiaCiDayNum  { get; set; }
       public int? JiaCiMonthNum  { get; set; }
       public int? JiaFeiDayNum  { get; set; }
       public int? JiaFeiMonthNum  { get; set; }
       public int? YiYouDayNum  { get; set; }
       public int? YiYouMonthNum  { get; set; }
       public int? YiYiDayNum  { get; set; }
       public int? YiYiMonthNum  { get; set; }
       public int? YiHeDayNum  { get; set; }
       public int? YiHeMonthNum  { get; set; }
       public int? YiCiDayNum  { get; set; }
       public int? YiCiMonthNum  { get; set; }
       public int? YiFeiDayNum  { get; set; }
       public int? YiFeiMonthNum { get; set; }
    }
    public class MonthAnfYearTJclass
    {
        public string BuMenid{ get; set; }
        public string ProductGGId{ get; set; }
        public string ProductJB{ get; set; }
        public int? SumTrueCL { get; set; }
        public int? MaxCLNum { get; set; }
        public int? TrueCLNum { get; set; }
        public int? ZhengPinDayNum { get; set; }
        public int? ZhengPinMonthNum { get; set; }
        public int? CiPinDayNum { get; set; }
        public int? CiPinMonthNum { get; set; }
        public int? JiaZhengPinNum { get; set; }
        public int? JiaCiPinNum { get; set; }
        public int? YiZhengPinNum { get; set; }
        public int? YiCiPinNum { get; set; }
    }
}
