using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.Enum;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class UserInfoController :BaseController //Controller
    {
        //
        // GET: /UserInfo/
        IBLL.IUserInfoService UserInfoService{get;set;}
        IBLL.IRoleInfoService RoleInfoService { get; set; }
        IBLL.IActionInfoService ActionInfoService { get; set; }
        IBLL.IBumenInfoSetService BumenInfoSetService { get; set; }
       // short Delflag = (short)DelFlagEnum.Normarl;
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult GetJson()
        {
            //var City = T_CityService.LoadEntities(x => x.DelFlag == Delflag).DefaultIfEmpty();
            //var temp = from u in City
            //           select new
            //           {
            //               ID = u.ID,
            //               city = u.City                                                  
            //           };
            return Json(new {  }, JsonRequestBehavior.AllowGet);
        }
        #region 企业用户账号管理
        public ActionResult Zhgl()
        {
           return View();
        }
        #region 创建小号
        public ActionResult Addzhgl(UserInfo Uinfo)
        {
            string rt=string.Empty;
            //检查用户是否重复
            if (SelectUserName(Uinfo))
            {
                rt = "IsCongfu";
                return Content("IsCongfu");
            }
            //检查创建用户是否到达上线
            var Ucount = UserInfoService.LoadEntities(x => x.MasterID == LoginUser.ID).DefaultIfEmpty();
            if (Ucount != null)
            {
                if (Ucount.Count() >= LoginUser.UserXiaoHao)
                {
                    rt = "UserUP";
                    return Content("UserUP");
                }
            }
            else
            {
                Uinfo.MasterID = LoginUser.ID;
                Uinfo.ThisMastr = false;
                Uinfo.UPwd = Model.Enum.AddMD5.GaddMD5(Uinfo.UPwd);
                Uinfo.Click = LoginUser.Click;
                Uinfo.OverTime = LoginUser.OverTime;
                Uinfo.SubTime = MvcApplication.GetT_time();
                Uinfo.ModifiedOn = Uinfo.SubTime;
                UserInfoService.AddEntity(Uinfo);
                var Tuserinfo = UserInfoService.LoadEntities(x => x.UName == Uinfo.UName).FirstOrDefault();
                ////父级ID
                //UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
                //var userRoleIdList = (from r in userInfo.RoleInfo
                //                      select r.ID).ToList();
                //获取区域归属 基础区域
                var Tloginuser = UserInfoService.LoadEntities(x => x.ID == LoginUser.ID).FirstOrDefault();
                //UserInfo_City ct = Tloginuser.UserInfo_City.FirstOrDefault();
                //ct.UserInfo_ID = Tuserinfo.ID;
                //UserInfo_CityService.AddEntity(ct);


                //获取小号权限 小号权限是10
                List<int> list = new List<int>();
                list.Add(10);
                if (UserInfoService.setuserorderidnfo(Tuserinfo.ID, list))
                {
                    rt = "UserUP"; return Content("ok"); }
                else
                { rt = "UserUP"; return Content("NO"); }
            }
            return Content(rt);
          
        }

        private bool SelectUserName(UserInfo Uinfo)
        {
            return UserInfoService.LoadEntities(x => x.UName == Uinfo.UName).FirstOrDefault() != null;
        }
        #endregion

        #region 修改小号
        public ActionResult Editzhgl(UserInfo userInfo)
        {
            var tu = UserInfoService.LoadEntities(x => x.ID == userInfo.ID).FirstOrDefault();
            tu.UName = userInfo.UName;           
            tu.ModifiedOn = DateTime.Now;
            tu.UPwd = Model.Enum.AddMD5.GaddMD5(userInfo.UPwd);
            tu.Sort = userInfo.Sort;
            tu.Remark = userInfo.Remark;
            if (UserInfoService.EditEntity(tu))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #endregion

        #region 获取用户数据
        public ActionResult GetUserInfo()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            string name = Request["name"];
            string remark = Request["remark"];
            string zhgl = Request["zhgl"] != null ? Request["zhgl"] : string.Empty;
            //构建搜索条件
            int totalCount=0;
            UserInfoParam userInfoParam = new UserInfoParam() {               
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                UserName = name,
                Remark = remark,
                QuXian = LoginUser.QuXian,
                BumenID=LoginUser.BuMenID
                
                //IsMaster = true,
                //C_id = zhgl != null ? zhgl != string.Empty ? LoginUser.ID : 0 : 0
            };                     
            var userInfoList = UserInfoService.LoadSearchEntities(userInfoParam);
           var temp = from u in userInfoList
                      select new { ID = u.ID, UserName = u.UName, UserPass = u.UPwd, Remark = u.Remark, RegTime = u.SubTime, OverTime=u.OverTime,
                          UserXiaoHao=u.UserXiaoHao,
                          Click=u.Click,
                          ThisMastr=u.ThisMastr,
                          MasterID=u.MasterID,
                          CityID=u.CityID,
                          Umoney=u.Umoney,
                          PerSonName=u.PerSonName,
                          QuXian=u.QuXian,
                          BuMenID = u.BuMenID,
                          BuMen=u.BumenInfoSet.Name,
                          UPwd =u.UPwd
                      };
           return Json(new { rows = temp, total = userInfoParam.TotalCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除用户数据
        public ActionResult DeleteUserInfo()
        {
            string strId=Request["strId"];
           string[]strIds=strId.Split(',');
           List<int> list = new List<int>();
           foreach (string id in strIds)
           {
               list.Add(int.Parse(id));
           }
           if (UserInfoService.DeleteEntities(list))
           {
               return Content("ok");
           }
           else
           {
               return Content("no");
           }
              
        }
        #endregion

        #region 添加用户信息
        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            //检查用户是否重复
            if (SelectUserName(userInfo))
            {
                return Content("IsCongfu");
            }
            userInfo.DelFlag = 0;
            userInfo.ModifiedOn = DateTime.Now;
            userInfo.SubTime = DateTime.Now;
            userInfo.UPwd = Model.Enum.AddMD5.GaddMD5(userInfo.UPwd);
            userInfo.OverTime = new DateTime(2020, 1, 1);
            UserInfoService.AddEntity(userInfo);
            var ucinfo = UserInfoService.LoadEntities(x => x.UName == userInfo.UName).FirstOrDefault();
            //UserInfo_City uc = new UserInfo_City();
            //uc.UserInfo_ID = ucinfo.ID;
            //uc.T_City_ID = (Int32)userInfo.CityID;
            //UserInfo_CityService.AddEntity(uc);
            return Content("ok");
        }
        #endregion

        #region 查询要修改的数据
        public ActionResult GetUserInfoModel()
        {
            int id = int.Parse(Request["id"]);
           UserInfo userInfo=UserInfoService.LoadEntities(u=>u.ID==id).FirstOrDefault();
            
           if (userInfo != null)
           {
              // return Json(new{serverData=userInfo,msg="ok"}, JsonRequestBehavior.AllowGet);
               return Content(Common.SerializerHelper.SerializeToString(new { serverData = userInfo, msg = "ok" }));
           }
           else
           {
               return Content(Common.SerializerHelper.SerializeToString(new { msg = "no" }));
           }
        }
        #endregion

        #region 完成用户信息的修改
        public ActionResult EditUserInfo(UserInfo userInfo)
        {

            userInfo.ModifiedOn = DateTime.Now;
            userInfo.OverTime = new DateTime(2030, 1, 1);
            //userInfo.UPwd = UserInfoService.LoadEntities(x => x.ID == userInfo.ID).FirstOrDefault().UPwd;
            //var uct= UserInfo_CityService.LoadEntities(x=>x.UserInfo_ID==userInfo.ID).FirstOrDefault();
            //uct.T_City_ID = (int)userInfo.CityID;
            //UserInfo_CityService.EditEntity(uct);            
            if (UserInfoService.EditEntity(userInfo))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        
        #endregion

        #region 为用户分配角色
        public ActionResult SetUserRoleInfo()
        {
            int userId = int.Parse(Request["userId"]);
           UserInfo userInfo=UserInfoService.LoadEntities(u=>u.ID==userId).FirstOrDefault();
           ViewBag.UserInfo = userInfo;
            //查询所有的角色信息
           short delFlag = (short)DelFlagEnum.Normarl;
           var roleInfoList = RoleInfoService.LoadEntities(r=>r.DelFlag==delFlag).ToList();
            //找出用户已经有的角色的编号
           var userRoleIdList = (from r in userInfo.RoleInfoes
                                 select r.ID).ToList();
           ViewBag.AllRoleInfo = roleInfoList;
           ViewBag.AllExtRoleId = userRoleIdList;
           return View();
        }
        #endregion
        /// <summary>
        /// 完成对用户角色的分配
        /// </summary>
        /// <returns></returns>
        public ActionResult SetuserRole()
        {
            int userid = int.Parse(Request["userId"]);
            //接受表单中所用的KEY  所用表单NAME属性的值
            //Request.Form[]接受NAME的值

            string[] allkeys= Request.Form.AllKeys;
            List<int> list = new List<int>();
            //只要前缀只包含CBA_
            foreach (string key in allkeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string K = key.Replace("cba_","");
                    list.Add(int.Parse(K));
                }
            }
            if (UserInfoService.setuserorderidnfo(userid, list))
            {
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }
        #region 为特殊用户分配权限

        public ActionResult SetUserAction() {
            int userid = int.Parse(Request["UserId"]);
            //查询要分配权限的用户信息
            var userinfo = UserInfoService.LoadEntities(u => u.ID == userid).FirstOrDefault();
            //获取所有权限信息
            short delflag = (short)DelFlagEnum.Normarl;
            var allActionList = ActionInfoService.LoadEntities(a => a.DelFlag == delflag).ToList();
            //获取用户已有权限
            var allUserActionIDlist = userinfo.R_UserInfo_ActionInfo.ToList();

            ViewBag.userinfo = userinfo;
            ViewBag.allActionList = allActionList;
            ViewBag.allUserActionIDlist = allUserActionIDlist;
            return View();
        }
        #endregion
        #region //异步处理特殊权限信息
        public ActionResult SetActionUser()
        {
            int userId = int.Parse(Request["userId"]);
            int actionId = int.Parse(Request["actionId"]);
            bool ispass = Request["value"] == "true" ? true:false;
            if (UserInfoService.SetUserActionInfo(userId, actionId, ispass))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 清除用户特殊权限信息
        public ActionResult deleteActionUser()
        {
            int userID = int.Parse(Request["userId"]);
            int actionID = int.Parse(Request["action"]);
            if (UserInfoService.DelUserinfo_actioninfo(userID, actionID))
            {
                return Content("ok:"+ actionID);
            }
            else
            {
                return Content("no");
            }
        }
        #endregion


        //获取部门信息
        public ActionResult GetBuMen() {

            var sbm = BumenInfoSetService.LoadEntities(x => x.DelFlag == 0 && x.Gushu < 99).DefaultIfEmpty();
            var temp = from a in sbm
                       select new
                       {
                           ID = a.ID,
                           MyTexts = a.Name
                       };
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
    }
}
