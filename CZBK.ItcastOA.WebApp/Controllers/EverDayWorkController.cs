using CZBK.ItcastOA.Model;
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

        //获取日程信息
        public ActionResult GetSchedule()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;
            var temp = ScheduleService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.ID > 0, x => x.ID, false);
            var Rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           UserID = a.UserID,
                           ScheduleTime = a.ScheduleTime,
                           ScheduleAddTime = a.ScheduleAddTime,
                           ScheduleUpdateTime = a.ScheduleUpdateTime,
                           ScheduleText = a.ScheduleText,
                           ScheduleTypeID = a.ScheduleTypeID,
                           TextReadBak = a.TextReadBak,
                           TextReadUser = a.TextReadUser,
                           TextReadTime = a.TextReadTime,
                           FileItemID = a.FileItemID
        };
            return Json(new { rows = Rtmp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }

        //获取Type状态信息
        public ActionResult GetScheduleType() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;
            var temp = ScheduleTypeService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.Del == 0, x => x.ID, false);
            var Rtmp = from a in temp
                       select new {
                          ID= a.ID,
                          ItemText=a.ItemText
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
        public ActionResult AddScheduleType(ScheduleType sdt) {
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
                    return Json(new { msg="操作错误，没有删除成功！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

    }
}
