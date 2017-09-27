using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class SmsMobileModel
    {
        public int id{get;set;}// 主键

        public int eprId{get;set;}// 企业ID

        public string userId{get;set;}// 用户ID

        public string batchNum{get;set;}// 批次号(唯一)

        public string clientMsgId{get;set;}// 客户端msgId

        public string mobile{get;set;}// 发送的手机号码

        public string name{get;set;}// 发送人名称

        public string msgId{get;set;}// 短信编号

        public DateTime saveTime{get;set;}// 提交时间

        public DateTime sendTime{get;set;}// 发送时间

        public int gatewayNum{get;set;}// 网关编号

        public int submitStatus{get;set;}// 发送状态(未提交0，已提交1，失败-1)

        public int reportStatus{get;set;}// 回执状态

        public int smsCount{get;set;}// 短信数量（内容长度计算）

        public int sendCount{get;set;}// 发送次数（默认0）

    }//end
}//end
