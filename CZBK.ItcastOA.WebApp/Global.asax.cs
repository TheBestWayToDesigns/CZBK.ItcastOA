using CZBK.ItcastOA.BLL;
using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.WebApp.Models;
using log4net;
using Spring.Context;
using Spring.Context.Support;
using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CZBK.ItcastOA.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : SpringMvcApplication //System.Web.HttpApplication
    {
        public static int editC { get; set; }
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();//读取Log4Net配置信息
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string fileLogPath = Server.MapPath("/Log/");
            //WaitCallback
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    if (MyExceptionAttribute.ExceptionQueue.Count > 0)
                    {
                       Exception ex= MyExceptionAttribute.ExceptionQueue.Dequeue();//出队
                       //string fileName = DateTime.Now.ToString("yyyy-MM-dd")+".txt";
                       //File.AppendAllText(fileLogPath + fileName, ex.ToString(), System.Text.Encoding.Default);
                       ILog logger = LogManager.GetLogger("errorMsg");
                       logger.Error(ex.ToString());
                    }
                    else
                    {
                        Thread.Sleep(3000);//如果队列中没有数据，休息避免造成CPU的空转.
                    }
                }


            },fileLogPath);

        }

        public static DateTime GetT_time()
        {
            return DateTime.Now;
        }

      
   
     

       
    }
}