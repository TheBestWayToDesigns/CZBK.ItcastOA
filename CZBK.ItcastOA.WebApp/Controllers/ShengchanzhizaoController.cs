using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class ShengchanzhizaoController : BaseController
    {
        //
        // GET: /Shengchanzhizao/

        IBLL.IT_SczzDanjuService T_SczzDanjuService { get; set; }
        IBLL.IT_ShengChanZhiZhaoTopNameService T_ShengChanZhiZhaoTopNameService { get; set; }
        IBLL.IT_SczzItemService T_SczzItemService { get; set; }
        IBLL.IT_CanPanService T_CanPanService { get; set; }
        public ActionResult Index()
        {
            ViewBag.quanx = LoginUser.QuXian;
            return View();
        }
        //添加型号 与产品头部信息
        public ActionResult ADDitems()
        {
            var obj = Request["obj"].ToString().Trim();
            var VL = int.Parse(Request["VL"]);
            if (VL == 1)
            {
                var tmp = T_SczzItemService.LoadEntities(x => x.Text == obj).FirstOrDefault();
                if (tmp != null)
                {
                    return Json("Isdistict", JsonRequestBehavior.AllowGet);
                }
                T_SczzItem TS = new T_SczzItem();
                TS.AddTime = MvcApplication.GetT_time();
                TS.AddUser = LoginUser.ID;
                TS.Text = obj;
                TS.Del = 0;
                T_SczzItemService.AddEntity(TS);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            else if (VL == 0)
            {
                var tmp = T_ShengChanZhiZhaoTopNameService.LoadEntities(x => x.TopText == obj).FirstOrDefault();
                if (tmp != null)
                {
                    return Json("Isdistict", JsonRequestBehavior.AllowGet);
                }
                T_ShengChanZhiZhaoTopName tt = new T_ShengChanZhiZhaoTopName();
                tt.addtime = MvcApplication.GetT_time();
                tt.adduser = LoginUser.ID;
                tt.Del = 0;
                tt.TopText = obj;
                T_ShengChanZhiZhaoTopNameService.AddEntity(tt);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            else
            { return Json("errer", JsonRequestBehavior.AllowGet); }

        }

        //获取信息列表
        public ActionResult GetHref()
        {
            int pageIndex = Request["page"] != null ? Request["page"].ToString().Trim().Length<=0?1: int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            string MyColums = Request["MyColums"];
            int totalCount;
            if (MyColums == "CPlist")
            {
                var actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.AddUser == LoginUser.ID && a.del != 1, a => a.AddTime, false);
                var temp = from a in actioninfolist
                           select new
                           {
                           };
                return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
            }
            else
            if (MyColums == "CPxinghao")//规格型号
            {
                var actioninfolist = T_SczzItemService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.Del != 1, a => a.AddTime, false);
                var temp = from a in actioninfolist
                           select new
                           {
                               a.ID,
                               a.AddTime,
                               a.Text

                           };
                return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
            }
            if (MyColums == "JiHuaName")//单据
            {
                var actioninfolist = T_ShengChanZhiZhaoTopNameService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.Del != 1, a => a.addtime, false);
                var temp = from a in actioninfolist
                           select new
                           {
                               a.ID,
                               a.TopText,
                               a.addtime
                           };
                return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //删除 表头和规格型号
        public ActionResult Delitems()
        {

            var id = Convert.ToInt64(Request["delID"]);
            var MyColums = Request["MyColums"].ToString();
            if (MyColums == "CPxinghao")//规格型号
            {
                T_SczzItem ts = T_SczzItemService.LoadEntities(x => x.ID == id && x.AddUser == LoginUser.ID).FirstOrDefault();
                if (ts == null)
                {
                    return Json("noperson", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ts.Del = 1;
                    if (T_SczzItemService.EditEntity(ts))
                    { return Json("ok", JsonRequestBehavior.AllowGet); }
                }
            }
            else if (MyColums == "JiHuaName")
            {
                var top = T_ShengChanZhiZhaoTopNameService.LoadEntities(x => x.ID == id && x.adduser == LoginUser.ID).FirstOrDefault();
                if (top == null)
                {
                    return Json("noperson", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    top.Del = 1;
                    if (T_ShengChanZhiZhaoTopNameService.EditEntity(top))
                    { return Json("ok", JsonRequestBehavior.AllowGet); }
                }

            }

            return Json("no", JsonRequestBehavior.AllowGet);
        }

        //获取选项列表 
        public ActionResult GetSelect()
        {


            var actioninfolist = T_SczzItemService.LoadEntities(x => x.Del == 0).DefaultIfEmpty();
            var Caiz = from a in actioninfolist
                       select new
                       {
                           a.ID,
                           a.AddTime,
                           a.Text
                       };

            var topss = T_ShengChanZhiZhaoTopNameService.LoadEntities(x => x.Del == 0).DefaultIfEmpty();
            var Top = from a in topss
                      select new
                      {
                          a.ID,
                          a.addtime,
                          a.TopText
                      };

            return Json(new { Top = Top, Caiz = Caiz, ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //创建制造计划单
        public ActionResult AddTable()
        {

            T_SczzDanju tsd = new T_SczzDanju();
            tsd.TextNameID = Convert.ToInt64(Request["topnameid"]);
            tsd.AddTime = MvcApplication.GetT_time();
            tsd.ZhuangTai = 0;
            tsd.AddUser = LoginUser.ID;
            tsd.del = 0;
            T_SczzDanjuService.AddEntity(tsd);
            int totalCount = 0;
            var temp = GetSCZZdanju(out totalCount);
            return Json(new { result = true, temp = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        //创建详细数据
        public ActionResult AddTableone()
        {
            T_CanPan tcp = new T_CanPan();
            var thisID = Request["ID"].ToString().Trim().Length <= 0 ? 0 : Convert.ToInt64(Request["ID"]);

            if (thisID == 0)
            {
                tcp.SczzDanjuID = Convert.ToInt64(Request["TextNameID"]);
                var tscdj= T_SczzDanjuService.LoadEntities(x => x.ID == tcp.SczzDanjuID).FirstOrDefault();
                if (tscdj == null)
                {
                    return Json(new { result = "no", msg = "数据错误请联系管理员" }, JsonRequestBehavior.AllowGet);
                } else if (tscdj.ZhuangTai!=0) {
                    return Json(new { result = "no", msg = "已审核的信息不可在次添加信息！" }, JsonRequestBehavior.AllowGet);
                }
                tcp.Cpname = Request["Cpname"];
                tcp.ImageInt = Request["ImageInt"];
                if (Request["CpShuliang"].ToString().Trim().Length <= 0)
                {
                    return Json(new { result = "no", msg = "数量不可为空" }, JsonRequestBehavior.AllowGet);
                }
                tcp.CpShuliang = Convert.ToDecimal(Request["CpShuliang"]);
                tcp.SczzItemID = Convert.ToInt64(Request["SczzItemID"]);
                tcp.Bak = Request["Bak"];
                tcp.DEL = 0;
                tcp.AddTime = MvcApplication.GetT_time();
                tcp.AddUserID = LoginUser.ID;
                if (Request["overtime"] == null && Request["overtime"].ToString().Trim().Length == 0)
                {
                    return Json(new { result = "no", msg="完工时间不可为空！" }, JsonRequestBehavior.AllowGet);
                }
                if (Request["overtime"] != null&& Request["overtime"].ToString().Trim().Length>0)
                {
                    tcp.OverTime = Convert.ToDateTime(Request["overtime"]);
                        tcp.OverUserID = LoginUser.ID;
                }
                

                T_CanPanService.AddEntity(tcp);
                var temp = HuoquXLS((long)tcp.SczzDanjuID);
                return Json(new { result = "ok", temp = temp }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                T_CanPanService.EditEntity(tcp);
                return Json(new { result = false, ex = "" }, JsonRequestBehavior.AllowGet);
            }

        }
        //获取详细单据
        public ActionResult RetSczzDanju()
        {
            int totalCount = 0;
            var temp = GetSCZZdanju(out totalCount);

            return Json(new { result = true, temp = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        public IQueryable GetSCZZdanju(out int totalCount)
        {

            int pageIndex = Request["page"] != null ? Request["page"].ToString().Trim().Length <= 0?1:int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int zt = Request["zt"]==null?0: Request["zt"].ToString().Trim().Length<=0?0: int.Parse(Request["zt"]);
            var actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.ZhuangTai == zt, a => a.AddTime, false);
            //根据LoginUser.Umoney 判断浏览权限
            if (LoginUser.QuXian == 0)
            {
                actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 &&a.ZhuangTai==zt, a => a.AddTime, false);
            }
            else if (LoginUser.QuXian ==3)
            {
                if (zt < 2)
                {
                    actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.ZhuangTai == zt && a.UpShenHe == null, a => a.AddTime, false);
                }
                else if (zt == 2)
                {
                    actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.ZhuangTai == zt , a => a.AddTime, false);
                }
                else
                {
                    actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.UpShenHe == LoginUser.ID && a.ZhuangTai != 2, a => a.AddTime, false);
                }
                
            }
            else if (LoginUser.QuXian == 4)
            {
                if (zt < 2)
                {
                    actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.ZhuangTai == zt && a.CheJianShenHe == null, a => a.AddTime, false);
                }
                else if (zt == 2)
                {
                    actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.ZhuangTai == zt , a => a.AddTime, false);
                }
                else
                { actioninfolist = T_SczzDanjuService.LoadPageEntities<DateTime?>(pageIndex, pageSize, out totalCount, a => a.del == 0 && a.CheJianShenHe == LoginUser.ID && a.ZhuangTai != 2, a => a.AddTime, false); }

                
            }

            var temp = from a in actioninfolist
                       select new
                       {
                           ID = a.ID,
                           text = a.T_ShengChanZhiZhaoTopName.TopText,
                           time = a.AddTime,
                           Zhuangtai = a.ZhuangTai
                       };
            return temp;
        }

        //获取详细数据信息
        public ActionResult DanjuXiangXi()
        {
            var id = Convert.ToInt64(Request["id"]);
            var tdj = T_SczzDanjuService.LoadEntities(x => x.ID == id && x.del == 0).DefaultIfEmpty();

            var dj = from a in tdj
                     select new
                     {
                         ID = a.ID,
                         textname = a.T_ShengChanZhiZhaoTopName.TopText,
                         zhuangtai = a.ZhuangTai,
                         jhtcr = a.AddUserinfo.PerSonName,
                         sccjr=a.Scshuser.PerSonName,
                         jhzdr = a.jhzdruser.PerSonName,
                         jhpzr = a.Upshuser.PerSonName,
                         jgcj = a.CJSHuser.PerSonName,
                         panduan = a.AddUserinfo.QuXian == 0 ? LoginUser.QuXian == 0?false: true : false
                     };
            if (tdj == null)
            {
                return Json(new { ret = "ison" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var temolist = from a in tdj.FirstOrDefault().T_CanPan
                               where a.DEL == 0
                               select new
                               {
                                   ID = a.ID,
                                   Text = a.T_SczzItem.Text,
                                   Cpname = a.Cpname,
                                   CpShuliang = a.CpShuliang,
                                   ImageInt = a.ImageInt,
                                   AddTime = a.AddTime,
                                   OverTime = a.OverTime,
                                   Bak = a.Bak
                               };
                temolist = temolist.OrderByDescending(x => x.AddTime);
                return Json(new { ret = "ok", temp = temolist, fristtp = dj  }, JsonRequestBehavior.AllowGet);
            }

        }
        //删除创建的名称规格信息
        public ActionResult DelCPGG()
        {
            var djid = Convert.ToInt64(Request["TextNameID"]);
            var id = Convert.ToInt64(Request["id"]);

            var tem = T_CanPanService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (tem.T_SczzDanju.ZhuangTai == 1)
            {
                return Json(new {  ret = "" ,mes="不可删除已审核信息！"}, JsonRequestBehavior.AllowGet);
            }
            tem.DEL = 1;

            if (T_CanPanService.EditEntity(tem))
            {
                var temp = HuoquXLS(djid);
                return Json(new { temp = temp, ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }

        }
        public IQueryable HuoquXLS(long id)
        {
           
            var temps = T_CanPanService.LoadEntities(x => x.DEL == 0 && x.AddUserID == LoginUser.ID && x.SczzDanjuID == id);
            if (LoginUser.QuXian > 0)
            { temps = T_CanPanService.LoadEntities(x => x.DEL == 0 && x.SczzDanjuID == id); }
            
            var temp = from a in temps
                       select new
                       {
                           ID = a.ID,
                           Text = a.T_SczzItem.Text,
                           Cpname = a.Cpname,
                           CpShuliang = a.CpShuliang,
                           ImageInt = a.ImageInt,
                           AddTime = a.AddTime,
                           OverTime = a.OverTime,
                           Bak = a.Bak
                       };
            temp = temp.OrderByDescending(x => x.AddTime);
            return temp;
        }

        //删除单据显示信息 丢到垃圾回收站
        public ActionResult DelDanjiu()
        {

            var id = Convert.ToInt64(Request["delid"]);
            var edittemp = T_SczzDanjuService.LoadEntities(x => x.ID == id && x.AddUser == LoginUser.ID).FirstOrDefault();
            if (edittemp.ZhuangTai != 0)
            {
                return Json(new { ret = "no", tmp = "已经审核信息不可删除！" }, JsonRequestBehavior.AllowGet);
            }
            edittemp.del = 1;

            if (T_SczzDanjuService.EditEntity(edittemp))
            {
                return Json(new { ret = "ok", tmp = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            { return Json(new { ret = "no", tmp = "操作未成功！" }, JsonRequestBehavior.AllowGet); }

        }
        //审核操作方法
        public ActionResult ShenHe()
        {
            var id = int.Parse(Request["shid"]);

            var temp = T_SczzDanjuService.LoadEntities(x => x.ID == id&&x.del==0).FirstOrDefault();
            if (temp == null)
            {
                return Json(new { ret = "no", tmp = "数据库无你要审核的信息，数据ID错误" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (LoginUser.QuXian == 0)
                {
                    #region MyRegion
                    if (temp.ShenChanShenHe != null)
                    {
                        return Json(new { ret = "no", tmp = "已审核！不可重复审核！" }, JsonRequestBehavior.AllowGet);
                    }
                    temp.ShenChanShenHe = LoginUser.ID;
                    temp.ShenChanShenHetime = MvcApplication.GetT_time();
                    //判断 所有人员是否已经审核
                    if (ShiFuAllShenhe(temp))
                    {
                        temp.ZhuangTai = 2;
                    }

                    if (T_SczzDanjuService.EditEntity(temp))
                    {
                        return Json(new { ret = "ok", tmp = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { ret = "no", tmp = "审核失败" }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                } else
                if (LoginUser.QuXian == 1)
                {
                    #region MyRegion
                    if (temp.Jhzdr != null)
                    {
                        return Json(new { ret = "no", tmp = "已审核！不可重复审核！" }, JsonRequestBehavior.AllowGet);
                    }
                    if (temp.T_CanPan.Where(x => x.DEL == 0).All(x => x.OverTime != null))
                    {
                        temp.Jhzdr = LoginUser.ID;

                        temp.JhzdrTime = MvcApplication.GetT_time();
                        temp.TextNameID = 2;
                        temp.ZhuangTai = 1;
                        if (T_SczzDanjuService.EditEntity(temp))
                        {
                            return Json(new { ret = "ok", tmp = "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { ret = "no", tmp = "审核失败" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { ret = "no", tmp = "完工时间必须全部添加才可审核！" }, JsonRequestBehavior.AllowGet);
                    }

                    #endregion
                }
                else if (LoginUser.QuXian >1)
                {
                    if (LoginUser.QuXian == 3)
                    {
                        if (temp.UpShenHe != null)
                        {
                            return Json(new { ret = "no", tmp = "已审核！不可重复审核！" }, JsonRequestBehavior.AllowGet);
                        }
                        temp.UpShenHe = LoginUser.ID;
                        temp.UPshenhetime = MvcApplication.GetT_time();
                        //判断 所有人员是否已经审核
                        if (ShiFuAllShenhe(temp))
                        {
                            temp.ZhuangTai = 2;
                        }
                        if (T_SczzDanjuService.EditEntity(temp))
                        {
                            return Json(new { ret = "ok", tmp = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if(LoginUser.QuXian == 4)
                    {
                        if (temp.CheJianShenHe != null)
                        {
                            return Json(new { ret = "no", tmp = "已审核！不可重复审核！" }, JsonRequestBehavior.AllowGet);
                        }
                        temp.CheJianShenHe = LoginUser.ID;
                        temp.CheJianShenHetime = MvcApplication.GetT_time();
                        //判断 所有人员是否已经审核
                        if (ShiFuAllShenhe(temp))
                        {
                            temp.ZhuangTai = 2;
                        }
                        if (T_SczzDanjuService.EditEntity(temp))
                        {
                            return Json(new { ret = "ok", tmp = "" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    
                }
                return Json(new { ret = "no", tmp = "审核失败" }, JsonRequestBehavior.AllowGet);
            }

            
        }

        //添加完工时间审核
        public ActionResult OverTimes()
        {
            var id = Convert.ToInt64(Request["id"]);
            var times = Convert.ToDateTime(Request["OverTime"]);
            var ThisCanpin= T_CanPanService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (ThisCanpin == null)
            {
                return Json(new { ret = "未找到要修改的选项！" }, JsonRequestBehavior.AllowGet);
            }
            if (ThisCanpin.T_SczzDanju.ZhuangTai == 1)
            {
                return Json(new { ret = "信息已审核完毕不可更改时间！" }, JsonRequestBehavior.AllowGet);
            }
            ThisCanpin.OverTime = times;
            ThisCanpin.OverUserID = LoginUser.ID;
            if (T_CanPanService.EditEntity(ThisCanpin))
            {
                var temp = HuoquXLS((long)ThisCanpin.SczzDanjuID);
                return Json(new { ret = "ok" ,temp=temp}, JsonRequestBehavior.AllowGet);
            }
            else
            { return Json(new { ret = "修改失败！" }, JsonRequestBehavior.AllowGet); }
            
        }

        //判断是否全部审核完毕 该信息已结束
        private bool ShiFuAllShenhe(T_SczzDanju temp) {
            if (temp.UpShenHe != null && temp.ShenChanShenHe != null && temp.CheJianShenHe != null && temp.Jhzdr != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
