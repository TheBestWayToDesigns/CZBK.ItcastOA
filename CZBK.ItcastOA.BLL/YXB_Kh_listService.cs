using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.Enum;
using CZBK.ItcastOA.Model.SearchParam;

namespace CZBK.ItcastOA.BLL
{
    
    public partial class YXB_Kh_listService : BaseService<YXB_Kh_list>, IYXB_Kh_listService
    {
        public IQueryable<YXB_Kh_list> loadBaoBeientities(UserInfoParam ups) {
            var temp = this.GetCurrentDbSession.YXB_Kh_listDal.LoadEntities(x => x.DelFlag == 0 && x.NewTime>=ups.Uptime&&x.NewTime<=ups.Dwtime&&x.UserInfo.BuMenID==ups.BumenID).DefaultIfEmpty();
            if (ups.Person > 0)
            {
                temp = temp.Where(x => x.AddUser == ups.Person);
            }
            if (ups.KHname>0) {
                temp = temp.Where(x => x.id == ups.KHname);
            }
            if (ups.addess!=null)
            {
                if (ups.addess.Trim().Length >0)
                { temp = temp.Where(x => x.T_BaoJiaToP.Where(w => w.Addess.Contains(ups.addess)).Count() > 0); }
            }

            ups.TotalCount = temp.Count();
            return temp.OrderByDescending<YXB_Kh_list, DateTime>(u => u.NewTime).Skip<YXB_Kh_list>((ups.PageIndex - 1) * ups.PageSize).Take<YXB_Kh_list>(ups.PageSize);

        }
    }
 }
