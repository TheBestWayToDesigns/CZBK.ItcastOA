using CZBK.ItcastOA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class MasterBaoJiaController : BaseController
    {
        //
        // GET: /MasterBaoJia/
        IBLL.IYXB_BaojiaService YXB_BaojiaService { get; set; }
        IBLL.IYXB_Kh_listService YXB_Kh_listService { get; set; }
        IBLL.IT_BaoJiaToPService T_BaoJiaToPService { get; set; }
        IBLL.ISysFieldService SysFieldService { get; set; }
        IBLL.IT_YSItemsService T_YSItemsService { get; set; }
        IBLL.IT_WinBakService T_WinBakService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IT_ChanPinNameService T_ChanPinNameService { get; set; }
        IBLL.IT_WinBakFaHuoService T_WinBakFaHuoService { get; set; }

        short delFlag = (short)DelFlagEnum.Normarl;
        public ActionResult Index()
        {
            //用户名列表
            ViewBag.user = UserInfoService.LoadEntities(x => x.DelFlag !=1&&x.BuMenID==1).ToList();
            //状态列表
            ViewBag.items= T_YSItemsService.LoadEntities(x => x.Items == 1).ToList();
            //客户名称 与 项目名称列表
            ViewBag.KeHuName = YXB_Kh_listService.LoadEntities(x => x.DelFlag == 0).ToList();
            return View();
        }
        public ActionResult GetMoneyInfo()
        {
            int delflg = Request["delflg"] == null ? 0 :int.Parse( Request["delflg"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int win =  Request["win"]== null ? 0 :int.Parse( Request["win"]);
            delflg = win == 1 ? 1 : delflg;
            int totalCount = 0;
            if (win == 1)
            {
                return Json(new { rows = "", ret = delflg, total = totalCount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Adata = YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.DelFlag == delFlag && x.ZhuangTai == delflg && x.WIN == win, x => x.AddTime, false);
                var temp = from a in Adata
                           select new SlcClass
                           {
                               ID = a.id,
                               CPname = a.T_ChanPinName.MyTexts,
                               CPXingHao = a.T_ChanPinName1.MyTexts,
                               CPShuLiang = a.CPShuLiang,
                               AddTime = a.AddTime,
                               ZhuangTai = a.ZhuangTai,
                               BaoJiaMoney = a.BaoJiaMoney,
                               BaoJiaPerson = a.BaoJiaPerson,
                               BaoJiaTime = a.BaoJiaTime,
                               WIN = a.WIN,
                               GhTime = a.T_BaoJiaToP.GhTime,
                               JiShuYaoQiu = a.T_BaoJiaToP.JiShuYaoQiu,
                               Addess = a.T_BaoJiaToP.Addess,
                               DaiBanYunShu = a.T_BaoJiaToP.DaiBanYunShu,
                               JieShuanFanShi = a.T_BaoJiaToP.JieShuanFanShi,
                               HeTongQianDing = a.T_BaoJiaToP.HeTongQianDing,
                               TOPaddtime = a.T_BaoJiaToP.AddTime,
                               KHname = a.T_BaoJiaToP.YXB_Kh_list.KHname,
                               KHComname = a.T_BaoJiaToP.KHComname,
                               KHperson = a.T_BaoJiaToP.YXB_Kh_list.KHperson,
                               KHfaren = a.T_BaoJiaToP.YXB_Kh_list.KHfaren,
                               KHzhiwu = a.T_BaoJiaToP.YXB_Kh_list.KHzhiwu,
                               KHphoto = a.T_BaoJiaToP.YXB_Kh_list.KHphoto,
                               NewTime = a.T_BaoJiaToP.YXB_Kh_list.NewTime,
                               UName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.UName,
                           };
                var templist = temp.ToList();
                for (int i = 0; i < templist.Count; i++)
                {
                    templist[i].Addess = ArrF(templist[i].Addess);
                }
                return Json(new { rows = templist, ret = delflg, total = totalCount }, JsonRequestBehavior.AllowGet);
            }
           
           
           
           
        }
        public ActionResult editMoney()
        {
            var rID =Convert.ToInt64( Request["resultId"]);
            var eMoney = Request["finalRusult"];
           var baojia= YXB_BaojiaService.LoadEntities(x => x.id == rID).FirstOrDefault();
            
            //检查更改金额之前是否存在值
            if (baojia.BaoJiaMoney == null)
            {
                baojia.BaoJiaMoney = Convert.ToDecimal(eMoney);
                baojia.BaoJiaPerson = LoginUser.ID;
                baojia.BaoJiaTime = MvcApplication.GetT_time();
                baojia.ZhuangTai = 1;
                YXB_BaojiaService.EditEntity(baojia);
            }
            else
            {
                baojia.EditQianMoney=baojia.BaoJiaMoney;
                baojia.UpdataTime= MvcApplication.GetT_time();
                baojia.UpdataUserID = LoginUser.ID;
                baojia.BaoJiaMoney = Convert.ToDecimal(eMoney);               
                baojia.ZhuangTai = 1;
                YXB_BaojiaService.EditEntity(baojia);
            }
            

            return Json(new {  ret = true }, JsonRequestBehavior.AllowGet);
        }
        //信息完成处理
        public ActionResult WinChuLi(T_WinBak Twbak)
        {
            //  var WinMoney = Request["WinMoney"]==null?0:Request["WinMoney"].ToString().Trim().Length<=0?0:Convert.ToDecimal(Request["WinMoney"]);
      
            var Baojia = YXB_BaojiaService.LoadEntities(x => x.BaoJiaTop_id ==Twbak.BaoJiaTOPID).DefaultIfEmpty();
            List<YXB_Baojia> ybj = new List<YXB_Baojia>();
            foreach (var it in Baojia)
            {
                it.WinMoney = Convert.ToDecimal(Request["EidtMoney" + it.id]);
                it.WIN = 1;
                //修改完成报价信息
                ybj.Add(it);
            }

            Twbak.AddPerson = LoginUser.ID;
            Twbak.AddTime = MvcApplication.GetT_time();
            if (T_WinBakService.UpHeTongWinADD(ybj, Twbak))
            { return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet); }
            else
            { return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet); }           
          
        }
        public string ArrF(string add)
        {
            string ArryAddess = "";
            string straddess = add;
            var arry = straddess.Split(',');
            foreach (string s in arry)
            {
                long ThisS = Convert.ToInt64(s);
                string thisf = SysFieldService.LoadEntities(x => x.ID == ThisS).FirstOrDefault().MyTexts;
                ArryAddess = ArryAddess.Trim().Length <= 0 ? thisf : ArryAddess + "-" + thisf;
            }
            return ArryAddess;
        }
        //复杂查询
        //获取产品名称
        public ActionResult GetCPname()
        {
            int action = int.Parse(Request["Action"]);
            var tmc = T_ChanPinNameService.LoadEntities(x => x.Del == 0);
            var tempName = from a in tmc
                           where a.MyColums == "CPname"
                           select new
                           {
                               ID = a.ID,
                               MyText = a.MyTexts,
                               MyColums = a.MyColums
                           };
            var tempXingH = from a in tmc
                            where a.MyColums == "CPxinghao"
                            select new
                            {
                                ID = a.ID,
                                MyText = a.MyTexts,
                                MyColums = a.MyColums
                            };
            tempName = tempName.OrderBy(p => p.MyText);
            tempXingH = tempXingH.OrderBy(p => p.MyText);
            return Json(action==1?tempName:tempXingH, JsonRequestBehavior.AllowGet);
        }
        //多选查询
        public ActionResult DuoXuanSearch()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int win = Request["win"] == null ? 0 : int.Parse(Request["win"]);   
            UserInfoParam uim = new UserInfoParam();
            uim.Uptime = Convert.ToDateTime(Request["UpTime"]);
            uim.Dwtime = Convert.ToDateTime(Request["DwTime"]);
            uim.zt = Request["Zt"] == null ? 0 : int.Parse(Request["Zt"]); 
            uim.addess = Request["addess"];
            uim.Person = Request["Person"]==null?0 : Request["Person"].Length <= 0 ? 0 : int.Parse( Request["Person"]);
            uim.KHname = Request["KHname"] == null ? 0 : Request["KHname"].Length<=0?0: int.Parse(Request["KHname"]);
            uim.CPname = Request["CPname"] == null ? 0 : Request["CPname"].Length <= 0 ? 0 : int.Parse(Request["CPname"]);
            uim.CPxh = Request["CPxh"] == null ? 0 : Request["CPxh"].Length <= 0 ?0: int.Parse(Request["CPxh"]);
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            var temp1 = YXB_BaojiaService.LoadSearchEntities(uim);
            var temp = from a in temp1
                       select new SlcClass
                       {
                           ID = a.id,
                           CPname = a.T_ChanPinName.MyTexts,
                           CPXingHao = a.T_ChanPinName1.MyTexts,
                           CPShuLiang = a.CPShuLiang,
                           AddTime = a.AddTime,
                           ZhuangTai = a.ZhuangTai,
                           BaoJiaMoney = a.BaoJiaMoney,
                           BaoJiaPerson = a.BaoJiaPerson,
                           BaoJiaTime = a.BaoJiaTime,
                           WIN = a.WIN,
                           GhTime = a.T_BaoJiaToP.GhTime,
                           JiShuYaoQiu = a.T_BaoJiaToP.JiShuYaoQiu,
                           Addess = a.T_BaoJiaToP.Addess,
                           DaiBanYunShu = a.T_BaoJiaToP.DaiBanYunShu,
                           JieShuanFanShi = a.T_BaoJiaToP.JieShuanFanShi,
                           HeTongQianDing = a.T_BaoJiaToP.HeTongQianDing,
                           TOPaddtime = a.T_BaoJiaToP.AddTime,
                           KHname = a.T_BaoJiaToP.YXB_Kh_list.KHname,
                           KHComname = a.T_BaoJiaToP.KHComname,
                           KHperson = a.T_BaoJiaToP.YXB_Kh_list.KHperson,
                           KHfaren = a.T_BaoJiaToP.YXB_Kh_list.KHfaren,
                           KHzhiwu = a.T_BaoJiaToP.YXB_Kh_list.KHzhiwu,
                           KHphoto = a.T_BaoJiaToP.YXB_Kh_list.KHphoto,
                           NewTime = a.T_BaoJiaToP.YXB_Kh_list.NewTime,
                           UName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.UName,
                           WinMoney = a.WinMoney,
                           WinStr = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().T_YSItems.MyText
                           
                       };
            var templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
            return Json(new { rows = templist,  total = uim.TotalCount }, JsonRequestBehavior.AllowGet);
              
        }
        
        //完成合同
        public ActionResult OverHeTongt() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            var Tbjtop = T_BaoJiaToPService.LoadEntities(a => a.DelFlag==0&& a.YXB_Baojia.All(x => x.WIN == 0)).DefaultIfEmpty().ToList();
            List<T_BaoJiaToP> tbjp = new List<T_BaoJiaToP>();
            foreach (var t in Tbjtop)
            {
                if (t == null)
                {
                    List<SlcClass> lscs = new List<SlcClass>();
                    return Json(new { rows = lscs, total = TotalCount }, JsonRequestBehavior.AllowGet);
                }
                if (t.YXB_Baojia.All(x => x.BaoJiaMoney != null) && t.YXB_Baojia.Count > 0)
                {

                    tbjp.Add(t);
                }
            }          
            var temp = from a in tbjp
                       select new SlcClass
                       {
                           ID = a.id,
                           JiShuYaoQiu = a.JiShuYaoQiu,
                           Addess = a.Addess,
                           DaiBanYunShu = a.DaiBanYunShu,
                           JieShuanFanShi = a.JieShuanFanShi,
                           HeTongQianDing = a.HeTongQianDing,
                           TOPaddtime = a.AddTime,
                           KHname = a.YXB_Kh_list.KHname,
                           KHComname = a.KHComname,
                           KHperson = a.YXB_Kh_list.KHperson,
                           KHfaren = a.YXB_Kh_list.KHfaren,
                           KHzhiwu = a.YXB_Kh_list.KHzhiwu,
                           KHphoto = a.YXB_Kh_list.KHphoto,
                           NewTime = a.YXB_Kh_list.NewTime,
                           UName = a.YXB_Kh_list.UserInfo.UName
                       };
            var templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
            return Json(new { rows = templist, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        //获取完成合同的详细数据
        public ActionResult GetWinHeTongData(){
            var id = Request["id"] == null ? 0 : int.Parse(Request["id"]);
            if (id == 0)
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
            var temp = T_BaoJiaToPService.LoadEntities(x => x.id == id).FirstOrDefault();
            string XiangMuName = temp.KHComname;
            string bak = temp.T_WinBak.FirstOrDefault()==null?"":temp.T_WinBak.FirstOrDefault().Bak;
            var mmp = from a in temp.YXB_Baojia
                      select new
                      {
                          ID = a.id,
                          CPname = a.T_ChanPinName.MyTexts,
                          CpXingHao = a.T_ChanPinName1.MyTexts,
                          CpMoney = a.WinMoney,
                          BaoJiaMoney=a.BaoJiaMoney,
                          CPShuLiang = a.CPShuLiang
                         
                      };
            return Json(new {ret="ok",temp=mmp,XiangMuName=XiangMuName, bak= bak }, JsonRequestBehavior.AllowGet);
        }

        //获取合同进行完成后操作的数据查询
        public ActionResult GetWinHeTongWinData() {

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int win = Request["win"] == null ? 0 : int.Parse(Request["win"]);
            UserInfoParam uim = new UserInfoParam();
            uim.Uptime = Convert.ToDateTime(Request["UpTime"]);
            uim.Dwtime = Convert.ToDateTime(Request["DwTime"]);
            uim.zt = Request["YyZt"] == null ? 0 : Request["YyZt"].Length <= 0 ? 0:int.Parse(Request["YyZt"]);
            uim.addess = Request["addess"];
            uim.Person = Request["Person"] == null ? 0 : Request["Person"].Length <= 0 ? 0 : int.Parse(Request["Person"]);
            uim.KHname = Request["KHname"] == null ? 0 : Request["KHname"].Length <= 0 ? 0 : int.Parse(Request["KHname"]);
            uim.CPname = Request["CPname"] == null ? 0 : Request["CPname"].Length <= 0 ? 0 : int.Parse(Request["CPname"]);
            uim.CPxh = Request["CPxh"] == null ? 0 : Request["CPxh"].Length <= 0 ? 0 : int.Parse(Request["CPxh"]);
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            var temp1 = T_WinBakService.LoadSearchEntities(uim);
            var temp = from a in temp1
                       select new SlcClass
                       {
                           ID = a.ID,
                           //CPname = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().T_ChanPinName.MyTexts,
                           //CPXingHao = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().T_ChanPinName1.MyTexts,
                           //CPShuLiang = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().CPShuLiang,
                           //BaoJiaMoney = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().BaoJiaMoney,
                           //WinMoney = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().WinMoney,
                           //WIN = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().WIN,

                           TopId=a.T_BaoJiaToP.id,
                           BaoJiaPerson = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().BaoJiaPerson,
                           JiShuYaoQiu = a.T_BaoJiaToP.JiShuYaoQiu,
                           Addess = a.T_BaoJiaToP.Addess,
                           DaiBanYunShu = a.T_BaoJiaToP.DaiBanYunShu,
                           JieShuanFanShi = a.T_BaoJiaToP.JieShuanFanShi,
                           HeTongQianDing = a.T_BaoJiaToP.HeTongQianDing,
                           KHname = a.T_BaoJiaToP.YXB_Kh_list.KHname,
                           KHComname = a.T_BaoJiaToP.KHComname,
                           KHperson = a.T_BaoJiaToP.YXB_Kh_list.KHperson,
                           KHfaren = a.T_BaoJiaToP.YXB_Kh_list.KHfaren,
                           KHzhiwu = a.T_BaoJiaToP.YXB_Kh_list.KHzhiwu,
                           KHphoto = a.T_BaoJiaToP.YXB_Kh_list.KHphoto,
                           UName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.UName,
                           WinMoney = a.T_BaoJiaToP.YXB_Baojia.DefaultIfEmpty().Sum(s => s.CPShuLiang * s.WinMoney),
                          
                           AddTime = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().AddTime,
                           TOPaddtime = a.T_BaoJiaToP.AddTime,
                           BaoJiaTime = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().BaoJiaTime,
                           GhTime = a.T_BaoJiaToP.GhTime,
                           NewTime = a.T_BaoJiaToP.YXB_Kh_list.NewTime,
                           WinStr = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().T_YSItems.MyText,
                           Winbak= a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().Bak,
                           FahuoMoney=  a.T_WinBakFaHuo.Where(x=>x.WinBakID==a.ID).Sum(x=>x.FaHuoMoney)

                       };
            var templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
            return Json(new { rows = templist, total = uim.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        //获取发货信息
        public ActionResult FaHuoSelect()
        {
            var winbakID = int.Parse(Request["ID"]);
            int TotalCount = 0;
            var temp = GetListWINfAHUO(winbakID,out TotalCount);
            return Json(new {  rows = temp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        //追加发货信息
        public ActionResult AddFahuo(T_WinBakFaHuo wbf)
        {
           var winbakfh=  T_WinBakFaHuoService.LoadEntities(x => x.WinBakID == wbf.WinBakID).DefaultIfEmpty();
            var fahuoid = winbakfh == null ? 0 : winbakfh.Max(x => x.FaHuoID);            
            wbf.AddTime = MvcApplication.GetT_time();
            wbf.AddUser = LoginUser.ID;
            wbf.FaHuoID = fahuoid==null?1: fahuoid++;
            wbf.Del = 0;
            wbf.WinFH = 0;
            var SumMoney = winbakfh.Sum(x => x.FaHuoMoney)+wbf.FaHuoMoney;

            var Tmoney = T_WinBakService.LoadEntities(x => x.ID == wbf.WinBakID).FirstOrDefault();
            if(Tmoney == null)
            {
                return Json(new { ret = "no", msg = "数据库中为找到要发货的总金额！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
               decimal? money=  Tmoney.T_BaoJiaToP.YXB_Baojia.Sum(x => x.CPShuLiang * x.WinMoney);
                if (SumMoney > money)
                {
                    return Json(new { ret = "no", msg = "超出供货金额！" }, JsonRequestBehavior.AllowGet);
                }
            }

            

            T_WinBakFaHuoService.AddEntity(wbf);

            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        private List<SlcClass> GetListWINfAHUO(long id,out int TotalCount) {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            if (T_WinBakFaHuoService.LoadEntities(x => x.WinBakID == id).FirstOrDefault() == null)
            {
                List<SlcClass> ms = new List<SlcClass>();
                TotalCount = 0;
                return  ms;
            }
            var temp = T_WinBakFaHuoService.LoadPageEntities(pageIndex, pageSize,out TotalCount, x => x.WinBakID == id, x => x.AddTime, true);
            var rtemp = from a in temp
                        select new SlcClass
                        {
                            ID = a.ID,
                            WinMoney = a.FaHuoMoney,
                            NewTime = a.FaHuoTime,
                            AddTime = a.AddTime,
                            KHComname = a.T_WinBak.T_BaoJiaToP.YXB_Kh_list.KHComname,
                            KHperson = a.T_WinBak.T_BaoJiaToP.YXB_Kh_list.KHperson,
                            KHphoto = a.T_WinBak.T_BaoJiaToP.YXB_Kh_list.KHphoto,
                            KHname = a.T_WinBak.T_BaoJiaToP.YXB_Kh_list.KHname,
                            Addess = a.T_WinBak.T_BaoJiaToP.Addess

                        };
            var templist = rtemp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
            return templist;
        }
        //删除发货信息
        public ActionResult DelFahuo()
        {
            return Json(new { ret = "ok", temp = "" }, JsonRequestBehavior.AllowGet);
        }
        //查询月总汇
        public ActionResult GetMonthSumData()
        {
            var Utime = Convert.ToDateTime(Request["Utime"]);
            var Dtime = Convert.ToDateTime(Request["Dtime"]);
            List<Month> retMonth = new List<Month>();
            var oldtemp = T_BaoJiaToPService.LoadEntities(x => x.AddTime < Utime);
            if (T_BaoJiaToPService.LoadEntities(x => x.AddTime < Utime).FirstOrDefault() == null)
            {
                retMonth.Add(new Month { ID = 0, WinCount = 0, LostCount = 0, DaiDingCount = 0, SumCount = 0 });
            }
            else
            {
                retMonth.Add(GetMonthVar(oldtemp, 0));
            }
           

            oldtemp = T_BaoJiaToPService.LoadEntities(x=>x.AddTime>=Utime&&x.AddTime<=Dtime);
            retMonth.Add(GetMonthVar(oldtemp,  retMonth[0].DaiDingCount));

            oldtemp = T_BaoJiaToPService.LoadEntities(x => x.AddTime <= Dtime);
            retMonth.Add(GetMonthVar(oldtemp, 0));
            return Json(new { ret = "ok", temp = retMonth }, JsonRequestBehavior.AllowGet);
        }
        private Month GetMonthVar(IQueryable<T_BaoJiaToP> iq,int p)
        {
            var mscc = from a in iq
                       select new
                       {
                           Wcount = a.T_WinBak.Where(x => x.YuanYin == 1),
                           Lcount = a.T_WinBak.Where(x => x.YuanYin != 1)
                       };

            Month mh = new Month() ;
            mh.ID = 1;
            mh.WinCount = mscc.Sum(x => x.Wcount.Count());
            mh.LostCount = mscc.Sum(x => x.Lcount.Count());
            mh.DaiDingCount = iq.Where(x => x.T_WinBak.Count() == 0).Count() + p;
            mh.SumCount = mh.WinCount + mh.LostCount + mh.DaiDingCount;
            return mh;
        }
    }
}
