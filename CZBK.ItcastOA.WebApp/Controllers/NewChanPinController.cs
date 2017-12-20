using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZBK.ItcastOA.Model;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class NewChanPinController : BaseController
    {
        //
        // GET: /NewChanPin/
        IBLL.IT_ChanPinNameService T_ChanPinNameService { get; set; }
        IBLL.IYXB_WinCanPinService YXB_WinCanPinService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        //获取产品列表
        public ActionResult GetHref() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            string MyColums = Request["MyColums"];
            int totalCount;

            var actioninfolist = T_ChanPinNameService.LoadPageEntities<long>(pageIndex, pageSize, out totalCount, a => a.MyColums== MyColums && a.Del!=1, a => a.ID, false);
            var temp = from a in actioninfolist
                       select new
                       {
                           ID = a.ID,
                           MyTexts = a.MyTexts
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        //创建产品型号与名称
        public ActionResult ADDxinghaoName( )
        {
            string cpname = Request["Name"];
            var Seltcp = T_ChanPinNameService.LoadEntities(x => x.MyTexts == cpname).FirstOrDefault();
            if (Seltcp == null)
            {
                T_ChanPinName Tcp = new T_ChanPinName();
                Tcp.MyColums = Request["MyColums"];
                Tcp.MyTexts = Request["Name"];
                Tcp.CreatePerson = LoginUser.ID;
                Tcp.CreateTime = MvcApplication.GetT_time();
                Tcp.Del = 0;
                T_ChanPinNameService.AddEntity(Tcp);
                return Json("ok", JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                return Json("Isdistict", JsonRequestBehavior.AllowGet);
               
            }
        }
        //删除产品 信息
        public ActionResult DelChanPin()
        {
            long delID =Convert.ToInt64( Request["delID"]);
            var deldata = T_ChanPinNameService.LoadEntities(x=>x.ID== delID).FirstOrDefault();
            if (deldata.CreatePerson != LoginUser.ID)
            {
                return Json("noperson", JsonRequestBehavior.AllowGet);
            }
            else
            {
                deldata.Del = 1;
                if (T_ChanPinNameService.EditEntity(deldata))
                { return Json("ok", JsonRequestBehavior.AllowGet); }
                else
                { return Json("no", JsonRequestBehavior.AllowGet); }
            }            
        }
        //获取产品型号列表
        public ActionResult GetWinCpXh() {

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;

            var actioninfolist = YXB_WinCanPinService.LoadPageEntities<long>(pageIndex, pageSize, out totalCount, a =>  a.Del ==null, a => a.ID, false);
            var temp = from a in actioninfolist
                       select new
                       {
                           ID=a.ID,
                           CPname = a.T_ChanPinName.MyTexts,
                           Cpxinghao = a.T_ChanPinName1.MyTexts
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        //新增产品信息组合 
        public ActionResult addwinchanpin() {
            var cpnameid = Request["cpname"] == null ? 0 : int.Parse(Request["cpname"]);
            var cpxhid = Request["cpxh"] == null ? 0 : int.Parse(Request["cpxh"]);
            YXB_WinCanPin ywcp = new YXB_WinCanPin();
            ywcp.TCanpinID = cpnameid;
            ywcp.TXingHao = cpxhid;
            var IsNotNUll= YXB_WinCanPinService.LoadEntities(x => x.TCanpinID == cpnameid && x.TXingHao == cpxhid).FirstOrDefault();
            if (IsNotNUll != null)
            {
                return Json("数据库中已存在要添加的信息，请核对信息！", JsonRequestBehavior.AllowGet);
            }
            YXB_WinCanPinService.AddEntity(ywcp);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}
