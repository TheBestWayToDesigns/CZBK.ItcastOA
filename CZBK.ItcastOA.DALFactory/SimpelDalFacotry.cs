 

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
		
	    public static IT_JieKuanBillDal CreateT_JieKuanBillDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".T_JieKuanBillDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IT_JieKuanBillDal;
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
		
	    public static IUserInfoDal CreateUserInfoDal()
        {

            string classFulleName = ConfigurationManager.AppSettings["NameSpace"] + ".UserInfoDal";


            //object obj = Assembly.Load(ConfigurationManager.AppSettings["DalAssembly"]).CreateInstance(classFulleName, true);
            var obj  = CreateInstance(ConfigurationManager.AppSettings["DalAssemblyPath"], classFulleName);


            return obj as IUserInfoDal;
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