using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.SearchParam
{
   public class UserInfoParam:BaseParam
    {
       public string UserName { get; set; }
       public string Remark { get; set; }
        public string Str { get; set; }
        public int C_id { get; set; }
        public string money { get; set; }
        public string Tingshi { get; set; }
        public string Pingmu { get; set; }
        public string Zhuanxiu { get; set; }
        public string zhgl { get; set; }
        public object Tval { get; set; }
        public string Items { get; set; }
        public bool IsMaster { get; set; }
        public DateTime Uptime { get; set; }
        public DateTime Dwtime { get; set; }
        public int? zt { get; set; }
        public string addess { get; set; }
        public int? Person { get; set; }
        public int? KHname { get; set; }
        public string CPname { get; set; }
        public string CPxh { get; set; }
        public int? Jhname { get; set; }
        public int? adduser { get; set; }
        public int? SHuser { get; set; }
        public int? CPtext { get; set; }
        public int? QuXian { get; set; }
        public int? BumenID { get; set; }
        public long ID { get; set; }
        public IQueryable<ScheduleUser>  _Schedule { get; set; }

    }
}
