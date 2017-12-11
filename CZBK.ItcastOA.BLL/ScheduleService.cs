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

            var temp = this.GetCurrentDbSession.ScheduleDal.LoadEntities(x => x.ID > 0).DefaultIfEmpty();


            return null;
        }
    }   
}
