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
        IBLL.IYSGPinfoService YSGPinfoService { get; set; }
        IBLL.IYSGPtopService YSGPtopService { get; set; }
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
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
            int toalcount = 0;
            var temp = new List<YSGPtop>();
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.Del_f == 0, x => x.GPtime, false).DefaultIfEmpty().ToList();
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
                    var infoList = YSGPinfoService.LoadEntities(x => x.TopID == a.ID).DefaultIfEmpty().ToList();
                    List<GPinfo> lgi = new List<GPinfo>();
                    foreach (var c in infoList)
                    {
                        GPinfo gi = new GPinfo();
                        gi.WorkAddress = c.WorkAddress;
                        gi.WorkInfo = c.WorkInfo;
                        gi.WorkTrueStartTime = c.WorkTrueStartTime;
                        gi.WorkTrueEndTime = c.WorkTrueEndTime;
                        lgi.Add(gi);
                    }
                    gt.gpInfo = lgi;
                    rtmp.Add(gt);
                }
            }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllGPInfo()
        {
            long id = Convert.ToInt64(Request["topID"]);
            var temp = YSGPinfoService.LoadEntities(x => x.TopID == id).DefaultIfEmpty().ToList();
            List<GPinfo> lgi = new List<GPinfo>();
            foreach(var a in temp)
            {
                GPinfo gi = new GPinfo();
                gi.WorkAddress = a.WorkAddress;
                gi.WorkInfo = a.WorkInfo;
                gi.WorkTrueStartTime = a.WorkTrueStartTime;
                gi.WorkTrueEndTime = a.WorkTrueEndTime;
                lgi.Add(gi);
            }
            return Json(lgi, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGPtopInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            int toalcount = 0;
            var temp=new List<YSGPtop>();
            var rtmp = new List<GPtop>();
            var userList = UserInfoService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
                int uid = Convert.ToInt32(Request["uid"]);
                temp = YSGPtopService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.Del_f == 0&&x.AddUser== uid, x => x.GPtime, false).DefaultIfEmpty().ToList();
                if (temp != null && temp[0] != null)
                {
                    foreach(var a in temp)
                    {
                        GPtop gt = new GPtop();
                        gt.ID = a.ID;
                        gt.GPtime = a.GPtime;
                        gt.PGbumen = a.BumenInfoSet.Name;
                        if(a.PGperson.IndexOf("|") == -1)
                        {
                            int id = Convert.ToInt32(a.PGperson);
                            var userinfo = userList.Where(x => x.ID == id).FirstOrDefault();
                            gt.PGperson = userinfo.PerSonName;
                        }else
                        {
                            string userStr = "";
                            var ary = a.PGperson.Split('|');
                            foreach(var b in ary)
                            {
                                int sid = Convert.ToInt32(b.Trim());
                                var userinfo = userList.Where(x => x.ID == sid).FirstOrDefault();
                                userStr = userStr==""?  userinfo.PerSonName: userStr + "、" + userinfo.PerSonName;
                            }
                            gt.PGperson = userStr;
                        }
                        gt.StartTime = a.StartTime;
                        gt.EndTime = a.EndTime;
                        gt.WorkTime = a.WorkTime;
                        gt.PGpersonAllWages = a.PGpersonAllWages;
                        gt.SettlementAmount = a.SettlementAmount;
                        var infoList = YSGPinfoService.LoadEntities(x => x.TopID == a.ID).DefaultIfEmpty().ToList();
                        List<GPinfo> lgi = new List<GPinfo>();
                        foreach(var c in infoList)
                        {
                            GPinfo gi = new GPinfo();
                            gi.WorkAddress = c.WorkAddress;
                            gi.WorkInfo = c.WorkInfo;
                            gi.WorkTrueStartTime = c.WorkTrueStartTime;
                            gi.WorkTrueEndTime = c.WorkTrueEndTime;
                            lgi.Add(gi);
                        }
                        gt.gpInfo = lgi;
                        rtmp.Add(gt);
                    }
                }
            return Json(new { rows = rtmp, total = toalcount }, JsonRequestBehavior.AllowGet);
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
            return Json( rtmp, JsonRequestBehavior.AllowGet);
        }
        //获取派工部门下人员
        public ActionResult GetPGperson()
        {
            int bumenid = Convert.ToInt32(Request["BMID"]);
            var temp = UserInfoService.LoadEntities(x=>x.DelFlag==0&&x.BuMenID==bumenid).DefaultIfEmpty().ToList();
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
            yt.AddUser = Convert.ToInt32(Request["uid"]);
            yt.AddTime = DateTime.Now;
            yt.Del_f = 0;
            var temp = YSGPtopService.AddEntity(yt);
            var YSGPinfo = Request["YSGPinfo"];
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
                YSGPinfoService.AddEntity(yi);
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
    public class GPtop
    {
        public long ID { get; set; }
        public DateTime GPtime {get;set;}
        public string PGbumen {get;set;}
        public string PGperson { get;set;}
        public DateTime StartTime { get;set;}
        public DateTime EndTime { get;set;}
        public decimal WorkTime { get;set;}
        public decimal PGpersonAllWages { get;set;}
        public decimal SettlementAmount { get;set;}
        public string AddUser { get;set;}
        public DateTime AddTime { get;set;}
        public List<GPinfo> gpInfo { get; set; }
    }
    public class GPinfo
    {
        public string WorkAddress { get; set; }
        public string WorkInfo { get; set; }
        public DateTime WorkTrueStartTime { get; set; }
        public DateTime WorkTrueEndTime { get; set; }
    }
}
