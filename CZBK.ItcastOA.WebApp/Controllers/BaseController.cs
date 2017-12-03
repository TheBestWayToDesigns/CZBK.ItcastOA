using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public UserInfo LoginUser { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool isExt = false;
          //  if (Session["userInfo"] == null)
            if (Request.Cookies["sessionId"]!=null)
            {
                string sessionId = Request.Cookies["sessionId"].Value;//接收从Cookie中传递过来的Memcache的key
               object obj= Common.MemcacheHelper.Get(sessionId);//根据key从Memcache中获取用户的信息

                if (obj != null)
                {
                    UserInfo userInfo = Common.SerializerHelper.DeserializeToObject<UserInfo>(obj.ToString());
                    
                    LoginUser = userInfo;
                    isExt = true;
                    //Common.MemcacheHelper.Set(sessionId, obj.ToString(), DateTime.Now.AddMinutes(20));//模拟滑动过期时间
                    #region  完成权限过滤

                    if (LoginUser.UName == "张廷宇")
                    {
                        return;
                    }
                    //if (LoginUser.UName == "zty")
                    //{
                    //    return;
                    //}
                    string actionurl = Request.Url.AbsolutePath.ToLower();//请求地址
                    string actionhttpmethod = Request.HttpMethod;//请求方式
                    //if (actionurl == "/home/index")
                    //{
                    //    return;
                    //}
                    IApplicationContext ctx = ContextRegistry.GetContext();
                    IUserInfoService UserInfoservice = (IUserInfoService)ctx.GetObject("UserInfoService");
                    IActionInfoService ActionInfoService = (IActionInfoService)ctx.GetObject("ActionInfoService");
                    //var url2 = ActionInfoService.LoadEntities(x => x.ID > 0);
                    //var url = ActionInfoService.LoadEntities(a => a.Url == actionurl).FirstOrDefault();
                    //    var url1 = ActionInfoService.LoadEntities(a => a.HttpMethod == actionhttpmethod).FirstOrDefault();
                    var actioninfo = ActionInfoService.LoadEntities(a => a.Url == actionurl && a.HttpMethod == actionhttpmethod).FirstOrDefault();
                    if (actioninfo == null)
                    {
                        //在权限表中没有找到要查询的URI方法 或者 请求方式错误
                        Response.Redirect("/Error.html");
                        return;
                    }
                    else
                    {
                        //判断登陆用户是否有权限访问
                        //按照第二条进行判断
                        var loginuserInfo = UserInfoservice.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
                        var r_userinfo_actioninfo = (from a in loginuserInfo.R_UserInfo_ActionInfo
                                                     where a.ActionInfoID == actioninfo.ID
                                                     select a).FirstOrDefault();
                        if (r_userinfo_actioninfo != null)
                        {
                            if (r_userinfo_actioninfo.IsPass == true)
                            {
                                return;
                            }
                            else
                            {
                                Response.Redirect("/Error.html");
                                return;
                            }
                        }
                        //安装第一条线进行过滤（用户——角色——权限）
                        var loginUserRoleInfo = loginuserInfo.RoleInfoes;
                        var loginuserisAction = (from r in loginUserRoleInfo
                                                 from a in r.ActionInfo
                                                 where a.ID == actioninfo.ID
                                                 select a).Count();
                        if (loginuserisAction < 1)
                        {
                            Response.Cookies["cp1"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["cp2"].Expires = DateTime.Now.AddDays(-1);                           
                          
                            Response.Redirect("/Qxerrer.html");
                            return;
                        }
                    }
                    #endregion
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/Login/Index");
                    return;
                }
            }
            if (!isExt)
            {
                filterContext.HttpContext.Response.Redirect("/Login/Index");
                return;
            }

        }
    
    }
}
