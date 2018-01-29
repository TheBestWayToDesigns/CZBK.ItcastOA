using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace OldOaModel.DAL
{
    public class DbContextFactory
    {
        public static DbContext CreateDbContext()
        {

            DbContext dbContextT = (DbContext)CallContext.GetData("dbContextT");
            if (dbContextT == null)
            {
                dbContextT = new caigouEntities();
                CallContext.SetData("dbContextT", dbContextT);
            }
            return dbContextT;
        }
    }
}
