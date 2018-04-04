using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class GeRenController : BaseController
    {
        //
        // GET: /GeRen/
        IBLL.IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {
            ViewData["userName"] = LoginUser.UName;
            ViewData["ID"] = LoginUser.ID;
            return View();
        }
        public ActionResult edituser()
        {

            var khid = Convert.ToInt64(Request["id"]);
            var Pass = Request["Pass"];
            var Sort = Request["Sort"] ;
            var u = UserInfoService.LoadEntities(x => x.ID == khid).FirstOrDefault();
            if (Sort != null)
            {
                u.PerSonName = Sort;
            }                        
            u.UPwd = Model.Enum.AddMD5.GaddMD5(Pass);
            UserInfoService.EditEntity(u);
            return Json(new {  ret = "ok"}, JsonRequestBehavior.AllowGet);
        }

        #region 微信用
        //修改公司小号
        public ActionResult editUserPhoneXH()
        {
            var xiaoHao = Convert.ToInt32(Request["XiaoHao"]);
            var temp = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
            if(temp != null)
            {
                temp.UserXiaoHao = xiaoHao;
            }
            if (UserInfoService.EditEntity(temp))
            {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取所有人员小号信息
        public ActionResult GetAllUserXH()
        {
            var temp = UserInfoService.LoadEntities(x => x.ID > 0 && x.UserXiaoHao != null).DefaultIfEmpty().ToList();
            if(temp != null || temp[0] != null)
            {
                var remp = from a in temp
                           select new
                           {
                               ID = a.ID,
                               Username = a.PerSonName,
                               XiaoHao = a.UserXiaoHao
                           };
                return Json(new { ret = "ok", rows = remp }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
