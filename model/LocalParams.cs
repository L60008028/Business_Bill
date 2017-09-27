using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace model
{
  /// <summary>
    /// 公共参数
    /// </summary>
    public class LocalParams : ApplicationSettingsBase
    {

        /// <summary>
        /// 短信id
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string UID
        {
            get { return this["UID"].ToString(); }
            set { this["UID"] = value; }
        }



        /// <summary>
        /// 短信密码
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string PWD
        {
            get { return this["PWD"].ToString(); }
            set { this["PWD"] = value; }
        }

 

        /// <summary>
        /// 企业ID出账特殊处理，算到同一个bnetaccount下
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string EprIdBnetAccount
        {
            get { return this["EprIdBnetAccount"].ToString(); }
            set { this["EprIdBnetAccount"] = value; }
        }


        [UserScopedSetting]
        [DefaultSettingValue("server=192.168.10.174;user id=sms;pwd=jy1314;persistsecurityinfo=True;database=gdkltx;")]
        public string SqlConnStr
        {
            get { return this["SqlConnStr"].ToString(); }
            set { this["SqlConnStr"] = value; }
        }

        /// <summary>
        /// 每天23点生成账单
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("0 59 23 * * ?")]
        public string CronExpression1
        {
            get { return this["CronExpression1"].ToString(); }
            set { this["CronExpression1"] = value; }
        }

        /// <summary>
        /// 空账单
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("0 0 * * * ?")]
        public string CronExpression2
        {
            get { return this["CronExpression2"].ToString(); }
            set { this["CronExpression2"] = value; }
        }

        /// <summary>
        /// 每月1号1点生成套餐账单
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("0 0 1 1 * ?")]
        public string CronExpressionTaocan
        {
            get { return this["CronExpressionTaocan"].ToString(); }
            set { this["CronExpressionTaocan"] = value; }
        }

        /// <summary>
        /// 每月最后一天23点55分，生成套餐结果算帐单
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("0 55 23 L * ?")]
        public string CronExpressionTaocanClose
        {
            get { return this["CronExpressionTaocanClose"].ToString(); }
            set { this["CronExpressionTaocanClose"] = value; }
        }

    }



}
