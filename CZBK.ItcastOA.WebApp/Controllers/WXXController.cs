using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class WXXController : BaseController
    {
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IWXXScoreInfoService WXXScoreInfoService { get; set; }
        IBLL.IWXXScoreUserService WXXScoreUserService { get; set; }
        //
        // GET: /WXX/

        public ActionResult Index()
        {
            return View();
        }

        //获取可评分人员数据
        public ActionResult GetCanPFInfo()
        {
            var temp = WXXScoreUserService.LoadEntities(x => x.UID == LoginUser.ID).DefaultIfEmpty().ToList();
            if(temp != null &&temp[0] != null)
            {
                var rtmp = from a in temp
                           select new
                           {
                               ID = a.CanGiveScoreUserID,
                               Name = a.UserInfo1.PerSonName
                           };
                return Json(new { ret = "ok",rows = rtmp},JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg="数据库无数据！" }, JsonRequestBehavior.AllowGet);
        }
        //添加评分记录
        public ActionResult AddScoreInfo()
        {
            if (Request["BJ"] == "add")
            {
                WXXScoreInfo wxxsi = new WXXScoreInfo();
                wxxsi.GiveScoreUserID = LoginUser.ID;
                wxxsi.BeiGiveScoreUserID = Convert.ToInt32(Request["uid"]);
                wxxsi.Score = Convert.ToSingle(Request["Score"]);
                wxxsi.State = 0;
                wxxsi.AddTime = DateTime.Now;
                WXXScoreInfoService.AddEntity(wxxsi);
                return Json(new { ret = "ok", msg = "评分成功" }, JsonRequestBehavior.AllowGet);
            } else
            {
                long id = Convert.ToInt64(Request["id"]);
                var temp = WXXScoreInfoService.LoadEntities(x => x.ID == id).FirstOrDefault();
                if(temp != null)
                {
                    temp.Score = Convert.ToSingle(Request["Score"]);
                    if (WXXScoreInfoService.EditEntity(temp))
                    {
                        return Json(new { ret = "ok", msg = "修改评分成功" }, JsonRequestBehavior.AllowGet);
                    }else
                    {
                        return Json(new { ret = "no", msg = "修改失败" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { ret = "no", msg = "数据库中无此数据" }, JsonRequestBehavior.AllowGet);
            }
        }
        //确认当前评分
        public ActionResult ConfirmThisScore()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = WXXScoreInfoService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if(temp != null)
            {
                temp.State = 1;
                temp.ChangeStateTime = DateTime.Now;
                if (WXXScoreInfoService.EditEntity(temp))
                {
                    return Json(new { ret = "ok", msg = "确认成功" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ret = "no", msg = "确认失败" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg = "数据库中无此数据" }, JsonRequestBehavior.AllowGet);
        }
    }
}
