using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.BLL
{
    public partial  class YJ_ScheduleDayService : BaseService<YJ_ScheduleDay>,IYJ_ScheduleDayService
    {
        #region 写入每日审批意见，已阅、给予人员意见
        public bool NewAddSEDDAY(YjsdayClass ysdday)
        {
            ysdday.Ysdy.DEL = 0;

            var textid = GetCurrentDbSession.YJ_ScheduleDayDal.LoadEntities(x => x.DEL == 0&&x.SchenuleTime==ysdday.Ysdy.SchenuleTime).Max(m => m.TextID);
            ysdday.Ysdy.TextID = !ysdday.IFours ?null: textid != null ? textid + 1 : 0;
            long? upreadid = null;
            //如果被查看人员没有创建审核数据 那么就新建一个初始数据
            if (ysdday.NewAdditem) {
                YJ_ScheduleDay yd = new YJ_ScheduleDay();
                yd.SchenuleTime = ysdday.Ysdy.SchenuleTime;
                yd.TextID = -1;
                yd.YJText = "没有创建意见信息，该信息为初始化信息！";
                yd.YJUserinfoID =Convert.ToInt32(ysdday.Ysdy.WriteUserID);
                yd.AddYJtime = DateTime.Now;
                yd.DEL = 0;
                yd.SeeUserInfoList = ysdday.Ysdy.YJUserinfoID.ToString()+",";
                var newadddat= GetCurrentDbSession.YJ_ScheduleDayDal.AddEntity(yd);
               
                YJ_ScheduleAction ysaadd = new YJ_ScheduleAction();
                //true 对日程整体建议
                ysaadd.UpSdeDayID = null;
                ysaadd.TheSdeDayID = newadddat.ID;
                this.GetCurrentDbSession.YJ_ScheduleActionDal.AddEntity(ysaadd);
              
                if (!GetCurrentDbSession.SaveChanges()) {
                    GetCurrentDbSession.YJ_ScheduleDayDal.DeleteEntity(newadddat);
                    this.GetCurrentDbSession.YJ_ScheduleActionDal.DeleteEntity(ysaadd);
                    GetCurrentDbSession.SaveChanges();
                    return false;
                }
                upreadid =newadddat.ID;
                ysdday.Ysdy.WriteUserID = upreadid;
            }
            ysdday.Ysdy.ID = 1;
            var Addta = this.GetCurrentDbSession.YJ_ScheduleDayDal.AddEntity(ysdday.Ysdy);
            //记录查看人员事件
            var addstr = GetCurrentDbSession.YJ_ScheduleDayDal.LoadEntities(x => x.ID == Addta.WriteUserID).FirstOrDefault();
            if (addstr != null) {
                if (addstr.SeeUserInfoList == null)
                {
                    addstr.SeeUserInfoList = ysdday.Ysdy.YJUserinfoID.ToString();
                    GetCurrentDbSession.YJ_ScheduleDayDal.EditEntity(addstr);
                }
                else
                {
                    var userinfuid = addstr.SeeUserInfoList.Split(',');

                    if (!userinfuid.Contains(ysdday.Ysdy.YJUserinfoID.ToString()))
                    {
                        addstr.SeeUserInfoList = addstr.SeeUserInfoList + "," + ysdday.Ysdy.YJUserinfoID.ToString();
                        GetCurrentDbSession.YJ_ScheduleDayDal.EditEntity(addstr);
                    }
                }
            }
            YJ_ScheduleAction ysa = new YJ_ScheduleAction();
            ysa.ID = 1;
            //true 对日程整体建议
            ysa.UpSdeDayID = !ysdday.IFours?null: ysdday.NewAdditem? upreadid: Addta.WriteUserID;
            ysa.TheSdeDayID = Addta.ID;
            this.GetCurrentDbSession.YJ_ScheduleActionDal.AddEntity(ysa);
            return this.GetCurrentDbSession.SaveChanges();
        }
        #endregion
     
        #region 查询已经阅览人员  dte 时间   UinfoId 撰写人员ID

        public string GetReadPerson(DateTime dte,int UinfoId) {
            string retStr=string.Empty;
            var temp = this.GetCurrentDbSession.YJ_ScheduleDayDal.LoadEntities(x => x.DEL == 0 && x.SchenuleTime == dte&&x.ISee==true).DefaultIfEmpty();
            if (temp.Count() > 0) {
                var disct = temp.GroupBy(x => x.YJUserinfoID).Where(m => m.Count() > 0).ToList();
                foreach (var str in disct)
                {
                    if (str.Key == null)
                    { continue; }
                    int id = (int)str.Key;
                    
                    retStr = retStr + this.GetCurrentDbSession.UserInfoDal.LoadEntities(x => x.ID == id).FirstOrDefault().PerSonName + ",";
                }
            }
            return retStr;
        }
        #endregion

    }
}
