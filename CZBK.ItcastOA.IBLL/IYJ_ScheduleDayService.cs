using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
     public partial  interface IYJ_ScheduleDayService:IBaseService<IYJ_ScheduleDayService>
    {
        bool NewAddSEDDAY(IYJ_ScheduleDayService ysdday);
    }
}
