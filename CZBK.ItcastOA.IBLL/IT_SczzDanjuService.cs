using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
 
    public partial interface IT_SczzDanjuService : IBaseService<T_SczzDanju>
    {

        IQueryable<T_SczzDanju> LoadSearchEntities(UserInfoParam userInfoSearchParam);

    }
}
