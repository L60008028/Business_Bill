using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 套餐信息
    /// </summary>
    public class TaoCanInfoModel
    {
        /// <summary>
        /// 标识列
        /// </summary>
        public int Id { get; set; }//标识列

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string TCName { get; set; }//套餐名称

        /// <summary>
        /// 套餐单价
        /// </summary>
        public string TCPrice { get; set; }//套餐单价

        /// <summary>
        /// 套餐外单价
        /// </summary>
        public string TCOutsidePrice { get; set; }//套餐外单价

        /// <summary>
        /// 套餐数量
        /// </summary>
        public int TCNumber { get; set; }//套餐数量


        /// <summary>
        /// 帐单单价
        /// </summary>
        public string Billprice { get; set; }//帐单单价

        /// <summary>
        /// 帐单数量
        /// </summary>
        public int Billnumber { get; set; }//帐单数量

    }//end
}//end
