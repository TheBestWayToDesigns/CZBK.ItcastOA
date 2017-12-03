 
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
	
	public partial class ActionInfoService :BaseService<ActionInfo>,IActionInfoService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.ActionInfoDal;
        }
    }   
	
	public partial class BumenInfoSetService :BaseService<BumenInfoSet>,IBumenInfoSetService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.BumenInfoSetDal;
        }
    }   
	
	public partial class DepartmentService :BaseService<Department>,IDepartmentService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.DepartmentDal;
        }
    }   
	
	public partial class FileItemService :BaseService<FileItem>,IFileItemService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.FileItemDal;
        }
    }   
	
	public partial class Login_listService :BaseService<Login_list>,ILogin_listService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.Login_listDal;
        }
    }   
	
	public partial class R_UserInfo_ActionInfoService :BaseService<R_UserInfo_ActionInfo>,IR_UserInfo_ActionInfoService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.R_UserInfo_ActionInfoDal;
        }
    }   
	
	public partial class RoleInfoService :BaseService<RoleInfo>,IRoleInfoService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.RoleInfoDal;
        }
    }   
	
	public partial class ScheduleService :BaseService<Schedule>,IScheduleService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.ScheduleDal;
        }
    }   
	
	public partial class ScheduleTypeService :BaseService<ScheduleType>,IScheduleTypeService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.ScheduleTypeDal;
        }
    }   
	
	public partial class ScheduleUserService :BaseService<ScheduleUser>,IScheduleUserService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.ScheduleUserDal;
        }
    }   
	
	public partial class SysFieldService :BaseService<SysField>,ISysFieldService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.SysFieldDal;
        }
    }   
	
	public partial class T_BaoJiaToPService :BaseService<T_BaoJiaToP>,IT_BaoJiaToPService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_BaoJiaToPDal;
        }
    }   
	
	public partial class T_BoolItemService :BaseService<T_BoolItem>,IT_BoolItemService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_BoolItemDal;
        }
    }   
	
	public partial class T_CanPanService :BaseService<T_CanPan>,IT_CanPanService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_CanPanDal;
        }
    }   
	
	public partial class T_ChanPinNameService :BaseService<T_ChanPinName>,IT_ChanPinNameService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_ChanPinNameDal;
        }
    }   
	
	public partial class T_SczzDanjuService :BaseService<T_SczzDanju>,IT_SczzDanjuService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_SczzDanjuDal;
        }
    }   
	
	public partial class T_SczzItemService :BaseService<T_SczzItem>,IT_SczzItemService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_SczzItemDal;
        }
    }   
	
	public partial class T_ShengChanZhiZhaoTopNameService :BaseService<T_ShengChanZhiZhaoTopName>,IT_ShengChanZhiZhaoTopNameService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_ShengChanZhiZhaoTopNameDal;
        }
    }   
	
	public partial class T_WinBakService :BaseService<T_WinBak>,IT_WinBakService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_WinBakDal;
        }
    }   
	
	public partial class T_WinBakFaHuoService :BaseService<T_WinBakFaHuo>,IT_WinBakFaHuoService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_WinBakFaHuoDal;
        }
    }   
	
	public partial class T_YSItemsService :BaseService<T_YSItems>,IT_YSItemsService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.T_YSItemsDal;
        }
    }   
	
	public partial class UserInfoService :BaseService<UserInfo>,IUserInfoService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.UserInfoDal;
        }
    }   
	
	public partial class YXB_BaojiaService :BaseService<YXB_Baojia>,IYXB_BaojiaService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.YXB_BaojiaDal;
        }
    }   
	
	public partial class YXB_Kh_listService :BaseService<YXB_Kh_list>,IYXB_Kh_listService
    {
        public override void SetCurretnDal()
        {
            CurrentDal = this.GetCurrentDbSession.YXB_Kh_listDal;
        }
    }   
	
}