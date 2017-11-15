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
   
    public partial class T_SczzDanjuService : BaseService<T_SczzDanju>, IT_SczzDanjuService
    {
        public IQueryable<T_SczzDanju> LoadSearchEntities(Model.SearchParam.UserInfoParam usp)
        {

            var temp = this.GetCurrentDbSession.T_SczzDanjuDal.LoadEntities(u => u.AddTime >= usp.Uptime && u.AddTime <= usp.Dwtime&&u.del==0);

            //temp.Where(u=>u.T_BaoJiaToP.T_WinBak)
            if (usp.Jhname != 0)
            {
                temp = temp.Where<T_SczzDanju>(x =>x.TextNameID==usp.Jhname);
            }

            if (usp.adduser > 0)
            {
                temp = temp.Where<T_SczzDanju>(u => u.AddUser==usp.adduser);
            }
            if (usp.SHuser != 0)
            {
                temp = temp.Where<T_SczzDanju>(u => u.UpShenHe == usp.SHuser);
                temp = temp.Where<T_SczzDanju>(u => u.ShenChanShenHe == usp.SHuser);
                temp = temp.Where<T_SczzDanju>(u => u.CheJianShenHe == usp.SHuser);
                temp = temp.Where<T_SczzDanju>(u => u.Jhzdr == usp.SHuser);
            }
            if (usp.CPtext != 0)
            {
                temp = temp.Where<T_SczzDanju>(u => Convert.ToBoolean( u.T_CanPan.Select(x=>x.SczzItemID==usp.CPtext)));
            }
            if (usp.zt>=0)
            {
                temp = temp.Where<T_SczzDanju>(u => u.ZhuangTai==usp.zt);
            }
            usp.TotalCount = temp.Count();
            return temp.OrderByDescending<T_SczzDanju, DateTime?>(u => u.AddTime).Skip<T_SczzDanju>((usp.PageIndex - 1) * usp.PageSize).Take<T_SczzDanju>(usp.PageSize);

        }
    }
}
