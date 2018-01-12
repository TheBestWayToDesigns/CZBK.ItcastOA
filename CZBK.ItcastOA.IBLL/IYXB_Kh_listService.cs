using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;

namespace CZBK.ItcastOA.IBLL
{
    
    public partial interface IYXB_Kh_listService : IBaseService<YXB_Kh_list>
    {

        IQueryable<YXB_Kh_list> loadBaoBeientities(UserInfoParam userInfoSearchParam);

    }
}
