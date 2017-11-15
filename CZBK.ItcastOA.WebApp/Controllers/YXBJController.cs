using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class YXBJController : BaseController
    {
        //
        // GET: /YXBJ/
        IBLL.IYXB_BaojiaService YXB_BaojiaService { get; set; }
        IBLL.IYXB_Kh_listService YXB_Kh_listService { get; set; }
        IBLL.IT_BaoJiaToPService T_BaoJiaToPService { get; set; }
        IBLL.ISysFieldService SysFieldService { get; set; }
        IBLL.IT_ChanPinNameService T_ChanPinNameService { get; set; }
        short delFlag = (short)DelFlagEnum.Normarl;
        public ActionResult Index()
        {
            return View();
        }
        #region 添加客户信息
        public ActionResult AddYXB_KH_list()
        {
            YXB_Kh_list khlist = new YXB_Kh_list();
            
            khlist.KHname = Request["KHname"];
            khlist.KHperson = Request["KHperson"];
            khlist.KHzhiwu = Request["KHzhiwu"];
            khlist.KHphoto = Request["KHphoto"];
            khlist.KHfaren = Request["KHfaren"];
            khlist.DelFlag = 0;
            khlist.NewTime = DateTime.Now;
            khlist.AddUser = LoginUser.ID;
            khlist.All_I = 0;
            var isnull = YXB_Kh_listService.LoadEntities(x => x.KHfaren == khlist.KHfaren).FirstOrDefault();
            if (isnull == null)
            {
                var isdistic = YXB_Kh_listService.LoadEntities(x => x.KHname == khlist.KHname).FirstOrDefault();
                if (isdistic == null)
                {
                    YXB_Kh_listService.AddEntity(khlist);
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("errer", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json("Isdistict", JsonRequestBehavior.AllowGet);

            }

        }
        #endregion


        public ActionResult GetList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;
           
            var actioninfolist = YXB_Kh_listService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, a => a.DelFlag == delFlag&&a.AddUser==LoginUser.ID, a => a.id, false);
            var temp = from a in actioninfolist
                       select new
                       {
                           ID = a.id,
                           KHname = a.KHname,
                           KHComname = a.KHComname,
                           KHperson = a.KHperson,
                           AddTime=a.NewTime,
                           KHfaren=a.KHfaren                           
                       };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetKhList()
        {
            var aifo = YXB_Kh_listService.LoadEntities(x => x.DelFlag == delFlag && x.AddUser == LoginUser.ID);
            var temp = from a in aifo
                       select new
                       {
                           ID = a.id,
                           KHname = a.KHname,
                           KHComname = a.KHComname
                       };
            return Json(new { rows = temp}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddBaoJiaoTOP()
        {
            T_BaoJiaToP tbop = new T_BaoJiaToP();
            string ret = "load";
            long ThisAddId = 0;
            if (Request["editID"] != null)
            {
                if (Request["editID"].Trim().Length <= 0)
                {
                    ret = addBaoJiaTOP(tbop, ref ThisAddId);
                }
                else
                {
                    //修改编辑数据

                }
            }
            else
            {
                ret = addBaoJiaTOP(tbop, ref ThisAddId);
            }

            return Json(new { ret = ret,ID= ThisAddId }, JsonRequestBehavior.AllowGet);
        }

        private string addBaoJiaTOP(T_BaoJiaToP tbop, ref long ThisAddId)
        {
            string ret;
            tbop.DelFlag = delFlag;
            tbop.AddTime = MvcApplication.GetT_time();
            string Ttime = Request["GhTime"];
            var tsplit = Ttime.Split('/');
            tbop.GhTime = new DateTime(int.Parse(tsplit[2]), int.Parse(tsplit[1]), int.Parse(tsplit[0]));
            tbop.Kh_List_id = Request["khidselect"] != null ? int.Parse(Request["khidselect"]) : 0;
            tbop.DaiBanYunShu = Request["DaiBanYunShu"];
            tbop.HeTongQianDing = Request["HeTongQianDing"];
            tbop.JieShuanFanShi = Request["JieShuanFanShi"];
            tbop.JiShuYaoQiu = Request["JiShuYaoQiu"];
            tbop.KHComname = Request["KHComname"];
            string Addess = Request["Province"].ToString() + "," + Request["City"].ToString() + "," + Request["Village"].ToString();
            tbop.Addess = Addess;
            try
            {
                T_BaoJiaToPService.AddEntity(tbop);
                var ThisAddId_list = YXB_Kh_listService.LoadEntities(x => x.id == tbop.Kh_List_id).FirstOrDefault();
                ThisAddId = ThisAddId_list.T_BaoJiaToP.Max(x => x.id);
                ret = "ok";
            }
            catch (Exception ex)
            {
                ret = ex.ToString();
            }

            return ret;
        }

        //添加产品详细数据
        public ActionResult AddBaoJiaoOne()
        {

            YXB_Baojia bj = new YXB_Baojia();
            bj.AddTime = MvcApplication.GetT_time();
            bj.DelFlag = delFlag;
            bj.ZhuangTai = 0;
            bj.CPname =Convert.ToInt64( Request["CPname"]);
            bj.CPXingHao = Convert.ToInt64(Request["CPXingHao"]);
            bj.CPShuLiang = int.Parse(Request["CPShuLiang"]);
            bj.BaoJiaTop_id = Convert.ToInt64(Request["editID"]);
            bj.WIN = 0;
            YXB_BaojiaService.AddEntity(bj);
            Common.MemcacheHelper.Set("Allstr", Convert.ToInt64(Common.MemcacheHelper.Get("Allstr"))+1);
            return GetysbBaojia(bj.BaoJiaTop_id);
        }

        private ActionResult GetysbBaojia(long BaoJiaTop_id)
        {
            var Adata = YXB_BaojiaService.LoadEntities(x => x.BaoJiaTop_id == BaoJiaTop_id && x.DelFlag==delFlag);
            var temp = from a in Adata
                       select new
                       {
                           ID = a.id,
                           CPname = a.T_ChanPinName.MyTexts,
                           CPXingHao = a.T_ChanPinName1.MyTexts,
                           CPShuLiang = a.CPShuLiang,
                           addTime = a.AddTime,
                           CPzt=a.ZhuangTai,
                           Money=a.BaoJiaMoney
                       };
            return Json(new { rows = temp, ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetBaoJiaMoney()
        {
            var baojiatopid =  Convert.ToInt64(Request["khid"]);

            var Adata = YXB_BaojiaService.LoadEntities(x => x.BaoJiaTop_id == baojiatopid && x.DelFlag == delFlag);
            var temp = from a in Adata
                       select new
                       {
                           ID = a.id,
                           CPname = a.T_ChanPinName.MyTexts,
                           CPXingHao = a.T_ChanPinName1.MyTexts,
                           CPShuLiang = a.CPShuLiang,
                           addTime = a.AddTime,
                           CPzt = a.ZhuangTai,
                           Money = a.BaoJiaMoney
                       };
            return Json(new { rows = temp, ret = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deletebaojia() {
            var editid= Convert.ToInt64(Request["editID"]);
            var delid= Convert.ToInt64(Request["delID"]);

            YXB_Baojia ybj = YXB_BaojiaService.LoadEntities(x => x.id == delid).FirstOrDefault();      
            YXB_BaojiaService.DeleteEntity(ybj);
            return GetysbBaojia(editid);
        }
        //显示客户信息
        public ActionResult GetKhDataId()
        {
            var khid = Convert.ToInt64(Request["khid"]);
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount=0;
            var Adata = T_BaoJiaToPService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.Kh_List_id == khid && x.DelFlag == delFlag,x=>x.AddTime,false);
       
            var temp = from a in Adata
                       select new
                       {
                           ID = a.id,
                           HeTongQianDing = a.HeTongQianDing,
                           JishuYaoqiu=a.JiShuYaoQiu,
                           Addess = a.Addess,
                           AddTime = a.AddTime,
                           KHComname= a.KHComname
                       };
            List<RetcTEMP> ts = new List<RetcTEMP>();
            foreach (var TP in temp)
            {
                RetcTEMP t = new RetcTEMP();
                t.ID = TP.ID;
                t.JishuYaoqiu = TP.JishuYaoqiu;
                t.AddTime = TP.AddTime;
                t.Seaddess=ArrF(TP.Addess);
                t.KHComname = TP.KHComname;
                ts.Add(t);
            }
            
            return Json(new { rows = ts, ret = "ok", total= totalCount }, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetAddress() {

            //var result = sysfileBll.GetSysFieldParentId(parentiD, MyColums);
            // return Json(result, JsonRequestBehavior.AllowGet);
            int Parentid = Request["parentiD"] != null ? Request["parentiD"]== "undefined" ? 0 : Request["parentiD"].Length<=0?0: Convert.ToInt32(Request["parentiD"]) : 0;
            string MyColums = Request["MyColums"].ToString();

            var SysField = SysFieldService.LoadEntities(x => x.ParentId == Parentid && x.MyColums==MyColums).DefaultIfEmpty();
            var temp = from a in SysField
                       select new
                       {
                           ID = a.ID,
                           MyTexts = a.MyTexts,
                           MyColums = a.MyColums
                       };
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        //读取产品名称与型号列表
        public ActionResult GetChanPinList() {
            
            var tmc = T_ChanPinNameService.LoadEntities(x=>x.Del==0);
            var tempName = from a in tmc
                           where a.MyColums == "CPname"
                           select new
                           {
                               ID=a.ID,
                               MyText=a.MyTexts,
                               MyColums=a.MyColums
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
            return Json(new { tempName = tempName, tempXingH= tempXingH, ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //获取含税列表
        public ActionResult GetHashui() {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
    public class RetcTEMP {
        public long ID { get; set; }
        public string HeTongQianDing { get; set; }
        public string JishuYaoqiu { get; set; }
        public string Seaddess { get; set; }
        public DateTime AddTime { get; set; }
        public string KHComname { get; set; }
    }
}
