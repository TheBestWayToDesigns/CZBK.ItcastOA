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
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
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
                wxxsi.State = Convert.ToBoolean(Request["BeSure"]) == true ? (short)1 : (short)0;
                wxxsi.AddTime = DateTime.Now;
                if (wxxsi.State == 1)
                {
                    wxxsi.ChangeStateTime = DateTime.Now;
                }
                WXXScoreInfoService.AddEntity(wxxsi);
                return Json(new { ret = "ok", msg = "评分成功" }, JsonRequestBehavior.AllowGet);
            } else
            {
                long id = Convert.ToInt64(Request["id"]);
                var temp = WXXScoreInfoService.LoadEntities(x => x.ID == id).FirstOrDefault();
                if(temp != null)
                {
                    temp.Score = Convert.ToSingle(Request["Score"]);
                    temp.Score = Convert.ToSingle(Request["Score"]);
                    temp.State = Convert.ToBoolean(Request["BeSure"]) == true ? (short)1 : (short)0;
                    temp.AddTime = DateTime.Now;
                    if (temp.State == 1)
                    {
                        temp.ChangeStateTime = DateTime.Now;
                    }
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
        //获取分数纪录信息
        public ActionResult GetScoreInfo()
        {
            var state = Convert.ToInt16(Request["State"]);
            var temp = WXXScoreInfoService.LoadEntities(x => x.State == state).DefaultIfEmpty().ToList();
            if(temp != null &&temp[0] != null)
            {
                return Json(new { ret = "ok", msg = "确认成功" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg = "数据库中无数据！" }, JsonRequestBehavior.AllowGet);
        }


        #region 微信后台管理员工具（评分权限）
        //获取评分人员1对1信息
        public ActionResult GetScoreUserInfo()
        {
            //判断是否为搜索
            var serchText = Request["serchText"] == "" || Request["serchText"] == null || Request["serchText"] == "null" ? "0" : Request["serchText"];
            if (serchText != "0")
            {
                var data = WXXScoreUserService.LoadEntities(x => x.UserInfo.PerSonName.Contains(serchText)).DefaultIfEmpty().ToList();
                if (data != null && data[0] != null)
                {
                    var remp = from a in data
                               select new
                               {
                                   ID = a.ID,
                                   Name = a.UserInfo.PerSonName,
                                   Username = a.UserInfo1.PerSonName
                               };
                    return Json(new { ret = "ok", rows = remp }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
            var temp = WXXScoreUserService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                var rtmp = from a in temp
                           select new
                           {
                               ID = a.ID,
                               Name = a.UserInfo.PerSonName,
                               Username = a.UserInfo1.PerSonName
                           };
                return Json(new { ret = "ok", rows = rtmp }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg = "数据库表中无数据" }, JsonRequestBehavior.AllowGet);
        }
        //添加评分人员1对1信息
        public ActionResult AddScoreUserInfo()
        {
            WXXScoreUser wxxsu = new WXXScoreUser();
            wxxsu.UID = Convert.ToInt32(Request["uid"]);
            wxxsu.CanGiveScoreUserID = Convert.ToInt32(Request["downUid"]);
            WXXScoreUserService.AddEntity(wxxsu);
            return Json(new { ret = "ok", msg = "添加成功" }, JsonRequestBehavior.AllowGet);
        }
        //删除评分人员1对1信息
        public ActionResult DelScoreUserInfo()
        {
            int id = Convert.ToInt32(Request["id"]);
            var temp = WXXScoreUserService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (temp != null)
            {
                if (WXXScoreUserService.DeleteEntity(temp))
                {
                    return Json(new { ret = "ok", msg = "删除成功" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ret = "no", msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg = "数据库中无此数据，请仔细核对" }, JsonRequestBehavior.AllowGet);
        }
        //获取所有部门或部门下属成员
        public ActionResult GetAllBuMenORUser()
        {
            if(Request["BMorUser"] == "BM")
            {
                var temp = BumenInfoSetService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
                if (temp != null && temp[0] != null)
                {
                    var rtmp = from a in temp
                               select new
                               {
                                   ID =a.ID,
                                   MyText = a.Name
                               };
                    return Json(new { ret = "ok", rows = rtmp }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ret = "no", msg = "数据表中无数据" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var bmid = Convert.ToInt32(Request["BMid"]);
                var temp = UserInfoService.LoadEntities(x => x.BuMenID == bmid).DefaultIfEmpty().ToList();
                if (temp != null && temp[0] != null)
                {
                    var rtmp = from a in temp
                               select new
                               {
                                   ID = a.ID,
                                   MyText = a.PerSonName
                               };
                    return Json(new { ret = "ok", rows = rtmp }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { ret = "no", msg = "数据表中无数据" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
