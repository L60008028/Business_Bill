using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.IO;
using System.Windows.Forms;
using dal;
using model;
using System.Globalization;

namespace bll
{
    public class CreateBillJob : IJob
    {


        private string ProductId = "100007";// 产品ID
        private string FactoryNo = "11075";// 厂家编号
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CreateBillJob));

        public void Execute(IJobExecutionContext context)
        {
            ProductId = GlobalParams.ProductId;
            FactoryNo = GlobalParams.FactoryNo;
            DateTime dt = DateTime.Now.AddHours(-1);//统计前一小时的；每天0点10执行，
            Work(dt, true);  
        }

        public void Work(DateTime dtnow, bool isupload)
        {

            DateTime dt = DateTime.Now;
            int yy = dtnow.Year;// 得到年
            int mm = dtnow.Month;// 得到月
            int dd = dtnow.Day;// 日
            int hh = dtnow.Hour;// 小时
            try
            {
                string strmm = mm > 9 ? mm + "" : "0" + mm;
                string strdd = dd > 9 ? dd + "" : "0" + dd;
                string strhh = hh > 9 ? hh + "" : "0" + hh;
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
                //深圳23点
                string fileName = ProductId + "_" + yy + strmm + strdd + strhh + "0001.txt.sending";// 文件名
                string filePath = dir + "\\" + fileName;
                string szftpfilename = ProductId + "_" + yy + strmm + strdd + strhh + "0001.txt";// ftp保存文件名
                try
                {
                    if (File.Exists(szftpfilename))
                    {
                        File.Delete(szftpfilename);
                    }
                }
                catch { }

                //东莞22点
                string strdghh = "22";// hh - 1 > 9 ? hh - 1 + "" : "0" + (hh - 1);//东莞
                string dgfileName = ProductId + "_" + yy + strmm + strdd + strdghh + "0001.txt.sending";// 文件名
                string dgfilePath = dir + "\\" + dgfileName;
                string dgftpfilename = ProductId + "_" + yy + strmm + strdd + strdghh + "0001.txt";// ftp保存的文件名

                try
                {
                    if (File.Exists(dgftpfilename))
                    {
                        File.Delete(dgftpfilename);
                    }
                }
                catch { }

                //中山21点
                string strzshh = "21";// hh - 2 > 9 ? hh - 2 + "" : "0" + (hh - 2);//中山
                string zsfileName = ProductId + "_" + yy + strmm + strdd + strzshh + "0001.txt.sending";// 文件名
                string zsfilePath = dir + "\\" + zsfileName;
                string zsftpfilename = ProductId + "_" + yy + strmm + strdd + strzshh + "0001.txt";// ftp保存的文件名
                try
                {
                    if (File.Exists(zsftpfilename))
                    {
                        File.Delete(zsftpfilename);
                    }
                }
                catch { }

                //潮洲19点
                string strczhh = "19";// hh - 4 > 9 ? hh - 4 + "" : "0" + (hh - 4);//潮洲
                string czfileName = ProductId + "_" + yy + strmm + strdd + strczhh + "0001.txt.sending";// 文件名
                string czfilePath = dir + "\\" + zsfileName;
                string czftpfilename = ProductId + "_" + yy + strmm + strdd + strczhh + "0001.txt";// ftp保存的文件名
                try
                {
                    if (File.Exists(czftpfilename))
                    {
                        File.Delete(czftpfilename);
                    }
                }
                catch { }

                EprInfoDAL eprDAO = new EprInfoDAL();
                SmsMobileDAL mobileDAO = new SmsMobileDAL();
                BnetInfoDAL bnetDAO = new BnetInfoDAL();

                //查出所有企业信息和bnet信息
                Dictionary<int, EprInfoModel> dicEprInfo = eprDAO.GetEprInfoAll();
                Dictionary<string, BnetInfoModel> dicBnetInfo = bnetDAO.GetAll();

                //帐单
                List<string> szbillList = new List<string>();// 深圳帐单
                List<string> dgbillList = new List<string>();//东莞帐单
                List<string> zsbillList = new List<string>();//中山帐单
                List<string> czbillList = new List<string>();//潮洲帐单
                List<string> daystatlist = new List<string>();//帐号报表
                Dictionary<int, MobileTypeCountModel> eprSendCount = mobileDAO.GetSumSMScountByDay(dicEprInfo.Keys.ToList(), yy, mm, dd);//查出企业发送量
                Dictionary<int, MobileTypeCountModel> billCount = new Dictionary<int, MobileTypeCountModel>();//打折后企业的发送量
                foreach (int key in eprSendCount.Keys)
                {
                    try
                    {
                        MobileTypeCountModel mtcm = null;
                        EprInfoModel eprinfo = null;
                        try
                        {
                            mtcm = eprSendCount[key];
                            eprinfo = dicEprInfo[key];
                            if (eprinfo != null && eprinfo.taocanId > 0)
                            {
                                continue;//套餐的不处理
                            }
                        }
                        catch (Exception ex)
                        {
                            MyDelegateFunc.WriteFmLog("打折异常[eprinfo]：" + ex.ToString());
                        }

                        if (eprinfo != null && (mtcm.CTcount > 0 || mtcm.CMcount > 0))
                        {
                            if (string.IsNullOrEmpty(eprinfo.bnetId) || eprinfo.bnetId.Equals("888"))
                            {
                                continue;
                            }

                            double dprice = 10.00;
                            int billct = mtcm.CTcount;
                            int billcm = mtcm.CMcount;
                            BnetInfoModel bnetf = null;
                            try
                            {
                                bnetf = dicBnetInfo[eprinfo.bnetId];
                            }
                            catch (Exception ex)
                            {
                                MyDelegateFunc.WriteFmLog("打折异常[bnetf]：" + ex.ToString());
                            }

                            if (bnetf != null)
                            {

                                //中山
                                if (bnetf.BnetAccount.StartsWith("760"))//eprinfo.areaId == 17
                                {
                                    billCount.Add(key, mtcm);
                                    continue;
                                }

                                string tprice = bnetf.Price;
                                if (!string.IsNullOrEmpty(tprice))
                                {
                                    bool b = double.TryParse(tprice, out dprice);
                                    if (dprice <= 0.00 || !b)
                                    {
                                        dprice = 10.00;
                                    }
                                }
                                MobileTypeCountModel mtcm_bill = new MobileTypeCountModel();
                               
                                //double bcount_ct = billct * (dprice / 10);
                                mtcm_bill.CTcount = (int)(billct * (dprice / 10));// (int)Math.Ceiling(billct * (dprice / 10));

                                //  double bcount_cm = billcm * (dprice / 10);
                                mtcm_bill.CMcount = (int)(billcm * (dprice / 10));// (int)Math.Ceiling(billcm * (dprice / 10));
                                mtcm_bill.Pirce=dprice+"";
                                mtcm_bill.InitialNum = billct + billcm;
                                string inf = string.Format("INFO CreateBillJob==>企业id:{0},清单数量：{1},帐单数量：{2},单价：{3}", key, billcm + billct, mtcm_bill.CMcount + mtcm_bill.CTcount, tprice);
                                logger.Info(inf);
                                MyDelegateFunc.WriteFmLog(inf);
                                billCount.Add(key, mtcm_bill);

                            }//BnetInfo 
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("CreateBillJob==>计算账单条数异常：" + ex.Message);
                    }

                }//foreach eprCount


                //移动联通,电信
                foreach (int key in billCount.Keys)
                {
                    EprInfoModel eprinfo = eprDAO.GetEprInfo(key);
                    string bnetId = eprinfo.bnetId;
                    string bnetAccount = eprinfo.bnetAccount;
                    MobileTypeCountModel mtcm = billCount[key];//帐单数量

                    try
                    {
                        //生成sql
                        object[] obj = { eprinfo.id, bnetId, bnetAccount, mtcm.CTcount, mtcm.CUcount, mtcm.CMcount, yy, mm, dd, eprinfo.areaId, mtcm.Pirce,mtcm.InitialNum,mtcm.TotalCount};
                        string sql = string.Format("Insert into JY15.gdkltx.dbo.t_billdaystat(EprId,BnetId,BnetAccount,CTcount,CUcount,CMcount,YY,MM,DD,AreaId,Price,InitialNum,TaoCanType) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},'{10}',{11},'{12}')", obj);
                        logger.Info(sql);
                        daystatlist.Add(sql);
                    }
                    catch (Exception ex)
                    {
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>保存sqlList失败：" + ex.ToString());
                    }
                    for (int i = 0; i < mtcm.CTcount; i++)
                    {
                        StringBuilder sbstr = new StringBuilder();
                        sbstr.Append(bnetId);
                        sbstr.Append("|");
                        sbstr.Append(bnetAccount);
                        sbstr.Append("|");
                        sbstr.Append("0|1|0|0|");
                        sbstr.Append(dtnow.ToString("yyyy/MM/dd H:mm:ss", DateTimeFormatInfo.InvariantInfo));
                        szbillList.Add(sbstr.ToString());
                        /*
                        if (bnetAccount.StartsWith("755"))//深圳
                        {
                            szbillList.Add(sbstr.ToString());
                        }
                        else if (bnetAccount.StartsWith("769"))//东莞
                        {
                            dgbillList.Add(sbstr.ToString());
                        }
                        else if (bnetAccount.StartsWith("760"))//中山
                        {
                            zsbillList.Add(sbstr.ToString());
                        }
                        else if (bnetAccount.StartsWith("0768"))//潮洲
                        {
                            czbillList.Add(sbstr.ToString());
                        }
                         * */

                    }

                    //电信
                    for (int j = 0; j < mtcm.CMcount; j++)
                    {
                        StringBuilder sbstr = new StringBuilder();
                        sbstr.Append(bnetId);
                        sbstr.Append("|");
                        sbstr.Append(bnetAccount);
                        sbstr.Append("|");
                        sbstr.Append("0|0|1|0|");
                        sbstr.Append(dtnow.ToString("yyyy/MM/dd H:mm:ss", DateTimeFormatInfo.InvariantInfo));
                        szbillList.Add(sbstr.ToString());
                        /*
                        if (bnetAccount.StartsWith("755"))//深圳
                        {
                            szbillList.Add(sbstr.ToString());
                        }
                        else if (bnetAccount.StartsWith("769"))//东莞
                        {
                            dgbillList.Add(sbstr.ToString());
                        }
                        else if (bnetAccount.StartsWith("760"))//中山
                        {
                            zsbillList.Add(sbstr.ToString());
                        }
                        else if (bnetAccount.StartsWith("0768") || bnetAccount.StartsWith("768"))//潮洲
                        {
                            czbillList.Add(sbstr.ToString());
                        }
                         * */

                    }

                }//foreach billCount

                try
                {
                    if (daystatlist != null && daystatlist.Count > 0)
                    {
                        int result = SQLbll.BatchExec(daystatlist);
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>保存sql fail：" + result);
                    }
                }
                catch (Exception ex)
                {
                    MyDelegateFunc.WriteFmLog("CreateBillJob==>保存sql失败：" + daystatlist[0]);
                    MyDelegateFunc.WriteFmLog("CreateBillJob==>保存sql失败：" + ex.Message);
                }


                //生成文件,上传

                FtpClient ftp = new FtpClient();
                LocalParams lp = new LocalParams();
                HttpHelper http = new HttpHelper();
                //深圳账单
                if (szbillList != null)
                {
                    bool bsz = MyFileOptions.WriteFileTXT(szbillList, filePath);
                    MyDelegateFunc.WriteFmLog("CreateBillJob==>生成sz文件[" + szftpfilename + "]结果：" + bsz);
                    if (bsz && isupload)
                    {
                        bool boolf = false;
                        int i = 0;
                        do
                        {
                            boolf = ftp.Upload(ftpdir, szftpfilename, filePath);
                            i++;
                        } while (!boolf && i < 3);
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>上传sz账单[" + szftpfilename + "]结果：" + boolf);
                        if (boolf)
                        {
                            try
                            {
                                File.Move(filePath, dir + "\\" + szftpfilename);
                            }
                            catch (Exception ex)
                            {
                                MyDelegateFunc.WriteFmLog("CreateBillJob==>修改sz文件名[" + filePath + "]to[" + szftpfilename + "]失败:" + ex.Message);
                                logger.Error("CreateBillJob==>修改sz文件名[" + filePath + "]to[" + szftpfilename + "]失败:" + ex.Message);
                            }
                        }
                        else
                        {

                            string sendresult = http.Send(lp.UID, lp.PWD, "18002579031", 111, "上传sz文件[" + filePath + "]失败【嘉盈资讯】");
                            MyDelegateFunc.WriteFmLog("CreateBillJob==>上传sz文件[" + filePath + "]失败,发送短信结果：" + sendresult);
                            logger.Error("CreateBillJob==>上传sz文件[" + filePath + "]失败");

                        }

                    }
                    else
                    {

                        string sendresult = http.Send(lp.UID, lp.PWD, "18002579031", 111, "生成sz文件[" + filePath + "]失败【嘉盈资讯】");
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>生成sz文件[" + filePath + "]失败,发送短信结果：" + sendresult);
                        logger.Error("CreateBillJob==>生成sz文件[" + filePath + "]失败");
                    }
                }
                else
                {

                    string sendresult = http.Send(lp.UID, lp.PWD, "18002579031", 111, "szbillList null【嘉盈资讯】");
                    MyDelegateFunc.WriteFmLog("CreateBillJob=FTP=>文件szbillList null,发送短信结果：" + sendresult);
                }

                //东莞账单
                if (dgbillList != null)
                {
                    bool bdg = MyFileOptions.WriteFileTXT(dgbillList, dgfilePath);
                    if (bdg && isupload)
                    {
                        bool booldg = ftp.Upload(ftpdir, dgftpfilename, dgfilePath);
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>上传东莞账单[" + dgfilePath + "]结果：" + booldg);
                        if (booldg)
                        {
                            try
                            {
                                File.Move(dgfilePath, dir + "\\" + dgftpfilename);
                            }
                            catch (Exception ex)
                            {

                                logger.Error("CreateBillJob==>修改东莞文件名[" + dgfilePath + "]to[" + dgftpfilename + "]失败:" + ex.Message);
                            }
                        }
                    }
                    else
                    {


                        logger.Error("CreateBillJob==>生成dg文件[" + dgfilePath + "]失败");
                    }

                }



                //中山账单
                if (zsbillList != null)
                {
                    bool zsb = MyFileOptions.WriteFileTXT(zsbillList, zsfilePath);
                    if (zsb && isupload)
                    {
                        bool booldg = ftp.Upload(ftpdir, zsftpfilename, zsfilePath);
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>上传中山账单[" + zsfilePath + "]结果：" + booldg);
                        if (booldg)
                        {
                            try
                            {
                                File.Move(zsfilePath, dir + "\\" + zsftpfilename);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("CreateBillJob==>修改中山文件名[" + zsfilePath + "]to[" + zsftpfilename + "]失败:" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        logger.Error("CreateBillJob==>生成中山文件[" + zsfilePath + "]失败");
                    }

                }


                //潮洲账单
                if (czbillList != null)
                {
                    bool czb = MyFileOptions.WriteFileTXT(czbillList, czfilePath);
                    if (czb && isupload)
                    {
                        bool booldg = ftp.Upload(ftpdir, czftpfilename, czfilePath);
                        MyDelegateFunc.WriteFmLog("CreateBillJob==>上传潮洲账单[" + czfilePath + "]结果：" + booldg);
                        if (booldg)
                        {
                            try
                            {
                                File.Move(zsfilePath, dir + "\\" + czftpfilename);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("CreateBillJob==>修改潮洲文件名[" + czfilePath + "]to[" + czftpfilename + "]失败:" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        logger.Error("CreateBillJob==>生成潮洲文件[" + czfilePath + "]失败");
                    }

                }




            }
            catch (Exception ex)
            {
                MyDelegateFunc.WriteFmLog("CreateBillJob==>Exception:" + ex.Message);
                logger.Error("CreateBillJob==>Exception:" + ex.Message);
            }

        }
    }//end
}//end
