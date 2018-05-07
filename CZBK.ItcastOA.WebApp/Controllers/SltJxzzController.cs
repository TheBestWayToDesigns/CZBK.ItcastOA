using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class SltJxzzController : BaseController
    {
        //
        // GET: /SltJxzz/
        IBLL.IT_jxzztjbService T_jxzztjbService { get; set; }
        IBLL.ISeb_NumberService Seb_NumberService { get; set; }
        IBLL.IUser_Person_sltService User_Person_sltService { get; set; }
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }

        public ActionResult Index()
        {
            var UserName = User_Person_sltService.LoadEntities(x => x.ID > 0).GroupBy(x => x.UserID).Select(x => x.FirstOrDefault());
            //人员名称表
            ViewBag.sltUser = UserName.ToList();
            //零件名称表
            var LingjianName = Seb_NumberService.LoadEntities(x => x.Items == 1).DefaultIfEmpty();
            ViewBag.ljname = LingjianName.ToList();
            //设备编号名称
            var NumberName = Seb_NumberService.LoadEntities(x => x.Items == 0).DefaultIfEmpty();
            ViewBag.ljname = NumberName.ToList();
            //
            return View();
        }
        public ActionResult Getdata() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int  toalcount=0;
            
            var Tdata = T_jxzztjbService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.del ==0,x=>x.Addtime, true);


            var temp = from a in Tdata
                       select new
                       {
                           ID = a.ID,
                           a.Addtime,
                           a.Wtime,
                           LJname=a.Seb_Number.Ttext,
                           a.User_Person_slt,
                           sbname=a.Seb_Number1.Ttext,
                           a.ImgNumber,
                           a.Iint,a.Slt_kg,a.Slt_BFB,a.Slt_Feipin,a.Slt_hege,a.Slt_hegeNo,a.StupTime,a.OverTime,a.ThisHaveTime,a.HaveTime,
                           bumne=a.BumenInfoSet.Name

                       };
            //var templist = temp.ToList();
            //for (int i = 0; i < templist.Count; i++)
            //{
            //    templist[i].Addess = ArrF(templist[i].Addess);
            //}
            return Json(new { rows = temp, total = toalcount }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult sltperson() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int TotalCount = 0;
            IQueryable<User_Person_slt> ups = User_Person_sltService.LoadPageEntities(pageIndex, pageSize, out TotalCount, x => x.ID > 0, m => m.ID, false).DefaultIfEmpty();
            var temp = from a in ups
                       select new
                       {
                           ID=a.ID,
                           Name = a.UserInfo.PerSonName,
                           Job_Name = a.Job_Name,
                           HoursWage= a.HoursWage,
                           Wage_slt=a.Wage_slt,
                           AddTime=a.AddTime
                       };
            
            return Json(new { rows = temp, total = TotalCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddeditSltPer(User_Person_slt ups) {
            if (ups.ID > 0)
            {
                var isthis = User_Person_sltService.LoadEntities(x => x.ID == ups.ID).FirstOrDefault();
                isthis.HoursWage = ups.HoursWage;
                isthis.Job_Name = ups.Job_Name;
                isthis.UserID = ups.UserID;
                isthis.Wage_slt = ups.Wage_slt;
                isthis.AddTime = DateTime.Now;
                //ups.UserInfo = isthis.UserInfo;
                //ups.AddTime = DateTime.Now;
                User_Person_sltService.EditEntity(isthis);
            }
            else {
                ups.AddTime = DateTime.Now;
                User_Person_sltService.AddEntity(ups);
            }
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //修改信息前获取信息
        public ActionResult GetPersonInfo()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = User_Person_sltService.LoadEntities(x => x.ID == id).FirstOrDefault();
            User_Person_slt ups = new User_Person_slt();
            ups.ID = temp.ID;
            ups.AddTime = temp.AddTime;
            ups.Job_Name = temp.Job_Name;
            ups.UserID = temp.UserID;
            ups.Wage_slt = temp.Wage_slt;
            ups.HoursWage = temp.HoursWage;
            
            return Json( new { ret = ups }, JsonRequestBehavior.AllowGet);
        }
        //获取机加车间人员名单
        public ActionResult GetYuanGongList()
        {
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == 2).DefaultIfEmpty().ToList();
            var rtmp = from a in temp
                           select new
                           {
                               ID = a.ID,
                               Name = a.PerSonName
                           };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        //创建日报表信息
        public ActionResult addDayBb(T_jxzztjb tjjb) {

            return Json("", JsonRequestBehavior.AllowGet);

        }
    }
}
