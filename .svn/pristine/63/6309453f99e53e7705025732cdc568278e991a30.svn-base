using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace dal
{
    public class EprInfoDAL
    {
        LocalParams lp;
        public EprInfoDAL()
        {
            lp = new LocalParams();
            MySqlHelper.ConnectionstringLocalTransaction = lp.SqlConnStr;
        }
        private EprInfoModel FillData(SqlDataReader sdr)
        {
            try
            {
                EprInfoModel m = new EprInfoModel();
                m.id = sdr.GetInt32(0);
                m.bnetId = sdr.GetString(1);
                m.bnetAccount = sdr.GetString(2);
                m.areaId = sdr.GetInt32(3);
                m.taocanId = sdr.GetInt32(4);
                return m;
            }
            catch (Exception)
            {
                
            }
            return null;
        }


        /// <summary>
        /// 初始套餐
        /// </summary>
        /// <returns></returns>
        public bool InitTaocanId()
        {
           
            try
            {
                string sql = "update JY15.gdkltx.dbo.t_eprinfo set TaocanId=ModifyTaocanId";
                int re= MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionstringLocalTransaction, CommandType.Text, sql, null);
                MyDelegateFunc.WriteFmLog("EprInfoDAL=>InitTaocanId re:" + re);
                return true;
            }catch(Exception ex)
            {
                MyDelegateFunc.WriteFmLog("EprInfoDAL=>InitTaocanId Exception:" + ex.ToString());
            }
            return false;
        }
        public EprInfoModel GetEprInfo(int Id)
        {
            string sql = string.Format("select Id,BnetId,BnetAccount,AreaId,TaocanId from JY15.gdkltx.dbo.t_eprinfo where Id={0}", Id);
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(lp.SqlConnStr, CommandType.Text, sql, null))
            {
                if (sdr.Read())
                {
                    EprInfoModel m = this.FillData(sdr);
                    return m;
                }
            }
            return null;
        }

        public Dictionary<int, EprInfoModel> GetEprInfoAll()
        {
            Dictionary<int, EprInfoModel> dic = new Dictionary<int, EprInfoModel>();
            string sql = "select Id,BnetId,BnetAccount,AreaId,TaocanId from jy15.gdkltx.dbo.t_eprinfo";
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(lp.SqlConnStr, CommandType.Text, sql, null))
            {
                while (sdr.Read())
                {
                    EprInfoModel m = this.FillData(sdr);
                    dic.Add(m.id, m);
                }
            }
            return dic;
        }

        /// <summary>
        /// 套餐企业信息
        /// </summary>
        /// <returns></returns>
        public List<EprInfoModel> GetTaoCanEprInfoAll()
        {
            List<EprInfoModel> dic = new List<EprInfoModel>();
            string sql = "select Id,BnetId,BnetAccount,AreaId,TaocanId from t_eprinfo where isEnable=0 and TaocanId>0";
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(lp.SqlConnStr, CommandType.Text, sql, null))
            {
                while (sdr.Read())
                {
                    EprInfoModel m = this.FillData(sdr);
                    dic.Add(m);
                }
            }
            return dic;
        }

        /// <summary>
        /// 套餐企业信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, EprInfoModel> GetTaoCanEprInfoAll2()
        {
            Dictionary<int, EprInfoModel> dic = new Dictionary<int, EprInfoModel>();
            string sql = "select Id,BnetId,BnetAccount,AreaId,TaocanId from t_eprinfo where isEnable=0 and TaocanId>0";
            using (SqlDataReader sdr = SqlHelper.ExecuteReader(lp.SqlConnStr, CommandType.Text, sql, null))
            {
                while (sdr.Read())
                {
                    EprInfoModel m = this.FillData(sdr);
                    dic.Add(m.id, m);
                }
            }
            return dic;
        }

    }//end
}//end
