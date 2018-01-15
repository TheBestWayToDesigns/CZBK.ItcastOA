using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
     public partial  interface IYJ_ScheduleDayService:IBaseService<YJ_ScheduleDay>
    {
        /// <summary>
        /// 新增每日总汇数据
        /// </summary>
        /// <param name="ysdday">新增数据</param>
        /// <returns></returns>
        bool NewAddSEDDAY(YjsdayClass ysdday);
        /// <summary>
        /// 获取审核人员名称字符串
        /// </summary>
        /// <param name="dte">要查询的时间</param>
        /// <param name="UinfoId">要查询的人员ID</param>
        /// <returns></returns>
        string GetReadPerson(DateTime dte, int UinfoId);
    }
}
