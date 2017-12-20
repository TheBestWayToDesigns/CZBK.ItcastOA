using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CZBK.ItcastOA.Model;

namespace CZBK.ItcastOA.WebApp.Hubs
{
    public class ChatHub : Hub
    {
        private IList<string> userList = UserInfo.userList;

        private readonly static Dictionary<string, string> _connections = new Dictionary<string, string>();
        /// </summary>
        /// <param name="name1">发起者</param>
        /// <param name="name2">消息接收者</param>
        public void SendByGroup(string name1, string name2)
        {
            //Client内为用户的id，是唯一的，SendMessage函数是前端函数，意思是服务器将该消息推送至前端
            Clients.Client(_connections[name2]).SendMessage("来自用户" + name1 + " " + DateTime.Now.ToString("yyyy/MM/ddhh:mm:ss") + "的消息推送！");
        }

        /// <summary>
        /// 用户上线函数
        /// </summary>
        /// <param name="name"></param>
        public void SendLogin(string name)
        {
            if (!userList.Contains(name))
            {
                userList.Add(name);
                //这里便是将用户id和姓名联系起来
                _connections.Add(name, Context.ConnectionId);
            }
            else
            {
                //每次登陆id会发生变化
                _connections[name] = Context.ConnectionId;
            }
            //新用户上线，服务器广播该用户名
            Clients.All.loginUser(userList);
        }
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
        public void Hello()
        {
            Clients.All.hello();
        }
    }
    public class UserInfo
    {
        public static IList<string> userList = new List<string>();
    }
}