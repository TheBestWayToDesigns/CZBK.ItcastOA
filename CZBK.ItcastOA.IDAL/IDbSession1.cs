 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IDAL
{
	public partial interface IDBSession
    {

	
		IActionInfoDal ActionInfoDal{get;set;}
	
		IBumenInfoSetDal BumenInfoSetDal{get;set;}
	
		ICarNumberDal CarNumberDal{get;set;}
	
		IDepartmentDal DepartmentDal{get;set;}
	
		IExamineScheduleDal ExamineScheduleDal{get;set;}
	
		IFileItemDal FileItemDal{get;set;}
	
		IFileTypeDal FileTypeDal{get;set;}
	
		IGongLuBaoXiaoBillDal GongLuBaoXiaoBillDal{get;set;}
	
		ILogin_listDal Login_listDal{get;set;}
	
		IR_UserInfo_ActionInfoDal R_UserInfo_ActionInfoDal{get;set;}
	
		IRoleInfoDal RoleInfoDal{get;set;}
	
		IScheduleDal ScheduleDal{get;set;}
	
		IScheduleTypeDal ScheduleTypeDal{get;set;}
	
		IScheduleUserDal ScheduleUserDal{get;set;}
	
		IShareFileOrNoticeDal ShareFileOrNoticeDal{get;set;}
	
		IShareTypeDal ShareTypeDal{get;set;}
	
		IsysdiagramDal sysdiagramDal{get;set;}
	
		ISysFieldDal SysFieldDal{get;set;}
	
		IT_BaoJiaToPDal T_BaoJiaToPDal{get;set;}
	
		IT_BaoXiaoBillDal T_BaoXiaoBillDal{get;set;}
	
		IT_BaoxiaoItemsDal T_BaoxiaoItemsDal{get;set;}
	
		IT_BoolItemDal T_BoolItemDal{get;set;}
	
		IT_CanPanDal T_CanPanDal{get;set;}
	
		IT_ChanPinNameDal T_ChanPinNameDal{get;set;}
	
		IT_JieKuanBillDal T_JieKuanBillDal{get;set;}
	
		IT_SczzDanjuDal T_SczzDanjuDal{get;set;}
	
		IT_SczzItemDal T_SczzItemDal{get;set;}
	
		IT_ShengChanZhiZhaoTopNameDal T_ShengChanZhiZhaoTopNameDal{get;set;}
	
		IT_WinBakDal T_WinBakDal{get;set;}
	
		IT_WinBakFaHuoDal T_WinBakFaHuoDal{get;set;}
	
		IT_YSItemsDal T_YSItemsDal{get;set;}
	
		IUserInfoDal UserInfoDal{get;set;}
	
		IYJ_ScheduleActionDal YJ_ScheduleActionDal{get;set;}
	
		IYJ_ScheduleDayDal YJ_ScheduleDayDal{get;set;}
	
		IYXB_BaojiaDal YXB_BaojiaDal{get;set;}
	
		IYXB_BaoJiaEidtMoneyDal YXB_BaoJiaEidtMoneyDal{get;set;}
	
		IYXB_Kh_listDal YXB_Kh_listDal{get;set;}
	
		IYXB_WinCanPinDal YXB_WinCanPinDal{get;set;}
	}	
}