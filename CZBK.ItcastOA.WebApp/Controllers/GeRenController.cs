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
                u.Sort = Sort;
            }                        
            u.UPwd = Model.Enum.AddMD5.GaddMD5(Pass);
            UserInfoService.EditEntity(u);
            return Json(new {  ret = "ok"}, JsonRequestBehavior.AllowGet);
        }

    }
}
