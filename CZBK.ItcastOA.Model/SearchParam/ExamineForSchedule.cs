using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.SearchParam
{
    public class ExamineForSchedule : Schedule
    {
        public string ExamineText { get; set; }
        public string ExamineUser { get; set; }
    }
}
