 

using CZBK.ItcastOA.DAL;
using CZBK.ItcastOA.IDAL;
using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.DALFactory
{
	public partial class DBSession : IDBSession
    {
	
		private IActionInfoDal _ActionInfoDal;
        public IActionInfoDal ActionInfoDal
        {
            get
            {
                if(_ActionInfoDal == null)
                {
                   // _ActionInfoDal = new ActionInfoDal();
				    _ActionInfoDal =AbstractFactory.CreateActionInfoDal();
                }
                return _ActionInfoDal;
            }
            set { _ActionInfoDal = value; }
        }
	
		private IBumenInfoSetDal _BumenInfoSetDal;
        public IBumenInfoSetDal BumenInfoSetDal
        {
            get
            {
                if(_BumenInfoSetDal == null)
                {
                   // _BumenInfoSetDal = new BumenInfoSetDal();
				    _BumenInfoSetDal =AbstractFactory.CreateBumenInfoSetDal();
                }
                return _BumenInfoSetDal;
            }
            set { _BumenInfoSetDal = value; }
        }
	
		private IDepartmentDal _DepartmentDal;
        public IDepartmentDal DepartmentDal
        {
            get
            {
                if(_DepartmentDal == null)
                {
                   // _DepartmentDal = new DepartmentDal();
				    _DepartmentDal =AbstractFactory.CreateDepartmentDal();
                }
                return _DepartmentDal;
            }
            set { _DepartmentDal = value; }
        }
	
		private IFileItemDal _FileItemDal;
        public IFileItemDal FileItemDal
        {
            get
            {
                if(_FileItemDal == null)
                {
                   // _FileItemDal = new FileItemDal();
				    _FileItemDal =AbstractFactory.CreateFileItemDal();
                }
                return _FileItemDal;
            }
            set { _FileItemDal = value; }
        }
	
		private ILogin_listDal _Login_listDal;
        public ILogin_listDal Login_listDal
        {
            get
            {
                if(_Login_listDal == null)
                {
                   // _Login_listDal = new Login_listDal();
				    _Login_listDal =AbstractFactory.CreateLogin_listDal();
                }
                return _Login_listDal;
            }
            set { _Login_listDal = value; }
        }
	
		private IR_UserInfo_ActionInfoDal _R_UserInfo_ActionInfoDal;
        public IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal
        {
            get
            {
                if(_R_UserInfo_ActionInfoDal == null)
                {
                   // _R_UserInfo_ActionInfoDal = new R_UserInfo_ActionInfoDal();
				    _R_UserInfo_ActionInfoDal =AbstractFactory.CreateR_UserInfo_ActionInfoDal();
                }
                return _R_UserInfo_ActionInfoDal;
            }
            set { _R_UserInfo_ActionInfoDal = value; }
        }
	
		private IRoleInfoDal _RoleInfoDal;
        public IRoleInfoDal RoleInfoDal
        {
            get
            {
                if(_RoleInfoDal == null)
                {
                   // _RoleInfoDal = new RoleInfoDal();
				    _RoleInfoDal =AbstractFactory.CreateRoleInfoDal();
                }
                return _RoleInfoDal;
            }
            set { _RoleInfoDal = value; }
        }
	
		private IScheduleDal _ScheduleDal;
        public IScheduleDal ScheduleDal
        {
            get
            {
                if(_ScheduleDal == null)
                {
                   // _ScheduleDal = new ScheduleDal();
				    _ScheduleDal =AbstractFactory.CreateScheduleDal();
                }
                return _ScheduleDal;
            }
            set { _ScheduleDal = value; }
        }
	
		private IScheduleTypeDal _ScheduleTypeDal;
        public IScheduleTypeDal ScheduleTypeDal
        {
            get
            {
                if(_ScheduleTypeDal == null)
                {
                   // _ScheduleTypeDal = new ScheduleTypeDal();
				    _ScheduleTypeDal =AbstractFactory.CreateScheduleTypeDal();
                }
                return _ScheduleTypeDal;
            }
            set { _ScheduleTypeDal = value; }
        }
	
		private IScheduleUserDal _ScheduleUserDal;
        public IScheduleUserDal ScheduleUserDal
        {
            get
            {
                if(_ScheduleUserDal == null)
                {
                   // _ScheduleUserDal = new ScheduleUserDal();
				    _ScheduleUserDal =AbstractFactory.CreateScheduleUserDal();
                }
                return _ScheduleUserDal;
            }
            set { _ScheduleUserDal = value; }
        }
	
		private IsysdiagramDal _sysdiagramDal;
        public IsysdiagramDal sysdiagramDal
        {
            get
            {
                if(_sysdiagramDal == null)
                {
                   // _sysdiagramDal = new sysdiagramDal();
				    _sysdiagramDal =AbstractFactory.CreatesysdiagramDal();
                }
                return _sysdiagramDal;
            }
            set { _sysdiagramDal = value; }
        }
	
		private ISysFieldDal _SysFieldDal;
        public ISysFieldDal SysFieldDal
        {
            get
            {
                if(_SysFieldDal == null)
                {
                   // _SysFieldDal = new SysFieldDal();
				    _SysFieldDal =AbstractFactory.CreateSysFieldDal();
                }
                return _SysFieldDal;
            }
            set { _SysFieldDal = value; }
        }
	
		private IT_BaoJiaToPDal _T_BaoJiaToPDal;
        public IT_BaoJiaToPDal T_BaoJiaToPDal
        {
            get
            {
                if(_T_BaoJiaToPDal == null)
                {
                   // _T_BaoJiaToPDal = new T_BaoJiaToPDal();
				    _T_BaoJiaToPDal =AbstractFactory.CreateT_BaoJiaToPDal();
                }
                return _T_BaoJiaToPDal;
            }
            set { _T_BaoJiaToPDal = value; }
        }
	
		private IT_BoolItemDal _T_BoolItemDal;
        public IT_BoolItemDal T_BoolItemDal
        {
            get
            {
                if(_T_BoolItemDal == null)
                {
                   // _T_BoolItemDal = new T_BoolItemDal();
				    _T_BoolItemDal =AbstractFactory.CreateT_BoolItemDal();
                }
                return _T_BoolItemDal;
            }
            set { _T_BoolItemDal = value; }
        }
	
		private IT_CanPanDal _T_CanPanDal;
        public IT_CanPanDal T_CanPanDal
        {
            get
            {
                if(_T_CanPanDal == null)
                {
                   // _T_CanPanDal = new T_CanPanDal();
				    _T_CanPanDal =AbstractFactory.CreateT_CanPanDal();
                }
                return _T_CanPanDal;
            }
            set { _T_CanPanDal = value; }
        }
	
		private IT_ChanPinNameDal _T_ChanPinNameDal;
        public IT_ChanPinNameDal T_ChanPinNameDal
        {
            get
            {
                if(_T_ChanPinNameDal == null)
                {
                   // _T_ChanPinNameDal = new T_ChanPinNameDal();
				    _T_ChanPinNameDal =AbstractFactory.CreateT_ChanPinNameDal();
                }
                return _T_ChanPinNameDal;
            }
            set { _T_ChanPinNameDal = value; }
        }
	
		private IT_SczzDanjuDal _T_SczzDanjuDal;
        public IT_SczzDanjuDal T_SczzDanjuDal
        {
            get
            {
                if(_T_SczzDanjuDal == null)
                {
                   // _T_SczzDanjuDal = new T_SczzDanjuDal();
				    _T_SczzDanjuDal =AbstractFactory.CreateT_SczzDanjuDal();
                }
                return _T_SczzDanjuDal;
            }
            set { _T_SczzDanjuDal = value; }
        }
	
		private IT_SczzItemDal _T_SczzItemDal;
        public IT_SczzItemDal T_SczzItemDal
        {
            get
            {
                if(_T_SczzItemDal == null)
                {
                   // _T_SczzItemDal = new T_SczzItemDal();
				    _T_SczzItemDal =AbstractFactory.CreateT_SczzItemDal();
                }
                return _T_SczzItemDal;
            }
            set { _T_SczzItemDal = value; }
        }
	
		private IT_ShengChanZhiZhaoTopNameDal _T_ShengChanZhiZhaoTopNameDal;
        public IT_ShengChanZhiZhaoTopNameDal T_ShengChanZhiZhaoTopNameDal
        {
            get
            {
                if(_T_ShengChanZhiZhaoTopNameDal == null)
                {
                   // _T_ShengChanZhiZhaoTopNameDal = new T_ShengChanZhiZhaoTopNameDal();
				    _T_ShengChanZhiZhaoTopNameDal =AbstractFactory.CreateT_ShengChanZhiZhaoTopNameDal();
                }
                return _T_ShengChanZhiZhaoTopNameDal;
            }
            set { _T_ShengChanZhiZhaoTopNameDal = value; }
        }
	
		private IT_WinBakDal _T_WinBakDal;
        public IT_WinBakDal T_WinBakDal
        {
            get
            {
                if(_T_WinBakDal == null)
                {
                   // _T_WinBakDal = new T_WinBakDal();
				    _T_WinBakDal =AbstractFactory.CreateT_WinBakDal();
                }
                return _T_WinBakDal;
            }
            set { _T_WinBakDal = value; }
        }
	
		private IT_WinBakFaHuoDal _T_WinBakFaHuoDal;
        public IT_WinBakFaHuoDal T_WinBakFaHuoDal
        {
            get
            {
                if(_T_WinBakFaHuoDal == null)
                {
                   // _T_WinBakFaHuoDal = new T_WinBakFaHuoDal();
				    _T_WinBakFaHuoDal =AbstractFactory.CreateT_WinBakFaHuoDal();
                }
                return _T_WinBakFaHuoDal;
            }
            set { _T_WinBakFaHuoDal = value; }
        }
	
		private IT_YSItemsDal _T_YSItemsDal;
        public IT_YSItemsDal T_YSItemsDal
        {
            get
            {
                if(_T_YSItemsDal == null)
                {
                   // _T_YSItemsDal = new T_YSItemsDal();
				    _T_YSItemsDal =AbstractFactory.CreateT_YSItemsDal();
                }
                return _T_YSItemsDal;
            }
            set { _T_YSItemsDal = value; }
        }
	
		private IUserInfoDal _UserInfoDal;
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if(_UserInfoDal == null)
                {
                   // _UserInfoDal = new UserInfoDal();
				    _UserInfoDal =AbstractFactory.CreateUserInfoDal();
                }
                return _UserInfoDal;
            }
            set { _UserInfoDal = value; }
        }
	
		private IYXB_BaojiaDal _YXB_BaojiaDal;
        public IYXB_BaojiaDal YXB_BaojiaDal
        {
            get
            {
                if(_YXB_BaojiaDal == null)
                {
                   // _YXB_BaojiaDal = new YXB_BaojiaDal();
				    _YXB_BaojiaDal =AbstractFactory.CreateYXB_BaojiaDal();
                }
                return _YXB_BaojiaDal;
            }
            set { _YXB_BaojiaDal = value; }
        }
	
		private IYXB_Kh_listDal _YXB_Kh_listDal;
        public IYXB_Kh_listDal YXB_Kh_listDal
        {
            get
            {
                if(_YXB_Kh_listDal == null)
                {
                   // _YXB_Kh_listDal = new YXB_Kh_listDal();
				    _YXB_Kh_listDal =AbstractFactory.CreateYXB_Kh_listDal();
                }
                return _YXB_Kh_listDal;
            }
            set { _YXB_Kh_listDal = value; }
        }
	}	
}