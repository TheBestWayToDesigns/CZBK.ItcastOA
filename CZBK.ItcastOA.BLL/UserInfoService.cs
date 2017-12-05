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
   public partial class UserInfoService:BaseService<UserInfo>,IUserInfoService
    {



       /// <summary>
       /// 批量删除
       /// </summary>
       /// <param name="list">要删除的记录的编号</param>
       /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
           
            var userInfoList = this.GetCurrentDbSession.UserInfoDal.LoadEntities(c=>list.Contains(c.ID));
            foreach (var userInfo in userInfoList)
            {
                this.GetCurrentDbSession.UserInfoDal.DeleteEntity(userInfo);
            }

            return this.GetCurrentDbSession.SaveChanges();
        }
        /// <summary>
        /// 删除特殊用户权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public bool DelUserinfo_actioninfo(int userID, int actionID)
        {
            var R_userinfo_actioninfo = this.GetCurrentDbSession.R_UserInfo_ActionInfoDal.LoadEntities(a => a.UserInfoID == userID && a.ActionInfoID == actionID).FirstOrDefault();
            if (R_userinfo_actioninfo != null)
            {
                this.GetCurrentDbSession.R_UserInfo_ActionInfoDal.DeleteEntity(R_userinfo_actioninfo);
                return this.GetCurrentDbSession.SaveChanges();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 多条件搜索用户信息
        /// </summary>
        /// <param name="userInfoSearchParam"></param>
        /// <returns></returns>
        public IQueryable<UserInfo> LoadSearchEntities(Model.SearchParam.UserInfoParam userInfoSearchParam)
        {

            short delFlag=(short)DelFlagEnum.Normarl;
            var temp = this.GetCurrentDbSession.UserInfoDal.LoadEntities(c => c.DelFlag == delFlag);
            if (userInfoSearchParam.QuXian != null)
            {
                if (userInfoSearchParam.QuXian != 999)
                {
                    temp = this.GetCurrentDbSession.UserInfoDal.LoadEntities(c => c.DelFlag == delFlag && c.BuMenID == userInfoSearchParam.BumenID);
                }
            }
            else
            { }
            
          
            if (userInfoSearchParam.IsMaster)
            {
                temp = temp.Where<UserInfo>(u => u.ThisMastr== userInfoSearchParam.IsMaster);
            }
            if (userInfoSearchParam.C_id>0)
            {
                temp = temp.Where<UserInfo>(u => u.MasterID==userInfoSearchParam.C_id);
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.UserName))
            {
                temp = temp.Where<UserInfo>(u=>u.UName.Contains(userInfoSearchParam.UserName));
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Remark))
            {
                temp = temp.Where<UserInfo>(u=>u.Remark.Contains(userInfoSearchParam.Remark));
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Remark))
            {
                temp = temp.Where<UserInfo>(u => u.Remark.Contains(userInfoSearchParam.Remark));
            }
            userInfoSearchParam.TotalCount = temp.Count();
            return temp.OrderBy<UserInfo, int>(u => u.ID).Skip<UserInfo>((userInfoSearchParam.PageIndex - 1) * userInfoSearchParam.PageSize).Take<UserInfo>(userInfoSearchParam.PageSize);

        }
        /// <summary>
        /// 完成对特殊用户权限的分配
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actionId"></param>
        /// <param name="ispass"></param>
        /// <returns></returns>
        public bool SetUserActionInfo(int userId, int actionId, bool ispass)
        {
            var R_userinfo_actioninfo = this.GetCurrentDbSession.R_UserInfo_ActionInfoDal.LoadEntities(a => a.ID == userId && a.ActionInfoID == actionId).FirstOrDefault();
            if (R_userinfo_actioninfo == null)
            {
                R_UserInfo_ActionInfo rua = new R_UserInfo_ActionInfo();
                rua.ActionInfoID = actionId;
                rua.UserInfoID = userId;
                rua.IsPass = ispass;
                this.GetCurrentDbSession.R_UserInfo_ActionInfoDal.AddEntity(rua);
            }
            else
            {
                R_userinfo_actioninfo.IsPass = ispass;
            }
            return this.GetCurrentDbSession.SaveChanges();
        }

        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool setuserorderidnfo(int userid, List<int> list)
        {
            var userinfo = this.GetCurrentDbSession.UserInfoDal.LoadEntities(u
                => u.ID == userid).FirstOrDefault();//获取用户
            if (userinfo != null)
            {
                //删除用户角色
                userinfo.RoleInfoes.Clear();

                foreach (int roleid in list)
                {
                    var roleinfo = this.GetCurrentDbSession.RoleInfoDal.LoadEntities(r => r.ID == roleid).FirstOrDefault();
                    userinfo.RoleInfoes.Add(roleinfo);//通过导航属性RoleInfo 进行修改
                }
                return this.GetCurrentDbSession.SaveChanges();//最后执行savechanges
            }
            else
            {
                return false;
            }
        }
    }
   
}
