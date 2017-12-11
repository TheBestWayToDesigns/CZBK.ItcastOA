using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class EverDayWorkController : BaseController
    {
        //
        // GET: /EverDayWork/
        IBLL.IFileItemService FileItemService { get; set; }
        IBLL.IScheduleService ScheduleService { get; set; }
        IBLL.IScheduleTypeService ScheduleTypeService { get; set; }
        IBLL.IScheduleUserService ScheduleUserService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {
            //ViewBag.items = ScheduleTypeService.LoadEntities(x => x.Del == 0).DefaultIfEmpty()==;
            return View();
        }

        //获取下级用户名称
        public ActionResult GetDownUserall()
        {
            List<Uidorname> Luin = GetAllDownUser();
            return Json(Luin, JsonRequestBehavior.AllowGet);
        }
        //获取所有下级用户
        public List<Uidorname> GetAllDownUser()
        {
            var localID = Convert.ToInt64(LoginUser.ID);
            var tempSUser = ScheduleUserService.LoadEntities(x => x.UpID == localID).DefaultIfEmpty();
            List<Uidorname> Luin = new List<Uidorname>();
            ForUser(tempSUser, Luin);
            return Luin;
        }
        //迭代
        public void ForUser(IQueryable<ScheduleUser> tsu,List<Uidorname> Luin)
        {
            foreach (var tpuser in tsu)
            {
                Uidorname uin = new Uidorname();
                uin.ID = Convert.ToInt32(tpuser.UserID);
                uin.name = tpuser.UserInfo.PerSonName;
                Luin.Add(uin);
                var duibi = ScheduleUserService.LoadEntities(x => x.UpID == tpuser.UserID).FirstOrDefault();
                if (duibi != null)
                {
                   var temp=  ScheduleUserService.LoadEntities(x => x.UpID == tpuser.UserID).DefaultIfEmpty();
                    ForUser(temp, Luin);
                }                
                
            }
        }
        //获取下级用户日程
        public ActionResult GetDownUser()
        {
            var DownUserID = Request["AdduserID"] != null ? Convert.ToInt64(Request["AdduserID"]) : 0;
            int PageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int PageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int TotalCount = 0;
            if (DownUserID != 0)
            {
                var tempSchedule = ScheduleService.LoadEntities(x => x.UserID == DownUserID).DefaultIfEmpty();
                var tRtmp = from b in tempSchedule
                            select new
                            {
                                ID = b.ID,
                                UserID = b.UserInfo.PerSonName,
                                ScheduleTime = b.ScheduleTime,
                                ScheduleAddTime = b.ScheduleAddTime,
                                ScheduleUpdateTime = b.ScheduleUpdateTime,
                                ScheduleText = b.ScheduleText,
                                ScheduleTypeID = b.ScheduleTypeID,
                                TextReadBak = b.TextReadBak,
                                TextReadUser = b.UserInfo1.UName,
                                TextReadTime = b.TextReadTime,
                                FileItemID = b.FileItemID,
                                ReadUser = b.TextReadUser
                            };
                return Json(new { rows = tRtmp, total = TotalCount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List < Uidorname > Luin = GetAllDownUser();
                List<int> list = new List<int>();
                foreach(var a in Luin)
                {
                    list.Add(a.ID);
                }
                var sc = ScheduleService.LoadEntities(x =>list.Contains(x.UserID));
                var tRtmp = from a in sc
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                ScheduleAddTime = a.ScheduleAddTime,
                                ScheduleUpdateTime = a.ScheduleUpdateTime,
                                ScheduleText = a.ScheduleText,
                                ScheduleTypeID = a.ScheduleTypeID,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.UName,
                                TextReadTime = a.TextReadTime,
                                FileItemID = a.FileItemID
                            };
                return Json(new { rows = tRtmp, total = TotalCount }, JsonRequestBehavior.AllowGet);
            }

        }

        //获取日程
        public ActionResult GetSchedule()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;
            var temp = ScheduleService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.UserID == LoginUser.ID, x => x.ID, false);
            var Rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           UserID = a.UserInfo1.UName,
                           ScheduleTime = a.ScheduleTime,
                           ScheduleAddTime = a.ScheduleAddTime,
                           ScheduleUpdateTime = a.ScheduleUpdateTime,
                           ScheduleText = a.ScheduleText,
                           ScheduleTypeID = a.ScheduleTypeID,
                           TextReadBak = a.TextReadBak,
                           TextReadUser = a.UserInfo1.UName,
                           TextReadTime = a.TextReadTime,
                           FileItemID = a.FileItemID
                       };
            return Json(new { rows = Rtmp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }

        //获取Type状态信息
        public ActionResult GetScheduleType()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;
            var temp = ScheduleTypeService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.Del == 0, x => x.ID, false);
            var Rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           ItemText = a.ItemText
                       };

            return Json(new { rows = Rtmp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        //获取日程类型
        public ActionResult GetScheduleTypeall()
        {
            var temp = ScheduleTypeService.LoadEntities(x => x.ID > 0);
            var Rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           ItemText = a.ItemText
                       };
            return Json(Rtmp, JsonRequestBehavior.AllowGet);
        }
        //添加日程
        public ActionResult AddSchedule(Schedule sd)
        {

            sd.UserID = LoginUser.ID;
            sd.ScheduleAddTime = DateTime.Now;
            sd.TextReadBak = "未审核";
            ScheduleService.AddEntity(sd);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }

        //修改日程
        public ActionResult UpdateSchedule(Schedule sd)
        {
            sd.ScheduleUpdateTime = DateTime.Now;
            ScheduleService.EditEntity(sd);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }

        //删除日程
        public ActionResult DelSchedule()
        {
            var id = Convert.ToInt32(Request["id"]);
            var temp = ScheduleService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (temp == null)
            { return Json(new { msg = "数据库中无要修改的信息！" }, JsonRequestBehavior.AllowGet); }
            else
            {
                if (ScheduleService.DeleteEntity(temp))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "操作错误，没有删除成功！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //获取上传文件
        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = Request.Files["fileIconUp"];
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);//获取上传的文件名
                string fileExt = Path.GetExtension(filename);//获取扩展名
                string dir = "/MenuIcon/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                string filenewName = Guid.NewGuid().ToString();
                string fulldir = dir + filenewName + fileExt;
                file.SaveAs(Request.MapPath(fulldir));
                return Content("yes:" + fulldir);
            }
            else
            {
                return Content("no:文件类型错误，文件扩展名错误！");
            }
        }




        //添加日程状态
        public ActionResult AddScheduleType(ScheduleType sdt)
        {
            sdt.Del = 0;
            ScheduleTypeService.AddEntity(sdt);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //删除日程状态
        public ActionResult DelType()
        {
            var id = Convert.ToInt32(Request["id"]);
            var temp = ScheduleTypeService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (temp == null)
            { return Json(new { msg = "数据库中无要删除的信息！" }, JsonRequestBehavior.AllowGet); }
            else
            {
                if (ScheduleTypeService.DeleteEntity(temp))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msg = "操作错误，没有删除成功！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        //查阅下级用户
        public ActionResult ReviewSchedule(Schedule sc)
        {
            var textReadBak = Request["TextReadBak"];
            var id = Convert.ToInt64(Request["ReviewScheduleID"]);
            var temp= ScheduleService.LoadEntities(x => x.ID == id).FirstOrDefault();
            var user = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
            temp.TextReadUser = LoginUser.ID;
            temp.TextReadBak = textReadBak;
            temp.TextReadTime = DateTime.Now;
            ScheduleService.EditEntity(temp);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
    public class Uidorname {
        public int ID { get; set; }
        public string name { get; set; }
    }
}
