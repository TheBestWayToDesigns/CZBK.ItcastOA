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

        //
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
            var temp = BumenInfoSetService.LoadEntities(x => x.ID > 0).DefaultIfEmpty();
            return Json(temp,JsonRequestBehavior.AllowGet);
        }
        //获取部门用户名字
        public ActionResult GetShareToUser()
        {
            var temp = BumenInfoSetService.LoadEntities(x => x.ID > 0).DefaultIfEmpty();
            return Json(temp, JsonRequestBehavior.AllowGet);
        }


        public class STUBuMen
        {
            private int ID { get; set; }
            private string Name { get; set; }
        }
    }
}
