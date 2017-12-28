using CZBK.ItcastOA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZBK.ItcastOA.Model;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class DepartmentController : BaseController
    {
        //
        // GET: /Department/
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IScheduleUserService ScheduleUserService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }


        public ActionResult Index()
        {
            return View();
        }
        //获取部门数据
        public ActionResult GetDepartment()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            var temp = BumenInfoSetService.LoadPageEntities(pageIndex, pageSize, out totalCount,x => x.ID > 0, a => a.ID, true);
            var rtmp = from u in temp
                       select new
                       {
                           ID = u.ID,
                           DepName = u.Name,
                           AddTime = u.SubTime
                       };
            return Json(new { rows = rtmp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }


        //获取上下级信息
        public ActionResult GetScheduleUser()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            var temp = ScheduleUserService.LoadPageEntities(pageIndex,pageSize,out totalCount,x => x.ID > 0,a => a.ID,true);
            var rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           UserID = a.UserInfo.PerSonName,
                           UpID = a.UserInfo1.PerSonName
                       };
            return Json(new { rows = rtmp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }

        //添加部门
        public ActionResult AddBuMen(BumenInfoSet bis)
        {
            bis.SubTime = DateTime.Now;
            bis.Renark = "0";
            bis.DelFlag = 0;
            bis.Gushu = 0;
            BumenInfoSetService.AddEntity(bis);
            return Json(new { ret = "ok"}, JsonRequestBehavior.AllowGet);
        }

        //修改部门名称
        public ActionResult EditBuMenName()
        {
            int bmid = Convert.ToInt32(Request["bmid"]);
            string newName = Request["New"];
            var temp = BumenInfoSetService.LoadEntities(x => x.ID == bmid).FirstOrDefault();
            temp.Name = newName;
            BumenInfoSetService.EditEntity(temp);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }

        //获取所有部门
        public ActionResult GetAllBuMen()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            List<BMinfo> list = new List<BMinfo>();
            foreach(var a in temp)
            {
                if (a == null)
                {
                    continue;
                }
                BMinfo bmif = new BMinfo();
                bmif.ID = a.ID;
                bmif.Name = a.Name;
                list.Add(bmif);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //获取部门下所有员工
        public ActionResult GetAllBMUser()
        {
            var bmid = Convert.ToInt32(Request["BMid"]);
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == bmid).DefaultIfEmpty().ToList();
            List<BMinfo> list = new List<BMinfo>();
            foreach (var a in temp)
            {
                if (a == null)
                {
                    continue;
                }
                BMinfo bmif = new BMinfo();
                bmif.ID = a.ID;
                bmif.Name = a.PerSonName;
                list.Add(bmif);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //添加上下级关系
        public ActionResult AddUpOrDown(ScheduleUser su)
        {
            var temp = ScheduleUserService.LoadEntities(x => x.UserID == su.UserID).DefaultIfEmpty().ToList();
            if (temp.Count > 0)
            {
                if (temp[0] == null)
                {
                    var rtmp = ScheduleUserService.LoadEntities(x => x.UserID == su.UpID).DefaultIfEmpty().ToList();
                    if (rtmp.Count > 0) {
                        if (rtmp[0] == null)
                        {
                            ScheduleUserService.AddEntity(su);
                            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                        }else
                        {
                            foreach (var a in rtmp)
                            {
                                if(a.UpID == su.UserID)
                                {
                                    return Json(new { ret = "no", msg = "您所添加的下级是您所添加上级的上级，请仔细核对数据库！" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            ScheduleUserService.AddEntity(su);
                            return Json(new { ret = "ok"}, JsonRequestBehavior.AllowGet);
                        }
                    }else
                    {
                        ScheduleUserService.AddEntity(su);
                        return Json(new { ret = "ok"}, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var rtmp = ScheduleUserService.LoadEntities(x => x.UserID == su.UpID).DefaultIfEmpty().ToList();
                    foreach (var a in temp)
                    {
                        if (a.UpID == su.UpID)
                        {
                            return Json(new { ret = "no", msg = "您所添加的上下级关系已经存在！" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    foreach (var b in rtmp) {
                        if (b.UpID == su.UserID)
                        {
                            return Json(new { ret = "no", msg = "您所添加的下级是您所添加上级的上级，请仔细核对数据库！" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    ScheduleUserService.AddEntity(su);
                    return Json(new { ret = "ok"}, JsonRequestBehavior.AllowGet);
                }
            }else
            {
                ScheduleUserService.AddEntity(su);
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }


        //删除上下级关系
        public ActionResult DelUpOrDown()
        {
            int id = Convert.ToInt32(Request["id"]);
            var temp = ScheduleUserService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (ScheduleUserService.DeleteEntity(temp))
            {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "no",msg = "删除失败！" }, JsonRequestBehavior.AllowGet);
            }
        }

        public class BMinfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

    }
}
