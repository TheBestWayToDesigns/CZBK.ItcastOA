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
            
            var Addta = this.GetCurrentDbSession.YJ_ScheduleDayDal.AddEntity(ysdday.Ysdy);
            YJ_ScheduleAction ysa = new YJ_ScheduleAction();
            //true 对日程整体建议

            ysa.UpSdeDayID = ysdday.IFours?null:Addta.WriteUserID;
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
