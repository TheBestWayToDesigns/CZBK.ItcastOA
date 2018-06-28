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
                           Class = a.Class,
                           Groups = a.Groups,
                           CiPinNum = a.CiPinNum,
                           HeGePinNum = a.HeGePinNum,
                           YiDengPinNum = a.YiDengPinNum,
                           YouDengPinNum = a.YouDengPinNum,
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
            ei.Class = temp.Class;
            ei.Groups = temp.Groups;
            ei.CiPinNum = temp.CiPinNum;
            ei.HeGePinNum = temp.HeGePinNum;
            ei.YiDengPinNum = temp.YiDengPinNum;
            ei.YouDengPinNum = temp.YouDengPinNum;
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
        //按月统计
        public ActionResult TongJiByMonth()
        {
            int bmID = Convert.ToInt32(Request["bmID"]);
            DateTime dtStart = Convert.ToDateTime(Request["monthExcel"]);
            DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1 * (dtStart.Day));
            var temp = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime >= dtStart && x.Wtime <= dtEnd && x.Del_f == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                temp = temp.OrderBy(x => x.Wtime).ToList();
            }
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Wtime = a.Wtime,
                           BuMenid = a.BumenInfoSet.Name,
                           ProductNameId = a.T_ChanPinName2.MyTexts,
                           ProductGGId = a.T_ChanPinName.MyTexts,
                           ProductJB = a.T_ChanPinName1.MyTexts,
                           Class = a.Class,
                           Groups = a.Groups,
                           CiPinNum = a.CiPinNum,
                           HeGePinNum = a.HeGePinNum,
                           YiDengPinNum = a.YiDengPinNum,
                           YouDengPinNum = a.YouDengPinNum,
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //按年统计
        public ActionResult TongJiByYear()
        {
            int bmID = Convert.ToInt32(Request["bmID"]);
            DateTime dtStart = Convert.ToDateTime(Request["yearExcel"]);
            DateTime dtEnd = dtStart.AddYears(1);
            var temp = T_SCCJService.LoadEntities(x => x.BuMenid == bmID && x.Wtime >= dtStart && x.Wtime <= dtEnd && x.Del_f == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                temp = temp.OrderBy(x => x.Wtime).ToList();
            }
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Wtime = a.Wtime,
                           BuMenid = a.BumenInfoSet.Name,
                           ProductNameId = a.T_ChanPinName2.MyTexts,
                           ProductGGId = a.T_ChanPinName.MyTexts,
                           ProductJB = a.T_ChanPinName1.MyTexts,
                           Class = a.Class,
                           Groups = a.Groups,
                           CiPinNum = a.CiPinNum,
                           HeGePinNum = a.HeGePinNum,
                           YiDengPinNum = a.YiDengPinNum,
                           YouDengPinNum = a.YouDengPinNum,
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
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
        public int YouDengPinNum { get; set; }
        public short Del_f { get; set; }
    }
}
