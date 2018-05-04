using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class SltJxzzController : BaseController
    {
        //
        // GET: /SltJxzz/
        IBLL.IT_jxzztjbService T_jxzztjbService { get; set; }
        IBLL.ISeb_NumberService Seb_NumberService { get; set; }
        IBLL.IUser_Person_sltService User_Person_sltService { get; set; }
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }

        public ActionResult Index()
        {
            var UserName = User_Person_sltService.LoadEntities(x => x.ID > 0).GroupBy(x => x.UserID).Select(x => x.First());
            //人员名称表
            ViewBag.sltUser = UserName.ToList();
            //零件名称表
            var LingjianName = Seb_NumberService.LoadEntities(x => x.Items == 1).DefaultIfEmpty();
            ViewBag.ljname = LingjianName.ToList();
            //设备编号名称
            var NumberName = Seb_NumberService.LoadEntities(x => x.Items == 0).DefaultIfEmpty();
            ViewBag.ljname = NumberName.ToList();
            //
            return View();
        }
        public ActionResult Getdata() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int win = Request["win"] == null ? 0 : int.Parse(Request["win"]);
            UserInfoParam uim = new UserInfoParam();

            uim.Uptime = Convert.ToDateTime(Request["UpTime"]);
            uim.Dwtime = Convert.ToDateTime(Request["DwTime"]);
            uim.zt = Request["Zt"] == null ? 0 : int.Parse(Request["Zt"]);
            uim.addess = Request["addess"];
            uim.Person = Request["Person"] == null ? 0 : Request["Person"].Length <= 0 ? 0 : int.Parse(Request["Person"]);
            uim.KHname = Request["KHname"] == null ? 0 : Request["KHname"].Length <= 0 ? 0 : int.Parse(Request["KHname"]);
            uim.CPname = Request["CPname"] == null ? 0 : Request["CPname"].Length <= 0 ? 0 : int.Parse(Request["CPname"]);
            uim.CPxh = Request["CPxh"] == null ? 0 : Request["CPxh"].Length <= 0 ? 0 : int.Parse(Request["CPxh"]);
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            uim.BumenID = LoginUser.BuMenID;
            //var temp1 = YXB_BaojiaService.LoadSearchEntities(uim);
            //var temp = from a in temp1
            //           select new SlcClass
            //           {
            //               ID = a.id,
            //               CPname = a.T_ChanPinName1.MyTexts,
            //               CPXingHao = a.T_ChanPinName2.MyTexts,
            //               CPShuLiang = a.CPShuLiang,
            //               AddTime = a.AddTime,
            //               ZhuangTai = a.ZhuangTai,
            //               BaoJiaMoney = a.BaoJiaMoney,
            //               BaoJiaPerson = a.BaoJiaPerson,
            //               BaoJiaTime = a.BaoJiaTime,
            //               WIN = a.WIN,
            //               GhTime = a.T_BaoJiaToP.GhTime,
            //               JiShuYaoQiu = a.T_BaoJiaToP.JiShuYaoQiu,
            //               Addess = a.T_BaoJiaToP.Addess,
            //               DaiBanYunShu = a.T_BaoJiaToP.DaiBanYunShu,
            //               JieShuanFanShi = a.T_BaoJiaToP.JieShuanFanShi,
            //               HeTongQianDing = a.T_BaoJiaToP.HeTongQianDing,
            //               TOPaddtime = a.T_BaoJiaToP.AddTime,
            //               KHname = a.T_BaoJiaToP.YXB_Kh_list.KHname,
            //               KHComname = a.T_BaoJiaToP.KHComname,
            //               KHperson = a.T_BaoJiaToP.YXB_Kh_list.KHperson,
            //               KHfaren = a.T_BaoJiaToP.YXB_Kh_list.KHfaren,
            //               KHzhiwu = a.T_BaoJiaToP.YXB_Kh_list.KHzhiwu,
            //               KHphoto = a.T_BaoJiaToP.YXB_Kh_list.KHphoto,
            //               NewTime = a.T_BaoJiaToP.YXB_Kh_list.NewTime,
            //               UName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.PerSonName,
            //               WinMoney = a.WinMoney,
            //               WinYunFei = a.WinYunFei,
            //               WinStr = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().T_YSItems.MyText,
            //               HanShui = a.T_BaoJiaToP.T_BoolItem.str,
            //               BaoJiaYunFei = a.BaoJiaYunFei

            //           };
            //var templist = temp.ToList();
            //for (int i = 0; i < templist.Count; i++)
            //{
            //    templist[i].Addess = ArrF(templist[i].Addess);
            //}
            return Json(new { rows = "", total = uim.TotalCount }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult sltperson() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            IQueryable<User_Person_slt> ups = User_Person_sltService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.ID > 0, m => m.ID, false).DefaultIfEmpty();
            var temp = from a in ups
                       select new
                       {
                           a.ID,
                           a.Job_Name,
                           a.HoursWage,
                           a.Wage_slt,
                           a.AddTime
                       };
            
            return Json(new { rows = temp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddeditSltPer(User_Person_slt ups) {

            if (ups.ID > 0)
            {
                var isthis = User_Person_sltService.LoadEntities(x => x.ID == ups.ID).FirstOrDefault();
                ups.UserInfo = isthis.UserInfo;
                User_Person_sltService.EditEntity(ups);
            }
            else {
                User_Person_sltService.AddEntity(ups);
            }
            

            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}
