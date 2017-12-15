using CZBK.ItcastOA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System.IO;
using System.Data;

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
                               CPname = a.CPname,
                               CPXingHao = a.CPXingHao,
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
                               HanShui=a.T_BaoJiaToP.T_BoolItem.str,
                               BaoJiaYunFei= a.BaoJiaYunFei,
                               Remark=a.Remark,
                               CpJB = a.T_ChanPinName.MyTexts,
                               Denjiu= a.T_BaoJiaToP.T_YSItems.MyText
                               
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
            var eYunfei = Request["Yunfei"];
            var baojia= YXB_BaojiaService.LoadEntities(x => x.id == rID).FirstOrDefault();            
            //检查更改金额之前是否存在值
            if (baojia.BaoJiaMoney == null)
            {
                baojia.BaoJiaMoney = Convert.ToDecimal(eMoney);
                baojia.BaoJiaYunFei= Convert.ToDecimal(eYunfei);
                baojia.BaoJiaPerson = LoginUser.ID;
                baojia.BaoJiaTime = MvcApplication.GetT_time();
                baojia.ZhuangTai = 1;
                YXB_BaojiaService.EditEntity(baojia);
            }
            else
            {
                baojia.EditQianMoney=baojia.BaoJiaMoney;
                baojia.EditQianYunFei = baojia.BaoJiaYunFei;
                baojia.UpdataTime= MvcApplication.GetT_time();
                baojia.UpdataUserID = LoginUser.ID;
                baojia.BaoJiaMoney = Convert.ToDecimal(eMoney);  
                baojia.BaoJiaYunFei = Convert.ToDecimal(eYunfei);
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
                it.WinYunFei= Convert.ToDecimal(Request["EidtYunFei" + it.id]);
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
            uim.CPname = Request["CPname"] == null ? "" : Request["CPname"].Length <= 0 ?"": Request["CPname"];
            uim.CPxh = Request["CPxh"] == null ? "" : Request["CPxh"].Length <= 0 ?"": Request["CPxh"];
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            var temp1 = YXB_BaojiaService.LoadSearchEntities(uim);
            var temp = from a in temp1
                       select new SlcClass
                       {
                           ID = a.id,
                           CPname = a.CPname,
                           CPXingHao = a.CPXingHao,
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
                           WinYunFei=a.WinYunFei,
                           WinStr = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().T_YSItems.MyText,
                           HanShui=a.T_BaoJiaToP.T_BoolItem.str,
                           BaoJiaYunFei=a.BaoJiaYunFei

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
                           UName = a.YXB_Kh_list.UserInfo.UName,
                           Denjiu=a.T_YSItems.MyText
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
            string HanShuiStr = temp.HanShuiID == null ? "" : temp.T_BoolItem.str;
            string bak = temp.T_WinBak.FirstOrDefault()==null?"":temp.T_WinBak.FirstOrDefault().Bak;
            var mmp = from a in temp.YXB_Baojia
                      select new
                      {
                          ID = a.id,
                          CPname = a.CPname,
                          CpXingHao = a.CPXingHao,
                          CpMoney = a.WinMoney,
                          WinYunFei=a.WinYunFei,
                          BaoJiaMoney=a.BaoJiaMoney,
                          CPShuLiang = a.CPShuLiang,
                          BaoJiaYunFei= a.BaoJiaYunFei,
                          Remark=a.Remark,
                          Cpdengji= a.T_ChanPinName.MyTexts
                         
                      };
            return Json(new {ret="ok",temp=mmp,XiangMuName=XiangMuName,HanShuiStr= HanShuiStr,bak = bak }, JsonRequestBehavior.AllowGet);
        }

        //获取合同进行完成后操作的数据查询
        public ActionResult GetWinHeTongWinData() {
            int TotalCount = 0;
            var rtt = GeteSS(out TotalCount);
            return Json(new { rows = rtt, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        private List<SlcClass> GeteSS(out int intcount)
        {

            bool OutBool = Request["outexe"] == null ? false : Convert.ToBoolean(Request["outexe"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            pageSize = OutBool ? pageSize : pageSize;
            int win = Request["win"] == null ? 0 : int.Parse(Request["win"]);
            UserInfoParam uim = new UserInfoParam();
            uim.Uptime = Convert.ToDateTime(Request["UpTime"]);
            uim.Dwtime = Convert.ToDateTime(Request["DwTime"]);
            uim.zt = Request["YyZt"] == null ? 0 : Request["YyZt"].Length <= 0 ? 0 : int.Parse(Request["YyZt"]);
            uim.addess = Request["addess"];
            uim.Person = Request["Person"] == null ? 0 : Request["Person"].Length <= 0 ? 0 : int.Parse(Request["Person"]);
            uim.KHname = Request["KHname"] == null ? 0 : Request["KHname"].Length <= 0 ? 0 : int.Parse(Request["KHname"]);
            uim.CPname = Request["CPname"] == null ? "" : Request["CPname"].Length <= 0 ?"": Request["CPname"];
            uim.CPxh = Request["CPxh"] == null ? "" : Request["CPxh"].Length <= 0 ? "" : Request["CPxh"];
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            var temp1 = T_WinBakService.LoadSearchEntities(uim);
            var temp = from a in temp1
                       select new SlcClass
                       {
                           ID = a.ID,
                           TopId = a.T_BaoJiaToP.id,
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
                           WinMoney = a.T_BaoJiaToP.YXB_Baojia.DefaultIfEmpty().Sum(s => s.CPShuLiang * (s.WinMoney + s.WinYunFei)),
                           AddTime = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().AddTime,
                           TOPaddtime = a.T_BaoJiaToP.AddTime,
                           BaoJiaTime = a.T_BaoJiaToP.YXB_Baojia.FirstOrDefault().BaoJiaTime,
                           GhTime = a.T_BaoJiaToP.GhTime,
                           NewTime = a.T_BaoJiaToP.YXB_Kh_list.NewTime,
                           WinStr = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().T_YSItems.MyText,
                           Winbak = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().Bak,


                       };
            var templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
                long tid = templist[i].TopId;
                var topbj = YXB_BaojiaService.LoadEntities(x => x.BaoJiaTop_id == tid).DefaultIfEmpty();
                templist[i].FahuoMoney = topbj.Sum(x => x.T_WinBakFaHuo.Sum(m => m.FahuoInt) * (x.WinMoney + x.WinYunFei));

            }
            intcount = uim.TotalCount;
            return templist;

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
            var mmp = YXB_BaojiaService.LoadEntities(x => x.id == wbf.BaoJia_ID).FirstOrDefault();
            if (mmp == null)
            {
                return Json(new { ret = "no", msg = "报价单中没有要修改的报价数据！请联系管理员！" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                int fhint= T_WinBakFaHuoService.LoadEntities(x => x.BaoJia_ID == wbf.BaoJia_ID)==null?0:Convert.ToInt32( T_WinBakFaHuoService.LoadEntities(x => x.BaoJia_ID == wbf.BaoJia_ID).Sum(x => x.FahuoInt));
                if (fhint + wbf.FahuoInt > mmp.CPShuLiang)
                {
                    return Json(new { ret = "no", msg = "发货总数不可大于发货量！" }, JsonRequestBehavior.AllowGet);
                }
            }
            wbf.AddTime = MvcApplication.GetT_time();
            wbf.AddUser = LoginUser.ID;
            wbf.Del = 0;
            T_WinBakFaHuoService.AddEntity(wbf);

            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        private IQueryable GetListWINfAHUO(long TopId, out int TotalCount)
        {

            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;           
            var temp= YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.BaoJiaTop_id == TopId && x.DelFlag == 0 && x.WIN == 1, x => x.AddTime, true);
           
            var rtemp = from a in temp
                        select new 
                        {
                            ID = a.id,
                            CpName = a.CPname,
                            CpXinghao = a.CPXingHao,
                            CPShuLiang = a.CPShuLiang,
                            WinMoney = a.WinMoney,
                            WinYunFei = a.WinYunFei,
                            AddTime = a.AddTime,
                            Fahuoyint= a.T_WinBakFaHuo.Sum(x=>x.FahuoInt),
                            FahuoTime=a.T_WinBakFaHuo.Max(x=>x.FaHuoTime),
                            
                        };            
            return rtemp;
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
                retMonth.Add(new Month { ID = 0, WinCount = 0, LostCount = 0, DaiDingCount = 0, SumCount = 0 ,WinDML=0,lostDML=0,DdDml=0,SumDMLCount=0});
            }
            else
            {
                retMonth.Add(GetMonthVar(oldtemp, 0,0));
            }
            //获取成功失败原因列表
            var yyItem = T_YSItemsService.LoadEntities(x => x.Items==1&&x.ID!=1);
            List<Items>[] Rlis = new List<Items>[2];
            Rlis[0] = AddList(yyItem, oldtemp);
            oldtemp = T_BaoJiaToPService.LoadEntities(x=>x.AddTime>=Utime&&x.AddTime<=Dtime).DefaultIfEmpty();
            

           
            Rlis[1]=AddList(yyItem, oldtemp);

            retMonth.Add(GetMonthVar(oldtemp,  retMonth[0].DaiDingCount,retMonth[0].DdDml));

            oldtemp = T_BaoJiaToPService.LoadEntities(x => x.AddTime <= Dtime);
            retMonth.Add(GetMonthVar(oldtemp, 0,0));

            #region 成功失败待定金额百分比
            var LostYuanyinItem = T_WinBakService.LoadEntities(x => x.AddTime < Utime).DefaultIfEmpty();
            //获取报价人员
            var Uload = UserInfoService.LoadEntities(x => x.BuMenID == 1).DefaultIfEmpty();   
            //获取成功或失败的信息       
            var DMYuanyinItem = T_WinBakService.LoadEntities(x => x.AddTime >= Utime && x.AddTime <= Dtime).DefaultIfEmpty();
            //获取待定信息
            var DaiDingData = T_BaoJiaToPService.LoadEntities(x => x.AddTime >= Utime && x.AddTime <= Dtime).Where(x => x.T_WinBak.Count() == 0).DefaultIfEmpty();

           //.Sum(a => a.YXB_Baojia.Sum(m => m.CPShuLiang * (m.BaoJiaMoney + m.BaoJiaYunFei)));
            var DMyyItem = from a in DMYuanyinItem
                           select new {
                               PerName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.PerSonName,
                               WinMoney = a.YuanYin == 1 ? a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei)):0,
                               LostMoney=a.YuanYin!=1 ? a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei)) : 0,
                               DaiDingMoney=a.T_BaoJiaToP.YXB_Baojia.Where(m=>m.WIN==0).Sum(m => m.CPShuLiang * (m.BaoJiaMoney + m.BaoJiaYunFei)),
                                
                           };
            List<Items> WinLostMoney = new List<Items>();

            foreach (var f in Uload)
            {
                Items its = new Items();
                its.PName = f.PerSonName;
                its.Wmoney = DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.WinMoney) == null ? 0 : DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.WinMoney);
                its.Lmoney = DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.LostMoney) == null ? 0 : DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.LostMoney);
                var itm = DaiDingData.Where(x => x.YXB_Kh_list.UserInfo.PerSonName == f.PerSonName).DefaultIfEmpty().Sum(x => x.YXB_Baojia.Sum(y => (y.BaoJiaMoney + y.BaoJiaYunFei) * y.CPShuLiang));
                its.Dmoney = itm == null ? 0 : itm;
                its.WPercent = Rounds((its.Wmoney / DMyyItem.Sum(x => x.WinMoney) * 100) == null ? 0 : its.Wmoney / DMyyItem.Sum(x => x.WinMoney) * 100);
                its.LPercent = Rounds((its.Lmoney / DMyyItem.Sum(x => x.LostMoney) * 100) == null ? 0 :its.Lmoney / DMyyItem.Sum(x => x.LostMoney) * 100);
                var Summoneydd=DaiDingData.Sum(x => x.YXB_Baojia.Sum(y => (y.BaoJiaMoney + y.BaoJiaYunFei) * y.CPShuLiang));
                its.DPercent = Rounds( Summoneydd == null ? 0 : its.Dmoney / Summoneydd*100);
                WinLostMoney.Add(its);
            }
            //添加总数
            Items ims = new Items();
            ims.PName = "合计";
            ims.Wmoney = WinLostMoney.Sum(x => x.Wmoney);
            ims.Lmoney = WinLostMoney.Sum(x => x.Lmoney);
            ims.Dmoney = WinLostMoney.Sum(x => x.Dmoney);
            ims.WPercent = Convert.ToInt32(WinLostMoney.Sum(x => x.WPercent));
            ims.LPercent = Convert.ToInt32(WinLostMoney.Sum(x => x.LPercent)); 
            ims.DPercent = Convert.ToInt32(WinLostMoney.Sum(x => x.DPercent));
            WinLostMoney.Add(ims);
            #endregion


            return Json(new { ret = "ok", temp = retMonth ,LostItem=Rlis,XiangXi= WinLostMoney }, JsonRequestBehavior.AllowGet);
        }
        private Month GetMonthVar(IQueryable<T_BaoJiaToP> iq,int p,decimal? ddmm)
        {
            var mscc = from a in iq
                       select new
                       {
                           Wcount = a.T_WinBak.Where(x => x.YuanYin == 1),
                           Lcount = a.T_WinBak.Where(x => x.YuanYin != 1),
                       };

            Month mh = new Month() ;
            mh.ID = 1;
            mh.WinCount = mscc.Sum(x => x.Wcount.Count());
            mh.LostCount = mscc.Sum(x => x.Lcount.Count());
            var ttw = iq.Where(x => x.T_WinBak.Count() == 0).FirstOrDefault();
            mh.DaiDingCount = ttw==null?p: iq.Where(x => x.T_WinBak.Count() == 0).Count() + p;
            mh.SumCount = mh.WinCount + mh.LostCount + mh.DaiDingCount;

            var t = mscc.Sum(x => x.Wcount.Sum(a => a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei))));
            mh.WinDML = Math.Round( Convert.ToDecimal(t==null?0:t),2);
            t = mscc.Sum(x => x.Lcount.Sum(a => a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei))));
            mh.lostDML = t == null ? 0 : t;
            var gg = iq.Where(x => x.T_WinBak.Count() == 0).Sum(a => a.YXB_Baojia.Sum(m => m.CPShuLiang * (m.BaoJiaMoney + m.BaoJiaYunFei)));
            mh.DdDml= (gg ==null?0:gg)+ ddmm;


            mh.SumDMLCount = CdmlNull(mh.WinDML) + CdmlNull(mh.lostDML) + CdmlNull(mh.DdDml);
            //Convert.ToDecimal( iq.Where(b=>b.T_WinBak.Where(ix=>ix.YuanYin==1)).Sum(x => x.YXB_Baojia.Sum(y => y.CPShuLiang * (y.WinMoney + y.WinYunFei))));
            return mh;
        }
        private List<Items> AddList(IQueryable<T_YSItems> yyItem, IQueryable<T_BaoJiaToP> oldtemp)
        {
            var s = from a in oldtemp
                    from m in yyItem
                    select new
                    {
                        count = a.T_WinBak.Where(x => x.YuanYin == m.ID).Count(),
                        text = m.MyText
                    };
            List<Items> ts = new List<Items>();
            foreach (var a in yyItem)
            {
                Items tis = new Items();
                tis.Text = a.MyText;
                tis.Count = s.Where(x => x.count > 0 && x.text == a.MyText).Count();
                ts.Add(tis);
            }
            return ts;
        }
        private decimal Rounds(decimal? val)
        {
            return decimal.Round(Convert.ToDecimal(val), 2);
        }
        //删除发货详细信息
        public ActionResult DelFahuoXiangXI() {
            var id = Convert.ToInt64(Request["ID"]);
            var temp = T_WinBakFaHuoService.LoadEntities(x => x.ID == id).FirstOrDefault();
            T_WinBakFaHuoService.DeleteEntity(temp);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //获取发货详细信息
        public ActionResult FaHuoXiangXI()
        {
            var id = Convert.ToInt64(Request["ID"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            var temp = T_WinBakFaHuoService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x =>x.BaoJia_ID==id, x => x.AddTime, true);
            var ttemp = from a in temp
                        select new
                        {
                            ID = a.ID,
                            FaHuoBak= a.FaHuoBak,
                            FahuoInt= a.FahuoInt,
                            FaHuoTime=a.FaHuoTime,
                            AddTime=a.AddTime,
                            PerSonName= a.UserInfo1.PerSonName
                        };
        
            return Json(new { ret = "ok", rows = ttemp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }


        private decimal CdmlNull(decimal? dml)
        {
            return dml == null ? 0 : Convert.ToDecimal(dml);
        }
        public FileResult DownLoadExcel()
        {           
            DataTable Tdt = new DataTable();
            Tdt.Columns.Add("id", typeof(int));
            Tdt.Columns.Add("SqBaoJiaPer", typeof(string));
            Tdt.Columns.Add("Comname", typeof(string));
            Tdt.Columns.Add("KhName", typeof(string));
            Tdt.Columns.Add("KhPer", typeof(string));
            DataRow dr = Tdt.NewRow();
            dr["id"] = 0;
            dr["SqBaoJiaPer"] = "SqBaoJiaPer";
            dr["Comname"] = "Comname";
            dr["KhName"] = "KhName";
            dr["KhPer"] = "KhPer";
            Tdt.Rows.Add(dr);
            DataTable dt = Tdt;//获取需要导出的datatable数据  
                               //创建Excel文件的对象     
            try
            {
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

                //添加一个sheet  
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                //给sheet1添加第一行的头部标题  
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                //row1.RowStyle.FillBackgroundColor = "";  
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row1.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                }
                //将数据逐步写入sheet1各个行  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString().Trim());
                    }
                }
                string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间  
                                                                         // 写入到客户端   
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", strdate + "Excel.xls");
            }
            catch (Exception ex)
            {
                string ms = ex.ToString();
                return null;
            }
           
        }
        //获取城市列表
        public ActionResult GetCity() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            var CityTemy = SysFieldService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.MyColums == "Province", x => x.MyTexts, false);
            if (Request["ParentId"] != null)
            {
                var sp = int.Parse(Request["ParentId"]);
                CityTemy = SysFieldService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.ParentId == sp&&x.ID!=sp, x => x.MyTexts, false);
            }
          
            
            var temp = from a in CityTemy
                       select new {
                          ID= a.ID,
                          Text=a.MyTexts,
                          ParentId=a.ParentId,
                          MyColums= a.MyColums
                       };
            return Json(new { rows = temp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        //追加城市列表信息
        public ActionResult AddSysfied() {
            int? ParentID = Convert.ToInt32(Request["Parent"]);
            var strval = Request["strval"];
            var val = Request["val"];
            var isdistc= SysFieldService.LoadEntities(x => x.ParentId == ParentID && x.MyColums == strval && x.MyTexts == val).FirstOrDefault();
            if (isdistc != null)
            {
                return Json(new { ret = "no", msg = "不可添加重复信息！" }, JsonRequestBehavior.AllowGet);
            }
            SysField sfd = new SysField();
            sfd.MyTexts = val;
            sfd.ParentId = ParentID;
            sfd.MyColums = strval;
            SysFieldService.AddEntity(sfd); 
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //删除追加的城市列表信息
        public ActionResult DelSysfiedF()
        {
            int? did = Convert.ToInt32(Request["id"]);
           
            var isdistc = SysFieldService.LoadEntities(x => x.ID==did).FirstOrDefault();
            if (isdistc != null)
            {
                if (SysFieldService.DeleteEntity(isdistc))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "no", msg = "删除失败！" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { ret = "no", msg = "未找到要删除的信息ID！" }, JsonRequestBehavior.AllowGet);

            }  
        }
    }
}
