using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CZBK.ItcastOA.BLL
{
    public partial class ScheduleService : BaseService<Schedule>, IScheduleService
    {
        public IQueryable<Schedule> LoadSearchEntities(Model.SearchParam.UserInfoParam uip)
        {
            
            var temp = this.GetCurrentDbSession.ScheduleDal.LoadEntities(x=>x.ID>0).DefaultIfEmpty();
            if (uip.ID > 0)
            {
                IQueryable<Schedule> iqs = null;
                List<Schedule> lts = null;
                IQueryable<ScheduleUser> icu = uip._Schedule;
              
                var tempb = icu.Select(x => x.UserInfo.Schedules.Where(w => w.UserID == uip.ID)).DefaultIfEmpty();
                 var tems= tempb.ToList();
                foreach (var v in tempb)
                {
                    if (v.Count() > 0)
                    {
                        var mt = v.ToList();
                        lts.AddRange(mt);
                        string s = "";
                    }
                }
               // temp = this.GetCurrentDbSession.ScheduleDal.LoadEntities(x => x.UserID==uip.ID).DefaultIfEmpty();
            }
            //temp = temp.Where(x => icu.Count(t => t.UpID == x.UserID) > 0).DefaultIfEmpty();
            uip.TotalCount = temp.Count();
            return temp.OrderByDescending<Schedule, long>(u => u.ID).Skip<Schedule>((uip.PageIndex - 1) * uip.PageSize).Take<Schedule>(uip.PageSize);
        }
    }   
}
