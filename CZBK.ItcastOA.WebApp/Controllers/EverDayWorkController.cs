using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
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
        //添加日程状态
        public ActionResult AddSchedule(ScheduleType sdt) {
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
                if (ScheduleTypeService.EditEntity(temp))
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
