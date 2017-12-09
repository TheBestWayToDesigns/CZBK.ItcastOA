using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model
{
    public class EqualityComparer : IEqualityComparer<ActionInfo>
    {
        public bool Equals(ActionInfo x, ActionInfo y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(ActionInfo obj)
        {
            return obj.GetHashCode();
        }
    }
    public class EComSczz : IEqualityComparer<T_SczzDanju>
    {
        public bool Equals(T_SczzDanju x, T_SczzDanju y)
        {
            return x.AddUser == y.AddUser;
        }

        public int GetHashCode(T_SczzDanju obj)
        {
            return obj.GetHashCode();
        }
    }
    public class EqualityComparerSch : IEqualityComparer<Schedule>
    {
        public bool Equals(Schedule x, Schedule y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Schedule obj)
        {
            return obj.GetHashCode();
        }
    }
}
