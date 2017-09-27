using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using dal;
using model;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace bll
{
    /// <summary>
    /// 套餐账单，每月一号1点，先按套餐数量出账，放在1点的文件中
    /// </summary>
    public class CreateTaoCanBillJob : IJob
    {
        private string ProductId = "100007";// 产品ID
        private string FactoryNo = "11075";// 厂家编号
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CreateBillJob));

        public void Execute(IJobExecutionContext context)
        {
            //先更新帐单
            EprInfoDAL eprinfoDAL = new EprInfoDAL();
            bool b = eprinfoDAL.InitTaocanId();
            MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob=>初始化套餐reslt=" + b);

            ProductId = GlobalParams.ProductId;
            FactoryNo = GlobalParams.FactoryNo;
            DateTime dt = DateTime.Now;
            Work(dt, true);
        }

        public void Work(DateTime dtnow, bool isupload)
        {
            DateTime dt = DateTime.Now;
            int yy = dtnow.Year;// 得到年
            int mm = dtnow.Month;// 得到月
            int dd = dtnow.Day;// 日
            int hh = dtnow.Hour;// 小时

            string strmm = mm > 9 ? mm + "" : "0" + mm;
            string strdd = dd > 9 ? dd + "" : "0" + dd;
            string strhh = hh > 9 ? hh + "" : "0" + hh;
            strhh = "01";
            string ftpdir = yy + strmm + strdd;// ftp上的每天一个文件夹
            string dir = Application.StartupPath + "\\" + FactoryNo;
            DirectoryInfo dim = new DirectoryInfo(dir);
            if (!dim.Exists)
            {
                dim.Create();
            }
            dir += "\\" + yy + strmm + strdd;// 文件夹(厂家编号/yyyyMMdd)
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
            {
                di.Create();
            }
            //深圳
            string fileName = ProductId + "_" + yy + strmm + strdd + strhh + "0001.txt.sending";// 文件名
            string filePath = dir + "\\" + fileName;
            string szftpfilename = ProductId + "_" + yy + strmm + strdd + strhh + "0001.txt";// ftp保存文件名

        
            EprInfoDAL eprinfoDAL = new EprInfoDAL();
            List<EprInfoModel> eprLst = eprinfoDAL.GetTaoCanEprInfoAll();
            List<string> szbillList = new List<string>();// 深圳帐单
            List<string> daystatlist = new List<string>();//帐号报表
            if (eprLst != null && eprLst.Count > 0)
            {
                TaoCanInfoDAL taocanDAL = new TaoCanInfoDAL();
                Dictionary<int, TaoCanInfoModel> taocandic = taocanDAL.GetAll();
                foreach (EprInfoModel e in eprLst)
                {
                    if (taocandic.ContainsKey(e.taocanId))
                    {
                        //他网和电信比率 8：2
                        TaoCanInfoModel tc = taocandic[e.taocanId];
                        int ctnumber = (int)(tc.Billnumber * 0.8);//移动账单数量
                        int cmnumber = tc.Billnumber - ctnumber;//电信账单数量

                        string bnetId = e.bnetId, bnetAccount = e.bnetAccount;
                        try
                        {
                            //生成sql
                            object[] obj = { e.id, bnetId, bnetAccount, ctnumber, 0, cmnumber, yy, mm, dd };
                            string sql = string.Format("Insert into t_billdaystat(EprId,BnetId,BnetAccount,CTcount,CUcount,CMcount,YY,MM,DD) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8})", obj);
                            daystatlist.Add(sql);
                        }
                        catch (Exception ex)
                        {
                            MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>生成sqlList失败：" + ex.Message);
                        }


                        for (int i = 0; i < ctnumber; i++)
                        {
                            StringBuilder sbstr = new StringBuilder();
                            sbstr.Append(e.bnetId);
                            sbstr.Append("|");
                            sbstr.Append(e.bnetAccount);
                            sbstr.Append("|");
                            sbstr.Append("0|1|0|0|");
                            sbstr.Append(dtnow.ToString("yyyy/MM/dd H:mm:ss", DateTimeFormatInfo.InvariantInfo));
                            szbillList.Add(sbstr.ToString());
                        }
                        for (int j = 0; j < cmnumber; j++)
                        {
                            StringBuilder sbstr = new StringBuilder();
                            sbstr.Append(e.bnetId);
                            sbstr.Append("|");
                            sbstr.Append(e.bnetAccount);
                            sbstr.Append("|");
                            sbstr.Append("0|0|1|0|");
                            sbstr.Append(dt.ToString("yyyy/MM/dd H:mm:ss", DateTimeFormatInfo.InvariantInfo));
                        }
                    }
                }
            }


            try
            {
                if (daystatlist != null && daystatlist.Count > 0)
                {
                    int result = SQLbll.BatchExec(daystatlist);
                    MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>保存sql fail：" + result);
                }
            }
            catch (Exception ex)
            {
                MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>保存sql失败：" + daystatlist[0]);
                MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>保存sql失败：" + ex.Message);
            }

            if (isupload)
            {
              
                FtpClient ftp = new FtpClient();
                //深圳账单
                if (szbillList != null)
                {
                    bool bsz = MyFileOptions.WriteFileTXT(szbillList, filePath);
                    MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>生成sz套餐文件[" + szftpfilename + "]结果：" + bsz);
                    if (bsz)
                    {

                        bool boolf = ftp.Upload(ftpdir, szftpfilename, filePath);
                        MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>上传sz套餐账单[" + szftpfilename + "]结果：" + boolf);
                        if (boolf)
                        {
                            try
                            {
                                File.Move(filePath, dir + "\\" + szftpfilename);
                            }
                            catch (Exception ex)
                            {
                                MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>修改sz套餐文件名[" + filePath + "]to[" + szftpfilename + "]失败:" + ex.Message);
                                logger.Error("CreateTaoCanBillJob==>修改sz套餐文件名[" + filePath + "]to[" + szftpfilename + "]失败:" + ex.Message);
                            }
                        }
                        else
                        {
                            MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob=FTP=>上传sz套餐文件[" + filePath + "]失败");
                            logger.Error("CreateTaoCanBillJob=FTP=>上传sz套餐文件[" + filePath + "]失败");
                        }

                    }
                    else
                    {
                        MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob==>生成sz文件[" + filePath + "]失败");
                        logger.Error("CreateTaoCanBillJob==>生成sz文件[" + filePath + "]失败");
                    }
                }
            }



        }
    }//end
}//end
