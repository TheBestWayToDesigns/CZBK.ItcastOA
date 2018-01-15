using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.SearchParam
{
    public class YjsdayClass:EverDaySelect
    {
        public long ID { get; set; }
        public long AddID { get; set; }
        public long EditID { get; set; }
        public long UpdataID { get; set; }
        public long PersonID { get; set; }
        public long DataID { get; set; }
        public bool YesOrNo { get; set; }

      


        public bool IFours { get; set; }
        public YJ_ScheduleDay Ysdy { get; set; }

    }
}
