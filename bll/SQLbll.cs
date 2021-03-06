﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dal;
using System.Data.SqlClient;
using model;

namespace bll
{

    public class SQLbll
    {

       
        /// <summary>
        /// 检测数据库连接串是否有效
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static bool IsConn(string constr)
        {
            try
            {
                string sql = "select now()";
                sql = "select getdate()";
               // object obj = MySqlHelper.ExecuteScalar(constr, System.Data.CommandType.Text, sql, null);
                object obj = SqlHelper.ExecuteScalar(constr, System.Data.CommandType.Text, sql, null);
                if (obj != null)
                {
                    return true;
                }
            }
            catch (SqlException ex)
            { 

            }
            return false;
        }

        public static BnetInfoModel GetBnetInfo(string id,string u)
        {
            BnetInfoDAL dfd = new BnetInfoDAL();
            return dfd.GetByIdAccount(id,u);
        }
        public static int BatchExec(List<string> list)
        {
            LocalParams lp = new LocalParams();
            List<string> relist = SqlHelper.BatchExec(lp.SqlConnStr, list);
           return relist.Count;
        }

        public static object ExecuteScalar(string sql)
        {
            LocalParams lp = new LocalParams();
            try
            {
               
                object obj = SqlHelper.ExecuteScalar(lp.SqlConnStr, System.Data.CommandType.Text, sql, null);
                return obj;
            }
            catch (Exception ex)
            {

                model.MyDelegateFunc.WriteFmLog("SQLbll=>ExecuteScalar:ExceptionSQL:" + sql);
                model.MyDelegateFunc.WriteFmLog("SQLbll=>ExecuteScalar:ExceptionSQL:" + lp.SqlConnStr);
                model.MyDelegateFunc.WriteFmLog("SQLbll=>ExecuteScalar:Exception:" + ex);
            }
            return null;
        }

    }//end
}
