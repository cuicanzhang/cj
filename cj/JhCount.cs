using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cj
{
    class JhCount
    {        
        /// <summary>
        /// ItemInfo 类记录数组元素重复次数
        /// </summary>
        /// <param name="value">数组元素值</param>
        public JhCount(string value)
        {
            Value = value;
            RepeatNum = 1;
        }
        /// <summary>
        /// 数组元素的值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 数组元素重复的次数
        /// </summary>
        public int RepeatNum { get; set; }
    }
}
