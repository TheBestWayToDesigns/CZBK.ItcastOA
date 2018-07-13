 

using CZBK.ItcastOA.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.DALFactory
{
    public partial class AbstractFactory
    {
      
   
		
	    public static IActionInfoDal CreateActionInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ActionInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IActionInfoDal;
        }
		
	    public static IBumenInfoSetDal CreateBumenInfoSetDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".BumenInfoSetDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IBumenInfoSetDal;
        }
		
	    public static ICarNumberDal CreateCarNumberDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".CarNumberDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as ICarNumberDal;
        }
		
	    public static IDepartmentDal CreateDepartmentDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".DepartmentDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IDepartmentDal;
        }
		
	    public static IExamineScheduleDal CreateExamineScheduleDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ExamineScheduleDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IExamineScheduleDal;
        }
		
	    public static IFileItemDal CreateFileItemDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".FileItemDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IFileItemDal;
        }
		
	    public static IFileTypeDal CreateFileTypeDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".FileTypeDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IFileTypeDal;
        }
		
	    public static IGongLuBaoXiaoBillDal CreateGongLuBaoXiaoBillDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".GongLuBaoXiaoBillDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IGongLuBaoXiaoBillDal;
        }
		
	    public static ILogin_listDal CreateLogin_listDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".Login_listDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as ILogin_listDal;
        }
		
	    public static IR_UserInfo_ActionInfoDal CreateR_UserInfo_ActionInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".R_UserInfo_ActionInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IR_UserInfo_ActionInfoDal;
        }
		
	    public static IRoleInfoDal CreateRoleInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".RoleInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IRoleInfoDal;
        }
		
	    public static IScheduleDal CreateScheduleDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ScheduleDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IScheduleDal;
        }
		
	    public static IScheduleTypeDal CreateScheduleTypeDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ScheduleTypeDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IScheduleTypeDal;
        }
		
	    public static IScheduleUserDal CreateScheduleUserDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ScheduleUserDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IScheduleUserDal;
        }
		
	    public static ISeb_NumberDal CreateSeb_NumberDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".Seb_NumberDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as ISeb_NumberDal;
        }
		
	    public static IShareFileOrNoticeDal CreateShareFileOrNoticeDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ShareFileOrNoticeDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IShareFileOrNoticeDal;
        }
		
	    public static IShareTypeDal CreateShareTypeDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".ShareTypeDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IShareTypeDal;
        }
		
	    public static IsysdiagramDal CreatesysdiagramDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".sysdiagramDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IsysdiagramDal;
        }
		
	    public static ISysFieldDal CreateSysFieldDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".SysFieldDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as ISysFieldDal;
        }
		
	    public static IT_BaojiaItemIDDal CreateT_BaojiaItemIDDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_BaojiaItemIDDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_BaojiaItemIDDal;
        }
		
	    public static IT_BaoJiaToPDal CreateT_BaoJiaToPDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_BaoJiaToPDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_BaoJiaToPDal;
        }
		
	    public static IT_BaoXiaoBillDal CreateT_BaoXiaoBillDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_BaoXiaoBillDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_BaoXiaoBillDal;
        }
		
	    public static IT_BaoxiaoItemsDal CreateT_BaoxiaoItemsDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_BaoxiaoItemsDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_BaoxiaoItemsDal;
        }
		
	    public static IT_BoolItemDal CreateT_BoolItemDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_BoolItemDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_BoolItemDal;
        }
		
	    public static IT_CanPanDal CreateT_CanPanDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_CanPanDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_CanPanDal;
        }
		
	    public static IT_ChanPinNameDal CreateT_ChanPinNameDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_ChanPinNameDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_ChanPinNameDal;
        }
		
	    public static IT_CSC_CardDal CreateT_CSC_CardDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_CSC_CardDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_CSC_CardDal;
        }
		
	    public static IT_jgzztjbDal CreateT_jgzztjbDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_jgzztjbDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_jgzztjbDal;
        }
		
	    public static IT_JieKuanBillDal CreateT_JieKuanBillDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_JieKuanBillDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_JieKuanBillDal;
        }
		
	    public static IT_jxzztjbDal CreateT_jxzztjbDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_jxzztjbDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_jxzztjbDal;
        }
		
	    public static IT_SCCJDal CreateT_SCCJDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_SCCJDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_SCCJDal;
        }
		
	    public static IT_SczzDanjuDal CreateT_SczzDanjuDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_SczzDanjuDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_SczzDanjuDal;
        }
		
	    public static IT_SczzItemDal CreateT_SczzItemDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_SczzItemDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_SczzItemDal;
        }
		
	    public static IT_ShengChanZhiZhaoTopNameDal CreateT_ShengChanZhiZhaoTopNameDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_ShengChanZhiZhaoTopNameDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_ShengChanZhiZhaoTopNameDal;
        }
		
	    public static IT_WinBakDal CreateT_WinBakDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_WinBakDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_WinBakDal;
        }
		
	    public static IT_WinBakFaHuoDal CreateT_WinBakFaHuoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_WinBakFaHuoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_WinBakFaHuoDal;
        }
		
	    public static IT_YSItemsDal CreateT_YSItemsDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_YSItemsDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_YSItemsDal;
        }
		
	    public static IT_YXbj_masterDal CreateT_YXbj_masterDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_YXbj_masterDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_YXbj_masterDal;
        }
		
	    public static IT_ZhiPiaoTongDal CreateT_ZhiPiaoTongDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_ZhiPiaoTongDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_ZhiPiaoTongDal;
        }
		
	    public static IUser_Person_sltDal CreateUser_Person_sltDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".User_Person_sltDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IUser_Person_sltDal;
        }
		
	    public static IUserbakDal CreateUserbakDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".UserbakDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IUserbakDal;
        }
		
	    public static IUserInfoDal CreateUserInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".UserInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IUserInfoDal;
        }
		
	    public static IWXX_FormIDDal CreateWXX_FormIDDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXX_FormIDDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXX_FormIDDal;
        }
		
	    public static IWXXBaoJiaQuanXianDal CreateWXXBaoJiaQuanXianDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXXBaoJiaQuanXianDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXXBaoJiaQuanXianDal;
        }
		
	    public static IWXXMenuInfoDal CreateWXXMenuInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXXMenuInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXXMenuInfoDal;
        }
		
	    public static IWXXPhoneNumDal CreateWXXPhoneNumDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXXPhoneNumDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXXPhoneNumDal;
        }
		
	    public static IWXXScoreInfoDal CreateWXXScoreInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXXScoreInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXXScoreInfoDal;
        }
		
	    public static IWXXScoreUserDal CreateWXXScoreUserDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXXScoreUserDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXXScoreUserDal;
        }
		
	    public static IWXXUserInfoDal CreateWXXUserInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".WXXUserInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IWXXUserInfoDal;
        }
		
	    public static IYJ_ScheduleActionDal CreateYJ_ScheduleActionDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YJ_ScheduleActionDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYJ_ScheduleActionDal;
        }
		
	    public static IYJ_ScheduleDayDal CreateYJ_ScheduleDayDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YJ_ScheduleDayDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYJ_ScheduleDayDal;
        }
		
	    public static IYSGPinfoDal CreateYSGPinfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YSGPinfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYSGPinfoDal;
        }
		
	    public static IYSGPtopDal CreateYSGPtopDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YSGPtopDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYSGPtopDal;
        }
		
	    public static IYXB_BaojiaDal CreateYXB_BaojiaDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YXB_BaojiaDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYXB_BaojiaDal;
        }
		
	    public static IYXB_BaoJiaEidtMoneyDal CreateYXB_BaoJiaEidtMoneyDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YXB_BaoJiaEidtMoneyDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYXB_BaoJiaEidtMoneyDal;
        }
		
	    public static IYXB_Kh_listDal CreateYXB_Kh_listDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YXB_Kh_listDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYXB_Kh_listDal;
        }
		
	    public static IYXB_WinCanPinDal CreateYXB_WinCanPinDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".YXB_WinCanPinDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IYXB_WinCanPinDal;
        }
	}
	
}