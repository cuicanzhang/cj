using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public static bool isInputNumber(KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
               e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.OemPeriod)
            {
                //按下了Alt、ctrl、shift等修饰键  
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
                else
                {
                    return true;
                }

            }
            else//按下了字符等其它功能键  
            {
                e.Handled = true;
            }
            return false;
        }
        public static int checkColor(string a, string b, string c)
        {
            int i = 0;
            if (a == b && a == c && a == c) { i = 3; }
            else if ((a == b && b != c) || (a != b && b == c) || (a == c && b != c)) { i = 2; }
            else if (int.Parse(b) - int.Parse(a) == 1 && int.Parse(c) - int.Parse(b) == 1) { i = 1; }
            return i;
        }
    }
}
