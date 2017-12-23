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
    public partial class YXB_BaojiaService : BaseService<YXB_Baojia>, IYXB_BaojiaService
    {
        public IQueryable<YXB_Baojia> LoadSearchEntities(Model.SearchParam.UserInfoParam sisp)
        {
            
            var temp = this.GetCurrentDbSession.YXB_BaojiaDal.LoadEntities(u => u.AddTime >= sisp.Uptime && u.AddTime <= sisp.Dwtime&&u.DelFlag==0);

            //temp.Where(u=>u.T_BaoJiaToP.T_WinBak)
            if (sisp.zt != 0)
            {
                temp = temp.Where<YXB_Baojia>(x => x.T_BaoJiaToP.T_WinBak.FirstOrDefault()!=null? x.T_BaoJiaToP.T_WinBak.FirstOrDefault().YuanYin == sisp.zt:x.T_BaoJiaToP.T_WinBak.Count<=0);
            }
            
            if (sisp.addess.Trim().Length > 0)
            {
                temp = temp.Where<YXB_Baojia>(u => u.T_BaoJiaToP.Addess.Contains(sisp.addess));
            }
            if (sisp.Person != 0)
            {
                temp = temp.Where<YXB_Baojia>(u => u.T_BaoJiaToP.YXB_Kh_list.UserInfo.ID == sisp.Person);
            }
            if (sisp.KHname != 0)
            {
                temp = temp.Where<YXB_Baojia>(u => u.T_BaoJiaToP.Kh_List_id == sisp.KHname);
            }
            if (sisp.CPname!=0)
            {
                temp = temp.Where<YXB_Baojia>(u => u.CPname == sisp.CPname);
            }
            if (sisp.CPxh!= 0)
            {
                temp = temp.Where<YXB_Baojia>(u => u.CPXingHao == sisp.CPxh);
            }
            sisp.TotalCount = temp.Count();
            return temp.OrderByDescending<YXB_Baojia, DateTime>(u => u.AddTime).Skip<YXB_Baojia>((sisp.PageIndex - 1) * sisp.PageSize).Take<YXB_Baojia>(sisp.PageSize);

        }
        
    }
    
}
