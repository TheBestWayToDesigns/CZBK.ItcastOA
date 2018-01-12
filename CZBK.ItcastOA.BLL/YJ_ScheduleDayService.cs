using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.BLL
{
    public partial  class YJ_ScheduleDayService:BaseService<YJ_ScheduleDay>,IYJ_ScheduleDayService
    {
        public bool NewAddSEDDAY(YJ_ScheduleDay ysdday) {
            return true;
        }
    }
}
