using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OldOaModel.DAL;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class OldOaController : BaseController
    {
        //
        // GET: /OldOa/
        IBLL.IUserbakService UserbakService { get; set; }

        public ActionResult Index()
        {
            var thisbak = UserbakService.LoadEntities(x => x.UserInfoID == LoginUser.ID).FirstOrDefault();
            if (thisbak == null) {
                return Content("<div style='margin - left:auto; margin - right:auto'>数据ID没有绑定！</div>");
            }
            //TModel.DAL.AA_InventoryDal Tda = new TModel.DAL.AA_InventoryDal();
            //var tempCp = Tda.LoadEntities(x => x.idwarehouse == 5 && x.isSale == 1).DefaultIfEmpty();
            OldOaModel.DAL.T_UsersDal udl = new T_UsersDal();
            var Loduser = udl.LoadEntities(x => x.id == thisbak.OldUserID).FirstOrDefault();
            ViewBag.user = Loduser;
            return View();
        }

    }
}
