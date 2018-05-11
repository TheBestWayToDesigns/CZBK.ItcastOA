using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using Microsoft.Ajax.Utilities;
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
        IBLL.IT_BoolItemService T_BoolItemService { get; set; }
        IBLL.IT_jgzztjbService T_jgzztjbService { get; set; }

        public ActionResult Index()
        {
            var UserName = User_Person_sltService.LoadEntities(x => x.UserInfo.BuMenID==26).GroupBy(x => x.UserID).Select(x => x.FirstOrDefault());
            //人员名称表
            ViewBag.sltUser = UserName.ToList();
            var UserNameJG = User_Person_sltService.LoadEntities(x => x.UserInfo.BuMenID == 43).GroupBy(x => x.UserID).Select(x => x.FirstOrDefault());
            //人员名称表
            ViewBag.sltUserJG = UserNameJG.ToList();
            //零件名称表
            var LingjianName = Seb_NumberService.LoadEntities(x => x.Items == 1).DefaultIfEmpty();
            ViewBag.ljname = LingjianName.ToList();
            //设备编号名称
            var NumberName = Seb_NumberService.LoadEntities(x => x.Items == 0).DefaultIfEmpty();
            ViewBag.ljname = NumberName.ToList();
            return View();
        }
        //获取日报信息
        public ActionResult Getdata() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            int  toalcount=0;
            IQueryable<T_jxzztjb> Tdata = T_jxzztjbService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.del ==null,x=>x.Addtime, true).DefaultIfEmpty();
            List<jsxtCls> tList = new List<jsxtCls>();
            foreach (var a in Tdata)
            {
                jsxtCls jc = new jsxtCls();
                jc.ID = a.ID;
                jc.Addtime = a.Addtime;
                jc.Wtime = a.Wtime;
                jc.ImgName_ID = a.Seb_Number1.Ttext;
                jc.UPslt_ID = a.User_Person_slt.UserInfo.PerSonName;
                jc.Seb_number_ID = a.Seb_Number.Ttext;
                jc.ImgNumber = a.ImgNumber;
                jc.Iint = a.Iint;
                jc.Slt_kg = a.Slt_kg;
                jc.Slt_BFB = a.Slt_BFB;
                jc.Slt_Feipin = a.Slt_Feipin;
                jc.Slt_hege = a.Slt_hege;
                jc.Slt_hegeNo = a.Slt_hegeNo;
                jc.StupTime = a.StupTime;
                jc.OverTime = a.OverTime;
                jc.ThisHaveTime = a.ThisHaveTime;
                jc.HaveTime = a.HaveTime;
                jc.WorkHours = a.WorkHours;
                jc.UpBumen_id = a.BumenInfoSet.Name;
                jc.RestYesOrNo = a.RestYesOrNo;
                jc.Wage_slt = a.User_Person_slt.Wage_slt;
                jc.HoursWage = a.User_Person_slt.HoursWage;
                jc.Job_Name = a.User_Person_slt.Job_Name;
                jc.SumHoursWage = jc.WorkHours * jc.HoursWage;
                tList.Add(jc);
            }
            IQueryable<T_jgzztjb> Tdata2 = T_jgzztjbService.LoadPageEntities(pageIndex, pageSize, out toalcount, x => x.del == 0, x => x.Addtime, true).DefaultIfEmpty();
            foreach (var a in Tdata2)
            {
                jsxtCls jc = new jsxtCls();
                jc.ID = a.ID;
                jc.Addtime = a.Addtime;
                jc.Wtime = a.Wtime;
                jc.ImgName_ID = a.Seb_Number.Ttext;
                jc.UPslt_ID = a.User_Person_slt.UserInfo.PerSonName;
                jc.ImgNumber = a.ImgNumber;
                jc.Iint = a.Iint;
                jc.Slt_kg = a.Slt_kg;
                jc.Slt_BFB = a.Slt_BFB;
                jc.Slt_Feipin = a.Slt_Feipin;
                jc.Slt_hege = a.Slt_hege;
                jc.Slt_hegeNo = a.Slt_hegeNo;
                jc.StupTime = a.StupTime;
                jc.OverTime = a.OverTime;
                jc.ThisHaveTime = a.ThisHaveTime;
                jc.HaveTime = a.HaveTime;
                jc.WorkHours = a.WorkHours;
                jc.UpBumen_id = a.BumenInfoSet.Name;
                jc.RestYesOrNo = a.RestYesOrNo;
                jc.Wage_slt = a.User_Person_slt.Wage_slt;
                jc.HoursWage = a.User_Person_slt.HoursWage;
                jc.Job_Name = a.User_Person_slt.Job_Name;
                jc.SumHoursWage = jc.WorkHours * jc.HoursWage;
                tList.Add(jc);
            }
            return Json(new { rows = tList, total = toalcount }, JsonRequestBehavior.AllowGet);
        }
        //获取生产人员信息
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
        //添加生产员工信息
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
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == 26||x.BuMenID==43).DefaultIfEmpty().ToList();
            var rtmp = from a in temp
                           select new
                           {
                               ID = a.ID,
                               Name = a.PerSonName
                           };
            return Json(rtmp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoExcelType()
        {
            var temp = T_BoolItemService.LoadEntities(x => x.ItemsID == 3).DefaultIfEmpty();
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        //创建机加车间日报表信息
        public ActionResult addDayBb(T_jxzztjb tjjb) {
            tjjb.Addtime = DateTime.Now;
            tjjb.del = 0;
            if(tjjb.ImgName_ID == 0)
            {
                var Imgstr_name = Request["ImgName_ID"];
                var temp = Seb_NumberService.LoadEntities(x => x.Ttext == Imgstr_name && x.Items == 1).FirstOrDefault();
                if (temp != null)
                {
                    tjjb.ImgName_ID = temp.ID;
                }else
                {
                    Seb_Number sb = new Seb_Number();
                    sb.Items = 1;
                    sb.Ttext = Imgstr_name;
                    var rtmp = Seb_NumberService.AddEntity(sb);
                    tjjb.ImgName_ID = rtmp.ID;
                }
            }
            T_jxzztjbService.AddEntity(tjjb);
            return Json(new { ret = "ok"}, JsonRequestBehavior.AllowGet);
        }
        //创建结构车间日报表信息
        public ActionResult addDayBbJG(T_jgzztjb tjjb)
        {
            tjjb.Addtime = DateTime.Now;
            tjjb.del = 0;
            if (tjjb.ImgName_ID == 0)
            {
                var Imgstr_name = Request["ImgName_ID"];
                var temp = Seb_NumberService.LoadEntities(x => x.Ttext == Imgstr_name && x.Items == 1).FirstOrDefault();
                if (temp != null)
                {
                    tjjb.ImgName_ID = temp.ID;
                }
                else
                {
                    Seb_Number sb = new Seb_Number();
                    sb.Items = 1;
                    sb.Ttext = Imgstr_name;
                    var rtmp = Seb_NumberService.AddEntity(sb);
                    tjjb.ImgName_ID = rtmp.ID;
                }
            }
            T_jgzztjbService.AddEntity(tjjb);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //获取部门id（机加或铆焊）
        public ActionResult GetInfoExcelMsg()
        {
            int id = Convert.ToInt32(Request["bloID"]);
            var sdata = T_BoolItemService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if(sdata != null)
            {
                List<serchMsg> lsm = new List<serchMsg>();
                if (sdata.@int == 0)
                {
                    var temp = BumenInfoSetService.LoadEntities(x => x.ID == 26 || x.ID == 43).DefaultIfEmpty();
                    foreach (var a in temp)
                    {
                        serchMsg sm = new serchMsg();
                        sm.ID = a.ID;
                        sm.Name = a.Name;
                        lsm.Add(sm);
                    }
                }
                else if (sdata.@int == 1)
                {
                    var temp = BumenInfoSetService.LoadEntities(x => x.ID > 0).DefaultIfEmpty();
                    foreach (var a in temp)
                    {
                        serchMsg sm = new serchMsg();
                        sm.ID = a.ID;
                        sm.Name = a.Name;
                        lsm.Add(sm);
                    }
                } else if (sdata.@int == 3) {
                    var temp = BumenInfoSetService.LoadEntities(x => x.ID ==41&&x.ID==42).DefaultIfEmpty();
                    foreach (var a in temp)
                    {
                        serchMsg sm = new serchMsg();
                        sm.ID = a.ID;
                        sm.Name = a.Name;
                        lsm.Add(sm);
                    }
                } else
                {
                    return Json(new { rer = "no" }, JsonRequestBehavior.AllowGet);
                }
                return Json(lsm, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(new { rer="no"}, JsonRequestBehavior.AllowGet);
            }
        }
        //获取统计数据
        public ActionResult GetStatisticsInfo()
        {
            var bolID = Request["bolID"]==null?0: Convert.ToInt32(Request["bolID"]);
            if(bolID != 0)
            {
                int bumenID = Request["bmID"] == null ? 0 : Convert.ToInt32(Request["bmID"]);
                if (bolID == 9)//个人日报表
                {
                    DateTime excelDate = Convert.ToDateTime(Request["excelDate"]);
                    if(bumenID == 26)
                    {
                        var temp = T_jxzztjbService.LoadEntities(x => x.Wtime == excelDate).DefaultIfEmpty().ToList();
                        if (temp != null && temp[0] != null)
                        {
                            List<jsxtCls> tList = new List<jsxtCls>();
                            foreach (var a in temp)
                            {
                                jsxtCls jc = new jsxtCls();
                                jc.ID = a.ID;
                                jc.Addtime = a.Addtime;
                                jc.Wtime = a.Wtime;
                                jc.ImgName_ID = a.Seb_Number1.Ttext;
                                jc.UPslt_ID = a.User_Person_slt.UserInfo.PerSonName;
                                jc.Seb_number_ID = a.Seb_Number.Ttext;
                                jc.ImgNumber = a.ImgNumber;
                                jc.Iint = a.Iint;
                                jc.Slt_kg = a.Slt_kg;
                                jc.Slt_BFB = a.Slt_BFB;
                                jc.Slt_Feipin = a.Slt_Feipin;
                                jc.Slt_hege = a.Slt_hege;
                                jc.Slt_hegeNo = a.Slt_hegeNo;
                                jc.StupTime = a.StupTime;
                                jc.OverTime = a.OverTime;
                                jc.ThisHaveTime = a.ThisHaveTime;
                                jc.HaveTime = a.HaveTime;
                                jc.WorkHours = a.WorkHours;
                                jc.UpBumen_id = a.BumenInfoSet.Name;
                                jc.RestYesOrNo = a.RestYesOrNo;
                                jc.Wage_slt = a.User_Person_slt.Wage_slt;
                                jc.HoursWage = a.User_Person_slt.HoursWage;
                                jc.Job_Name = a.User_Person_slt.Job_Name;
                                jc.SumHoursWage = jc.WorkHours * jc.HoursWage;
                                tList.Add(jc);
                            }
                            return Json(tList, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else{
                        var temp = T_jgzztjbService.LoadEntities(x => x.Wtime == excelDate).DefaultIfEmpty().ToList();
                        if (temp != null && temp[0] != null)
                        {
                            List<jsxtCls> tList = new List<jsxtCls>();
                            foreach (var a in temp)
                            {
                                jsxtCls jc = new jsxtCls();
                                jc.ID = a.ID;
                                jc.Addtime = a.Addtime;
                                jc.Wtime = a.Wtime;
                                jc.ImgName_ID = a.Seb_Number.Ttext;
                                jc.UPslt_ID = a.User_Person_slt.UserInfo.PerSonName;
                                jc.ImgNumber = a.ImgNumber;
                                jc.Iint = a.Iint;
                                jc.Slt_kg = a.Slt_kg;
                                jc.Slt_BFB = a.Slt_BFB;
                                jc.Slt_Feipin = a.Slt_Feipin;
                                jc.Slt_hege = a.Slt_hege;
                                jc.Slt_hegeNo = a.Slt_hegeNo;
                                jc.StupTime = a.StupTime;
                                jc.OverTime = a.OverTime;
                                jc.ThisHaveTime = a.ThisHaveTime;
                                jc.HaveTime = a.HaveTime;
                                jc.WorkHours = a.WorkHours;
                                jc.UpBumen_id = a.BumenInfoSet.Name;
                                jc.RestYesOrNo = a.RestYesOrNo;
                                jc.Wage_slt = a.User_Person_slt.Wage_slt;
                                jc.HoursWage = a.User_Person_slt.HoursWage;
                                jc.Job_Name = a.User_Person_slt.Job_Name;
                                jc.SumHoursWage = jc.WorkHours * jc.HoursWage;
                                tList.Add(jc);
                            }
                            return Json(tList, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else {
                    DateTime dtStart = Convert.ToDateTime(Request["monthExcel"]);
                    DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1 * (dtStart.Day));
                    if (bolID == 10)//个人月报表
                    {
                        if (bumenID == 26)
                        {
                            var temp = T_jxzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd).DefaultIfEmpty().ToList();
                            if (temp != null && temp[0] != null)
                            {
                                List<jsxtCls> tList = new List<jsxtCls>();
                                foreach (var a in temp)
                                {
                                    jsxtCls jc = new jsxtCls();
                                    jc.ID = a.ID;
                                    jc.Addtime = a.Addtime;
                                    jc.Wtime = a.Wtime;
                                    jc.ImgName_ID = a.Seb_Number1.Ttext;
                                    jc.UPslt_ID = a.User_Person_slt.UserInfo.PerSonName;
                                    jc.Seb_number_ID = a.Seb_Number.Ttext;
                                    jc.ImgNumber = a.ImgNumber;
                                    jc.Iint = a.Iint;
                                    jc.Slt_kg = a.Slt_kg;
                                    jc.Slt_BFB = a.Slt_BFB;
                                    jc.Slt_Feipin = a.Slt_Feipin;
                                    jc.Slt_hege = a.Slt_hege;
                                    jc.Slt_hegeNo = a.Slt_hegeNo;
                                    jc.StupTime = a.StupTime;
                                    jc.OverTime = a.OverTime;
                                    jc.ThisHaveTime = a.ThisHaveTime;
                                    jc.HaveTime = a.HaveTime;
                                    jc.WorkHours = a.WorkHours;
                                    jc.UpBumen_id = a.BumenInfoSet.Name;
                                    jc.RestYesOrNo = a.RestYesOrNo;
                                    jc.Wage_slt = a.User_Person_slt.Wage_slt;
                                    jc.HoursWage = a.User_Person_slt.HoursWage;
                                    jc.Job_Name = a.User_Person_slt.Job_Name;
                                    jc.SumHoursWage = jc.WorkHours * jc.HoursWage;
                                    tList.Add(jc);
                                }
                                return Json(tList, JsonRequestBehavior.AllowGet);
                            }
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var temp = T_jgzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd).DefaultIfEmpty().ToList();
                            if (temp != null && temp[0] != null)
                            {
                                List<jsxtCls> tList = new List<jsxtCls>();
                                foreach (var a in temp)
                                {
                                    jsxtCls jc = new jsxtCls();
                                    jc.ID = a.ID;
                                    jc.Addtime = a.Addtime;
                                    jc.Wtime = a.Wtime;
                                    jc.ImgName_ID = a.Seb_Number.Ttext;
                                    jc.UPslt_ID = a.User_Person_slt.UserInfo.PerSonName;
                                    jc.ImgNumber = a.ImgNumber;
                                    jc.Iint = a.Iint;
                                    jc.Slt_kg = a.Slt_kg;
                                    jc.Slt_BFB = a.Slt_BFB;
                                    jc.Slt_Feipin = a.Slt_Feipin;
                                    jc.Slt_hege = a.Slt_hege;
                                    jc.Slt_hegeNo = a.Slt_hegeNo;
                                    jc.StupTime = a.StupTime;
                                    jc.OverTime = a.OverTime;
                                    jc.ThisHaveTime = a.ThisHaveTime;
                                    jc.HaveTime = a.HaveTime;
                                    jc.WorkHours = a.WorkHours;
                                    jc.UpBumen_id = a.BumenInfoSet.Name;
                                    jc.RestYesOrNo = a.RestYesOrNo;
                                    jc.Wage_slt = a.User_Person_slt.Wage_slt;
                                    jc.HoursWage = a.User_Person_slt.HoursWage;
                                    jc.Job_Name = a.User_Person_slt.Job_Name;
                                    jc.SumHoursWage = jc.WorkHours * jc.HoursWage;
                                    tList.Add(jc);
                                }
                                return Json(tList, JsonRequestBehavior.AllowGet);
                            }
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                    } else if (bolID == 11)//车间月汇报表
                    {
                        var temp = T_jxzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd && x.UpBumen_id == bumenID).DefaultIfEmpty().ToList();
                        var temp2 = T_jgzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd && x.UpBumen_id == bumenID).DefaultIfEmpty().ToList();
                        if (temp != null && temp[0] != null)
                        {
                            List<CJExcel> tList = new List<CJExcel>();
                            foreach(var a in temp)
                            {
                                CJExcel cje = new CJExcel();
                                cje.WorkTime = a.Wtime;
                                cje.PersonName = a.User_Person_slt.UserInfo.PerSonName;
                                cje.WorkType = a.User_Person_slt.Job_Name;
                                cje.HoursWage = a.User_Person_slt.HoursWage;
                                cje.UpBumen = a.BumenInfoSet.Name;
                                cje.WorkHours = a.WorkHours;
                                cje.SumMoney = cje.WorkHours * cje.HoursWage;
                                tList.Add(cje);
                            }
                            foreach (var a in temp2)
                            {
                                CJExcel cje = new CJExcel();
                                cje.WorkTime = a.Wtime;
                                cje.PersonName = a.User_Person_slt.UserInfo.PerSonName;
                                cje.WorkType = a.User_Person_slt.Job_Name;
                                cje.HoursWage = a.User_Person_slt.HoursWage;
                                cje.UpBumen = a.BumenInfoSet.Name;
                                cje.WorkHours = a.WorkHours;
                                cje.SumMoney = cje.WorkHours * cje.HoursWage;
                                tList.Add(cje);
                            }
                            return Json(tList, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }else if(bolID == 12)//公司月总结表
                    {
                        var temp = T_jxzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd).DefaultIfEmpty().ToList();
                        var temp2 = T_jgzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd).DefaultIfEmpty().ToList();
                        if (temp != null && temp[0] != null)
                        {
                            var temp1 = temp.GroupBy(x => x.BumenInfoSet.Name).Select(x => new GSExcel { UpBumen = x.Key, SumHours = x.Sum(g => g.WorkHours), SumMoney=x.Sum(g=>g.User_Person_slt.HoursWage*g.WorkHours) }).ToList();
                            var temp3 = temp.GroupBy(x => x.BumenInfoSet.Name).Select(x => new GSExcel { UpBumen = x.Key, SumHours = x.Sum(g => g.WorkHours), SumMoney = x.Sum(g => g.User_Person_slt.HoursWage * g.WorkHours) }).ToList();
                            temp1.AddRange(temp3);
                            return Json(temp1, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    else if (bolID == 13)//轨枕月总结表
                    {
                        int id = 0;
                        if (bumenID == 41)
                        {
                            id = 26;
                        }else
                        {
                            id = 43;
                        }
                        var temp = T_jxzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd&& x.UpBumen_id == bumenID&&x.User_Person_slt.UserInfo.BuMenID==id).DefaultIfEmpty().ToList();
                        var temp2 = T_jgzztjbService.LoadEntities(x => x.Wtime >= dtStart && x.Wtime <= dtEnd && x.UpBumen_id == bumenID && x.User_Person_slt.UserInfo.BuMenID == id).DefaultIfEmpty().ToList();
                        if (temp != null && temp[0] != null)
                        {
                            var temp1 = temp.GroupBy(x => x.Seb_Number.Ttext).Select(x => new GZXExcel { UpBumen = x.Key, SumHours = x.Sum(g => g.WorkHours), SumMoney = x.Sum(g => g.WorkHours * g.User_Person_slt.HoursWage) }).ToList();
                            var temp3 = temp.GroupBy(x => x.Seb_Number.Ttext).Select(x => new GZXExcel { UpBumen = x.Key, SumHours = x.Sum(g => g.WorkHours), SumMoney = x.Sum(g => g.WorkHours * g.User_Person_slt.HoursWage) }).ToList();
                            temp1.AddRange(temp3);
                            return Json(temp1, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }else {
                ActionResult ar =  Getdata();
                return ar;
            }
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
        //获取要修改的数据
        public ActionResult GetEditInfo()
        {
            long id = Convert.ToInt64(Request["id"]);
            int sd = Convert.ToInt32(Request["state"]);
            if(sd == 0)
            {
                var temp = T_jxzztjbService.LoadEntities(x => x.ID == id).FirstOrDefault();
                jxInfo ji = new jxInfo();
                ji.ID = temp.ID;
                ji.Addtime = temp.Addtime;
                ji.Wtime = temp.Wtime;
                ji.ImgName_ID = temp.ImgName_ID;
                ji.UPslt_ID = temp.UPslt_ID;
                ji.Seb_number_ID = temp.Seb_number_ID;
                ji.ImgNumber = temp.ImgNumber;
                ji.Iint = temp.Iint;
                ji.Slt_kg = temp.Slt_kg;
                ji.Slt_hege = temp.Slt_hege;
                ji.Slt_hegeNo = temp.Slt_hegeNo;
                ji.Slt_Feipin = temp.Slt_Feipin;
                ji.Slt_BFB = temp.Slt_BFB;
                ji.StupTime = temp.StupTime;
                ji.OverTime = temp.OverTime;
                ji.ThisHaveTime = temp.ThisHaveTime;
                ji.HaveTime = temp.HaveTime;
                ji.UpBumen_id = temp.UpBumen_id;
                ji.del = temp.del;
                ji.RestYesOrNo = temp.RestYesOrNo;
                ji.WorkHours = temp.WorkHours;
                ji.LjName = temp.Seb_Number.Ttext;
                return Json(ji, JsonRequestBehavior.AllowGet);
            }else
            {
                var temp = T_jgzztjbService.LoadEntities(x => x.ID == id).FirstOrDefault();
                jgInfo ji = new jgInfo();
                ji.ID = temp.ID;
                ji.Addtime = temp.Addtime;
                ji.Wtime = temp.Wtime;
                ji.ImgName_ID = temp.ImgName_ID;
                ji.UPslt_ID = temp.UPslt_ID;
                ji.ImgNumber = temp.ImgNumber;
                ji.Iint = temp.Iint;
                ji.Slt_kg = temp.Slt_kg;
                ji.Slt_hege = temp.Slt_hege;
                ji.Slt_hegeNo = temp.Slt_hegeNo;
                ji.Slt_Feipin = temp.Slt_Feipin;
                ji.Slt_BFB = temp.Slt_BFB;
                ji.StupTime = temp.StupTime;
                ji.OverTime = temp.OverTime;
                ji.ThisHaveTime = temp.ThisHaveTime;
                ji.HaveTime = temp.HaveTime;
                ji.UpBumen_id = temp.UpBumen_id;
                ji.del = temp.del;
                ji.RestYesOrNo = temp.RestYesOrNo;
                ji.WorkHours = temp.WorkHours;
                return Json(ji, JsonRequestBehavior.AllowGet);
            }
        }
    }
    public class jsxtCls
    {
        public long ID { get; set; }
        public System.DateTime Addtime { get; set; }
        public System.DateTime Wtime { get; set; }
        public string ImgName_ID { get; set; }
        public string UPslt_ID { get; set; }
        public string Seb_number_ID { get; set; }
        public string ImgNumber { get; set; }
        public Nullable<decimal> Iint { get; set; }
        public Nullable<decimal> Slt_kg { get; set; }
        public Nullable<decimal> Slt_hege { get; set; }
        public Nullable<decimal> Slt_hegeNo { get; set; }
        public Nullable<decimal> Slt_Feipin { get; set; }
        public Nullable<decimal> Slt_BFB { get; set; }
        public Nullable<System.DateTime> StupTime { get; set; }
        public Nullable<System.DateTime> OverTime { get; set; }
        public Nullable<decimal> ThisHaveTime { get; set; }
        public Nullable<decimal> HaveTime { get; set; }
        public string UpBumen_id { get; set; }
        public Nullable<int> del { get; set; }
        public Nullable<short> RestYesOrNo { get; set; }
        public Nullable<decimal> Wage_slt { get; set; }
        public string Job_Name { get; set; }
        public Nullable<decimal> HoursWage { get; set; }
        public Nullable<decimal> SumHoursWage { get; set; }
        public Nullable<decimal> WorkHours { get; set; }
    }
    public class serchMsg
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class CJExcel
    {
        public DateTime? WorkTime { get; set; }
        public string PersonName { get; set; }
        public string WorkType { get; set; }
        public decimal? HoursWage { get; set; }
        public string UpBumen { get; set; }
        public string WorkInfo{ get; set; }
        public decimal? WorkDay { get; set; }
        public decimal? WorkHours { get; set; }
        public decimal? SumMoney { get; set; }
    }
    public class GSExcel
    {
        public string UpBumen { get; set; }
        public decimal? SumHours { get; set; }
        public decimal? SumMoney { get; set; }
        public string BmLeaderQZ { get; set; }
    }
    public class GZXExcel
    {
        public string UpBumen { get; set; }
        public decimal? SumHours { get; set; }
        public decimal? SumMoney { get; set; }
    }
    public class jxInfo
    {
        public long ID { get; set; }
        public System.DateTime Addtime { get; set; }
        public System.DateTime Wtime { get; set; }
        public long ImgName_ID { get; set; }
        public long UPslt_ID { get; set; }
        public long Seb_number_ID { get; set; }
        public string ImgNumber { get; set; }
        public Nullable<decimal> Iint { get; set; }
        public Nullable<decimal> Slt_kg { get; set; }
        public Nullable<decimal> Slt_hege { get; set; }
        public Nullable<decimal> Slt_hegeNo { get; set; }
        public Nullable<decimal> Slt_Feipin { get; set; }
        public Nullable<decimal> Slt_BFB { get; set; }
        public Nullable<System.DateTime> StupTime { get; set; }
        public Nullable<System.DateTime> OverTime { get; set; }
        public Nullable<decimal> ThisHaveTime { get; set; }
        public Nullable<decimal> HaveTime { get; set; }
        public Nullable<int> UpBumen_id { get; set; }
        public Nullable<int> del { get; set; }
        public Nullable<short> RestYesOrNo { get; set; }
        public Nullable<decimal> WorkHours { get; set; }
        public string LjName { get; set; }
    }
    public class jgInfo
    {
        public long ID { get; set; }
        public System.DateTime Addtime { get; set; }
        public System.DateTime Wtime { get; set; }
        public long ImgName_ID { get; set; }
        public long UPslt_ID { get; set; }
        public string ImgNumber { get; set; }
        public Nullable<decimal> Iint { get; set; }
        public Nullable<decimal> Slt_kg { get; set; }
        public Nullable<decimal> Slt_hege { get; set; }
        public Nullable<decimal> Slt_hegeNo { get; set; }
        public Nullable<decimal> Slt_Feipin { get; set; }
        public Nullable<decimal> Slt_BFB { get; set; }
        public Nullable<System.DateTime> StupTime { get; set; }
        public Nullable<System.DateTime> OverTime { get; set; }
        public Nullable<decimal> ThisHaveTime { get; set; }
        public Nullable<decimal> HaveTime { get; set; }
        public Nullable<int> UpBumen_id { get; set; }
        public Nullable<int> del { get; set; }
        public Nullable<short> RestYesOrNo { get; set; }
        public Nullable<decimal> WorkHours { get; set; }
    }
}
