﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// bnet帐号实体
    /// </summary>
    public class BnetInfoModel
    {
        public int Id { get; set; }
        public string BnetId { get; set; }
        public string BnetAccount { get; set; }
        public string CustomerName { get; set; }
        public string OperType { get; set; }
        public string ProductId { get; set; }
        public int IsUsed { get; set; }
        public int IsReply { get; set; }
        public string Sign { get; set; }
        public string Price { get; set; }
        public string OperId { get; set; }
        public string AppSystemId { get; set; }
        public DateTime ReplyTime { get; set; }
        public DateTime AddTime { get; set; }
        public int AttributeId { get; set; }
        public string TaoCanType { get; set; }
    }//end
}//end
