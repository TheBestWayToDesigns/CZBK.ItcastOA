using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace CZBK.ItcastOA.TModel.DAL
{
    public class DbContextFactory
    {
        public static DbContext CreateDbContext()
        {

            DbContext dbContextT = (DbContext)CallContext.GetData("dbContextT");
            if (dbContextT == null)
            {
                dbContextT = new UFTData439745_000001Entities();
                CallContext.SetData("dbContextT", dbContextT);
            }
            return dbContextT;
        }
    }
}