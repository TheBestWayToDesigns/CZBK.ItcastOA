using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class BillPrintController : BaseController
    {
        //
        // GET: /BillPrint/
        IBLL.IT_BaoXiaoBillService T_BaoXiaoBillService { get; set; }
        IBLL.IT_BaoxiaoItemsService T_BaoxiaoItemsService { get; set; }
        IBLL.IT_JieKuanBillService T_JieKuanBillService { get; set; }
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; } 

        public ActionResult Index()
        {

            return View();
        }
        //获取借款单信息
        public ActionResult GETjiekuandata()
        {
            int delflg = Request["delflg"] == null ? 0 : int.Parse(Request["delflg"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int totalCount = 0;
            var Adata = T_JieKuanBillService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.Del == 0 && x.UserAdd == LoginUser.ID, x => x.AddTime, false);

            var temp = from a in Adata
                       select new
                       {
                           a.ID,
                           a.AddTime,
                           a.Bak,
                           a.BillTime,
                           Bumen = a.BumenInfoSet.Name,
                           a.CardNumber,
                           a.Del,
                           a.JieKuanMoney,
                           a.JieKuanPerson,
                           a.JieKuanYuanYin,
                           a.OpenHang,
                           a.SkdwName,
                           UPname = a.UserInfo.PerSonName
                       };

            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        //添加或修改
        public ActionResult AddOrEditJiekuan(T_JieKuanBill jkb)
        {
            if (jkb.ID <= 0)
            {
                jkb.AddTime = MvcApplication.GetT_time();
                jkb.Del = 0;
                jkb.UserAdd = LoginUser.ID;
                T_JieKuanBillService.AddEntity(jkb);
                return Json(new { ret = "ok", msg = "添加成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (T_JieKuanBillService.EditEntity(jkb))
                {
                    return Json(new { ret = "ok", msg = "修改成功！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "ok", msg = "修改出错，联系管理员！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //作废单据
        public ActionResult DELJiekuan()
        {
            long id = Request["id"] == null ? 0 : Convert.ToInt64(Request["id"]);
            if (id == 0)
            {
                return Json(new { ret = "ok", msg = "ID不可为空！" }, JsonRequestBehavior.AllowGet);
            }
            T_JieKuanBill Jkb = T_JieKuanBillService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (Jkb == null)
            {
                return Json(new { ret = "ok", msg = "数据库中未找到该ID，联系管理员！" }, JsonRequestBehavior.AllowGet);
            }
            Jkb.Del = 1;
            if (T_JieKuanBillService.EditEntity(Jkb))
            {
                return Json(new { ret = "ok", msg = "修改成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "ok", msg = "修改出错，联系管理员！" }, JsonRequestBehavior.AllowGet);
            }

        }

        // 获取报销数据
        public ActionResult GETBaoXiaodata()
        {
            int delflg = Request["delflg"] == null ? 0 : int.Parse(Request["delflg"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int totalCount = 0;
            var Adata = T_BaoXiaoBillService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.Del == 0 && x.AddUserID == LoginUser.ID, x => x.Addtime, false);

            var temp = from a in Adata
                       select new
                       {
                           a.ID,
                           a.BaoxianDanwei,
                           a.Addtime,
                           a.IntTime,
                           Icount = a.T_BaoxiaoItems.Count,
                           a.Del
                       };

            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);

        }
        //添加或修改报销数据
        public ActionResult AddOrEditBxBill(T_BaoXiaoBill jkb)
        {
            if (jkb.ID <= 0)
            {
                jkb.Addtime = MvcApplication.GetT_time();
                jkb.Del = 0;
                jkb.AddUserID = LoginUser.ID;
                T_BaoXiaoBillService.AddEntity(jkb);
                return Json(new { ret = "ok", msg = "添加成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var temp = T_BaoXiaoBillService.LoadEntities(x => x.ID == jkb.ID).FirstOrDefault();
                temp.Del = 1;
                if (T_BaoXiaoBillService.EditEntity(temp))
                {
                    return Json(new { ret = "ok", msg = "修改成功！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "ok", msg = "修改出错，联系管理员！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //获取部门
        public ActionResult GetBumen()
        {
            var tvm = BumenInfoSetService.LoadEntities(x => x.Gushu < 99 && x.DelFlag == 0).DefaultIfEmpty();
            var temp = from a in tvm
                       select new
                       {
                           ID = a.ID,
                           MyTexts = a.Name
                       };
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        // 获取报表列数据
        public ActionResult GETBaoXiaolist()
        {
            int delflg = Request["delflg"] == null ? 0 : int.Parse(Request["delflg"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int totalCount = 0;
            int BillId = Request["BillId"] == null ? 0 : int.Parse(Request["BillId"]);
            var Adata = T_BaoxiaoItemsService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.Del == 0 && x.BaoXiaoID == BillId, x => x.AddTime, false);

            var temp = from a in Adata
                       select new
                       {
                           a.ID,
                           a.BaoXiaoName,
                           a.DanJiuInt,
                           a.BaoXiaoMoeny,
                           a.AddTime,
                           a.Del
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);

        }


        //添加或修改报销数据列表
        public ActionResult AddOrEditBxlistBill(T_BaoxiaoItems jkb)
        {
            if (jkb.ID <= 0)
            {
                if (T_BaoxiaoItemsService.LoadEntities(x => x.BaoXiaoID == jkb.BaoXiaoID && x.Del == 0).Count() >= 5)
                {
                    return Json(new { ret = "no", msg = "单据条数上线不可添加！" }, JsonRequestBehavior.AllowGet);
                }
                jkb.AddTime = MvcApplication.GetT_time();
                int index = jkb.BaoXiaoName.IndexOf("(");
                jkb.BaoXiaoName = jkb.BaoXiaoName.Remove(index);
                jkb.Del = 0;
                T_BaoxiaoItemsService.AddEntity(jkb);
                return Json(new { ret = "ok", msg = "添加成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (T_BaoxiaoItemsService.EditEntity(jkb))
                {
                    return Json(new { ret = "ok", msg = "修改成功！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "修改出错，联系管理员！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //软删除报销凭证
        public ActionResult DelBxlistbill()
        {
            var id = Request["DelID"] == null ? 0 : Convert.ToInt32(Request["DelID"]);
            if (id == 0)
            {
                return Json(new { msg = "ID错误！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var temp = T_BaoxiaoItemsService.LoadEntities(x => x.ID == id).FirstOrDefault();
                if (Convert.ToDateTime(temp.AddTime).ToString("yyyy-MM-dd") != MvcApplication.GetT_time().ToString("yyyy-MM-dd"))
                {
                    return Json(new { msg = "过期删除，只可删除当天创建信息！" }, JsonRequestBehavior.AllowGet);
                }
                if (temp == null)
                {
                    return Json(new { msg = "查询的数据为空！" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    temp.Del = 1;
                    if (T_BaoxiaoItemsService.EditEntity(temp))
                    {
                        return Json(new { ret = "ok", msg = "修改成功！" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = "在修改中出错！" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

        }

        //获取所有报销内容
        public ActionResult GetBaoXiaoall()
        {
            //获取所有本用户创建过的报销内容
            int uid = LoginUser.ID;
            var tempBXB = T_BaoXiaoBillService.LoadEntities(x => x.AddUserID == uid).DefaultIfEmpty().ToList();
            List<T_BaoxiaoItems> bilist = new List<T_BaoxiaoItems>();
            foreach (var a in tempBXB)
            {
                var tempBXI = T_BaoxiaoItemsService.LoadEntities(x => x.BaoXiaoID == a.ID).DefaultIfEmpty().ToList();
                foreach (var d in tempBXI)
                {
                    if (d == null || d.BaoXiaoName == null)
                    {
                        continue;
                    }
                    else
                    {
                        bilist.Add(d);
                    }
                }
            }
            List<T_BaoxiaoItems> sb1 = new List<T_BaoxiaoItems>();
            if (bilist.Count <= 1)
            {
                sb1.AddRange(bilist);
            }
            else {
                sb1 = bilist.Where((x, i) => bilist.FindIndex(z => z.BaoXiaoName == x.BaoXiaoName) == i).ToList();
            }
            Random rd = new Random();
            List<BaoXiaoLR> list = new List<BaoXiaoLR>();
            foreach (var c in sb1)
            {
                BaoXiaoLR bxlr = new BaoXiaoLR();
                bxlr.ID = c.ID + rd.Next(100,10000);
                bxlr.Text = c.BaoXiaoName;
                list.Add(bxlr);
            }
            //获取所有本部门创建过得借款单内容
            var tempJKB = T_JieKuanBillService.LoadEntities(x => x.UserAdd == uid).DefaultIfEmpty().ToList();
            List<T_JieKuanBill> sb2 = new List<T_JieKuanBill>();
            if (tempJKB.Count <= 1)
            {
                sb2.AddRange(tempJKB);
            }
            else
            {
                sb2 = tempJKB.Where((x, i) => tempJKB.FindIndex(z => z.SkdwName == x.SkdwName) == i).ToList();
            }
            List<BaoXiaoLR> JKBLR = new List<BaoXiaoLR>();
            foreach (var d in sb2)
            {
                if (d == null || d.SkdwName == null)
                {
                    continue;
                }
                else
                {
                    BaoXiaoLR jklr = new BaoXiaoLR();
                    jklr.ID = d.ID;
                    jklr.Text = d.SkdwName+"($)";
                    JKBLR.Add(jklr);
                }
            }
            list.AddRange(JKBLR);
            //users.Where((x, i) => users.FindIndex(z => z.name == x.name) == i).ToList();拉姆达表达式去重
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取借款内容的借款金额
        public ActionResult GetJieKuanMoney()
        {
            long jkid = Convert.ToInt64(Request["id"]);
            var temp = T_JieKuanBillService.LoadEntities(x => x.ID == jkid).FirstOrDefault();
            if (temp == null || temp.JieKuanMoney == null)
            {
                return Json(new { ret = "no"},JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "ok",jkmoney = temp.JieKuanMoney},JsonRequestBehavior.AllowGet);
        }

        //获取借款部门下属所有成员
        public ActionResult GetJKPerson()
        {
            int bmid = Convert.ToInt32(Request["BMID"]);
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == bmid).DefaultIfEmpty().ToList();
            List<JieKuanPerson> list = new List<JieKuanPerson>();
            foreach(var a in temp)
            {
                if (a == null || a.PerSonName == null)
                {
                    continue;
                }
                JieKuanPerson jkr = new JieKuanPerson();
                jkr.ID = a.ID;
                jkr.Name = a.PerSonName;
                list.Add(jkr);
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        //获取借款部门下所有收款单位
        public ActionResult GetBuMenAllSKDW()
        {
            int bmid = Convert.ToInt32(Request["BMID"]);
            var temp = T_JieKuanBillService.LoadEntities(x => x.BuMenid == bmid).DefaultIfEmpty().ToList();
            List<T_JieKuanBill> sb = new List<T_JieKuanBill>();
            if (temp.Count <= 1)
            {
                sb.AddRange(temp);
            }
            else
            {
                var a = temp.Where((x, i) => temp.FindIndex(z => z.SkdwName == x.SkdwName) == i).ToList();
                sb.AddRange(a);
            }
            
            List<BaoXiaoLR> list = new List<BaoXiaoLR>();
            foreach(var a in sb)
            {
                if (a == null || a.SkdwName == null)
                {
                    continue;
                }
                BaoXiaoLR bxlr = new BaoXiaoLR();
                bxlr.ID = a.ID;
                bxlr.Text = a.SkdwName;
                list.Add(bxlr);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取当前收款单位所有开户行
        public ActionResult GetAllFaHuoBak()
        {
            string skdw = Request["SKDW"];
            var temp = T_JieKuanBillService.LoadEntities(x => x.SkdwName == skdw).DefaultIfEmpty().ToList();
            List<T_JieKuanBill> sb = new List<T_JieKuanBill>();
            if (temp.Count <= 1)
            {
                sb.AddRange(temp);
            }
            else
            {
                var a = temp.Where((x, i) => temp.FindIndex(z => z.OpenHang == x.OpenHang) == i).ToList();
                sb.AddRange(a);
            }
            List<BaoXiaoLR> list = new List<BaoXiaoLR>();
            foreach (var a in sb)
            {
                if (a == null || a.OpenHang == null)
                {
                    continue;
                }
                BaoXiaoLR bxlr = new BaoXiaoLR();
                bxlr.ID = a.ID;
                bxlr.Text = a.OpenHang;
                list.Add(bxlr);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取当前开户银行的所有卡号
        public ActionResult GetAllFaHuoBakCardNumber()
        {
            string fhb = Request["FHB"];
            var cardNum = T_JieKuanBillService.LoadEntities(x => x.OpenHang == fhb).DefaultIfEmpty().ToList();
            List<T_JieKuanBill> sb = new List<T_JieKuanBill>();
            if (cardNum.Count <= 1)
            {
                sb.AddRange(cardNum);
            }
            else
            {
                var a = cardNum.Where((x, i) => cardNum.FindIndex(z => z.CardNumber == x.CardNumber) == i).ToList();
                sb.AddRange(a);
            }
            List<BaoXiaoLR> list = new List<BaoXiaoLR>();
            foreach (var a in sb)
            {
                if (a == null || a.CardNumber == null)
                {
                    continue;
                }
                BaoXiaoLR bxlr = new BaoXiaoLR();
                bxlr.ID = a.ID;
                bxlr.Text = a.CardNumber;
                list.Add(bxlr);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //返回借款人id
        public ActionResult ReturnUserID()
        {
            int bm = Convert.ToInt32(Request["BuMenID"]);
            string jkp = Request["personName"];
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == bm && x.PerSonName == jkp).FirstOrDefault();
            if (temp != null)
            {
                return Json(new {ret = "ok",uid = temp.ID },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "no", uid = "在该部门下查无此人！" }, JsonRequestBehavior.AllowGet);
            }
        }

        //报销内容类
        public class BaoXiaoLR
        {
            public long ID { get; set; }
            public string Text { get; set; }
        }

        //借款人类
        public class JieKuanPerson
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

    }
}
