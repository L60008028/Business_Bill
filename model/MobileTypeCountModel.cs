using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{

    /// <summary>
    /// 运营商数量
    /// </summary>
    public class MobileTypeCountModel
    {
        /// <summary>
        /// 折算前总数
        /// </summary>
        public int InitialNum { get; set; }

        /// <summary>
        /// 套餐类别
        /// </summary>
        public string TaoCanType { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string Pirce { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public int TotalCount { get;set;}
        /// <summary>
        /// 移动数量
        /// </summary>
       public int CTcount { get; set; }

        /// <summary>
        /// 联通数量
        /// </summary>
       public int CUcount { get; set; }

        /// <summary>
        /// 电信数量
        /// </summary>
       public int CMcount { get; set; }
    }
}
