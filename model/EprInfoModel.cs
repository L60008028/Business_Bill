using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace model
{
   public class EprInfoModel
    {
        public int id{get;set;}// 企业ID,标识列(从1000开始)

        public string company{get;set;}// 公司名

        public string address{get;set;}// 地址

        public int eprType{get;set;}// 企业类别,0普通1vip

        public string homePage{get;set;}// 公司主页

        public string smsSign{get;set;}// 短信签名

        public int canSendCount{get;set;}// 可发送短信条数（每月）

        public int alreadyCount{get;set;}// 已发送短信条数（每月）

        public int allSendCount{get;set;}// 累计已发送条数

        public string contact{get;set;}// 联系人

        public string mobile{get;set;}// 手机

        public string phone{get;set;}// 座机

        public string email{get;set;}// 邮箱

        public string fax{get;set;}// 传真

        public string saleName{get;set;}// 客户经理姓名

        public int isEnable{get;set;}// 企业账号是否可用

        public int cMGateway{get;set;}// 移动短信网关号

        public int cTGateway{get;set;}// 电信短信网关号

        public int cUGateway{get;set;}// 联通电信网关号

        public string cMExtNum{get;set;}// 移动短信网关扩展号

        public string cTExtNum{get;set;}// 电信短信网关扩展号

        public string cUExtNum{get;set;}// 联通电信网关扩展号

        public DateTime registerDate{get;set;}// 注册日期

        public string registerIP{get;set;}// 注册IP

        public DateTime updateTime{get;set;}// 更新时间

        public string remark{get;set;}// 备注

        public string bindIP{get;set;}// 接口用户使用（多个逗号隔开）

        public int whiteCount{get;set;}// 白名单数量（默认3W）

        public int priority{get;set;}// 优先级

        public int tradeType{get;set;}// 行业类型

        //新增领航帐号2014-09-16
        public string bnetId{get;set;}

        public string bnetAccount{get;set;}
        //新增区域ID
        public int areaId{get;set;}
        //套餐修改的id
        public int taocanId { get; set; }
        //套餐修改的id
        public int modifyTaocanId{get;set;}

    }//end
}//end
