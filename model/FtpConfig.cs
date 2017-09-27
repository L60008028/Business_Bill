using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace model
{
    /// <summary>
    /// ftp参数
    /// </summary>
    public class FtpConfig : ApplicationSettingsBase
    {
        [UserScopedSetting]
        [DefaultSettingValue("14.146.230.23")]
        public string IP
        {
            get { return this["IP"].ToString(); }
            set { this["IP"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("21")]
        public int Port
        {
            get { return int.Parse(this["Port"].ToString()); }
            set { this["Port"] = value; }
        }


        [UserScopedSetting]
        [DefaultSettingValue("szjyap")]
        public string LoginName
        {
            get { return this["LoginName"].ToString(); }
            set { this["LoginName"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("jyapsz")]
        public string Password
        {
            get { return this["Password"].ToString(); }
            set { this["Password"] = value; }
        }

    }
}
