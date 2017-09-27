using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using MySql.Data.MySqlClient;
using System.Data;

namespace dal
{
    public class TaoCanInfoDAL
    {
        LocalParams lp;
        public TaoCanInfoDAL()
        {
            lp = new LocalParams();
            MySqlHelper.ConnectionstringLocalTransaction = lp.SqlConnStr;
        }

        public static string TABLE_NAME = "t_taocaninfo";
        private TaoCanInfoModel FillData(MySqlDataReader sdr)
        {
            TaoCanInfoModel model = null;
            try
            {
                model = new TaoCanInfoModel();
                model.Id = sdr.GetInt32(0);
                model.TCName = sdr.GetString(1);
                model.TCPrice = sdr.GetString(2);
                model.TCNumber = sdr.GetInt32(3);
                model.TCOutsidePrice = sdr.GetString(4);
                model.Billprice = sdr.GetString(5);
                model.Billnumber = sdr.GetInt32(6);

            }
            catch (Exception ex)
            {
                MyDelegateFunc.WriteFmLog("TaoCanInfoDAL=>FillData Exception:"+ex.ToString());
            }

            return model;
        }

        public Dictionary<int, TaoCanInfoModel> GetAll()
        {
            Dictionary<int, TaoCanInfoModel> dic = new Dictionary<int, TaoCanInfoModel>();
            string sql = "select * from t_taocaninfo";
            using (MySqlDataReader sdr = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionstringLocalTransaction, CommandType.Text, sql, null))
            {
                while (sdr.Read())
                {
                    TaoCanInfoModel m = this.FillData(sdr);
                    dic.Add(m.Id, m);
                }
            }
            return dic;
        }

    }//end
}//end
