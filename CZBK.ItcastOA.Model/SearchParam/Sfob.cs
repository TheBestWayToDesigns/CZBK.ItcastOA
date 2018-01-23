using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.SearchParam
{
    public class Sfob : ShareFileOrNotice
    {
        public string alluserinfo { get; set; }
    }
    public class YJ_DAY : YJ_ScheduleDay
    {
        public string uPjyname { get; set; }
        public string YJUserinfoIDname { get; set; }
        public string thislistname { get; set; }
    }
    public class RetDayCount{
        public int id { get; set; }
        public int day { get; set; }
        public int count { get; set; }
        public DateTime time { get; set; }
        public string CtPerson { get; set; }
        public List<DCperson> LdcPerson { get; set; }
    }
    public class DCperson {

        public string Person { get; set; }
        public int count { get; set; }
    }
}
