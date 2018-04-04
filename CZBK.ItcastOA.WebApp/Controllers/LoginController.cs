using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        IBLL.IWXXUserInfoService WXXUserInfoService { get; set; }
        public ActionResult Index()
        {
            WechatCheckSerer wcs = new WechatCheckSerer();
            bool b = wcs.CheckServer();
            if (b == true)
            {
                return View();
            }
            CheckCookieInfo();
            return View();
            
        }
        #region 校验用户的登录信息
        public ActionResult CheckLogin()
        {
            //是否采用验证码
            var IsNotVali = Request["IsNotVali"];
            if (IsNotVali == null)
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
                    return Json(new { ret = "IsNotPass" }, JsonRequestBehavior.AllowGet);
                }
                //检查使用时间
                if (userInfo.OverTime < MvcApplication.GetT_time())
                {
                    return Json(new { ret = "使用时间到期" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //检查之前是否登陆过  清除上次登陆时间
                    Common.MemcacheHelper.Delete(userInfo.Login_now);
                    
                    //作为Memcache的key
                    string sessionId =Guid.NewGuid().ToString();
                    //使用Memcache代替Session解决数据在不同Web服务器之间共享的问题。
                    Common.MemcacheHelper.Set(sessionId,Common.SerializerHelper.SerializeToString(userInfo), DateTime.Now.AddHours(5));
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
                    }
                    object cjson = Common.MemcacheHelper.Get(sessionId);                    
                    UserInfo Loguserinfo = cjson != null? Common.SerializerHelper.DeserializeToObject<UserInfo>(cjson.ToString()):null;

                    bool wxbol = false;
                    if (Request["wx"] == "yes")
                    {                        
                            wxbol = CheckWXopenid(userInfo.ID);
                    }

                    if (Convert.ToBoolean(userInfo.ThisMastr))
                    {                        
                        return Json( new{ ret= "master", temp = Loguserinfo, uid =userInfo.ID,uname=userInfo.PerSonName,cooks=sessionId,bol=wxbol },JsonRequestBehavior.AllowGet);
                    }
                    else
                    {                      
                        return Json(new { ret = "ok",temp= Loguserinfo, uid = userInfo.ID, uname = userInfo.PerSonName, cooks = sessionId, bol = wxbol }, JsonRequestBehavior.AllowGet);
                    }
               }
            }
           else
           {
                return Json(new { ret = "IsNotName" }, JsonRequestBehavior.AllowGet);
           }

        }
        #endregion
        #region 微信获取openid
        public ActionResult getOpenid()
        {
            //获取openid
            // //// 链接地址
            //int id = Convert.ToInt32(Request.QueryString["ID"]);
            string code = Request["js_code"].ToString();
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + Request["appid"] + "&secret=" + Request["secret"] + "&js_code=" + code + "&grant_type=" + Request["grant_type"];
            WebRequest request = WebRequest.Create(@url);
            request.Method = "GET";
            request.ContentType = "text/html:charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return Json(new { ret = "ok", openInfo = retString }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 微信用（绑定微信账号）
        //检查是否已绑定微信openid
        public bool CheckWXopenid( int uid)
        {
            var temp = WXXUserInfoService.LoadEntities(x => x.UID == uid).FirstOrDefault();
            if(temp != null)
            {
                return temp.WXID == null ? false : true;
            }else
            {
                return false;
            }
        }
        //给未绑定用户绑定微信openid
        public void AddWXOpenID()
        {
            var uid = Convert.ToInt32(Request["uid"]);
            var temp = WXXUserInfoService.LoadEntities(x => x.UID == uid).FirstOrDefault();
            if (temp == null)
            {
                WXXUserInfo wxxu = new WXXUserInfo();
                wxxu.UID = uid;
                if (Request["openid"] != null)
                {
                    wxxu.WXID = Request["openid"];
                }
                WXXUserInfoService.AddEntity(wxxu);
            }else
            {
                temp.WXID = Request["openid"];
                WXXUserInfoService.EditEntity(temp);
            }
        }
        //改绑微信openid
        public ActionResult EditWXOpenID()
        {
            var uid = Convert.ToInt32(Request["uid"]);
            var temp = WXXUserInfoService.LoadEntities(x => x.UID == uid).FirstOrDefault();
            if(temp != null)
            {
                temp.WXID = Request["openid"];
            }
            if (WXXUserInfoService.EditEntity(temp))
            {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(new { ret = "no " }, JsonRequestBehavior.AllowGet);
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
                    else
                    {
                        Response.Cookies["Lname"].Expires = DateTime.Now.AddDays(-1);
                    }
                   // Response.Cookies.Add(Request.Cookies["Lname"]);
                    
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

    #region 微信用于验证token
    /// <summary>
    /// 验证微信平台填写的服务器地址的有效性
    /// </summary>
    public class WechatCheckSerer
    {
        /// <summary>
        /// 验证微信平台填写的服务器地址的有效性
        /// </summary>
        public bool CheckServer()
        {
            string _token = "yssgoa";
            string _timestamp = HttpContext.Current.Request["timestamp"];
            string _nonce = HttpContext.Current.Request["nonce"];
            string _singature = HttpContext.Current.Request["signature"];
            string _echostr = HttpContext.Current.Request["echostr"];
            if (CheckSignAture(_token, _timestamp, _nonce, _singature))
            {
                if (!string.IsNullOrEmpty(_echostr))
                {
                    HttpContext.Current.Response.Write(_echostr);
                    HttpContext.Current.Response.End();
                    return true;
                }
                else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证签名是否一致
        /// </summary>
        /// <param name="token">微信平台设置的口令</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="signature">微信加密签名</param>
        /// <returns></returns>
        public bool CheckSignAture(string token, string timestamp, string nonce, string signature)
        {
            string[] strs = new string[] { token, timestamp, nonce };//把参数放到数组
            Array.Sort(strs);//加密/校验流程1、数组排序
            string sign = string.Join("", strs);
            sign = GetSHA1Str(sign);
            if (sign == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// SHA1加密方法
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns></returns>
        public string GetSHA1Str(string str)
        {
            byte[] _byte = Encoding.Default.GetBytes(str);
            HashAlgorithm ha = new SHA1CryptoServiceProvider();
            _byte = ha.ComputeHash(_byte);
            StringBuilder sha1Str = new StringBuilder();
            foreach (byte b in _byte)
            {
                sha1Str.AppendFormat("{0:x2}", b);
            }
            return sha1Str.ToString();
        }
    }
    #endregion
}
