using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IYXB_BaojiaService YXB_BaojiaService { get; set; }
        IBLL.IYXB_Kh_listService YXB_Kh_listService { get; set; }


        //
        // GET: /Home/
        public ActionResult Index()
        {
            if (LoginUser != null)
            {
                ViewData["userName"] = LoginUser.PerSonName;
            }
            return View();
        }
        public ActionResult master()
        {
            if (LoginUser != null)
            {
                ViewData["userName"] = LoginUser.UName;
            }
            return View();
        }
        
        public ActionResult HomePage()
        {
            if (LoginUser != null)
            {
                ViewData["userName"] = LoginUser.UName;
            }
            return View();
        }

        #region 校验用户权限 完成登陆用户菜单权限的过滤
        public ActionResult GetMenus() {

            var returnActionlist = GetMenum();
            //序列化 权限
            return Json(returnActionlist,JsonRequestBehavior.AllowGet);

        }
        public object GetMenum()
        {
            //1：根据用户 ——角色——权限 将登陆用户具有的菜单权限查询出来放在一个集合中
            var loginUserInfo = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();

            var loginUserRoleInfo = loginUserInfo.RoleInfoes;//获取登陆用户的角色信息
            short actionTypeEnum = (short)ActionInfoTypeEnum.MenuActionTypeEnum;//表示菜单权限
            //查询出角色对应的菜单权限
            var loginUserActionInfo = (from r in loginUserRoleInfo
                                       from a in r.ActionInfo
                                       where a.ActionTypeEnum == actionTypeEnum
                                       select a).ToList();
            //2：根据用户——权限

            //根据登陆用户查询o.R_UserInfo_ActionInfo中间表，然后在用导航属性查询权限表
            var r_userInfo_actionInfo = from r in loginUserInfo.R_UserInfo_ActionInfo select r.ActionInfo;

            //判断是否是菜单权限
            var loginUserMenuAction = (from r in r_userInfo_actionInfo
                                       where r.ActionTypeEnum == actionTypeEnum
                                       select r).ToList();
            //将存储登陆用户权限的两个集合合并
            loginUserActionInfo.AddRange(loginUserMenuAction);
            //查询出所有登陆用户禁止的权限的编号
            var loginForbActionInfo = (from r in loginUserInfo.R_UserInfo_ActionInfo
                                       where r.IsPass == false
                                       select r.ActionInfoID).ToList();
            //将禁止的权限从集合中过滤掉
            var loginUserAllowActionlist = loginUserActionInfo.Where(a => !loginForbActionInfo.Contains(a.ID));
            //去除重复的
            var loginUserAllowActionlists = loginUserAllowActionlist.Distinct(new EqualityComparer());
            loginUserAllowActionlists= loginUserAllowActionlists.OrderBy(x => x.Sort);
            var returnActionlist = from a in loginUserAllowActionlists
                                   select new { icon = a.MenuIcon, title = a.ActionInfoName, url = a.Url };
            
            return returnActionlist;
        }
        #endregion

        public ActionResult logindex() {
            return View();
        }
        public ActionResult GetBaojidan()
        {
            var editc = Request["edittus"]==null?-1: int.Parse(Request["edittus"]);
            var tus_c  =Convert.ToInt64(Common.MemcacheHelper.Get("Allstr"));
           
            if (editc != tus_c)
            {
                var Adata = YXB_BaojiaService.LoadEntities(x => x.DelFlag == 0 && x.ZhuangTai == 0).DefaultIfEmpty();
                var count = Adata != null ? Adata.Count() : 0;
                if (YXB_BaojiaService.LoadEntities(x => x.DelFlag == 0 && x.ZhuangTai == 0).FirstOrDefault() == null)
                {
                    count = 0;
                }
               
                return Json(new { count = count, ret = count == 0 ? "no" : "ok", tus_c = tus_c }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
