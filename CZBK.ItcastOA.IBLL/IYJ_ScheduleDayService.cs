using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
     public partial  interface IYJ_ScheduleDayService:IBaseService<YJ_ScheduleDay>
    {
        bool NewAddSEDDAY(YJ_ScheduleDay ysdday);
    }
}
