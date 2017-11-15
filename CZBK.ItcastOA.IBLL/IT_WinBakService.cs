using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CZBK.ItcastOA.IBLL
{
   
    public partial interface IT_WinBakService : IBaseService<T_WinBak>
    {
       
        IQueryable<T_WinBak> LoadSearchEntities(UserInfoParam userInfoSearchParam);
        bool UpHeTongWinADD(List<YXB_Baojia> Lybj, T_WinBak twb);


    }
}
