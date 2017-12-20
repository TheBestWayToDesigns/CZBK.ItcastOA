using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.SignalR
{
    public class MessageHub:Hub
    {
        public void NoticeUser()
        {
            Clients.User("123").newLog();
        }
    }
}