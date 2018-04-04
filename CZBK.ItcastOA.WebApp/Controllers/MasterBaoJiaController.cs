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
using NPOI.SS.UserModel;
using CZBK.ItcastOA.Model.OutExcel;
using NPOI.HSSF.Util;
using System.Net;
using System.Text;
using Newtonsoft.Json;

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
        IBLL.IYXB_BaoJiaEidtMoneyService YXB_BaoJiaEidtMoneyService { get; set; }
        IBLL.IYXB_WinCanPinService YXB_WinCanPinService { get; set; }
        IBLL.IWXX_FormIDService WXX_FormIDService { get; set; }
        IBLL.IWXXUserInfoService WXXUserInfoService { get; set; }




       // short delFlag = (short)DelFlagEnum.Normarl;
        public ActionResult Index()
        {
            //用户名列表
            ViewBag.user = UserInfoService.LoadEntities(x => x.DelFlag != 1  && x.Click == null && (x.BuMenID == 1||x.BuMenID==20)).ToList();
            //状态列表
            ViewBag.items = T_YSItemsService.LoadEntities(x => x.Items == 1).ToList();
            //客户名称 与 项目名称列表
            ViewBag.KeHuName = YXB_Kh_listService.LoadEntities(x => x.DelFlag == 0).ToList();


            return View();
        }
        public ActionResult GetMenum()
        {
            //1：根据用户 ——角色——权限 将登陆用户具有的菜单权限查询出来放在一个集合中
            var loginUserInfo = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();

            var loginUserRoleInfo = loginUserInfo.RoleInfoes;//获取登陆用户的角色信息
            string actionTypeEnum = "1";//表示菜单权限
            //查询出角色对应的菜单权限
            var loginUserActionInfo = (from r in loginUserRoleInfo
                                       from a in r.ActionInfo
                                       where a.ActionMethodName == actionTypeEnum
                                       select a).ToList();
            //2：根据用户——权限

            //根据登陆用户查询o.R_UserInfo_ActionInfo中间表，然后在用导航属性查询权限表
            var r_userInfo_actionInfo = from r in loginUserInfo.R_UserInfo_ActionInfo select r.ActionInfo;

            //判断是否是菜单权限
            var loginUserMenuAction = (from r in r_userInfo_actionInfo
                                       where r.ActionMethodName == actionTypeEnum
                                       select r).ToList();
            //将存储登陆用户权限的两个集合合并
            loginUserActionInfo.AddRange(loginUserMenuAction);
            //查询出所有登陆用户禁止的权限的编号
            var loginForbActionInfo = (from r in loginUserInfo.R_UserInfo_ActionInfo
                                       where r.IsPass == false
                                       select r.ActionInfoID).ToList();
            //将禁止的权限从集合中过滤掉
            var loginUserAllowActionlist = loginUserActionInfo.Where(a => !loginForbActionInfo.Contains(a.ID));
            //去除重复的
            var loginUserAllowActionlists = loginUserAllowActionlist.Distinct(new EqualityComparer());
            loginUserAllowActionlists = loginUserAllowActionlists.OrderBy(x => x.Sort);
            var returnActionlist = from a in loginUserAllowActionlists
                                   select new { icon = a.MenuIcon, title = a.ActionInfoName, url = a.ControllerName };
            return Json(new { rows = returnActionlist }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMoneyInfo()
        {
            int delflg = Request["delflg"] == null ? 0 : int.Parse(Request["delflg"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int win = Request["win"] == null ? 0 : int.Parse(Request["win"]);
            delflg = win == 1 ? 1 : delflg;
            int totalCount = 0;
            if (win == 1)
            {
                return Json(new { rows = "", ret = delflg, total = totalCount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<SlcClass> templist =new List<SlcClass>();
                LoadBaojia(pageIndex, pageSize, out totalCount, out templist, delflg, win, true);
                return Json(new { rows = templist, ret = delflg, total = totalCount }, JsonRequestBehavior.AllowGet);
            }

        }
        //提交报价前先在BaoJiaEidtMoney表中存个记录
        public bool createEditMoneyTab(long rid, string eM, string eY)
        {
            YXB_BaoJiaEidtMoney bjem = new YXB_BaoJiaEidtMoney();
            bjem.YXB_BJ_ID = rid;
            bjem.EidtUser_ID = LoginUser.ID;
            bjem.EidtTime = DateTime.Now;
            bjem.EditBJMoney = Convert.ToDecimal(eM);
            bjem.EditYFMoney = Convert.ToDecimal(eY);
            var s = YXB_BaoJiaEidtMoneyService.AddEntity(bjem);
            if (s == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //提交报价
        public ActionResult editMoney()
        {
            var rID = Convert.ToInt64(Request["resultId"]);
            var eMoney = Request["PMoney"];
            var eYunfei = Request["Yunfei"];
            bool b = createEditMoneyTab(rID, eMoney, eYunfei);
            if (b)
            {
                var baojia = YXB_BaojiaService.LoadEntities(x => x.id == rID).FirstOrDefault();
                //检查更改金额之前是否存在值
                if (baojia.BaoJiaMoney == null)
                {
                    baojia.BaoJiaMoney = Convert.ToDecimal(eMoney);
                    baojia.BaoJiaYunFei = Convert.ToDecimal(eYunfei);
                    baojia.BaoJiaPerson = LoginUser.ID;
                    baojia.BaoJiaTime = MvcApplication.GetT_time();
                    baojia.ZhuangTai = 1;
                    baojia.CheckMoney = 0;//未审核
                    YXB_BaojiaService.EditEntity(baojia);
                }
                else
                {
                    baojia.EditQianMoney = baojia.BaoJiaMoney;
                    baojia.EditQianYunFei = baojia.BaoJiaYunFei;
                    baojia.UpdataTime = MvcApplication.GetT_time();
                    baojia.UpdataUserID = LoginUser.ID;
                    baojia.BaoJiaMoney = Convert.ToDecimal(eMoney);
                    baojia.BaoJiaYunFei = Convert.ToDecimal(eYunfei);
                    baojia.ZhuangTai = 1;
                    baojia.CheckMoney = 0;//未审核
                    YXB_BaojiaService.EditEntity(baojia);
                }
                return Json(new { ret = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = false }, JsonRequestBehavior.AllowGet);
            }
        }
        //审核报价
        public ActionResult CheckBaoJiaYes()
        {
            var bjid = Convert.ToInt64(Request["bjid"]);
            var ckid = Request["ckid"];
            var temp = YXB_BaojiaService.LoadEntities(x => x.id == bjid).FirstOrDefault();
            temp.ZhuangTai = Convert.ToInt32(ckid);
            YXB_BaojiaService.EditEntity(temp);

            #region 调用微信通知方法
            var uid = temp.T_BaoJiaToP.YXB_Kh_list.AddUser;
            var bjTopID = temp.BaoJiaTop_id;
            var strReturn = SendTempletMessge(uid,temp);
            #endregion

            return Json(new { ret = "ok", strReturn= strReturn }, JsonRequestBehavior.AllowGet);
        }
        //信息完成处理
        public ActionResult WinChuLi(T_WinBak Twbak)
        {
            //  var WinMoney = Request["WinMoney"]==null?0:Request["WinMoney"].ToString().Trim().Length<=0?0:Convert.ToDecimal(Request["WinMoney"]);

            var Baojia = YXB_BaojiaService.LoadEntities(x => x.BaoJiaTop_id == Twbak.BaoJiaTOPID).DefaultIfEmpty();
            List<YXB_Baojia> ybj = new List<YXB_Baojia>();
            foreach (var it in Baojia)
            {
                it.WinMoney = Convert.ToDecimal(Request["EidtMoney" + it.id]);
                it.WinYunFei = Convert.ToDecimal(Request["EidtYunFei" + it.id]);
                it.WIN = 1;
                it.UpdataUserID = LoginUser.ID;
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
            return Json(action == 1 ? tempName : tempXingH, JsonRequestBehavior.AllowGet);
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
            uim.Person = Request["Person"] == null ? 0 : Request["Person"].Length <= 0 ? 0 : int.Parse(Request["Person"]);
            uim.KHname = Request["KHname"] == null ? 0 : Request["KHname"].Length <= 0 ? 0 : int.Parse(Request["KHname"]);
            uim.CPname = Request["CPname"] == null ? 0 : Request["CPname"].Length <= 0 ? 0 : int.Parse(Request["CPname"]);
            uim.CPxh = Request["CPxh"] == null ? 0 : Request["CPxh"].Length <= 0 ? 0 : int.Parse(Request["CPxh"]);
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            var temp1 = YXB_BaojiaService.LoadSearchEntities(uim);
            var temp = from a in temp1
                       select new SlcClass
                       {
                           ID = a.id,
                           CPname = a.T_ChanPinName1.MyTexts,
                           CPXingHao = a.T_ChanPinName2.MyTexts,
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
                           UName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.PerSonName,
                           WinMoney = a.WinMoney,
                           WinYunFei = a.WinYunFei,
                           WinStr = a.T_BaoJiaToP.T_WinBak.FirstOrDefault() == null ? null : a.T_BaoJiaToP.T_WinBak.FirstOrDefault().T_YSItems.MyText,
                           HanShui = a.T_BaoJiaToP.T_BoolItem.str,
                           BaoJiaYunFei = a.BaoJiaYunFei

                       };
            var templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
            return Json(new { rows = templist, total = uim.TotalCount }, JsonRequestBehavior.AllowGet);

        }

        //完成合同
        public ActionResult OverHeTongt()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            var Tbjtop = T_BaoJiaToPService.LoadEntities(a => a.DelFlag == 0 && a.YXB_Baojia.All(x => x.WIN == 0)).DefaultIfEmpty().ToList();
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
                           Denjiu = a.T_YSItems.MyText
                       };
            var templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
            return Json(new { rows = templist, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        //获取完成合同的详细数据
        public ActionResult GetWinHeTongData()
        {
            var id = Request["id"] == null ? 0 : int.Parse(Request["id"]);
            if (id == 0)
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
            var temp = T_BaoJiaToPService.LoadEntities(x => x.id == id).FirstOrDefault();
            string XiangMuName = temp.KHComname;
            string HanShuiStr = temp.HanShuiID == null ? "" : temp.T_BoolItem.str;
            string bak = temp.T_WinBak.FirstOrDefault() == null ? "" : temp.T_WinBak.FirstOrDefault().Bak;
            var mmp = from a in temp.YXB_Baojia
                      select new
                      {
                          ID = a.id,
                          CPname = a.T_ChanPinName1.MyTexts,
                          CpXingHao = a.T_ChanPinName2.MyTexts,
                          CpMoney = a.WinMoney,
                          WinYunFei = a.WinYunFei,
                          BaoJiaMoney = a.BaoJiaMoney,
                          CPShuLiang = a.CPShuLiang,
                          BaoJiaYunFei = a.BaoJiaYunFei,
                          Remark = a.Remark,
                          Cpdengji = a.T_ChanPinName.MyTexts

                      };
            return Json(new { ret = "ok", temp = mmp, XiangMuName = XiangMuName, HanShuiStr = HanShuiStr, bak = bak }, JsonRequestBehavior.AllowGet);
        }

        //获取合同进行完成后操作的数据查询
        public ActionResult GetWinHeTongWinData()
        {
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
            var temp = GetListWINfAHUO(winbakID, out TotalCount);
            return Json(new { rows = temp, total = TotalCount }, JsonRequestBehavior.AllowGet);
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
                int fhint = T_WinBakFaHuoService.LoadEntities(x => x.BaoJia_ID == wbf.BaoJia_ID) == null ? 0 : Convert.ToInt32(T_WinBakFaHuoService.LoadEntities(x => x.BaoJia_ID == wbf.BaoJia_ID).Sum(x => x.FahuoInt));
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
            var temp = YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.BaoJiaTop_id == TopId && x.DelFlag == 0 && x.WIN == 1, x => x.AddTime, true);

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
                            Fahuoyint = a.T_WinBakFaHuo.Sum(x => x.FahuoInt),
                            FahuoTime = a.T_WinBakFaHuo.Max(x => x.FaHuoTime),

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
                retMonth.Add(new Month { ID = 0, WinCount = 0, LostCount = 0, DaiDingCount = 0, SumCount = 0, WinDML = 0, lostDML = 0, DdDml = 0, SumDMLCount = 0 });
            }
            else
            {
                retMonth.Add(GetMonthVar(oldtemp, 0, 0));
            }
            //获取成功失败原因列表
            var yyItem = T_YSItemsService.LoadEntities(x => x.Items == 1 && x.ID != 1);
            List<Items>[] Rlis = new List<Items>[2];
            Rlis[0] = AddList(yyItem, oldtemp);
            oldtemp = T_BaoJiaToPService.LoadEntities(x => x.AddTime >= Utime && x.AddTime <= Dtime).DefaultIfEmpty();



            Rlis[1] = AddList(yyItem, oldtemp);

            retMonth.Add(GetMonthVar(oldtemp, retMonth[0].DaiDingCount, retMonth[0].DdDml));

            oldtemp = T_BaoJiaToPService.LoadEntities(x => x.AddTime <= Dtime);
            retMonth.Add(GetMonthVar(oldtemp, 0, 0));

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
                           select new
                           {
                               PerName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.PerSonName,
                               WinMoney = a.YuanYin == 1 ? a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei)) : 0,
                               LostMoney = a.YuanYin != 1 ? a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei)) : 0,
                               DaiDingMoney = a.T_BaoJiaToP.YXB_Baojia.Where(m => m.WIN == 0).Sum(m => m.CPShuLiang * (m.BaoJiaMoney + m.BaoJiaYunFei)),

                           };
            List<Items> WinLostMoney = new List<Items>();

            foreach (var f in Uload)
            {
                Items its = new Items();
                its.PName = f.PerSonName;
                its.Wmoney = DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.WinMoney) == null ? 0 : DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.WinMoney);
                its.Lmoney = DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.LostMoney) == null ? 0 : DMyyItem.Where(x => x.PerName == its.PName).Sum(x => x.LostMoney);
                var itm = DaiDingData.Where(x => x.YXB_Kh_list.UserInfo.ID == f.ID).DefaultIfEmpty().Sum(x => x.YXB_Baojia.Sum(y => (y.BaoJiaMoney + y.BaoJiaYunFei) * y.CPShuLiang));
                its.Dmoney = itm == null ? 0 : itm;
                its.WPercent = Rounds(DMyyItem.Sum(x => x.WinMoney) == 0 ? 0 : its.Wmoney / DMyyItem.Sum(x => x.WinMoney) * 100);
                its.LPercent = Rounds(DMyyItem.Sum(x => x.LostMoney) == 0 ? 0 : its.Lmoney / DMyyItem.Sum(x => x.LostMoney) * 100);
                var Summoneydd = DaiDingData.Sum(x => x.YXB_Baojia.Sum(y => (y.BaoJiaMoney + y.BaoJiaYunFei) * y.CPShuLiang));
                its.DPercent = Rounds(Summoneydd == null ? 0 : its.Dmoney / Summoneydd * 100);
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


            return Json(new { ret = "ok", temp = retMonth, LostItem = Rlis, XiangXi = WinLostMoney }, JsonRequestBehavior.AllowGet);
        }
        private Month GetMonthVar(IQueryable<T_BaoJiaToP> iq, int p, decimal? ddmm)
        {
            var mscc = from a in iq
                       select new
                       {
                           Wcount = a.T_WinBak.Where(x => x.YuanYin == 1),
                           Lcount = a.T_WinBak.Where(x => x.YuanYin != 1),
                       };

            Month mh = new Month();
            mh.ID = 1;
            mh.WinCount = mscc.Sum(x => x.Wcount.Count());
            mh.LostCount = mscc.Sum(x => x.Lcount.Count());
            var ttw = iq.Where(x => x.T_WinBak.Count() == 0).FirstOrDefault();
            mh.DaiDingCount = ttw == null ? p : iq.Where(x => x.T_WinBak.Count() == 0).Count() + p;
            mh.SumCount = mh.WinCount + mh.LostCount + mh.DaiDingCount;

            var t = mscc.Sum(x => x.Wcount.Sum(a => a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei))));
            mh.WinDML = Math.Round(Convert.ToDecimal(t == null ? 0 : t), 2);
            t = mscc.Sum(x => x.Lcount.Sum(a => a.T_BaoJiaToP.YXB_Baojia.Sum(m => m.CPShuLiang * (m.WinMoney + m.WinYunFei))));
            mh.lostDML = t == null ? 0 : t;
            var gg = iq.Where(x => x.T_WinBak.Count() == 0).Sum(a => a.YXB_Baojia.Sum(m => m.CPShuLiang * (m.BaoJiaMoney + m.BaoJiaYunFei)));
            mh.DdDml = (gg == null ? 0 : gg) + ddmm;


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
        public ActionResult DelFahuoXiangXI()
        {
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
            var temp = T_WinBakFaHuoService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.BaoJia_ID == id, x => x.AddTime, true);
            var ttemp = from a in temp
                        select new
                        {
                            ID = a.ID,
                            FaHuoBak = a.FaHuoBak,
                            FahuoInt = a.FahuoInt,
                            FaHuoTime = a.FaHuoTime,
                            AddTime = a.AddTime,
                            PerSonName = a.UserInfo1.PerSonName
                        };

            return Json(new { ret = "ok", rows = ttemp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }


        private decimal CdmlNull(decimal? dml)
        {
            return dml == null ? 0 : Convert.ToDecimal(dml);
        }
        //导出报表
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
            dr["SqBaoJiaPer"] = "SqBaoJiaPer1";
            dr["Comname"] = "Comname2";
            dr["KhName"] = "KhName3";
            dr["KhPer"] = "KhPer4";
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


                sheet1.CreateFreezePane(1, 1);// 冻结  
                                              // 设置列宽  
                IFont font = book.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 10;
                // 普通单元格样式  
                ICellStyle style = book.CreateCellStyle();
                style.SetFont(font);
                style.Alignment = HorizontalAlignment.Center;// 左右居中  
                style.VerticalAlignment = VerticalAlignment.Center;// 上下居中  
                style.WrapText = true;
                style.LeftBorderColor = HSSFColor.Black.Index;
                style.BorderLeft = BorderStyle.Thin;
                style.RightBorderColor = HSSFColor.Black.Index;
                style.BorderRight = BorderStyle.Thin;
                style.TopBorderColor = HSSFColor.Black.Index;
                style.BorderTop = BorderStyle.Thin;
                style.BottomBorderColor = HSSFColor.Black.Index;
                style.BorderBottom = BorderStyle.Thin;
                // style.setBorderBottom(HSSFCellStyle.BORDER_THIN); // 设置单元格的边框为粗体  
                // style.setBottomBorderColor(HSSFColor.BLACK.index); // 设置单元格的边框颜色．  
                // style.setFillForegroundColor(HSSFColor.WHITE.index);// 设置单元格的背景颜色．

                // ICellStyle style = OutExcel.Cellstyle();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row1.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    sheet1.SetColumnWidth(i, 6000);
                }
                //将数据逐步写入sheet1各个行  
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        ICell icl = rowtemp.CreateCell(j);
                        icl.SetCellValue(dt.Rows[i][j].ToString().Trim());
                        icl.CellStyle = style;
                       
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
        public ActionResult GetCity()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            var CityTemy = SysFieldService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.MyColums == "Province", x => x.MyTexts, false);
            if (Request["ParentId"] != null)
            {
                var sp = int.Parse(Request["ParentId"]);
                CityTemy = SysFieldService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.ParentId == sp && x.ID != sp, x => x.MyTexts, false);
            }


            var temp = from a in CityTemy
                       select new
                       {
                           ID = a.ID,
                           Text = a.MyTexts,
                           ParentId = a.ParentId,
                           MyColums = a.MyColums
                       };
            return Json(new { rows = temp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        //追加城市列表信息
        public ActionResult AddSysfied()
        {
            int? ParentID = Convert.ToInt32(Request["Parent"]);
            var strval = Request["strval"];
            var val = Request["val"];
            var isdistc = SysFieldService.LoadEntities(x => x.ParentId == ParentID && x.MyColums == strval && x.MyTexts == val).FirstOrDefault();
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

            var isdistc = SysFieldService.LoadEntities(x => x.ID == did).FirstOrDefault();
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

        //报价审核获取方法
        public ActionResult GetSHbaojia()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int toalCount = 0;
            List<SlcClass> templist;
            LoadBaojia(pageIndex, pageSize, out toalCount, out templist, 0, 0, false);
            return Json(new { rows = templist, total = toalCount }, JsonRequestBehavior.AllowGet);
        }

        private void LoadBaojia(int pageIndex, int pageSize, out int toalCount, out List<SlcClass> templist, int delflg, int win, bool isbool)
        {
            var Adata = YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out toalCount, x => x.DelFlag == 0 && x.WIN == win && x.ZhuangTai == delflg, x => x.AddTime, false);
            if (delflg == 1)
            {
                Adata = YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out toalCount, x => x.DelFlag == 0 && x.WIN == win && (x.ZhuangTai == delflg || x.ZhuangTai > 0), x => x.AddTime, false);
            }
            else if (delflg > 1)
            {
                delflg = delflg == 4 ? 1 : delflg;
                Adata = YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out toalCount, x => x.DelFlag == 0 && x.WIN == win && x.ZhuangTai == delflg, x => x.AddTime, false);
            }
            if (!isbool)
            { Adata = YXB_BaojiaService.LoadPageEntities(pageIndex, pageSize, out toalCount, x => x.ZhuangTai == 1 && x.DelFlag == 0 && x.WIN == win, x => x.AddTime, false); }

            var temp = from a in Adata
                       select new SlcClass
                       {
                           ID = a.id,
                           CPname = a.T_ChanPinName1.MyTexts,
                           CPXingHao = a.T_ChanPinName2.MyTexts,
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
                           UName = a.T_BaoJiaToP.YXB_Kh_list.UserInfo.PerSonName,
                           HanShui = a.T_BaoJiaToP.T_BoolItem.str,
                           BaoJiaYunFei = a.BaoJiaYunFei,
                           Remark = a.Remark,
                           CpJB = a.T_ChanPinName.MyTexts,
                           Denjiu = a.T_BaoJiaToP.T_YSItems.MyText
                       };
            templist = temp.ToList();
            for (int i = 0; i < templist.Count; i++)
            {
                templist[i].Addess = ArrF(templist[i].Addess);
            }
        }
        //获取历史报价数据
        public ActionResult GetOldBaojia()
        {
            var id = Convert.ToInt32(Request["id"]);
            var tem = YXB_BaoJiaEidtMoneyService.LoadEntities(x => x.YXB_BJ_ID == id).DefaultIfEmpty();
            if (tem == null)
            {
                return Json(new { msg = "", ret = "no" }, JsonRequestBehavior.AllowGet);
            }
            var temp = from a in tem
                       select new
                       {
                           ID = a.ID,
                           EditBJMoney = a.EditBJMoney,
                           EditYFMoney = a.EditYFMoney,
                           EidtTime = a.EidtTime,
                           PerSonName = a.UserInfo.PerSonName
                       };
            return Json(new { msg = temp, ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //获取产品除税表明细
        public ActionResult GetChuShuiTable()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
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

            var win = T_WinBakService.LoadEntities(x => x.AddTime >= uim.Uptime && x.AddTime < uim.Dwtime).DefaultIfEmpty();
            var chanpin = YXB_WinCanPinService.LoadEntities(x => x.ID > 0 && x.Del == null).DefaultIfEmpty();
            List<cls> Lcls = new List<cls>();
            foreach (var wins in win)
            {

                var s = from a in wins.T_BaoJiaToP.YXB_Baojia
                        select new cls
                        {
                            Cpname = a.T_ChanPinName1 != null ? a.T_ChanPinName1.MyTexts : "",
                            CpnameId = a.CPname,
                            CpXinghao = a.T_ChanPinName2 != null ? a.T_ChanPinName2.MyTexts : "",
                            CpXinghaoId = a.CPXingHao,
                            Hanshui = a.T_BaoJiaToP != null ? a.T_BaoJiaToP.T_BoolItem != null ? a.T_BaoJiaToP.T_BoolItem.str : "" : "",
                            Piaoju = a.T_BaoJiaToP != null ? a.T_BaoJiaToP.T_YSItems != null ? a.T_BaoJiaToP.T_YSItems.MyText : "" : "",
                            Money = a.WinMoney,
                            Yunfei = a.WinYunFei,
                            Khname = a.T_BaoJiaToP != null ? a.T_BaoJiaToP.YXB_Kh_list != null ? a.T_BaoJiaToP.YXB_Kh_list.KHname : "" : "",
                            Person = a.T_BaoJiaToP != null ? a.T_BaoJiaToP.YXB_Kh_list.UserInfo != null ? a.T_BaoJiaToP.YXB_Kh_list.UserInfo.PerSonName : "" : ""
                        };
                foreach (var l in s)
                {
                    Lcls.Add(l);
                }
            }
            List<cls> tempCls = new List<cls>();
            foreach (var cls in Lcls)
            {

                if (cls.Hanshui == "含税")
                {
                    cls.Money = cls.Money / (decimal)1.17;
                }
                var Thishave = tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault();
                cls cc = new cls();
                if (Thishave == null)
                {
                    cc.Cpname = cls.Cpname;
                    cc.CpnameId = cls.CpnameId;
                    cc.CpXinghao = cls.CpXinghao;
                    cc.CpXinghaoId = cls.CpXinghaoId;
                    cc.maxKhname = cls.Khname;
                    cc.minKhname = cls.Khname;
                    cc.maxMoney = cls.Money;
                    cc.minMoney = cls.Money;
                    cc.maxPerson = cls.Person;
                    cc.minPerson = cls.Person;
                    tempCls.Add(cc);
                }
                else
                {
                    decimal tm = decimal.Round(Convert.ToDecimal(cls.Money), 2);
                    if (cls.Money > Thishave.maxMoney)
                    {
                        tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault().maxMoney = tm;
                        tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault().maxKhname = cls.Khname;
                        tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault().maxPerson = cls.Person;
                    }
                    if (cls.Money < Thishave.minMoney)
                    {
                        tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault().minMoney = tm;
                        tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault().maxKhname = cls.Khname;
                        tempCls.Where(x => x.CpnameId == cls.CpnameId && x.CpXinghaoId == cls.CpXinghaoId).FirstOrDefault().maxPerson = cls.Person;
                    }
                }

            }


            return Json(new { rows = tempCls, total = uim.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        //获取客户报备信息
        public ActionResult GetKeHuBaoBei()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            var UpTime = Convert.ToDateTime(Request["UpTime"]);
            var DwTime = Convert.ToDateTime(Request["DwTime"]);
            var yxy = Request["yxy"] != "" ? Convert.ToInt32(Request["yxy"]) : 0;
            var khn = Request["khn"] != "" ? Convert.ToInt32(Request["khn"]) : 0;
            //var cs = Request["cs"] != "" ? Request["cs"] : "0";
            //var vs = Request["vs"] != "" ? Request["vs"] : "0";
            //var sd = Request["sd"] != "" ? Request["sd"] : "0";
            var addess = Request["addess"];
            UserInfoParam uip = new UserInfoParam();
            uip.PageIndex = pageIndex;
            uip.PageSize = pageSize;
            uip.Uptime = UpTime;
            uip.Dwtime = DwTime;
            uip.Person = yxy;
            uip.KHname = khn;
            uip.addess = addess;
            var temp_ = YXB_Kh_listService.loadBaoBeientities(uip);

           // var temp = T_BaoJiaToPService.LoadEntities(x => x.AddTime > UpTime && x.AddTime < DwTime).DefaultIfEmpty().ToList();


            var temp = from a in temp_
                       select new
                       {
                           ID = a.id,                          
                           KHname = a.KHname,                           
                           KHperson = a.KHperson,
                           KHfaren = a.KHfaren,
                           KHzhiwu = a.KHzhiwu,
                           KHphoto = a.KHphoto,
                           NewTime = a.NewTime,
                           UName = a.UserInfo.PerSonName,
                           khsgaaddess=a.KHSGAdrss,
                           bak=a.BeiZhu
                       };
            return Json(new { rows = temp, total = uip.TotalCount }, JsonRequestBehavior.AllowGet);

           
        }
        public ActionResult DELbbdata() {
            int ID =Convert.ToInt32( Request["ID"]);
            YXB_Kh_list yhl = YXB_Kh_listService.LoadEntities(x => x.id == ID).FirstOrDefault();
            yhl.DelFlag = 1;
            YXB_Kh_listService.EditEntity(yhl);
            return Json(new { ret="ok"},JsonRequestBehavior.AllowGet);
        }

        #region 微信通知用
        public string SendTempletMessge(int uid,YXB_Baojia bjinfo)
        {
            //var user = UserInfoService.LoadEntities(x => x.ID == uid).FirstOrDefault();
            var rtmp = WXXUserInfoService.LoadEntities(x => x.UID == uid).FirstOrDefault();
            if(rtmp == null)
            {
                return null;
            }
            string strReturn = string.Empty;
            try
            {
                #region 获取access_token
                string apiurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxc67a3c17709458e5&secret=34a9647b2c1120e443cdf14b1a0d6b46";
                WebRequest request = WebRequest.Create(@apiurl);
                request.Method = "POST";
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                string detail = reader.ReadToEnd();
                var jd = JsonConvert.DeserializeObject<WXApi>(detail);
                string token = (String)jd.access_token;
                DateTime dtime = MvcApplication.GetT_time();
                var wxx = WXX_FormIDService.LoadEntities(x => x.AddUserID == uid && x.StopTime > dtime).DefaultIfEmpty();
                WXX_FormID Minwxx = new WXX_FormID();
                if (wxx.ToList()[0] != null)
                {
                    Minwxx = wxx.OrderBy(x => x.StopTime).FirstOrDefault();
                }else
                {
                    return null;
                }
                #endregion
                #region 组装信息推送，并返回结果（其它模版消息于此类似）
                string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + token;
                var Money = bjinfo.WIN == 1 ? bjinfo.WinMoney : (bjinfo.EditQianMoney == null? bjinfo.BaoJiaMoney:bjinfo.EditQianMoney);
                var YunFei = bjinfo.WIN == 1 ? bjinfo.WinYunFei : (bjinfo.EditQianYunFei == null ? bjinfo.BaoJiaYunFei : bjinfo.EditQianYunFei);
                var sumMM = Money + YunFei;
                var BaoJiaTime = bjinfo.UpdataTime == null ? bjinfo.BaoJiaTime : bjinfo.UpdataTime;
                string temp = "{\"touser\": \"" + rtmp.WXID + "\"," +
                       "\"template_id\": \"3zN541eDUYsMVVZnqf6GEuZr7KDdOC1jamBsgEKHXY0\", " +
                       "\"topcolor\": \"#FF0000\", " +
                       "\"form_id\": \"" +Minwxx.FormID+"\","+
                       "\"data\": " +
                       "{\"first\": {\"value\": \"您好，您有一条报价通知信息\"}," +
                       "\"keyword1\": { \"value\": \""+ bjinfo.T_ChanPinName1.MyTexts +"("+ bjinfo.T_ChanPinName2.MyTexts +")\"}," +
                       "\"keyword2\": { \"value\": \""+ bjinfo.CPShuLiang +"\"}," +
                       "\"keyword3\": { \"value\": \""+ Money + "元\"}," +
                       "\"keyword4\": { \"value\": \""+ YunFei + "元\"}," +
                       "\"keyword5\": { \"value\": \""+ sumMM + "元\"}," +
                       "\"keyword6\": { \"value\": \""+ BaoJiaTime + "\"}," +
                       "\"remark\": {\"value\": \"\" }}}";
                #endregion
                //核心代码
                GetResponseData(temp, @url);
                strReturn = "推送成功";
                //删除使用过的formid
                WXX_FormIDService.DeleteEntity(Minwxx);
            }
            catch (Exception ex)
            {
                strReturn = ex.Message;
            }
            return strReturn;
        }
        public string GetResponseData(string JSONData, string Url)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 90000;
            //设置连接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();
            return strResult;
        }
        #endregion

    }

}
//微信通知用实体类
public class WXApi{
    public string access_token { set; get; }
}
public class cls {
    public long ID { get; set; }
    public string Cpname { get; set; }
    public long CpnameId { get; set; }
    public string CpXinghao { get; set; }
    public long CpXinghaoId { get; set; }
    public string Piaoju { get; set; }
    public string Hanshui { get; set; }
    public decimal? Money { get; set; }
    public decimal? Yunfei { get; set; }

    public string Khname { get; set; }

    public string Person { get; set; }
    public decimal? maxMoney { get; set; }
    public string maxKhname { get; set; }
    public string maxPerson { get; set; }

    public decimal? minMoney { get; set; }
    public string minKhname { get; set; }
    public string minPerson { get; set; }
}
