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
    public partial class T_WinBakService : BaseService<T_WinBak>, IT_WinBakService
    {
        public IQueryable<T_WinBak> LoadSearchEntities(Model.SearchParam.UserInfoParam sisp)
        {
           
            var temp = this.GetCurrentDbSession.T_WinBakDal.LoadEntities(u => u.ID>0);
            if (sisp.Uptime.ToString("yyyy-MM-dd")!= "0001-01-01")
            {
                temp = this.GetCurrentDbSession.T_WinBakDal.LoadEntities(u => u.AddTime >= sisp.Uptime && u.AddTime <= sisp.Dwtime);
            }
            if (sisp.zt != 0)
            {
                if (sisp.zt == -99)
                { temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.T_WinBak.FirstOrDefault() != null ? u.T_BaoJiaToP.T_WinBak.FirstOrDefault().YuanYin !=1 : u.ID > 0); }
                else
                { temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.T_WinBak.FirstOrDefault() != null ? u.T_BaoJiaToP.T_WinBak.FirstOrDefault().YuanYin == sisp.zt : u.ID > 0); }
                
            }
            if (sisp.addess!=null)
            {
                temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.Addess.Contains(sisp.addess));
            }
            if (sisp.Person!=0)
            {
                temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.YXB_Kh_list.UserInfo.ID == sisp.Person);
            }
            if (sisp.KHname != 0)
            {
                temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.Kh_List_id== sisp.KHname);
            }
            if (sisp.CPname!=0)
            {
                temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.YXB_Baojia.Where(m => m.CPname == sisp.CPname).DefaultIfEmpty() != null?Convert.ToBoolean( u.T_BaoJiaToP.YXB_Baojia.Select(m=>m.CPname==sisp.CPname)): Convert.ToBoolean(u.T_BaoJiaToP.YXB_Baojia.Select(m => m.ZhuangTai == 1)));
            }
            if (sisp.CPxh!= 0)
            {
                temp = temp.Where<T_WinBak>(u => u.T_BaoJiaToP.YXB_Baojia.Where(m => m.CPXingHao == sisp.CPxh).DefaultIfEmpty() != null ? Convert.ToBoolean(u.T_BaoJiaToP.YXB_Baojia.Select(m => m.CPXingHao == sisp.CPxh)) : Convert.ToBoolean(u.T_BaoJiaToP.YXB_Baojia.Select(m => m.ZhuangTai == 1)));
            }
            sisp.TotalCount = temp.Count();
            return temp.OrderByDescending<T_WinBak, DateTime?>(u => u.AddTime).Skip<T_WinBak>((sisp.PageIndex - 1) * sisp.PageSize).Take<T_WinBak>(sisp.PageSize);

        }
        public bool UpHeTongWinADD(List<YXB_Baojia> Lybj, T_WinBak twb)
        {
            foreach (var lb in Lybj)
            {
                GetCurrentDbSession.YXB_BaojiaDal.EditEntity(lb);
                YXB_BaoJiaEidtMoney bjm = new YXB_BaoJiaEidtMoney();
                bjm.YXB_BJ_ID = lb.id;
                bjm.EidtUser_ID =Convert.ToInt32(lb.UpdataUserID);
                bjm.EidtTime = DateTime.Now;
                bjm.EditBJMoney =Convert.ToDecimal( lb.WinMoney);
                bjm.EditYFMoney = Convert.ToDecimal(lb.WinYunFei);
                GetCurrentDbSession.YXB_BaoJiaEidtMoneyDal.AddEntity(bjm);
            }
            GetCurrentDbSession.T_WinBakDal.AddEntity(twb);
            if (GetCurrentDbSession.SaveChanges())
            {
                return true;

            }
            else
            {
                return false;
            }
            
        }
    }
}
