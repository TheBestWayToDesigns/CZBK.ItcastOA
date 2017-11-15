using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
    public partial interface IYXB_BaojiaService : IBaseService<YXB_Baojia>
    {

        IQueryable<YXB_Baojia> LoadSearchEntities(UserInfoParam userInfoSearchParam);

    }
    
}
