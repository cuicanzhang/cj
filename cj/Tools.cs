using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cj
{
    class Tools
    {
        public static string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }
    }
}
