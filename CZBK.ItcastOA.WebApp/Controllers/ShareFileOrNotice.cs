using CZBK.ItcastOA.BLL;
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
    public class ShareFileOrNoticeController : BaseController
    {
        IBLL.IShareFileOrNoticeService ShareFileOrNoticeService;
        IBLL.IShareTypeService ShareTypeService;
        IBLL.IBumenInfoSetService BumenInfoSetService;
        IBLL.IUserInfoService UserInfoService;
        IBLL.IFileTypeService FileTypeService;
        
        // GET: /ShareFileOrNotice/

        public ActionResult Index()
        {
            return View();
        }
        //获取共享文件
        public ActionResult GetShareFile()
        {
            int userID = LoginUser.ID;

            return Json(null,JsonRequestBehavior.AllowGet);
        }
        //添加共享文件
        public ActionResult AddShareFileDIV(ShareFileOrNotice sfon)
        {
            sfon.ShareUser = LoginUser.ID;
            sfon.ShareToUser = Request["STUstrName"];
            sfon.FileURL = Request["FileUrlID"];
            sfon.UploadFileTime = DateTime.Now;
            sfon.TypeID = 1;
            ShareFileOrNoticeService.AddEntity(sfon);
            return Json(new { ret = "ok"}, JsonRequestBehavior.AllowGet);
        }
        //获取所有部门名称
        public ActionResult GetAllBuMen()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            List<STUBuMen> list = new List<STUBuMen>();
            foreach(var a in temp)
            {
                STUBuMen stubm = new STUBuMen();
                stubm.ID = a.ID;
                stubm.Name = a.Name;
                list.Add(stubm);
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        //获取部门用户名字
        public ActionResult GetShareToUser()
        {
            var bmid = Convert.ToInt32(Request["BMID"]);
            var temp = UserInfoService.LoadEntities(x => x.BuMenID == bmid).DefaultIfEmpty().ToList();
            List<BMUser> list = new List<BMUser>();
            foreach (var a in temp)
            {
                BMUser bmu = new BMUser();
                bmu.ID = a.ID;
                bmu.Name = a.PerSonName;
                list.Add(bmu);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //获取所有文件类型
        public ActionResult GetAllFileType()
        {
            var temp = FileTypeService.LoadEntities(x => x.ID > 0).DefaultIfEmpty().ToList();
            List<FileTP> list = new List<FileTP>();
            foreach (var a in temp)
            {
                FileTP ftp = new FileTP();
                ftp.ID = a.ID;
                ftp.Name = a.FileTypeCHNName;
                list.Add(ftp);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public class STUBuMen
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        public class BMUser
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        public class FileTP
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}
