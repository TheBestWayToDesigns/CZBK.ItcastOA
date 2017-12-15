using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CZBK.ItcastOA.TModel.ClassDal
{
    public class Inventory
    {
        public int ID { get; set; }
        public string code { get; set; }
        public string Name { get; set; }
        public string XingH { get; set; }
        public string shorthand { get; set; }
    }
    public class MoedName
    {
        public int ID { get; set; }
        public string MyText { get; set; }
        public string MyColums { get; set; }
        public string code { get; set; }
        public int del { get; set; }
    }
}