using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dal;
using model;

namespace bll
{

    public class ChangeReportStatus
    {
        ChangeModl _cm;
        int totalCount = 0;
        int alreadyChangeCount = 0;
        int times = 1;
        public ChangeReportStatus(ChangeModl cm)
        {
            _cm = cm;
        }
        public void DoWork()
        {
            _cm.WriteStatus("开始...");
            do
            {
                MyWork();
            } while (alreadyChangeCount < totalCount);

            _cm.WriteStatus("完成...");
        }
        private void MyWork()
        {
            //37805-30244
            SmsMobileDAL smdal = new SmsMobileDAL();
            int cn = smdal.GetUnknowCount(_cm.EprId, _cm.UserId, _cm.Year, _cm.Month, _cm.Day);
            int thisCount = (int)Math.Ceiling(cn * _cm.Rate);
            if (times == 1)
            {
                totalCount = thisCount;
                times = 2;
            }
            _cm.WriteStatus("未知数量共" + cn + "条,处理数量：" + totalCount);
            _cm.WriteFmLog("未知数量共" + cn + "条,处理数量：" + totalCount);
            if (cn > 0 && alreadyChangeCount < totalCount)
            {
                List<SmsMobileModel> list = smdal.GetUnknowList(_cm.EprId, _cm.UserId, _cm.Year, _cm.Month, _cm.Day);
                if (list != null && list.Count > 0)
                {
                    List<string> sqlList = new List<string>();//sql语句
                    int min = list[0].id;
                    int max = list[list.Count - 1].id;
                    Random rnd = new Random();
                    for (int i = 0; i < list.Count; i++)
                    {
                        try
                        {
                           
                            int ii=rnd.Next(0,list.Count);
                           // if (i % 2 == 0)
                          //  {
                                SmsMobileModel sm = list[ii];
                                string sql = string.Format("update t_smsmobile set ReportStatus=2 where Id={0} and EprId='{1}'", sm.id, _cm.EprId);
                                sqlList.Add(sql);
                                alreadyChangeCount++;
                                if (alreadyChangeCount > totalCount)
                                {
                                    break;
                                }

                           // }
                        }
                        catch (Exception)
                        {

                        }

                    }//for


                    if (sqlList.Count > 0)
                    {
                        List<string> tmpList = new List<string>();
                        for (int j = 0; j < sqlList.Count;j++ )
                        {
                            tmpList.Add(sqlList[j]);
                            if(tmpList.Count==500 || j==sqlList.Count-1)
                            {
                                bool b=smdal.UpdateUnknowReportStatus(tmpList);
                                _cm.WriteFmLog("已执行:" + tmpList.Count + "条,结果："+b);
                                tmpList = new List<string>();
                             
                            }
                        }
                    }

                }//if
            }
        }
    }


    public class ChangeModl
    {
        public model.MyDelegateDefine.DelegateWriteChangeFmStatus WriteStatus;
        public model.MyDelegateDefine.DelegateWriteFmLog WriteFmLog;
        public string EprId { get; set; }
        public string UserId { get; set; }
        public double Rate { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
    }
}
