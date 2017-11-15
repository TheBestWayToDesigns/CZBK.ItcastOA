using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class SczzAdminController : BaseController
    {
        //
        // GET: /SczzAdmin/
        IBLL.IT_ShengChanZhiZhaoTopNameService T_ShengChanZhiZhaoTopNameService { get; set; }
        IBLL.IT_SczzItemService T_SczzItemService { get; set; }
        IBLL.IT_SczzDanjuService T_SczzDanjuService { get; set; }
        IBLL.IT_CanPanService T_CanPanService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getdata()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 25;
            UserInfoParam uim = new UserInfoParam();
            uim.Uptime = Convert.ToDateTime(Request["UpTime"]);
            uim.Dwtime = Convert.ToDateTime(Request["DwTime"]);
            uim.Jhname = Request["Jhname"] == null ? 0 : Request["Jhname"].Length <= 0 ? 0 : int.Parse(Request["Jhname"]);
            uim.adduser = Request["addUser"] == null ? 0 : Request["addUser"].Length <= 0 ? 0 : int.Parse(Request["addUser"]);
            uim.SHuser = Request["SHuser"] == null ? 0 : Request["SHuser"].Length <= 0 ? 0 : int.Parse(Request["SHuser"]);
            uim.CPtext = Request["CPtext"] == null ? 0 : Request["CPtext"].Length <= 0 ? 0 : int.Parse(Request["CPtext"]);
            uim.zt = Request["shzt"] == null ? 0 : Request["shzt"].Length <= 0 ? -1 : int.Parse(Request["shzt"]);
            uim.PageIndex = pageIndex;
            uim.PageSize = pageSize;
            uim.TotalCount = 0;
            var temp = T_SczzDanjuService.LoadSearchEntities(uim);
            var Rtemp = Fromnew(temp);
            return Json(new { rows = Rtemp, total = uim.TotalCount }, JsonRequestBehavior.AllowGet);
        }
        //返回计划创建人员
        public ActionResult AddUser()
        {
            var temp = T_SczzDanjuService.LoadEntities(x => x.del == 0);            
            var tem = from a in temp
                      select new 
                      {
                          MyTexts=a.AddUserinfo.PerSonName,
                          ID = a.AddUser
                      };
            var tem_sd = tem.Distinct();
           
            return Json(tem_sd, JsonRequestBehavior.AllowGet);

        }
  
        // 获取生产制造表头
        public ActionResult SczzTopName()
        {
            var temp = T_ShengChanZhiZhaoTopNameService.LoadEntities(x => x.Del == 0);
            var tem = from a in temp
                      select new
                      {
                          ID = a.ID,
                          MyTexts = a.TopText
                      };
            return Json(tem, JsonRequestBehavior.AllowGet);
        }
        //获取审核人信息 ShenHePerson
        public ActionResult ShenHePerson()
        {
            var temp = UserInfoService.LoadEntities(x => x.DelFlag == 0&&x.QuXian>=0);
            var tem = from a in temp
                      select new
                      {
                          ID = a.ID,
                          MyTexts = a.PerSonName
                      };
            return Json(tem, JsonRequestBehavior.AllowGet);
        }
        //获取产品材质信息 ChanPinCZ

        public ActionResult ChanPinCZ()
        {
            var temp = T_SczzItemService.LoadEntities(x => x.Del == 0 );
            var tem = from a in temp
                      select new
                      {
                          ID = a.ID,
                          MyTexts = a.Text
                      };
            return Json(tem, JsonRequestBehavior.AllowGet);
        }
        //返回查看详细列表
        public ActionResult GetOne()
        {
            var id =int.Parse( Request["id"]);
            var tmp = T_SczzDanjuService.LoadEntities(x => x.ID == id&&x.del==0);
            var temp = Fromnew(tmp);

            var Schanpin = tmp.FirstOrDefault().T_CanPan.Where(x => x.DEL == 0);

            var chanpin = from a in Schanpin
                          select new {
                              ID= a.ID,
                              Cpname= a.Cpname,
                              CpShuliang=  a.CpShuliang,
                              SczzItem= a.T_SczzItem.Text,
                              OverTime=  a.OverTime,
                              Bak= a.Bak,
                              ImageInt= a.ImageInt

                          };
            return Json(new { temp = temp, chanpin = chanpin }, JsonRequestBehavior.AllowGet);
        }

        private IQueryable Fromnew(IQueryable<T_SczzDanju> temp)
        {
            var Rtemp = from a in temp
                        select new
                        {
                            ID = a.ID,
                            TextName = a.T_ShengChanZhiZhaoTopName.TopText,
                            AddTopTime = a.AddTime,
                            ImageInt = a.Image,
                            Bak = a.Bak,
                            DjAddUser = a.AddUserinfo.PerSonName,
                            Jhpzr = a.Upshuser.PerSonName,
                            JhpzrEtime = a.UPshenhetime,
                            Jhtcr = a.Scshuser.PerSonName,
                            JhtcrEtime = a.ShenChanShenHetime,
                            Jgcj = a.CJSHuser.PerSonName,
                            JgcjEtime = a.CheJianShenHetime,
                            Jhzdr = a.jhzdruser.PerSonName,
                            JhzdrEtime = a.JhzdrTime,
                            Zt = a.ZhuangTai
                        };
            return Rtemp;
        }
    }
}
