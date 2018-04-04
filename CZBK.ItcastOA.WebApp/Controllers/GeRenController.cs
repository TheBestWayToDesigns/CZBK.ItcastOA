using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class GeRenController : BaseController
    {
        //
        // GET: /GeRen/
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IWXXPhoneNumService WXXPhoneNumService { get; set; }
        public ActionResult Index()
        {
            ViewData["userName"] = LoginUser.UName;
            ViewData["ID"] = LoginUser.ID;
            return View();
        }
        public ActionResult edituser()
        {
            var khid = Convert.ToInt64(Request["id"]);
            var Pass = Request["Pass"];
            var Sort = Request["Sort"] ;
            var u = UserInfoService.LoadEntities(x => x.ID == khid).FirstOrDefault();
            if (Sort != null)
            {
                u.PerSonName = Sort;
            }                        
            u.UPwd = Model.Enum.AddMD5.GaddMD5(Pass);
            UserInfoService.EditEntity(u);
            return Json(new {  ret = "ok"}, JsonRequestBehavior.AllowGet);
        }

        #region 微信用
        //修改公司大小号
        public ActionResult editUserPhoneXH()
        {
            var XiaoHao = Request["XiaoHao"] == "" || Request["XiaoHao"] == "null" || Request["XiaoHao"] == null ? 0 :Convert.ToInt32(Request["XiaoHao"]);
            var DaHao = Request["DaHao"] == "" || Request["DaHao"] == "null" || Request["DaHao"] == null ? 0 : Convert.ToInt64(Request["DaHao"]);
            var Name = Request["Name"] == "" || Request["Name"] == "null" || Request["Name"] == null ? "0" : Request["Name"];
            var BuMen = Request["BuMen"] == "" || Request["BuMen"] == "null" || Request["BuMen"] == null ? "0" : Request["BuMen"];
            var id = Convert.ToInt32(Request["id"]);
            var temp = WXXPhoneNumService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if(temp != null)
            {
                if(XiaoHao != 0)
                {
                    temp.XiaoHao = XiaoHao;
                }
                if (DaHao != 0)
                {
                    temp.DaHao = DaHao;
                }
                if(Name != "0")
                {
                    temp.Name = Name;
                } 
                if(BuMen != "0")
                {
                    temp.BuMen = BuMen;
                }
            }
            if (WXXPhoneNumService.EditEntity(temp))
            {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取所有人员号码信息
        public ActionResult GetAllUserXH()
        {
            //判断是否为搜索
            var serchText = Request["serchText"] == "" || Request["serchText"] == null || Request["serchText"] =="null"?"0": Request["serchText"];
            if(serchText != "0")
            {
                var data = WXXPhoneNumService.LoadEntities(x => x.Name.Contains(serchText)).DefaultIfEmpty().ToList();
                if(data != null && data[0] != null)
                {
                    var remp = from a in data
                               select new
                               {
                                   ID = a.ID,
                                   Username = a.Name,
                                   XiaoHao = a.XiaoHao,
                                   DaHao = a.DaHao,
                                   BuMen = a.BuMen
                               };
                    return Json(new { ret = "ok", rows = remp }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
            //搜索所有
            var temp = WXXPhoneNumService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            if(temp != null && temp[0] != null)
            {
                var remp = from a in temp
                           select new
                           {
                               ID = a.ID,
                               Username = a.Name,
                               XiaoHao = a.XiaoHao,
                               DaHao = a.DaHao,
                               BuMen = a.BuMen
                           };
                return Json(new { ret = "ok", rows = remp }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
        }
        //添加人员号码信息
        public ActionResult AddUserPhoneNum()
        {
            var XiaoHao = Request["XiaoHao"] == "" ? 0 : Convert.ToInt32(Request["XiaoHao"]);
            var DaHao = Request["DaHao"] == "" ? 0 : Convert.ToInt64(Request["DaHao"]);
            var Name = Request["name"] == "" ? "0" : Request["name"];
            var BuMen = Request["bumen"] == "" ? "0" : Request["bumen"];
            WXXPhoneNum wxpn = new WXXPhoneNum();
            if(XiaoHao != 0)
                {
                wxpn.XiaoHao = XiaoHao;
            }
            if (DaHao != 0)
            {
                wxpn.DaHao = DaHao;
            }
            if (Name != "0")
            {
                wxpn.Name = Name;
            }
            if (BuMen != "0")
            {
                wxpn.BuMen = BuMen;
            }
            WXXPhoneNumService.AddEntity(wxpn);
            return Json(new { ret = "ok", msg="添加成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
