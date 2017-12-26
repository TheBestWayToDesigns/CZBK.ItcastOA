using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IRoleInfoService RoleInfoService { get; set; }
        public ActionResult Index()
        {
            CheckCookieInfo();
            return View();
        }
        #region 校验用户的登录信息
        public ActionResult CheckLogin()
        {
            //1:判断验证码是否正确
            string validateCode = Session["validateCode"] == null ? string.Empty : Session["validateCode"].ToString();
            if (string.IsNullOrEmpty(validateCode))
            {
               
                return Content("notyzm");
            }
            Session["validateCode"] = null;
            string txtCode = Request["vCode"];
            if (!validateCode.Equals(txtCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Content("notyzm");
            }
            //2:判断用户输入的用户名与密码
            string userName = Request["LoginCode"];
            string userPwd = Request["LoginPwd"];
            userPwd = Model.Enum.AddMD5.GaddMD5(userPwd);
           UserInfo userInfo=UserInfoService.LoadEntities(u => u.UName == userName&&u.DelFlag!=1 ).FirstOrDefault();
           if (userInfo != null)
           {
                if (userInfo.UPwd != userPwd)
                {
                    return Json("IsNotPass");
                }
                //检查使用时间
                if (userInfo.OverTime < MvcApplication.GetT_time())
                {
                    return Content("no:使用时间到期!!");
                }
                else
                {
                    //检查之前是否登陆过  清除上次登陆时间
                    Common.MemcacheHelper.Delete(userInfo.Login_now);
                    
                    //作为Memcache的key
                    string sessionId =Guid.NewGuid().ToString();
                    //使用Memcache代替Session解决数据在不同Web服务器之间共享的问题。
                    Common.MemcacheHelper.Set(sessionId,Common.SerializerHelper.SerializeToString(userInfo), DateTime.Now.AddHours(1));
                    object obj = Common.MemcacheHelper.Get("Allstr");
                    if (obj==null)
                    { Common.MemcacheHelper.Set("Allstr", 0); }
                    
                    //将Memcache的key以cookie的形式返回到浏览器端的内存中，当用户再次请求其它的页面请求报文中会以Cookie将该值再次发送服务端。
                    Response.Cookies["sessionId"].Value = sessionId;
                    //把本次生产的SESSIONID写入数据库
                    userInfo.Login_now = sessionId;
                    UserInfoService.EditEntity(userInfo);
                    
                    //记住我
                    if (!string.IsNullOrEmpty(Request["checkMe"]))
                    {
                        HttpCookie cook1 = Response.Cookies["Lname"];
                        cook1.Values.Add("cp1", userInfo.UName);
                        cook1.Values.Add("cp2", userInfo.UPwd);
                        cook1.Expires = DateTime.Now.AddDays(3);
                        cook1.HttpOnly = true;
                        //HttpCookie cookie1 = new HttpCookie("cp1", userInfo.UName);
                        //HttpCookie cookie2 = new HttpCookie("cp2", userInfo.UPwd);
                        //cookie1.Expires = DateTime.Now.AddDays(3);
                        //cookie1.HttpOnly = true;
                        //cookie2.Expires = DateTime.Now.AddDays(3);
                        //cookie2.HttpOnly = true;
                        //Response.Cookies.Add(cookie1);
                        //Response.Cookies.Add(cookie2);
                    }
                    if (Convert.ToBoolean(userInfo.ThisMastr))
                    {
                        return Json("master");
                    }
                    else
                    {
                        return Json("ok");
                    }
                   
               }
            }
           else
           {
                return Json("IsNotName");
           }

        }
        #endregion

        #region 展示验证码
          public ActionResult ShowValidateCode()
        {
            Common.ValidateCode validateCode = new Common.ValidateCode();
            string code=validateCode.CreateValidateCode(4);
            Session["validateCode"] = code;
            byte[] buffer = validateCode.CreateValidateGraphic(code);
            return File(buffer, "image/jpeg");
        }
        #endregion

        #region 判断Cookie信息
          private void CheckCookieInfo()
          {
            if (Request.Cookies["Lname"] != null) {
                if (Request.Cookies["Lname"]["cp1"] != null && Request.Cookies["Lname"]["cp2"] != null)
                {
                    string userName = Request.Cookies["Lname"]["cp1"];
                    string userPwd = Request.Cookies["Lname"]["cp2"];
                    //判断Cookie中存储的用户密码和用户名是否正确.
                    var userInfo = UserInfoService.LoadEntities(u => u.UName == userName).FirstOrDefault();

                    if (userInfo != null)
                    {
                        //注意：将用户的密码保存到数据库时一定要加密。
                        //由于数据库中存储的密码是明文，所以这里需要将数据库中存储的密码两次MD5运算以后在进行比较。
                        if (userInfo.UPwd == userPwd)
                        {
                            string sessionId = Guid.NewGuid().ToString();//作为Memcache的key
                                                                         //使用Memcache代替Session解决数据在不同Web服务器之间共享的问题。
                            Common.MemcacheHelper.Set(sessionId, Common.SerializerHelper.SerializeToString(userInfo), DateTime.Now.AddMinutes(120));
                            // 将Memcache的key以cookie的形式返回到浏览器端的内存中，当用户再次请求其它的页面请求报文中会以Cookie将该值再次发送服务端。
                            Response.Cookies["sessionId"].Value = sessionId;

                            var myBrowserCaps = Request.Browser;
                            //var isMobile= myBrowserCaps.IsMobileDevice ? 1 : 0;

                            if (Convert.ToBoolean(userInfo.ThisMastr))
                            {
                                Response.Redirect("/Home/master");

                            }
                            else
                            {
                                if (myBrowserCaps.IsMobileDevice)
                                { Response.Redirect("/Home/Index"); }
                                else
                                { Response.Redirect("/Home/master"); }

                            }
                        }
                    }
                    Response.Cookies.Add(Request.Cookies["Lname"]);
                    Response.Cookies["Lname"].Expires = DateTime.Now.AddDays(-1);
                }
            }
              
          }

        #endregion
        #region 退出登录
          public ActionResult Logout()
          {
              if (Request.Cookies["sessionId"] != null)
              {
                  string key = Request.Cookies["sessionId"].Value;
                  Common.MemcacheHelper.Delete(key);
                 Response.Cookies.Add(Request.Cookies["Lname"]);
                 Response.Cookies["Lname"].Expires = DateTime.Now.AddDays(-1);
            }
              return Redirect("/Login/Index");
          }
        #endregion
        #region 注册用户
        #region 添加用户信息
        public ActionResult AddUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.UName = Request["Name"];
            userInfo.UPwd = Request["Pass"];
            userInfo.Remark = Request["Remark"];
            //检查用户是否重复
            if (SelectUserName(userInfo))
            {
                return Json("IsCongfu");
            }
            userInfo.DelFlag = 0;
            userInfo.ModifiedOn = DateTime.Now;
            userInfo.SubTime = DateTime.Now;
            userInfo.OverTime = new DateTime(2018, 12, 1);
            userInfo.UPwd = Model.Enum.AddMD5.GaddMD5(userInfo.UPwd);
            userInfo.ThisMastr = false;
            
            UserInfoService.AddEntity(userInfo);
            var ucinfo = UserInfoService.LoadEntities(x => x.UName == userInfo.UName).FirstOrDefault();
            List<int> list = new List<int>();
            list.Add(RoleInfoService.LoadEntities(x => x.RoleName == "营销员").FirstOrDefault().ID);
            UserInfoService.setuserorderidnfo(ucinfo.ID, list);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #endregion
        private bool SelectUserName(UserInfo Uinfo)
        {
            return UserInfoService.LoadEntities(x => x.UName == Uinfo.UName).FirstOrDefault() != null;
        }
        #endregion
        #region 找回密码
        public ActionResult zhaohui()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.UName = Request["Name"];
            userInfo.UPwd = Request["Pass"];
            userInfo.Remark = Request["Remark"];
            //检查用户是否重复
            if (!SelectUserName(userInfo))
            {
                return Json("IsNotName");
            }
            var  Thisname= UserInfoService.LoadEntities(x => x.UName == userInfo.UName).FirstOrDefault();
            if (Thisname.Remark == userInfo.Remark)
            {
                Thisname.UPwd = Model.Enum.AddMD5.GaddMD5(userInfo.UPwd); 
                UserInfoService.EditEntity(Thisname);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("IsZhNot", JsonRequestBehavior.AllowGet);
            }
           
        }
        #endregion
    }
}
