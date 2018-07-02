using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cj
{
    class Number
    {
        public Number(string a,string b,string c,string d,string e)
        {
            qh = a;
            jh = b;
            dx = c;
            ds = d;
            colour = e;
        }
        public string qh { get; set; }
        public string jh { get; set; }
        public string dx { get; set; }
        public string ds { get; set; }
        public string colour { get; set; }
    }
}
