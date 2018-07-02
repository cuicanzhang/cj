using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cj

{
    class config
    {
        public static string DatabaseFile = "system.dll";
        public static int dxd;
        public static int dxx;
        public static int dsd;
        public static int dss;
        public static int colour_blue;
        public static int colour_red;
        public static int colour_black;
        public static string DataSource
        {
            get
            {
                return string.Format("data source={0}", DatabaseFile);
            }
        }
    }
}
