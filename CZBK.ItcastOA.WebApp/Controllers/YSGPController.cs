using CZBK.ItcastOA.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class YSGPController : Controller
    {
        IBLL.IYSGPtopService YSGPtopService { get; set; }
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IT_BoolItemService T_BoolItemService { get; set; }
        IBLL.IYSGPmoneyService YSGPmoneyService { get; set; }
        //
        // GET: /YSGP/

        public ActionResult Index()
        {
            return View();
        }

        //获取工票头信息
        public ActionResult GetAllGPtopInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int bumenID = Request["BMID"] != null && Request["BMID"].Trim().Length > 0 ? int.Parse(Request["BMID"]) : 0;
            string uid = Request["uid"] != null && Request["uid"].Trim().Length > 0 ? Request["uid"] : "0";
            string startDate = Request["startDate"] != null && Request["startDate"].Trim().Length > 0 ? Request["startDate"] : "0";
            string endDate = Request["endDate"] != null && Request["endDate"].Trim().Length > 0 ? Request["endDate"] : "0";
            int toalcount = 0;
            var temp = new List<YSGPtop>();
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            if (bumenID == 0 && uid == "0")
            {
                temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.Del_f == 0 && x.GPState == 2, x => x.GPtime, false).DefaultIfEmpty().ToList();
            } else if (bumenID != 0 && uid == "0")
            {
                temp = YSGPtopService.LoadEntities(x => x.Del_f == 0 && x.PGbumen == bumenID && x.GPState == 2).DefaultIfEmpty().OrderBy(x => x.GPtime).ToList();
            } else if (bumenID != 0 && uid != "0")
            {
                temp = YSGPtopService.LoadEntities(x => x.Del_f == 0 && x.PGperson.Contains(uid) && x.GPState == 2).DefaultIfEmpty().OrderBy(x => x.GPtime).ToList();
            }
            if (startDate != "0" && endDate != "0")
            {
                DateTime start = Convert.ToDateTime(startDate);
                DateTime end = Convert.ToDateTime(endDate);
                temp = temp.Where(x => x.GPtime >= start && x.GPtime <= end).DefaultIfEmpty().ToList();
            }
            if (temp != null && temp[0] != null)
            {
                foreach (var a in temp)
                {
                    GPtop gt = new GPtop();
                    gt.ID = a.ID;
                    gt.GPtime = a.GPtime;
                    gt.PGbumen = a.BumenInfoSet.Name;
                    if (a.PGperson.IndexOf("|") == -1)
                    {
                        int id = Convert.ToInt32(a.PGperson);
                        var userinfo = userList.Where(x => x.ID == id).FirstOrDefault();
                        gt.PGperson = userinfo.PerSonName;
                    }
                    else
                    {
                        string userStr = "";
                        var ary = a.PGperson.Split('|');
                        foreach (var b in ary)
                        {
                            int sid = Convert.ToInt32(b.Trim());
                            var userinfo = userList.Where(x => x.ID == sid).FirstOrDefault();
                            userStr = userStr == "" ? userinfo.PerSonName : userStr + "、" + userinfo.PerSonName;
                        }
                        gt.PGperson = userStr;
                    }
                    gt.StartTime = a.StartTime;
                    gt.EndTime = a.EndTime;
                    gt.WorkTime = a.WorkTime;
                    gt.PGpersonAllWages = a.PGpersonAllWages;
                    gt.SettlementAmount = a.SettlementAmount;
                    gt.gpInfo = a.WorkInfo;
                    gt.WorkAddress = a.BumenInfoSet1.Name;
                    gt.pingJiaStateNum = a.pingjiaState;
                    gt.pingJiaState = a.pingjiaState == 0 ? "满意" : a.pingjiaState == 1 ? "一般" : "不满意";
                    rtmp.Add(gt);
                }
            }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        //微信用获取工票信息
        public ActionResult GetGPtopInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int toalcount = 0;
            var temp = new List<YSGPtop>();
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            int uid = Convert.ToInt32(Request["uid"]);
            string uidStr = Request["uid"];
            temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.Del_f == 0 && (x.AddUser == uid || x.PGperson.Contains(uidStr)), x => x.GPtime, false).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                foreach (var a in temp)
                {
                    GPtop gt = new GPtop();
                    gt.ID = a.ID;
                    gt.GPtime = a.GPtime;
                    gt.PGbumen = a.BumenInfoSet.Name;
                    if (a.PGperson.IndexOf("|") == -1)
                    {
                        int id = Convert.ToInt32(a.PGperson);
                        var userinfo = userList.Where(x => x.ID == id).FirstOrDefault();
                        gt.PGperson = userinfo.PerSonName;
                    } else
                    {
                        string userStr = "";
                        var ary = a.PGperson.Split('|');
                        foreach (var b in ary)
                        {
                            int sid = Convert.ToInt32(b.Trim());
                            var userinfo = userList.Where(x => x.ID == sid).FirstOrDefault();
                            userStr = userStr == "" ? userinfo.PerSonName : userStr + "、" + userinfo.PerSonName;
                        }
                        gt.PGperson = userStr;
                    }
                    gt.StartTime = a.StartTime;
                    gt.EndTime = a.EndTime;
                    gt.WorkTime = a.WorkTime;
                    gt.PGpersonAllWages = a.PGpersonAllWages;
                    gt.SettlementAmount = a.SettlementAmount;
                    gt.gpInfo = a.WorkInfo;
                    gt.WorkAddress = a.BumenInfoSet1.Name;
                    gt.GPState = T_BoolItemService.LoadEntities(x => x.ItemsID == 9 && x.@int == a.GPState).First().str;
                    gt.GPStateNum = a.GPState;
                    gt.pingJiaStateNum = a.pingjiaState;
                    gt.pingJiaState = a.pingjiaState == 0 ? "满意" : a.pingjiaState == 1 ? "一般" : "不满意";
                    rtmp.Add(gt);
                }
            }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        //获取用工部门待审核信息
        public ActionResult GetYGBuMenInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int toalcount = 0;
            var temp = new List<YSGPtop>();
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            int bumenID = Convert.ToInt32(Request["YGBMID"]);
            temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.WorkAddress == bumenID && x.Del_f == 0 && x.GPState == 0, x => x.GPtime, false).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                foreach (var a in temp)
                {
                    GPtop gt = new GPtop();
                    gt.ID = a.ID;
                    gt.GPtime = a.GPtime;
                    gt.PGbumen = a.BumenInfoSet.Name;
                    if (a.PGperson.IndexOf("|") == -1)
                    {
                        int id = Convert.ToInt32(a.PGperson);
                        var userinfo = userList.Where(x => x.ID == id).FirstOrDefault();
                        gt.PGperson = userinfo.PerSonName;
                    }
                    else
                    {
                        string userStr = "";
                        var ary = a.PGperson.Split('|');
                        foreach (var b in ary)
                        {
                            int sid = Convert.ToInt32(b.Trim());
                            var userinfo = userList.Where(x => x.ID == sid).FirstOrDefault();
                            userStr = userStr == "" ? userinfo.PerSonName : userStr + "、" + userinfo.PerSonName;
                        }
                        gt.PGperson = userStr;
                    }
                    gt.StartTime = a.StartTime;
                    gt.EndTime = a.EndTime;
                    gt.WorkTime = a.WorkTime;
                    gt.PGpersonAllWages = a.PGpersonAllWages;
                    gt.SettlementAmount = a.SettlementAmount;
                    gt.gpInfo = a.WorkInfo;
                    gt.WorkAddress = a.BumenInfoSet1.Name;
                    gt.GPState = T_BoolItemService.LoadEntities(x => x.ItemsID == 9 && x.@int == a.GPState).First().str;
                    gt.GPStateNum = a.GPState;
                    rtmp.Add(gt);
                }
            }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        //获取派工部门待审核信息
        public ActionResult GetPGBuMenInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int toalcount = 0;
            var temp = new List<YSGPtop>();
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            int bumenID = Convert.ToInt32(Request["PGBMID"]);
            temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.PGbumen == bumenID && x.Del_f == 0 && x.GPState == 1, x => x.GPtime, false).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                foreach (var a in temp)
                {
                    GPtop gt = new GPtop();
                    gt.ID = a.ID;
                    gt.GPtime = a.GPtime;
                    gt.PGbumen = a.BumenInfoSet.Name;
                    if (a.PGperson.IndexOf("|") == -1)
                    {
                        int id = Convert.ToInt32(a.PGperson);
                        var userinfo = userList.Where(x => x.ID == id).FirstOrDefault();
                        gt.PGperson = userinfo.PerSonName;
                    }
                    else
                    {
                        string userStr = "";
                        var ary = a.PGperson.Split('|');
                        foreach (var b in ary)
                        {
                            int sid = Convert.ToInt32(b.Trim());
                            var userinfo = userList.Where(x => x.ID == sid).FirstOrDefault();
                            userStr = userStr == "" ? userinfo.PerSonName : userStr + "、" + userinfo.PerSonName;
                        }
                        gt.PGperson = userStr;
                    }
                    gt.StartTime = a.StartTime;
                    gt.EndTime = a.EndTime;
                    gt.WorkTime = a.WorkTime;
                    gt.PGpersonAllWages = a.PGpersonAllWages;
                    gt.SettlementAmount = a.SettlementAmount;
                    gt.gpInfo = a.WorkInfo;
                    gt.WorkAddress = a.BumenInfoSet1.Name;
                    gt.GPState = T_BoolItemService.LoadEntities(x => x.ItemsID == 9 && x.@int == a.GPState).First().str;
                    gt.GPStateNum = a.GPState;
                    gt.pingJiaStateNum = a.pingjiaState;
                    gt.pingJiaState = a.pingjiaState == 0 ? "满意" : a.pingjiaState == 1 ? "一般" : "不满意";
                    rtmp.Add(gt);
                }
            }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHistoryList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int toalcount = 0;
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            int uid = Convert.ToInt32(Request["uid"]);
            var temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.Del_f == 0 && x.GPState != 0 && (x.PGQueRenPerson == uid || x.YGQueRenPerson == uid), x => x.GPtime, false).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                foreach (var a in temp)
                {
                    GPtop gt = new GPtop();
                    gt.ID = a.ID;
                    gt.GPtime = a.GPtime;
                    gt.PGbumen = a.BumenInfoSet.Name;
                    if (a.PGperson.IndexOf("|") == -1)
                    {
                        int id = Convert.ToInt32(a.PGperson);
                        var userinfo = userList.Where(x => x.ID == id).FirstOrDefault();
                        gt.PGperson = userinfo.PerSonName;
                    }
                    else
                    {
                        string userStr = "";
                        var ary = a.PGperson.Split('|');
                        foreach (var b in ary)
                        {
                            int sid = Convert.ToInt32(b.Trim());
                            var userinfo = userList.Where(x => x.ID == sid).FirstOrDefault();
                            userStr = userStr == "" ? userinfo.PerSonName : userStr + "、" + userinfo.PerSonName;
                        }
                        gt.PGperson = userStr;
                    }
                    gt.StartTime = a.StartTime;
                    gt.EndTime = a.EndTime;
                    gt.WorkTime = a.WorkTime;
                    gt.PGpersonAllWages = a.PGpersonAllWages;
                    gt.SettlementAmount = a.SettlementAmount;
                    gt.gpInfo = a.WorkInfo;
                    gt.WorkAddress = a.BumenInfoSet1.Name;
                    gt.GPState = T_BoolItemService.LoadEntities(x => x.ItemsID == 9 && x.@int == a.GPState).First().str;
                    gt.GPStateNum = a.GPState;
                    gt.pingJiaStateNum = a.pingjiaState;
                    gt.pingJiaState = a.pingjiaState == 0 ? "满意" : a.pingjiaState == 1 ? "一般" : "不满意";
                    gt.pgOrYG = a.PGQueRenPerson == uid ? (short)1 : (short)0;
                    rtmp.Add(gt);
                }
            }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        //部门审核工票
        public ActionResult SHGP()
        {
            short shyi = Convert.ToInt16(Request["shyj"]);
            long gpid = Convert.ToInt64(Request["gpid"]);
            int uid = Convert.ToInt32(Request["uid"]);
            var temp = YSGPtopService.LoadEntities(x => x.ID == gpid).FirstOrDefault();
            if (temp != null)
            {
                if (shyi == 1)
                {
                    short pingjia = Convert.ToInt16(Request["pingjia"]);
                    temp.GPState = shyi;
                    temp.YGQueRenPerson = uid;
                    temp.pingjiaState = pingjia;
                    temp.YGBuMenQRTime = DateTime.Now;
                } else if (shyi == 2 || shyi == -2)
                {
                    temp.GPState = shyi;
                    temp.PGQueRenPerson = uid;
                    temp.PGBuMenQRTime = DateTime.Now;
                }
                if (YSGPtopService.EditEntity(temp))
                {
                    return Json(new { ret = "ok", msg = "审核成功！" }, JsonRequestBehavior.AllowGet);
                } else
                {
                    return Json(new { ret = "no", msg = "审核失败！" }, JsonRequestBehavior.AllowGet);
                }
            } else
            {
                return Json(new { ret = "no", msg = "该条记录不存在，可能已被删除！" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取派工部门
        public ActionResult GetPGbumen()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.DelFlag == 0 && x.Renark == "2").DefaultIfEmpty().ToList();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Name = a.Name
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取统计方式
        public ActionResult GetSerchType()
        {
            var temp = T_BoolItemService.LoadEntities(x => x.ItemsID == 7).DefaultIfEmpty();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.@int,
                           Name = a.str
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取工作地点部门
        public ActionResult GetWorkbumen()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.DelFlag == 0&&x.Renark=="1").DefaultIfEmpty().ToList();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Name = a.Name
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取派工部门下人员
        public ActionResult GetPGperson()
        {
            int bumenid = Convert.ToInt32(Request["BMID"]);
            var temp = UserInfoService.LoadEntities(x => x.DelFlag == 0 && x.BuMenID == bumenid).DefaultIfEmpty().ToList();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Name = a.PerSonName
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //添加工票
        public ActionResult AddGP()
        {
            YSGPtop yt = new YSGPtop();
            yt.GPtime = Convert.ToDateTime(Request["gpDate"]);
            yt.PGbumen = Convert.ToInt32(Request["PGbumenID"]);
            yt.PGperson = Request["PGpersonID"];
            yt.StartTime = Convert.ToDateTime(Request["startTime"]);
            yt.EndTime = Convert.ToDateTime(Request["endTime"]);
            yt.WorkTime = Convert.ToDecimal(Request["WorkTime"]);
            yt.PGpersonAllWages = Convert.ToDecimal(Request["PGpersonAllWages"]);
            yt.SettlementAmount = Convert.ToDecimal(Request["SettlementAmount"]);
            yt.WorkInfo = Request["WorkInfo"];
            yt.WorkAddress = Convert.ToInt32(Request["WorkAddress"]);
            yt.AddUser = Convert.ToInt32(Request["uid"]);
            yt.AddTime = DateTime.Now;
            yt.GPState = 0;
            yt.Del_f = 0;
            var temp = YSGPtopService.AddEntity(yt);
            /* var YSGPinfo = Request["YSGPinfo"];
             JavaScriptSerializer Serializers = new JavaScriptSerializer();
             //json字符串转为数组对象, 反序列化
             List<YSGPinfo> objs = Serializers.Deserialize<List<YSGPinfo>>(YSGPinfo);
             foreach (var a in objs)
             {
                 YSGPinfo yi = new YSGPinfo();
                 yi.TopID = temp.ID;
                 yi.WorkAddress = a.WorkAddress;
                 yi.WorkInfo = a.WorkInfo;
                 yi.WorkTrueStartTime = a.WorkTrueStartTime;
                 yi.WorkTrueEndTime = a.WorkTrueEndTime;
                 yi.Del_f = 0;
                 YSGPinfoService.AddEntity(yi);
             }*/
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        //删除工票
        public ActionResult DelGP()
        {
            long topID = Convert.ToInt64(Request["topID"]);
            var temp = YSGPtopService.LoadEntities(x => x.ID == topID).FirstOrDefault();
            if (temp != null)
            {
                int uid = Convert.ToInt32(Request["uid"]);
                if (temp.AddUser == uid)
                {
                    if (temp.GPState <= 0)
                    {
                        temp.Del_f = 1;
                        if (YSGPtopService.EditEntity(temp))
                        {
                            return Json(new { ret = "ok", msg = "删除成功！" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { ret = "no", msg = "删除失败！" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { ret = "no", msg = "该条记录已进入审核阶段，无法删除！" }, JsonRequestBehavior.AllowGet);
                    }
                } else
                {
                    return Json(new { ret = "no", msg = "该条记录非本人所创建，无权删除！" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { ret = "no", msg = "数据库中无此数据" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取部门列表
        public ActionResult GetBuMenList()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.Renark == "2" && x.DelFlag == 0).DefaultIfEmpty();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Name = a.Name
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取部门下人员列表
        public ActionResult GetPersonList()
        {
            var bmid = Convert.ToInt32(Request["BMID"]);
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == bmid && x.DelFlag == 0).DefaultIfEmpty();
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           Name = a.PerSonName
                       };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //添加员工工资
        public ActionResult AddPersonMoney()
        {
            int uid = Convert.ToInt32(Request["uid"]);
            var data = YSGPmoneyService.LoadEntities(x => x.UID == uid).FirstOrDefault();
            if (data == null)
            {
                YSGPmoney ym = new YSGPmoney();
                ym.UID = uid;
                ym.GongZhong = Request["GongZhong"];
                ym.YueMoney = Convert.ToInt32(Request["YueMoney"]);
                ym.HoursMoney = ym.YueMoney / 30 / 10;
                ym.Del_f = 0;
                if (YSGPmoneyService.AddEntity(ym).ID > 0)
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
                }
            } else
            {
                data.GongZhong = Request["GongZhong"];
                data.YueMoney = Convert.ToInt32(Request["YueMoney"]);
                data.HoursMoney = Convert.ToInt32(Request["YueMoney"]) / 30 / 10;
                if (YSGPmoneyService.EditEntity(data))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //获取要修改员工数据
        public ActionResult GetEditMoneyInfo()
        {
            int id = Convert.ToInt32(Request["id"]);
            var data = YSGPmoneyService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (data != null)
            {
                return Json(new { ret = "ok", temp = data }, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(new { ret = "no", msg = "无此用户工资记录" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取员工工资时薪
        public ActionResult GetPersonHmoney()
        {
            if (Request["uidStr"].IndexOf("|") == -1)
            {
                int uid = Convert.ToInt32(Request["uidStr"]);
                var temp = YSGPmoneyService.LoadEntities(x => x.UID == uid).FirstOrDefault();
                if (temp != null)
                {
                    return Json(new { ret = "ok", money = temp.HoursMoney }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var uinfo = UserInfoService.LoadEntities(x => x.ID == uid).First();
                    string msg = "无派工人员【" + uinfo.PerSonName + "】的工资记录，无法自动计算，请联系网络部为您添加工资记录！";
                    return Json(new { ret = "no", msg = msg }, JsonRequestBehavior.AllowGet);
                }
            } else
            {
                decimal sumHmoney = 0;
                string[] uidStrAry = Request["uidStr"].Split('|');
                foreach (var a in uidStrAry)
                {
                    var uid = Convert.ToInt32(a);
                    var temp = YSGPmoneyService.LoadEntities(x => x.UID == uid).FirstOrDefault();
                    if (temp == null)
                    {
                        var uinfo = UserInfoService.LoadEntities(x => x.ID == uid).First();
                        string msg = "无派工人员【" + uinfo.PerSonName + "】的工资记录，无法自动计算，请联系网络部为您添加工资记录！";
                        return Json(new { ret = "no", msg = msg }, JsonRequestBehavior.AllowGet);
                    } else
                    {
                        sumHmoney += temp.HoursMoney;
                    }
                }
                return Json(new { ret = "ok", money = sumHmoney }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取所有时薪记录
        public ActionResult GetAllMoneyList()
        {
            var temp = YSGPmoneyService.LoadEntities(x => x.Del_f == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                var rtmp = from a in temp
                           select new
                           {
                               ID = a.ID,
                               Name = a.UserInfo.PerSonName,
                               GongZhong = a.GongZhong,
                               YueMoney = a.YueMoney,
                               HoursMoney = a.HoursMoney,
                               Del_f = a.Del_f
                           };
                return Json(new { ret = "ok", rows = rtmp }, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }
        //统计用工部门
        public ActionResult TongJiYGBM()
        {
            DateTime startDate = Convert.ToDateTime(Request["startTime"]);
            DateTime endDate = Convert.ToDateTime(Request["endTime"]);
            var temp = YSGPtopService.LoadEntities(x => x.GPtime >= startDate && x.GPtime <= endDate&&x.GPState==2 && x.Del_f == 0).DefaultIfEmpty().ToList();
            var bmList = BumenInfoSetService.LoadEntities(x => x.DelFlag == 0 && x.Gushu == 1).DefaultIfEmpty().ToList();
            List<YGTJ> temp1 = new List<YGTJ>();
            if (temp != null && temp[0] != null)
            {
                foreach (var a in bmList)
                {
                    YGTJ yg = new YGTJ();
                    yg.BuMenid = a.Name;
                    yg.mouthSumTime = 0;
                    var rtmp = temp.Where(x => x.WorkAddress == a.ID).DefaultIfEmpty().ToList();
                    if(rtmp != null && rtmp[0] != null){
                        foreach(var b in rtmp)
                        {
                            yg.mouthSumTime += b.WorkTime;
                        }
                    }
                    temp1.Add(yg);
                  }
                return Json(temp1, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //统计派工部门
        public ActionResult TongJiPGBM()
        {
            DateTime startDate = Convert.ToDateTime(Request["startTime"]);
            DateTime endDate = Convert.ToDateTime(Request["endTime"]);
            int bumenid = Convert.ToInt32(Request["BMID"]);
            var temp = YSGPtopService.LoadEntities(x => x.GPtime >= startDate && x.GPtime <= endDate &&x.PGbumen==bumenid && x.GPState == 2 && x.Del_f == 0).DefaultIfEmpty().ToList();
            var userList = UserInfoService.LoadEntities(x => x.BuMenID == bumenid&&x.DelFlag==0).DefaultIfEmpty();
            List<PGTJ> temp1 = new List<PGTJ>();
            if (temp != null && temp[0] != null)
            {
                foreach(var a in userList)
                {
                    var id = a.ID.ToString();
                    var rtmp = temp.Where(x => x.PGperson.Contains(id)).DefaultIfEmpty().ToList();
                    PGTJ pj = new PGTJ();
                    pj.BuMenid = a.BumenInfoSet.Name;
                    pj.PersonName = a.PerSonName;
                    pj.mouthSumTime = 0;
                    pj.PersonMouthMoney=a.YSGPmoneys.Where(x=>x.UID==a.ID).First().YueMoney;
                    if (rtmp != null && rtmp[0] != null)
                    {
                        foreach(var b in rtmp)
                        {
                            pj.mouthSumTime += b.WorkTime;
                        }
                        pj.monthTimeMoneySum = pj.mouthSumTime * a.YSGPmoneys.Where(x => x.UID == a.ID).First().HoursMoney;
                    }
                    temp1.Add(pj);
                }
                return Json(temp1, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //导出表
        [ValidateInput(false)]
        public void GetExcelTable()
        {
            string excelHtml = Request["OFtable"];
            string name = DateTime.Now.ToString();
            Response.Buffer = true;
            //输出的应用类型 
            Response.ContentType = "application/vnd.ms-excel";
            //设定编码方式，若输出的excel有乱码，可优先从编码方面解决
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //filenames是自定义的文件名
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + name + ".xls");
            //content是步骤1的html，注意是string类型
            Response.Write(excelHtml);
            Response.End();
        }
    }
    public class GPtop
    {
        public long ID { get; set; }
        public DateTime GPtime { get; set; }
        public string PGbumen { get; set; }
        public string PGperson { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal WorkTime { get; set; }
        public decimal PGpersonAllWages { get; set; }
        public decimal SettlementAmount { get; set; }
        public string AddUser { get; set; }
        public DateTime AddTime { get; set; }
        public string WorkAddress { get; set; }
        public string gpInfo { get; set; }
        public string GPState { get; set; }
        public int GPStateNum { get; set; }
        public string pingJiaState { get; set; }
        public short? pingJiaStateNum { get; set; }
        public short pgOrYG { get; set; }
    }
    public class YGTJ
    {
        public string BuMenid { get; set; }
        public decimal mouthSumTime { get; set; }
    }
    public class PGTJ
    {
        public string BuMenid { get; set; }
        public string PersonName { get; set; }
        public decimal mouthSumTime { get; set; }
        public decimal PersonMouthMoney { get; set; }
        public decimal monthTimeMoneySum { get; set; }
    }
}
