using CZBK.ItcastOA.BLL;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
        IBLL.IFileTypeService FileTypeService { get; set; }
        IBLL.IExamineScheduleService ExamineScheduleService { get; set; }
        IBLL.IYJ_ScheduleActionService YJ_ScheduleActionService { get; set; }
        IBLL.IYJ_ScheduleDayService YJ_ScheduleDayService { get; set; }

        int DJ = 2;
        public ActionResult Index()
        {
            return View();
        }
        //获取个人日程文件
        public ActionResult GetProFile()
        {
            return null;
        }
        //获取部门下级用户名称
        public ActionResult GetDownBuMenalluser()
        {
            List<Uidorname> uid = new List<Uidorname>();
            int a = Request["DBMid"] != "" && Request["DBMid"] != null ? Convert.ToInt32(Request["DBMid"]) : 0;
            if (a == 0)
            {
                return Json(uid, JsonRequestBehavior.AllowGet);
            }
            
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == a).DefaultIfEmpty();
            foreach (var s in temp)
            {
                Uidorname uon = new Uidorname();
                uon.ID = s.ID;
                uon.name = s.PerSonName;
                uid.Add(uon);
            }
            return Json(uid, JsonRequestBehavior.AllowGet);
        }
        //获取下级用户名称
        public ActionResult GetDownUserall()
        {
            List<Uidorname> Luin = GetAllDownUser();
            return Json(Luin, JsonRequestBehavior.AllowGet);
        }
        //获得所有此日程下的文件
        public List<AllFile> GetAllFile(int IDst)
        {
            var tempAFile = FileItemService.LoadEntities(x => x.FileFirstID == IDst).DefaultIfEmpty();
            List<AllFile> aFile = new List<AllFile>();
            foreach (var sfile in aFile)
            {
                AllFile allFile = new AllFile();
                allFile.ID = Convert.ToInt32(sfile.ID);
                allFile.Url = sfile.Url;
                allFile.FirstFileID = sfile.FirstFileID;
                aFile.Add(allFile);
            }
            return aFile;
        }
        //获取所有下级用户
        public List<Uidorname> GetAllDownUser()
        {
            var localID = Convert.ToInt64(LoginUser.ID);
            var tempSUser = ScheduleUserService.LoadEntities(x => x.UpID == localID).DefaultIfEmpty();
            List<Uidorname> Luin = new List<Uidorname>();
            ForUser(tempSUser, Luin);
            var a = Luin.Where((x, i) => Luin.FindIndex(z => z.ID == x.ID) == i).ToList();
            return a;
        }
        //迭代下级用户的次级用户
        public void ForUser(IQueryable<ScheduleUser> tsu, List<Uidorname> Luin)
        {

            foreach (var tpuser in tsu)
            {
                if (tpuser == null)
                {
                    continue;
                }
                Uidorname uin = new Uidorname();
                uin.ID = Convert.ToInt32(tpuser.UserID);
                uin.name = tpuser.UserInfo.PerSonName;
                Luin.Add(uin);
                var duibi = ScheduleUserService.LoadEntities(x => x.UpID == tpuser.UserID).FirstOrDefault();
                if (duibi != null)
                {
                    var temp = ScheduleUserService.LoadEntities(x => x.UpID == tpuser.UserID).DefaultIfEmpty();
                    ForUser(temp, Luin);
                }
            }
        }
        //获取下级用户日程
        public ActionResult GetDownUser()
        {
            var DownUserID = Request["DownUser"] != "" && Request["DownUser"] != null ? Convert.ToInt64(Request["DownUser"]) : 0;
            int PageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int PageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int TotalCount = 0;
            if (DownUserID > 0)
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
                                ScheduleTypeID = b.ScheduleType.ItemText,
                                TextReadBak = b.TextReadBak,
                                TextReadUser = b.UserInfo1.PerSonName,
                                TextReadTime = b.TextReadTime,
                                FileItemID = b.FileItemID,
                                ReadUser = b.TextReadUser
                            };

                TotalCount = tRtmp.Count();
                tRtmp = tRtmp.OrderByDescending(x => x.ScheduleAddTime).Skip((PageIndex - 1) * PageSize).Take(PageSize);
                return Json(new { rows = tRtmp, total = TotalCount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<int> list = new List<int>();
                if (Request["BumenID"] != null)
                {
                    var bumenid = Request["BumenID"] != "" && Request["BumenID"] != null ? Convert.ToInt32(Request["BumenID"]) : 0;
                    var bmif = BumenInfoSetService.LoadEntities(x => x.ID == bumenid).DefaultIfEmpty();
                    var userinfo = UserInfoService.LoadEntities(x => x.DelFlag == 0).DefaultIfEmpty();
                    var listid = from a in bmif
                                 from b in userinfo
                                 where a.ID == b.BuMenID
                                 select new
                                 {
                                     b.ID
                                 };
                    foreach (var b in listid) {
                        list.Add(b.ID);
                    }
                }
                else
                {
                    List<Uidorname> Luin = GetAllDownUser();

                    foreach (var a in Luin)
                    {
                        if (a.ID > 0)
                        {
                            list.Add(a.ID);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                var sc = ScheduleService.LoadEntities(x => list.Contains(x.UserID));
                var tRtmp = from a in sc
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                ScheduleAddTime = a.ScheduleAddTime,
                                ScheduleUpdateTime = a.ScheduleUpdateTime,
                                ScheduleText = a.ScheduleText,
                                ScheduleTypeID = a.ScheduleType.ItemText,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                FileItemID = a.FileItemID
                            };

                TotalCount = tRtmp.Count();
                tRtmp = tRtmp.OrderByDescending(x => x.ScheduleAddTime).Skip((PageIndex - 1) * PageSize).Take(PageSize);
                return Json(new { rows = tRtmp, total = TotalCount }, JsonRequestBehavior.AllowGet);
            }

        }


        //获取下级部门的所属员工
        public ActionResult GetDBMUser()
        {
            int a = Convert.ToInt32(Request["DBMid"]);
            List<Uidorname> list = new List<Uidorname>();
            var s = UserInfoService.LoadEntities(x => x.BuMenID == a).DefaultIfEmpty().ToList();
            foreach (var o in s)
            {
                Uidorname uid = new Uidorname();
                uid.ID = o.ID;
                uid.name = o.PerSonName;
                list.Add(uid);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取所有下级部门
        public List<BuMen> GetAllDownBuMen()
        {
            var localID = Convert.ToInt64(LoginUser.ID);
            var tempSUser = ScheduleUserService.LoadEntities(x => x.UpID == localID).DefaultIfEmpty();
            List<Uidorname> Luin = new List<Uidorname>();
            ForUser(tempSUser, Luin);
            List<BuMen> Luinfo = new List<BuMen>();
            foreach (var a in Luin)
            {
                var z = UserInfoService.LoadEntities(x => x.ID == a.ID).FirstOrDefault();
                BuMen bm = new BuMen();
                bm.BMID = Convert.ToInt32(z.BuMenID);
                if (Luinfo.Count != 0)
                {
                    for (int i = 0; i < Luinfo.Count; i++)
                    {
                        if (bm.BMID == Luinfo[i].BMID || bm.BMID == 3)
                        {
                            continue;
                        }
                        else if (i == Luinfo.Count - 1)
                        {
                            var y = BumenInfoSetService.LoadEntities(x => x.ID == bm.BMID).FirstOrDefault();
                            bm.Name = y.Name;
                            Luinfo.Add(bm);
                        } else
                        {
                            continue;
                        }
                    }
                } else
                {
                    if (bm.BMID != 3) {
                        var y = BumenInfoSetService.LoadEntities(x => x.ID == bm.BMID).FirstOrDefault();
                        bm.Name = y.Name;
                        Luinfo.Add(bm);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return Luinfo;
        }
        //获取下级部门名称
        public ActionResult GetDownBuMenall()
        {
            List<BuMen> Luin = GetAllDownBuMen();
            return Json(Luin, JsonRequestBehavior.AllowGet);
        }
        //获取修改日程行的汉字ID
        public ActionResult getUpdateUserID()
        {
            int schid = Convert.ToInt32(Request["id"]);
            List<Schedule> sch = ScheduleService.LoadEntities(x => x.ID == schid).ToList();
            int schuserid = sch[0].UserID;
            int schtypeid = Convert.ToInt32(sch[0].ScheduleTypeID);
            return Json(new { retuid = schuserid, rettpid = schtypeid }, JsonRequestBehavior.AllowGet);
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
                           UserID = a.UserInfo.PerSonName,
                           ScheduleTime = a.ScheduleTime,
                           ScheduleAddTime = a.ScheduleAddTime,
                           ScheduleUpdateTime = a.ScheduleUpdateTime,
                           ScheduleText = a.ScheduleText,
                           ScheduleTypeID = a.ScheduleType.ItemText,
                           ScheduleId=a.ScheduleTypeID,
                           TextReadBak = a.TextReadBak,
                           TextReadUser = a.UserInfo1.PerSonName,
                           TextReadTime = a.TextReadTime,
                           FileItemID = a.FileItemID,
                           ReadUser = a.TextReadUser
                       };
            return Json(new { rows = Rtmp, total = totalCount  }, JsonRequestBehavior.AllowGet);
        }

        //获取Type状态信息
        public ActionResult GetScheduleType()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            int totalCount;
            var ur = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
            var temp = ScheduleTypeService.LoadPageEntities(pageIndex, pageSize, out totalCount, x => x.BuMenId == ur.BuMenID || x.BuMenId == null, x => x.ID, false);
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
            var ur = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
            var temp = ScheduleTypeService.LoadEntities(x => x.BuMenId == ur.BuMenID || x.BuMenId == null);
            var Rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           ItemText = a.ItemText
                       };
            return Json(Rtmp, JsonRequestBehavior.AllowGet);
        }

        //获取上传文件
        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = Request.Files["fileupname"];
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);//获取上传的文件名
                string fileExt = Path.GetExtension(filename);//获取扩展名
                var temp = FileTypeService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
                for (int a = 0; a < temp.Count(); a++)
                {
                    if (fileExt == temp[a].FileTypeENGName)
                    {
                        string dir = "/files/ScheduleFiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                        Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                        string filenewName = Guid.NewGuid().ToString();
                        string fulldir = dir + filenewName + fileExt;
                        file.SaveAs(Request.MapPath(fulldir));
                        return Content("yes:" + fulldir);
                    }
                    else
                    {
                        continue;
                    }
                }
                return Content("no:文件类型错误或不支持，文件扩展名错误！");
            }
            else
            {
                return Content("no:请选择文件再上传");
            }
        }

        //补充文件添加表数据
        public ActionResult AddUploadFile()
        {
            string url = Request["Url"];
            string beizhu = Request["BeiZhu"];
            int schid = Convert.ToInt32(Request["SchID"]);
            if (url != null && url != "")
            {
                int addSchID = Convert.ToInt32(Request["AddUploadScheduleName"]);
                FileItem fi = new FileItem();
                if (Request["FirstFileName"] != null && Request["FirstFileName"] != "" && Request["FirstFileName"] != "null")
                {
                    int addFirstID = Convert.ToInt32(Request["FirstFileName"]);
                    fi.FileFirstID = addFirstID;
                    fi.Url = url;
                    fi.BeiZhu = beizhu;
                    fi.AddTime = DateTime.Now;
                    fi.Del = 0;
                    FileItemService.AddEntity(fi);
                } else
                {
                    fi.Url = url;
                    fi.BeiZhu = beizhu;
                    fi.AddTime = DateTime.Now;
                    fi.Del = 0;
                    var s = FileItemService.AddEntity(fi);
                    var temp = ScheduleService.LoadEntities(x => x.ID == schid).FirstOrDefault();
                    temp.FileItemID = s.ID;
                    ScheduleService.EditEntity(temp);
                }
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { ret = "no", msg = "请先上传文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

        //添加文件表数据
        public int UpdateFile(string beizhu, string url)
        {
            FileItem fi = new FileItem();
            fi.Url = url;
            fi.AddTime = DateTime.Now;
            fi.Del = 0;
            fi.BeiZhu = beizhu;
            var dm = FileItemService.AddEntity(fi);
            return dm.ID;
        }
        //添加日程
        public ActionResult AddSchedule(Schedule sd)
        {
            string url = Request["Url"];
            string beizhu = Request["BeiZhu"];
            if (url != null && url != "")
            {
                int fileid = UpdateFile(beizhu, url);
                sd.FileItemID = fileid;
            }
            sd.UserID = LoginUser.ID;
            sd.ScheduleAddTime = DateTime.Now;
            sd.TextReadBak = "未查阅";
            ScheduleService.AddEntity(sd);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }
        //查看文件
        public ActionResult checkFile()
        {
            int fileid =Convert.ToInt32(Request["id"]);
            var fileItem = FileItemService.LoadEntities(x => x.ID == fileid).FirstOrDefault();
            var fileItems = FileItemService.LoadEntities(x => x.FileFirstID == fileid).DefaultIfEmpty().ToList();
            fileItems.Add(fileItem);
            string s = "";
            foreach (var b in fileItems)
            {
                if (b != null) {
                    if ((b.Url == null || b.Url == "") && (b.BeiZhu != null || b.BeiZhu != ""))
                    {
                        s = s + b.BeiZhu + ",无文件相关路径(可能未上传文件),";
                    }
                    else if ((b.Url != null || b.Url != "") && (b.BeiZhu == null || b.BeiZhu == ""))
                    {
                        s = s + "无备注信息," + b.Url + ",";
                    }
                    else if ((b.Url == null || b.Url == "") && (b.BeiZhu == null || b.BeiZhu == ""))
                    {
                        s = s + "无备注信息,无文件相关路径(可能未上传文件),";
                    }
                    else
                    {
                        s = s + b.BeiZhu + "," + b.Url + ",";
                    }
                }
                else
                {
                    continue;
                }
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        //获取文件类型信息
        public ActionResult GetFileType()
        {
            var temp = FileTypeService.LoadEntities(x => x.ID > 0).DefaultIfEmpty();
            var Rtmp = from a in temp
                       select new
                       {
                           ID = a.ID,
                           FileTypeENGName = a.FileTypeENGName,
                           FileTypeCHNName = a.FileTypeCHNName
                       };
            return Json(Rtmp, JsonRequestBehavior.AllowGet);
        }
        //添加文件类型
        public ActionResult AddFileType(FileType ft)
        {
            FileTypeService.AddEntity(ft);
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

        //添加日程状态
        public ActionResult AddScheduleType(ScheduleType sdt)
        {
            sdt.Del = 0;
            sdt.BuMenId = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault().BuMenID;
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

        //删除文件类型
        public ActionResult DelFileType()
        {
            var id = Convert.ToInt32(Request["id"]);
            var temp = FileTypeService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (temp == null)
            { return Json(new { msg = "数据库中无要删除的信息！" }, JsonRequestBehavior.AllowGet); }
            else
            {
                if (FileTypeService.DeleteEntity(temp))
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
            var tablerows = Request["tablerows"].ToString();
            var temp = ScheduleService.LoadEntities(x => x.ID == id).FirstOrDefault();
            var user = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
            temp.TextReadUser = LoginUser.ID;
            temp.TextReadBak = textReadBak;
            temp.TextReadTime = DateTime.Now;
            ScheduleService.EditEntity(temp);
            return Json(new { ret = "ok" , trows = tablerows,text=textReadBak }, JsonRequestBehavior.AllowGet);
        }


        //获取所有该用户下已审核过的用户记录
        public ActionResult GetExamineSchedule()
        {
            var DownUserID = Request["DownUser2"] != "" && Request["DownUser2"] != null ? Convert.ToInt64(Request["DownUser2"]) : 0;
            int PageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int PageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 10;
            List<Uidorname> list = GetAllDownUser();
            List<Schedule> listsch1 = new List<Schedule>();
            List<Schedule> listsch2 = new List<Schedule>();
            if (list != null && list[0] != null)
            {
                foreach (var l in list)
                {
                    var temp = ScheduleService.LoadEntities(x => x.TextReadBak != "未查阅" && x.UserID == l.ID).DefaultIfEmpty().ToList();
                    if (temp == null || temp[0] == null)
                    {
                        continue;
                    }
                    else
                    {
                        listsch1.AddRange(temp);
                    }
                }
                foreach (var ls in listsch1)
                {
                    foreach (var l in list)
                    {
                        if (ls.TextReadUser == LoginUser.ID || ls.TextReadUser == l.ID)
                        {
                            listsch2.Add(ls);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            } else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            if (DownUserID > 0)
            {
                List<ExamineForSchedule> listsch3 = new List<ExamineForSchedule>();
                foreach (var a in listsch2)
                {
                    ExamineForSchedule efs = new ExamineForSchedule();
                    efs.ID = a.ID;
                    efs.UserID = a.UserID;
                    efs.ScheduleTime = a.ScheduleTime;
                    efs.ScheduleAddTime = a.ScheduleAddTime;
                    efs.ScheduleUpdateTime = a.ScheduleUpdateTime;
                    efs.ScheduleText = a.ScheduleText;
                    efs.ScheduleTypeID = a.ScheduleTypeID;
                    efs.TextReadBak = a.TextReadBak;
                    efs.TextReadUser = a.TextReadUser;
                    efs.TextReadTime = a.TextReadTime;
                    efs.FileItemID = a.FileItemID;
                    efs.UserInfo = a.UserInfo;
                    efs.UserInfo1 = a.UserInfo1;
                    listsch3.Add(efs);
                }
                List<ExamineForSchedule> dush1 = new List<ExamineForSchedule>();
                if (listsch2.Count > 0)
                {
                    foreach (var a in listsch3)
                    {
                        if (a.UserID == DownUserID)
                        {
                            dush1.Add(a);
                        }
                    }
                    List<ExamineForSchedule> exsh1 = new List<ExamineForSchedule>();
                    List<ExamineForSchedule> exsh2 = new List<ExamineForSchedule>();
                    foreach (var b in dush1)
                    {
                        var es = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                        if (es == null || es[0] == null)
                        {
                            exsh1.Add(b);
                        }
                        else
                        {
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    b.ExamineText = e.ExamineText;
                                    b.ExamineUser = e.UserInfo.PerSonName;
                                    exsh2.Add(b);
                                }
                            }
                        }
                    }
                    var Rtmp1 = from a in exsh1
                                select new
                                {
                                    ID = a.ID,
                                    UserID = a.UserInfo.PerSonName,
                                    ScheduleTime = a.ScheduleTime,
                                    TextReadBak = a.TextReadBak,
                                    TextReadUser = a.UserInfo1.PerSonName,
                                    TextReadTime = a.TextReadTime,
                                    FileItemID = a.FileItemID,
                                    ExamineState = "未审核",
                                    ExamineText = "",
                                    ExamineUser = ""
                                };
                    var Rtmp2 = from a in exsh2
                                select new
                                {
                                    ID = a.ID,
                                    UserID = a.UserInfo.PerSonName,
                                    ScheduleTime = a.ScheduleTime,
                                    TextReadBak = a.TextReadBak,
                                    TextReadUser = a.UserInfo1.PerSonName,
                                    TextReadTime = a.TextReadTime,
                                    FileItemID = a.FileItemID,
                                    ExamineState = "已审核",
                                    ExamineText = a.ExamineText,
                                    ExamineUser = a.ExamineUser
                                };
                    var Rtmp = Rtmp1.Union(Rtmp2);
                    return Json(new { rows = Rtmp, total = Rtmp.Count() }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                var bumenid = Request["BumenID2"] != "" && Request["BumenID2"] != null ? Convert.ToInt32(Request["BumenID2"]) : 0;
                if (bumenid > 0)
                {
                    List<ExamineForSchedule> listsch3 = new List<ExamineForSchedule>();
                    foreach (var a in listsch2)
                    {
                        ExamineForSchedule efs = new ExamineForSchedule();
                        efs.ID = a.ID;
                        efs.UserID = a.UserID;
                        efs.ScheduleTime = a.ScheduleTime;
                        efs.ScheduleAddTime = a.ScheduleAddTime;
                        efs.ScheduleUpdateTime = a.ScheduleUpdateTime;
                        efs.ScheduleText = a.ScheduleText;
                        efs.ScheduleTypeID = a.ScheduleTypeID;
                        efs.TextReadBak = a.TextReadBak;
                        efs.TextReadUser = a.TextReadUser;
                        efs.TextReadTime = a.TextReadTime;
                        efs.FileItemID = a.FileItemID;
                        efs.UserInfo = a.UserInfo;
                        efs.UserInfo1 = a.UserInfo1;
                        listsch3.Add(efs);
                    }
                    List<ExamineForSchedule> bmschlist = new List<ExamineForSchedule>();
                    foreach (var a in list)
                    {
                        var onep = UserInfoService.LoadEntities(x => x.ID == a.ID && x.BuMenID == bumenid).FirstOrDefault();
                        if (onep != null)
                        {
                            foreach (var b in listsch3)
                            {
                                if (b.UserID == onep.ID)
                                {
                                    bmschlist.Add(b);
                                }
                            }
                        }
                    }
                    
                    List<ExamineForSchedule> exsh1 = new List<ExamineForSchedule>();
                    List<ExamineForSchedule> exsh2 = new List<ExamineForSchedule>();
                    if (listsch2.Count > 0)
                    {
                        foreach (var b in bmschlist)
                        {
                            var es = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                            if (es == null || es[0] == null)
                            {
                                exsh1.Add(b);
                            }
                            else
                            {
                                foreach (var e in es)
                                {
                                    if (e != null)
                                    {
                                        b.ExamineText = e.ExamineText;
                                        b.ExamineUser = e.UserInfo.PerSonName;
                                        exsh2.Add(b);
                                    }

                                }
                            }
                        }
                        var Rtmp1 = from a in exsh1
                                    select new
                                    {
                                        ID = a.ID,
                                        UserID = a.UserInfo.PerSonName,
                                        ScheduleTime = a.ScheduleTime,
                                        TextReadBak = a.TextReadBak,
                                        TextReadUser = a.UserInfo1.PerSonName,
                                        TextReadTime = a.TextReadTime,
                                        FileItemID = a.FileItemID,
                                        ExamineState = "未审核",
                                        ExamineText = "",
                                        ExamineUser = ""
                                    };
                        var Rtmp2 = from a in exsh2
                                    select new
                                    {
                                        ID = a.ID,
                                        UserID = a.UserInfo.PerSonName,
                                        ScheduleTime = a.ScheduleTime,
                                        TextReadBak = a.TextReadBak,
                                        TextReadUser = a.UserInfo1.PerSonName,
                                        TextReadTime = a.TextReadTime,
                                        FileItemID = a.FileItemID,
                                        ExamineState = "已审核",
                                        ExamineText = a.ExamineText,
                                        ExamineUser = a.ExamineUser
                                    };
                        var Rtmp = Rtmp1.Union(Rtmp2);
                        return Json(new { rows = Rtmp, total = Rtmp.Count() }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    List<ExamineForSchedule> listsch3 = new List<ExamineForSchedule>();
                    foreach (var a in listsch2)
                    {
                        ExamineForSchedule efs = new ExamineForSchedule();
                        efs.ID = a.ID;
                        efs.UserID = a.UserID;
                        efs.ScheduleTime = a.ScheduleTime;
                        efs.ScheduleAddTime = a.ScheduleAddTime;
                        efs.ScheduleUpdateTime = a.ScheduleUpdateTime;
                        efs.ScheduleText = a.ScheduleText;
                        efs.ScheduleTypeID = a.ScheduleTypeID;
                        efs.TextReadBak = a.TextReadBak;
                        efs.TextReadUser = a.TextReadUser;
                        efs.TextReadTime = a.TextReadTime;
                        efs.FileItemID = a.FileItemID;
                        efs.UserInfo = a.UserInfo;
                        efs.UserInfo1 = a.UserInfo1;
                        listsch3.Add(efs);
                    }
                    List<ExamineForSchedule> exsh1 = new List<ExamineForSchedule>();
                    List<ExamineForSchedule> exsh2 = new List<ExamineForSchedule>();
                    if (listsch3.Count > 0)
                    {
                        foreach (var b in listsch3)
                        {
                            var es = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                            if (es == null || es[0] == null)
                            {
                                exsh1.Add(b);
                            }
                            else
                            {
                                foreach (var e in es)
                                {
                                    if (e != null)
                                    {
                                        b.ExamineText = e.ExamineText;
                                        b.ExamineUser = e.UserInfo.PerSonName;
                                        exsh2.Add(b);
                                    }

                                }
                            }
                        }
                        var Rtmp1 = from a in exsh1
                                    select new
                                    {
                                        ID = a.ID,
                                        UserID = a.UserInfo.PerSonName,
                                        ScheduleTime = a.ScheduleTime,
                                        TextReadBak = a.TextReadBak,
                                        TextReadUser = a.UserInfo1.PerSonName,
                                        TextReadTime = a.TextReadTime,
                                        FileItemID = a.FileItemID,
                                        ExamineState = "未审核",
                                        ExamineText = "",
                                        ExamineUser = ""
                                    };
                        var Rtmp2 = from a in exsh2
                                    select new
                                    {
                                        ID = a.ID,
                                        UserID = a.UserInfo.PerSonName,
                                        ScheduleTime = a.ScheduleTime,
                                        TextReadBak = a.TextReadBak,
                                        TextReadUser = a.UserInfo1.PerSonName,
                                        TextReadTime = a.TextReadTime,
                                        FileItemID = a.FileItemID,
                                        ExamineState = "已审核",
                                        ExamineText = a.ExamineText,
                                        ExamineUser = a.ExamineUser
                                    };
                        var Rtmp = Rtmp1.Union(Rtmp2);
                        return Json(new { rows = Rtmp, total = Rtmp.Count() }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        //获取审核查阅意见
        public ActionResult ExamineSchedule()
        {
            long sid = Convert.ToInt64(Request["id"]);
            var temp = ExamineScheduleService.LoadEntities(x => x.ScheduleID == sid).DefaultIfEmpty().ToList();
            string s = "";
            foreach (var a in temp)
            {
                if (a != null)
                {
                    if (a.ExamineText == null || a.ExamineText == "")
                    {
                        s = s + a.UserInfo.PerSonName + ",无审核意见,";
                    } else
                    {
                        s = s + a.UserInfo.PerSonName + "," + a.ExamineText + ",";
                    }
                }
                else
                {
                    continue;
                }
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        //添加审核意见
        public ActionResult AddExamineSchedule(ExamineSchedule es)
        {
            es.ExamineUser = LoginUser.ID;
            es.ExamineTime = DateTime.Now;
            es.ExamineState = 1;
            ExamineScheduleService.AddEntity(es);
            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
        }

        //获取日程详细信息
        public ActionResult GetScheduleInfo()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = ScheduleService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (temp != null)
            {
                return Json(new { UserID = temp.UserInfo.PerSonName, ScheduleTime = temp.ScheduleTime, ScheduleAddTime = temp.ScheduleAddTime, ScheduleTypeID = temp.ScheduleType.ItemText, ScheduleText = temp.ScheduleText }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        //获取日程详细信息(修改日程用)
        public ActionResult GetScheduleInfoForEditSch()
        {
            long id = Convert.ToInt64(Request["id"]);
            var temp = ScheduleService.LoadEntities(x => x.ID == id).FirstOrDefault();
            if (temp != null)
            {
                return Json(new {
                    ID =temp.ID ,
                    UserID =temp.UserID ,
                    ScheduleTime =temp.ScheduleTime ,
                    ScheduleAddTime =temp.ScheduleAddTime ,
                    ScheduleUpdateTime = temp.ScheduleUpdateTime,
                    ScheduleText = temp.ScheduleText,
                    ScheduleTypeID = temp.ScheduleTypeID,
                    TextReadBak = temp.TextReadBak,
                    TextReadUser = temp.TextReadUser,
                    TextReadTime = temp.TextReadTime,
                    FileItemID = temp.FileItemID
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //判断是否有权审核
        public ActionResult GetCanOrCant()
        {
            List<Uidorname> list = GetAllDownUser();
            string readname = Request["id"];
            string str = Request["state"];
            string name = Request["ExamineUser"];
            if (readname == LoginUser.PerSonName)
            {
                return Json(new { ret = "no", msg = "本人无权审核自己的意见！" }, JsonRequestBehavior.AllowGet);
            }
            if (str == "已审核")
            {
                if (name != LoginUser.PerSonName)
                {
                    foreach (var a in list)
                    {
                        if (a.name == name)
                        {
                            return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return Json(new { ret = "no", msg = "你无权对此条日程再审核！" }, JsonRequestBehavior.AllowGet);
                } else
                {
                    return Json(new { ret = "no", msg = "你不能对自己已审核的日程再审核！" }, JsonRequestBehavior.AllowGet);
                }
            } else
            {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }

        //获取全部日程相关计数信息
        public ActionResult GetNumber()
        {
            List<Uidorname> list = GetAllDownUser();
            List<Schedule> schsumlist = new List<Schedule>();
            foreach (var a in list)
            {
                var temp = ScheduleService.LoadEntities(x => x.UserID == a.ID).DefaultIfEmpty().ToList();
                if (temp != null && temp[0] != null)
                {
                    schsumlist.AddRange(temp);
                }
            }
            List<Schedule> nochecklist = new List<Schedule>();
            List<Schedule> checkedlist = new List<Schedule>();
            int num = 0;
            foreach (var b in schsumlist)
            {
                if (b != null) {
                    if (b.TextReadBak == "未查阅")
                    {
                        nochecklist.Add(b);
                    } else
                    {
                        checkedlist.Add(b);
                        foreach (var a in list)
                        {
                            if (b.TextReadUser == a.ID) {
                                var rtmp = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                                if (rtmp == null || rtmp[0] == null)
                                {
                                    num += 1;
                                }
                            }
                        }
                    }
                } else
                {
                    continue;
                }
            }

            var scheduleSum = schsumlist.Count;//日程总数
            var nocheckNum = nochecklist.Count;//未查阅数量
            var checkedNum = checkedlist.Count;//已查阅数量
            var examinedNum = num;//未审核数量
            return Json(new { scum = scheduleSum, nnum = nocheckNum, cnum = checkedNum, exnum = examinedNum }, JsonRequestBehavior.AllowGet);
        }

        //获取全部日程相关计数信息
        public ActionResult GetBMNumber()
        {
            int id = Convert.ToInt32(Request["id"]);
            List<Uidorname> list = GetAllDownUser();
            List<int> listint = new List<int>();
            List<Schedule> schsumlist = new List<Schedule>();
            List<Schedule> nochecklist = new List<Schedule>();
            List<Schedule> checkedlist = new List<Schedule>();
            foreach (var a in list)
            {
                var ub = ScheduleService.LoadEntities(x => x.UserID == a.ID && x.UserInfo.BuMenID == id).DefaultIfEmpty().ToList();
                if (ub != null && ub[0] != null)
                {
                    schsumlist.AddRange(ub);
                }
            }
            int num = 0;
            foreach (var b in schsumlist)
            {
                if (b != null)
                {
                    if (b.TextReadBak == "未查阅")
                    {
                        nochecklist.Add(b);
                    }
                    else
                    {
                        checkedlist.Add(b);
                        foreach (var a in list)
                        {
                            if (b.TextReadUser == a.ID)
                            {
                                var rtmp = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                                if (rtmp == null || rtmp[0] == null)
                                {
                                    num += 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            var scheduleSum = schsumlist.Count;//部门日程总数
            var nocheckNum = nochecklist.Count;//部门未查阅数量
            var checkedNum = checkedlist.Count;//部门已查阅数量
            var examinedNum = num;//部门未审核数量
            return Json(new { scum = scheduleSum, nnum = nocheckNum, cnum = checkedNum, exnum = examinedNum }, JsonRequestBehavior.AllowGet);
        }

        //查询所有此时间段内的日程
        public ActionResult FindAll()
        {
            var startTime = Convert.ToDateTime(Request["startTime"]);
            var endTime = Convert.ToDateTime(Request["endTime"]);
            List<Uidorname> list = GetAllDownUser();
            List<Schedule> listsch1 = new List<Schedule>();
            List<Schedule> listsch2 = new List<Schedule>();
            foreach (var l in list)
            {
                var temp = ScheduleService.LoadEntities(x => x.TextReadBak != "未查阅" && x.UserID == l.ID).DefaultIfEmpty().ToList();
                if (temp == null || temp[0] == null)
                {
                    continue;
                }
                else
                {
                    listsch1.AddRange(temp);
                }
            }
            foreach (var ls in listsch1)
            {
                foreach (var l in list)
                {
                    if (ls.TextReadUser == LoginUser.ID || ls.TextReadUser == l.ID)
                    {
                        listsch2.Add(ls);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            List<ExamineForSchedule> listsch3 = new List<ExamineForSchedule>();
            foreach (var a in listsch2)
            {
                ExamineForSchedule efs = new ExamineForSchedule();
                efs.ID = a.ID;
                efs.UserID = a.UserID;
                efs.ScheduleTime = a.ScheduleTime;
                efs.ScheduleAddTime = a.ScheduleAddTime;
                efs.ScheduleUpdateTime = a.ScheduleUpdateTime;
                efs.ScheduleText = a.ScheduleText;
                efs.ScheduleTypeID = a.ScheduleTypeID;
                efs.TextReadBak = a.TextReadBak;
                efs.TextReadUser = a.TextReadUser;
                efs.TextReadTime = a.TextReadTime;
                efs.FileItemID = a.FileItemID;
                efs.UserInfo = a.UserInfo;
                efs.UserInfo1 = a.UserInfo1;
                listsch3.Add(efs);
            }
            List<ExamineForSchedule> exsh1 = new List<ExamineForSchedule>();
            List<ExamineForSchedule> exsh2 = new List<ExamineForSchedule>();
            if (listsch3.Count > 0)
            {
                foreach (var b in listsch3)
                {
                    if (b.ScheduleTime > startTime && b.ScheduleTime < endTime) {
                        var es = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                        if (es == null || es[0] == null)
                        {
                            exsh1.Add(b);
                        }
                        else
                        {
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    b.ExamineText = e.ExamineText;
                                    b.ExamineUser = e.UserInfo.PerSonName;
                                    exsh2.Add(b);
                                }

                            }
                        }
                    }else
                    {
                        continue;
                    }
                }
                var Rtmp1 = from a in exsh1
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                ExamineState = "未审核",
                                ExamineText = "",
                                ExamineUser = ""
                            };
                var Rtmp2 = from a in exsh2
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                ExamineState = "已审核",
                                ExamineText = a.ExamineText,
                                ExamineUser = a.ExamineUser
                            };
                var Rtmp = Rtmp1.Union(Rtmp2);
                return Json(new { rows = Rtmp, total = Rtmp.Count() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //查询所有此时间段内该部门下的日程
        public ActionResult FindAll2()
        {
            var BMID = Request["BMID"] != "" && Request["BMID"] != null ? Convert.ToInt32(Request["BMID"]) : 0;
            var BMUID = Request["BMUID"] != "" && Request["BMUID"] != null ? Convert.ToInt32(Request["BMUID"]) : 0;
            var startTime = Convert.ToDateTime(Request["startTime"]);
            var endTime = Convert.ToDateTime(Request["endTime"]);
            List<Uidorname> list = GetAllDownUser();
            List<Schedule> listsch1 = new List<Schedule>();
            List<Schedule> listsch2 = new List<Schedule>();
            foreach (var l in list)
            {
                var temp = ScheduleService.LoadEntities(x => x.TextReadBak != "未查阅" && x.UserID == l.ID).DefaultIfEmpty().ToList();
                if (temp == null || temp[0] == null)
                {
                    continue;
                }
                else
                {
                    listsch1.AddRange(temp);
                }
            }
            foreach (var ls in listsch1)
            {
                foreach (var l in list)
                {
                    if (ls.TextReadUser == LoginUser.ID || ls.TextReadUser == l.ID)
                    {
                        listsch2.Add(ls);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (BMUID > 0)
            {
                for(int i =0;i<listsch2.Count;i++)
                {
                    if (listsch2[i].UserID != BMUID)
                    {
                        listsch2.Remove(listsch2[i]);
                    }else
                    {
                        continue;
                    }
                }
            }else
            {
                for (int i = 0; i < listsch2.Count; i++)
                {
                    if (listsch2[i].UserInfo.BuMenID != BMID)
                    {
                        listsch2.Remove(listsch2[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            List<ExamineForSchedule> listsch3 = new List<ExamineForSchedule>();
            foreach (var a in listsch2)
            {
                ExamineForSchedule efs = new ExamineForSchedule();
                efs.ID = a.ID;
                efs.UserID = a.UserID;
                efs.ScheduleTime = a.ScheduleTime;
                efs.ScheduleAddTime = a.ScheduleAddTime;
                efs.ScheduleUpdateTime = a.ScheduleUpdateTime;
                efs.ScheduleText = a.ScheduleText;
                efs.ScheduleTypeID = a.ScheduleTypeID;
                efs.TextReadBak = a.TextReadBak;
                efs.TextReadUser = a.TextReadUser;
                efs.TextReadTime = a.TextReadTime;
                efs.FileItemID = a.FileItemID;
                efs.UserInfo = a.UserInfo;
                efs.UserInfo1 = a.UserInfo1;
                listsch3.Add(efs);
            }
            List<ExamineForSchedule> exsh1 = new List<ExamineForSchedule>();
            List<ExamineForSchedule> exsh2 = new List<ExamineForSchedule>();
            if (listsch3.Count > 0)
            {
                foreach (var b in listsch3)
                {
                    if (b.ScheduleTime > startTime && b.ScheduleTime < endTime)
                    {
                        var es = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                        if (es == null || es[0] == null)
                        {
                            exsh1.Add(b);
                        }
                        else
                        {
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    b.ExamineText = e.ExamineText;
                                    b.ExamineUser = e.UserInfo.PerSonName;
                                    exsh2.Add(b);
                                }

                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                var Rtmp1 = from a in exsh1
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                ExamineState = "未审核",
                                ExamineText = "",
                                ExamineUser = ""
                            };
                var Rtmp2 = from a in exsh2
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                ExamineState = "已审核",
                                ExamineText = a.ExamineText,
                                ExamineUser = a.ExamineUser
                            };
                var Rtmp = Rtmp1.Union(Rtmp2);
                return Json(new { rows = Rtmp, total = Rtmp.Count() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //查询所有此时间段内该员工的日程
        public ActionResult FindAll3()
        {
            var DUID = Request["DUID"] != "" && Request["DUID"] != null ? Convert.ToInt32(Request["DUID"]) : 0;
            var startTime = Convert.ToDateTime(Request["startTime"]);
            var endTime = Convert.ToDateTime(Request["endTime"]);
            List<Uidorname> list = GetAllDownUser();
            List<Schedule> listsch1 = new List<Schedule>();
            List<Schedule> listsch2 = new List<Schedule>();
            foreach (var l in list)
            {
                var temp = ScheduleService.LoadEntities(x => x.TextReadBak != "未查阅" && x.UserID == l.ID).DefaultIfEmpty().ToList();
                if (temp == null || temp[0] == null)
                {
                    continue;
                }
                else
                {
                    listsch1.AddRange(temp);
                }
            }
            foreach (var ls in listsch1)
            {
                foreach (var l in list)
                {
                    if (ls.TextReadUser == LoginUser.ID || ls.TextReadUser == l.ID)
                    {
                        listsch2.Add(ls);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (DUID > 0)
            {
                for (int i = 0; i < listsch2.Count; i++)
                {
                    if (listsch2[i].UserID != DUID)
                    {
                        listsch2.Remove(listsch2[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            List<ExamineForSchedule> listsch3 = new List<ExamineForSchedule>();
            foreach (var a in listsch2)
            {
                ExamineForSchedule efs = new ExamineForSchedule();
                efs.ID = a.ID;
                efs.UserID = a.UserID;
                efs.ScheduleTime = a.ScheduleTime;
                efs.ScheduleAddTime = a.ScheduleAddTime;
                efs.ScheduleUpdateTime = a.ScheduleUpdateTime;
                efs.ScheduleText = a.ScheduleText;
                efs.ScheduleTypeID = a.ScheduleTypeID;
                efs.TextReadBak = a.TextReadBak;
                efs.TextReadUser = a.TextReadUser;
                efs.TextReadTime = a.TextReadTime;
                efs.FileItemID = a.FileItemID;
                efs.UserInfo = a.UserInfo;
                efs.UserInfo1 = a.UserInfo1;
                listsch3.Add(efs);
            }
            List<ExamineForSchedule> exsh1 = new List<ExamineForSchedule>();
            List<ExamineForSchedule> exsh2 = new List<ExamineForSchedule>();
            if (listsch3.Count > 0)
            {
                foreach (var b in listsch3)
                {
                    if (b.ScheduleTime > startTime && b.ScheduleTime < endTime)
                    {
                        var es = ExamineScheduleService.LoadEntities(x => x.ScheduleID == b.ID).DefaultIfEmpty().ToList();
                        if (es == null || es[0] == null)
                        {
                            exsh1.Add(b);
                        }
                        else
                        {
                            foreach (var e in es)
                            {
                                if (e != null)
                                {
                                    b.ExamineText = e.ExamineText;
                                    b.ExamineUser = e.UserInfo.PerSonName;
                                    exsh2.Add(b);
                                }

                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                var Rtmp1 = from a in exsh1
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                ExamineState = "未审核",
                                ExamineText = "",
                                ExamineUser = ""
                            };
                var Rtmp2 = from a in exsh2
                            select new
                            {
                                ID = a.ID,
                                UserID = a.UserInfo.PerSonName,
                                ScheduleTime = a.ScheduleTime,
                                TextReadBak = a.TextReadBak,
                                TextReadUser = a.UserInfo1.PerSonName,
                                TextReadTime = a.TextReadTime,
                                ExamineState = "已审核",
                                ExamineText = a.ExamineText,
                                ExamineUser = a.ExamineUser
                            };
                var Rtmp = Rtmp1.Union(Rtmp2);
                return Json(new { rows = Rtmp, total = Rtmp.Count() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //——————————————————日程总汇————————————————————————

        //循环领导分级


        public void AgainAndAgain(List<LeaderFJ> listLFJ, List<LeaderFJ> UpYiJiLeader)
        {
            List<LeaderFJ> CiJiLeader = new List<LeaderFJ>();
          
            foreach (var a in UpYiJiLeader)
            {
                var temp = ScheduleUserService.LoadEntities(x => x.UpID == a.ID).DefaultIfEmpty().ToList();
                if (temp != null && temp[0] != null)
                {
                    foreach (var b in temp)
                    {
                        LeaderFJ lfj = new LeaderFJ();
                        lfj.ID = b.UserID;
                        lfj.name = b.UserInfo.PerSonName;
                        lfj.UpID = b.UpID;
                        lfj.DJ = DJ;
                        CiJiLeader.Add(lfj);
                        foreach (var c in listLFJ)
                        {
                            if(c.ID == lfj.ID)
                            {
                                c.DJ = DJ;
                                c.UpID = b.UpID;
                                break;
                            }
                        }
                    }
                }
            }
            DJ += 1;
            if (listLFJ.Any(x => x.DJ == 10))
            {
                AgainAndAgain(listLFJ, CiJiLeader);
            }
        }
        
        //获取所有下级领导
        public ActionResult GetAllDownLeader()
        {
            //获取当前登陆人员的所有下属信息
            List<Uidorname> list = GetAllDownUser();
            //有下属信息
            List<LeaderFJ> leader = new List<LeaderFJ>();
            //无下属信息，赋予上级信息
            List<LeaderFJ> BottomUser = new List<LeaderFJ>();
            var Loads_ = ScheduleUserService.LoadEntities(x => x.Del == null);
            GetUpDownUserID(list, leader, BottomUser, Loads_);

            if (list != null && list[0] != null)
            {

                List<LeaderFJ> SYLeader = new List<LeaderFJ>();
                SYLeader = GetVoidLeader(leader, BottomUser).ToList();
                List<LeaderFJ> new1 = new List<LeaderFJ>();
                foreach (var a in SYLeader)
                {
                    int? ID = a.ID;
                    int DJ = a.DJ;
                    string name = a.name;
                    int SerchType = a.SerchType;
                    int? UpID = a.UpID;
                    LeaderFJ lfj = new LeaderFJ();
                    lfj.ID = ID;
                    lfj.DJ = DJ;
                    lfj.name = name;
                    var one = Loads_.Where(x => x.UserID == lfj.ID).FirstOrDefault();
                    if (one != null && lfj.DJ != 1)
                    {
                        lfj.UpID = one.UpID;
                    }
                    else
                    {
                        lfj.UpID = 1;
                    }
                    lfj.SerchType = 1;
                    new1.Add(lfj);
                }
                new1.AddRange(SYLeader);
                if (new1.Count < 1 || new1[0] == null)
                {
                    foreach (var a in BottomUser)
                    {
                        a.DJ = 1;
                        a.UpID = 1;
                    }
                }
                new1.AddRange(BottomUser);
                return Json(new1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取下属信息 分类方法
        /// </summary>
        /// <param name="list">所有下属信息</param>
        /// <param name="leader">返回有下属的ID</param>
        /// <param name="BottomUser">底层ID</param>
        /// <param name="Loads_"></param>
        private static void GetUpDownUserID(List<Uidorname> list, List<LeaderFJ> leader, List<LeaderFJ> BottomUser, IQueryable<ScheduleUser> Loads_)
        {
            if (list != null && list[0] != null)
            {
                foreach (var a in list)
                {
                    var rtmp = Loads_.Where(x => x.UpID == a.ID).DefaultIfEmpty().ToList();
                    if (rtmp != null && rtmp[0] != null)
                    {
                        //有下级进入方法
                        LeaderFJ lfj = new LeaderFJ();
                        lfj.ID = a.ID;
                        lfj.name = a.name;
                        lfj.DJ = 0;
                        leader.Add(lfj);
                    }
                    else
                    {
                        LeaderFJ lfj = new LeaderFJ();
                        lfj.ID = a.ID;
                        lfj.name = a.name;
                        lfj.DJ = 10;
                        lfj.SerchType = 1;
                        var one = Loads_.Where(x => x.UserID == a.ID).FirstOrDefault();
                        if (one != null)
                        {
                            lfj.UpID = one.UpID;
                        }
                        BottomUser.Add(lfj);
                    }
                }
            }
        }

        private List<LeaderFJ> GetVoidLeader(List<LeaderFJ> leader, List<LeaderFJ> BottomUser)
        {
            List<LeaderFJ> SYLeader = new List<LeaderFJ>();
            List<LeaderFJ> firstLeader = new List<LeaderFJ>();
            var loadchens = ScheduleUserService.LoadEntities(x => x.Del == null).DefaultIfEmpty();
            foreach (var a in leader)
            {
                if (a != null)
                {
                    var zaipanduan = loadchens.Where(x => x.UserID == a.ID && x.UpID == LoginUser.ID).DefaultIfEmpty().ToList();
                    if (zaipanduan != null && zaipanduan[0] != null)
                    {
                        a.DJ = 1;
                        a.UpID = 1;
                        DJ += 1;
                        firstLeader.Add(a);
                    }
                    else
                    {
                        
                        int Thisdj = 10;
                        int? thisUpid = null;
                        var Thispd = loadchens.Where(x => x.UserID == a.ID).DefaultIfEmpty().ToList();
                        foreach (var aa in Thispd) {
                            var ler = leader.Where(x => x.ID == aa.UpID).FirstOrDefault();
                            
                            Thisdj = ler!=null?ler.DJ + 1 : 10;
                            thisUpid = ler != null ? ler.ID:null;
                        }
                        
                        a.DJ = Thisdj;
                        if (a.DJ == 10)
                        {
                            SYLeader.Add(a);
                        }
                        else
                        {
                            a.UpID = thisUpid;
                            firstLeader.Add(a);
                        }                        
                    }
                }
                else
                {
                    continue;
                }
            }

            AgainAndAgain(SYLeader, firstLeader);

            List<LeaderFJ> llfj = new List<LeaderFJ>();
            llfj.AddRange(firstLeader);
            llfj.AddRange(SYLeader);
            foreach (var a in llfj)
            {
                a.SerchType = 2;
                if(a.DJ == 1)
                {
                    a.UpID = 2;
                }
            }
            SYLeader.AddRange(firstLeader);
            SYLeader.AddRange(BottomUser);
            return llfj;
        }


        //获取日程信息给信息汇总的表
        public ActionResult GetScheduleInfoForTab()
        {
            //
            string ScheduleSumStr = "";
            List<SchInfoForTab> Lsift = new List<SchInfoForTab>();
            string time = Request["time"];
            time = time.Replace("年", "-");
            time = time.Replace("月", "-");
            time = time.Replace("日", "");
            DateTime dt = Convert.ToDateTime(time);
            DateTime end = dt.AddDays(1);

            if (Request["anhao"]=="" || Request["anhao"]==null || Convert.ToInt32(Request["anhao"])!=9) {
                
                string idstr = Request["id"];
                //获取底层个人数据
                if (!idstr.Contains("info")) {
                    #region MyRegion
                    int id = Convert.ToInt32(Request["id"]);
                    var temp = ScheduleService.LoadEntities(x => x.UserID == id && x.ScheduleTime > dt && x.ScheduleTime < end).DefaultIfEmpty().ToList();
                    var ScheduleSum = temp.Where((x, i) => temp.FindIndex(z => z.ScheduleTypeID == x.ScheduleTypeID) == i).ToList();
                   
                    foreach (var a in ScheduleSum)
                    {
                        var rtmp = temp.Where(x => x.ScheduleTypeID == a.ScheduleTypeID).DefaultIfEmpty().ToList();
                        if (rtmp != null && rtmp[0] != null)
                        {
                            ScheduleSumStr = ScheduleSumStr + a.ScheduleType.ItemText + ":<b>" + rtmp.Count + "</b> ";
                        }
                    }
                    if (temp != null && temp[0] != null)
                    {
                        foreach (var a in temp)
                        {
                            SchInfoForTab sift = new SchInfoForTab();
                            sift.ScheduleTime = a.ScheduleTime;
                            sift.ScheduleText = a.ScheduleText;
                            sift.ScheduleTypeName = a.ScheduleType.ItemText;
                            sift.ScheduleID = a.ID;
                            sift.FileItemID = a.FileItemID;
                            sift.TextReadBak = a.TextReadBak;
                            sift.Name = a.UserInfo.PerSonName;
                            Lsift.Add(sift);
                        }
                      
                    }
                    else
                    {
                        return null;
                    }
                    #endregion
                }
                else//获取部门日总结
                {
                    #region MyRegion
                    int id = Convert.ToInt32(Request["id"].Replace("info", ""));                   
                    var Isnull= GetDayZongjie(id,ref Lsift,ref ScheduleSumStr, dt, end);
                    if (Isnull == null)
                    {
                        return null;
                    }                   
                    #endregion
                }
            }else
            {
                var Isnull = GetDayZongjie(LoginUser.ID, ref Lsift, ref ScheduleSumStr, dt, end);
                if (Isnull == null)
                {
                    return null;
                }              
           
            }
            return Json(new { lsift = Lsift, scheduleSumStr = ScheduleSumStr }, JsonRequestBehavior.AllowGet);
        }

        private object GetDayZongjie(int? id, ref List<SchInfoForTab> Lsift,ref string ScheduleSumStr,DateTime dt,DateTime end) {
            var tempSUser = ScheduleUserService.LoadEntities(x => x.UpID == id).DefaultIfEmpty();
            List<Schedule> schlist = new List<Schedule>();
            foreach (var a in tempSUser)
            {
                var ss1 = ScheduleService.LoadEntities(x => x.UserID == a.UserID && x.ScheduleTime > dt && x.ScheduleTime < end).ToList();
                if (ss1.Count > 0) {
                    schlist.AddRange(ss1);
                }
                //var ss = ScheduleService.LoadEntities(x => x.UserID == a.UserID && x.ScheduleTime > dt && x.ScheduleTime < end).DefaultIfEmpty().ToList();
                
            }
            var mstdata = ScheduleService.LoadEntities(x => x.UserID == id && x.ScheduleTime > dt && x.ScheduleTime < end).ToList();
            if (mstdata.Count > 0)
            {
                schlist.AddRange(mstdata);
            }
          
            //List<Schedule> schlist = list;
            if (schlist == null && schlist[0] == null)
            {
                return null;
            }
            var ScheduleSum = schlist.Where((x, i) => schlist.FindIndex(z => z.ScheduleTypeID == x.ScheduleTypeID) == i).ToList();

            foreach (var a in ScheduleSum)
            {
                var rtmp = schlist.Where(x => x.ScheduleTypeID == a.ScheduleTypeID).DefaultIfEmpty().ToList();
                if (rtmp != null && rtmp[0] != null)
                {
                    ScheduleSumStr = ScheduleSumStr + a.ScheduleType.ItemText + ":<b>" + rtmp.Count + "</b> ";
                }
            }

            foreach (var a in schlist)
            {
                SchInfoForTab sift = new SchInfoForTab();
                sift.ID= a.ID;
                sift.ScheduleTime = a.ScheduleTime;
                sift.ScheduleText = a.ScheduleText;
                sift.ScheduleTypeName = a.ScheduleType.ItemText;
                sift.ScheduleID = a.ID;
                sift.FileItemID = a.FileItemID;
                sift.Name = a.UserInfo.PerSonName;
                sift.TextReadBak = a.TextReadBak;
                Lsift.Add(sift);
            }
            return true;
        }
        //获取下级领导所批阅的信息
        public ActionResult GetScheduleInfoForTab2()
        {
            int id = Convert.ToInt32(Request["id"].Replace("info", ""));
            string time = Request["time"];
            time = time.Replace("年", "-");
            time = time.Replace("月", "-");
            time = time.Replace("日", "");
            DateTime dt = Convert.ToDateTime(time);
            var DownLeader = ScheduleUserService.LoadEntities(x => x.UpID == id).DefaultIfEmpty().ToList();
            List<DownLeaderForTab> list = new List<DownLeaderForTab>();
            foreach(var a in DownLeader)
            {
                DownLeaderForTab dlft = new DownLeaderForTab();
                dlft.ID = a.UserID;
                dlft.Name = a.UserInfo.PerSonName;
                var temp = YJ_ScheduleDayService.LoadEntities(x => x.YJUserinfoID == a.UserID && x.SchenuleTime == dt).FirstOrDefault();
                if (temp != null)
                {
                    if (temp.YJText != null)
                    {
                        dlft.Text = "<b>"+ temp.YJText +"</b>";
                    }else
                    {
                        dlft.Text = "<b>已阅！</b>";
                    }
                }else
                {
                    dlft.Text = "<laber style='color:silver'>无日总结信息！</laber>";
                }
                list.Add(dlft);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //获取点击已阅人员
        public ActionResult YiYueBtnFunc()
        {
            string time = Request["time"];
            time = time.Replace("年", "-");
            time = time.Replace("月", "-");
            time = time.Replace("日", "");
            DateTime dt = Convert.ToDateTime(time);
            string rtr = YJ_ScheduleDayService.GetReadPerson(dt,0);
            var ThisDat= YJ_ScheduleDayService.LoadEntities(x => x.DEL == 0 && x.SchenuleTime == dt&&x.YJText!=null).DefaultIfEmpty();
          
            var temp = from a in ThisDat
                       select new {
                           a.ID,
                           a.TextID,
                           a.AddYJtime,
                           a.ISee,
                           a.ISeeAddtime,
                           a.SchenuleTime,
                           a.SeeUserInfoList,
                           a.WriteUserID,
                           a.YJText,
                           YJUserinfoIDname = a.UserInfo1.PerSonName,
                           a.YJUserinfoID,
                       };
            if (ThisDat.ToList()[0] == null)
            {
                temp = null;
                return Json(new { rtr = rtr, temp = temp }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<YJ_DAY> lyjd = new List<YJ_DAY>();
                foreach (var a in temp)
                {
                    YJ_DAY yjl = new YJ_DAY();
                    yjl.ID = a.ID;
                    yjl.TextID = a.TextID;
                    yjl.AddYJtime = a.AddYJtime;
                    yjl.ISee = a.ISee;
                    yjl.ISeeAddtime = a.ISeeAddtime;
                    yjl.SchenuleTime = a.SchenuleTime;
                    yjl.SeeUserInfoList = a.SeeUserInfoList;
                    yjl.WriteUserID = a.WriteUserID;
                    yjl.YJText = a.YJText;
                    yjl.YJUserinfoIDname = a.YJUserinfoIDname;
                    yjl.YJUserinfoID = a.YJUserinfoID;
                    yjl.uPjyname = a.WriteUserID == null ? "" : YJ_ScheduleDayService.LoadEntities(x => x.ID == a.WriteUserID).First().UserInfo1.PerSonName;
                    if (a.SeeUserInfoList != null)
                    {
                        var namearry = a.SeeUserInfoList.Split(',') ;
                        foreach (var sr in namearry)
                        {
                            if (sr.Length <= 0)
                            {
                                continue;
                            }
                            int? strid = Convert.ToInt32(sr);
                            var name= UserInfoService.LoadEntities(x => x.ID == strid).FirstOrDefault().PerSonName;
                            yjl.thislistname = yjl.thislistname + name + ",";
                        }
                    }
                    lyjd.Add(yjl);
                }
                return Json(new { rtr = rtr, temp = lyjd }, JsonRequestBehavior.AllowGet);
            }
            
           
          
        }

        //给予意见
        public ActionResult GeiYJBtnFunc()
        {
            int? id = Convert.ToInt32(Request["id"]);
            string time = Request["time"];
            time = time.Replace("年", "-");
            time = time.Replace("月", "-");
            time = time.Replace("日", "");
            DateTime dt = Convert.ToDateTime(time);
           
            
            bool yesorno = Convert.ToBoolean(Request["yesorno"]);
            //yesorno  等于 false 是已阅按钮
            if (!yesorno) {
                if (id != 0)
                {
                    var thisid = YJ_ScheduleDayService.LoadEntities(X =>  X.YJUserinfoID == LoginUser.ID && X.ID == id).FirstOrDefault();
                    if (thisid != null)
                    {
                        return Json(new { ret = "dcyy" }, JsonRequestBehavior.AllowGet);
                    }
                    //写入新信息之前判断 不可重复给予建议
                    var thisfst = YJ_ScheduleDayService.LoadEntities(x => x.ID == id).FirstOrDefault();
                    if (thisfst != null)
                    {
                        if (thisfst.SeeUserInfoList != null)
                        {
                            var spt = thisfst.SeeUserInfoList.Split(',');
                            if (spt.Contains(LoginUser.ID.ToString()))
                            {
                                { return Json(new { ret = "不可重复提交信息" }, JsonRequestBehavior.AllowGet); }
                            }
                        }
                    }
                    else
                    {
                        { return Json(new { ret = "没有找到要给予信息的ID" }, JsonRequestBehavior.AllowGet); }
                    }
                }
                else {
                    var thisid = YJ_ScheduleDayService.LoadEntities(X => X.SchenuleTime == dt && X.YJUserinfoID == LoginUser.ID && X.YJText == null).FirstOrDefault();
                    if (thisid != null)
                    {
                        return Json(new { ret = "dcyy" }, JsonRequestBehavior.AllowGet);
                    }
                }
                
            }
            string yj = Request["yj"];
            YjsdayClass yc = new YjsdayClass();
            yc.NewAdditem = false;
            if (id != 0)
            {
                var cos = YJ_ScheduleDayService.LoadEntities(X => X.ID == id).FirstOrDefault();
                if (cos == null)
                {
                    yc.NewAdditem = true;
                }
                else
                {
                    int? thisid = YJ_ScheduleDayService.LoadEntities(X => X.ID == id).FirstOrDefault().YJUserinfoID;
                    if (thisid == LoginUser.ID)
                    {
                        return Json(new { ret = "Isdistc" }, JsonRequestBehavior.AllowGet);
                    }
                }
                        
            }
            yc.YesOrNo = true;
            YJ_ScheduleDay ysdy = new YJ_ScheduleDay();
            ysdy.AddYJtime = DateTime.Now;
            ysdy.SchenuleTime = dt;
            ysdy.ISee = true;
            ysdy.YJUserinfoID = LoginUser.ID;
            ysdy.YJText = yj.Length>0?yj:null;
            ysdy.ISeeAddtime = DateTime.Now;
            ysdy.WriteUserID = id==0?null:id;
            yc.Ysdy = ysdy;
            yc.IFours = yesorno;
            
            if (YJ_ScheduleDayService.NewAddSEDDAY(yc))
            {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }
        //返回提示信息
        public ActionResult GetTimesTS() {

            var time = DateTime.Now;
            int MaxInt = DateTime.DaysInMonth(time.Year, time.Month);
            DateTime Utime = new DateTime(time.Year, time.Month, 1);
            DateTime Dtime = new DateTime(time.Year, time.Month, MaxInt);

            List<Uidorname> list = GetAllDownUser();
            //有下属信息
            List<LeaderFJ> leader = new List<LeaderFJ>();
            //无下属信息，赋予上级信息
            List<LeaderFJ> BottomUser = new List<LeaderFJ>();
            GetUpDownUserID(list, leader, BottomUser, ScheduleUserService.LoadEntities(x => x.Del == null));

            List<RetDayCount> lrdc = new List<RetDayCount>();

            var temp = YJ_ScheduleDayService.LoadEntities(x => x.SchenuleTime >= Utime && x.SchenuleTime <= Dtime).DefaultIfEmpty().ToList();
            if (temp[0] == null)
            {
                return Json(new { ter = lrdc }, JsonRequestBehavior.AllowGet);
            }
            //按时间循环
            for (int i = 0; i < MaxInt; i++) {
               
                DateTime FDtime = new DateTime(time.Year, time.Month, i+1);
                RetDayCount rdc = new RetDayCount();
                rdc.time = FDtime;
                rdc.day = i+1;
                rdc.LdcPerson = new List<DCperson>();
                foreach (var l in leader) {
                    #region MyRegion
                    DCperson dp = new DCperson();
                   
                    var TempC = temp.Where(x => x.SchenuleTime == FDtime && x.YJUserinfoID == l.ID&&x.TextID!=null&&x.TextID>=0).DefaultIfEmpty().ToList();
                    //if (i == 2&&l.name=="张廷宇")
                    //{
                    //    string ss = "";
                    //}
                    if (TempC.Count() > 0)
                    {
                        if (TempC[0] != null)
                        {
                            dp.Person = TempC[0].UserInfo1.PerSonName;
                            dp.count = 0;
                            foreach (var pc in TempC)
                            {
                                string s = pc.SeeUserInfoList;
                                if (s == null)
                                {
                                    dp.count++;
                                }
                                else
                                {
                                    var Sst = s.Split(',');
                                    if (!Sst.Contains(LoginUser.ID.ToString()))
                                    {
                                        dp.count++;
                                    }
                                }
                            }
                            if (dp.count > 0) {
                                rdc.LdcPerson.Add(dp);
                            }
                            
                        }
                    }
                    #endregion
                }
                //找到当天我创建的信息
                if (temp[0] == null)
                {
                    return Json(new { ter = lrdc }, JsonRequestBehavior.AllowGet);
                }              
                var TempCc = temp.Where(x => x.SchenuleTime == FDtime && x.YJUserinfoID == LoginUser.ID&&x.TextID>=-1).DefaultIfEmpty().ToList();
                if (TempCc[0] != null) {
                    List<DCperson> dp = new List<DCperson>();
                    foreach (var upjy in TempCc)
                    {
                        if (upjy.YJ_ScheduleAction1.All(x => x.YJ_ScheduleDay!= null)) {
                            var tts = upjy.YJ_ScheduleAction1.Where(x => x.YJ_ScheduleDay.DEL==0).ToList();
                            foreach (var at in tts) {
                                var splic= at.YJ_ScheduleDay.SeeUserInfoList != null ? at.YJ_ScheduleDay.SeeUserInfoList.Split(',') : null;
                                if (splic != null) {
                                    if (splic.Contains(LoginUser.ID.ToString()))
                                    {
                                        continue;
                                    }
                                }                                
                                var lyj=leader.Where(sc => at.YJ_ScheduleDay.YJUserinfoID == sc.ID).ToList();
                                if (lyj.Count==0) {
                                   var dpw= dp.Where(x => x.Person == at.YJ_ScheduleDay.UserInfo1.PerSonName).FirstOrDefault();
                                    if (dpw != null)
                                    {
                                        dpw.count++;
                                    }
                                    else
                                    {
                                        DCperson dp_0 = new DCperson();
                                        dp_0.Person = at.YJ_ScheduleDay.UserInfo1.PerSonName;
                                        dp_0.count=1;
                                        dp.Add(dp_0);
                                    }
                                }
                                
                            }
                        }
                    }
                    rdc.LdcPerson.AddRange(dp);
                }
                
                
                lrdc.Add(rdc);
            }
            
            return Json(new { ter = lrdc }, JsonRequestBehavior.AllowGet);
        }

    }

    //下拉菜单下级用户类
    public class Uidorname {
        public int ID { get; set; }
        public string name { get; set; }
    }

    //获取所有文件的类
    public class AllFile
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public int FirstFileID { get; set; }
    }

    public class BuMen
    {
        public int BMID { get; set; }
        public string Name { get; set; }
    }

    public class LeaderFJ
    {
        public int? ID { get; set; }
        public string name { get; set; }
        public int? UpID { get; set; }
        public int DJ { get; set; }
        public int SerchType { get; set; }
    }
    public class SchInfoForTab
    {
        public long ID { get; set; }
        public DateTime ScheduleTime { get; set; }
        public string ScheduleText { get; set;}
        public string ScheduleTypeName { get; set; }
        public long ScheduleID { get; set; }
        public int? FileItemID { get; set; }
        public string Name { get; set; } 
        public string SchTypeNum { get; set; }
        public string TextReadBak { get; set; }
    }

    public class DownLeaderForTab
    {
        public string Name { get; set; }
        public int? ID { get; set; }
        public string Text { get; set; }
    }
}
