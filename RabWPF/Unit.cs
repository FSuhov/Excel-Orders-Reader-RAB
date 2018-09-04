using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rab_Forms
{
    public class Unit
    {
        public string Size { get; set; }
        public int Quantity { get; set; }
        public Unit(string s)
        {
            Size = s;
            Quantity = 0;
        }
    }
}