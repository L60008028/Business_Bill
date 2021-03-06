﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Windows.Forms;
using System.IO;
using dal;
using model;
using System.Globalization;

namespace bll
{

    /*
     * 全部按移动结算
     *1、发送量超过套餐：套餐内他网，电信55分，超出部分按套餐外价格折算；
     *2、发送量未超过套餐：按套餐数量，他网数量折算，剩余的是电信的；
     *3、未发送：按套餐数量，全部是电信；
     * */
    /// <summary>
    /// 套餐结算，每月最后一天，23：55执行,放在20点的文件中
    /// </summary>
    public class CreateTaoCanCloseBillJob : IJob
    {
        private string ProductId = "100007";// 产品ID
        private string FactoryNo = "11075";// 厂家编号
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CreateBillJob));

        public void Execute(IJobExecutionContext context)
        {

            //先更新帐单
           // EprInfoDAL eprinfoDAL = new EprInfoDAL();
           // bool b = eprinfoDAL.InitTaocanId();
           // MyDelegateFunc.WriteFmLog("CreateTaoCanBillJob=>初始化套餐reslt=" + b);


            ProductId = GlobalParams.ProductId;
            FactoryNo = GlobalParams.FactoryNo;
            DateTime dt = DateTime.Now;
            Work(dt, true);

        }


        /// <summary>
        /// 计算套餐及超出部分
        /// </summary>
        /// <param name="dtnow">日期</param>
        /// <param name="isupload">是否上传</param>
        public void Work(DateTime dtnow, bool isupload)
        {



            string begintime = DateTools.FirstDayOfMonth(dtnow).ToShortDateString();
            string endtime = DateTools.LastDayOfMonth(dtnow).ToShortDateString() + " 23:59:59";
            int yy = dtnow.Year;// 得到年
            int mm = dtnow.Month;// 得到月
            int dd = dtnow.Day;// 日
            int hh = dtnow.Hour;// 小时

            string strmm = mm > 9 ? mm + "" : "0" + mm;
            string strdd = dd > 9 ? dd + "" : "0" + dd;
            string strhh = hh > 9 ? hh + "" : "0" + hh;
            strhh = "20";
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
            Dictionary<int, EprInfoModel> eprDic = eprinfoDAL.GetTaoCanEprInfoAll2();


            SmsMobileDAL smsdal = new SmsMobileDAL();
            Dictionary<int, MobileTypeCountModel> eprCount = smsdal.GetSumSMScountByMomth(eprDic.Keys.ToList(), begintime, endtime);//企业发送统计
            Dictionary<int, MobileTypeCountModel> billCount = new Dictionary<int, MobileTypeCountModel>();//套餐企业的发送量

            TaoCanInfoDAL taocanDAL = new TaoCanInfoDAL();
            Dictionary<int, TaoCanInfoModel> taocanDic = taocanDAL.GetAll();//套餐企业信息
            List<string> daystatlist = new List<string>();//帐号报表
            List<string> szbillList = new List<string>();// 深圳帐单

            //计算账单数量
            foreach (int key in eprCount.Keys)
            {
                MobileTypeCountModel mtcm = eprCount[key];//发送量
                EprInfoModel eprinfo = eprDic[key];//企业信息
                if (!taocanDic.ContainsKey(eprinfo.taocanId))
                {
                    continue;
                }
                TaoCanInfoModel taocaninfo = taocanDic[eprinfo.taocanId];//套餐信息
                int billct = mtcm.CMcount + mtcm.CTcount - taocaninfo.TCNumber;//超出套餐数量

                //套餐账单                
                int taocanneiTotalCount = taocaninfo.Billnumber;


                if (billct > 0)//超出套餐量
                {
                    /*
                  int taocanneiCTbillcount = (int)(taocaninfo.Billnumber / 2);//套餐内移动数量
                  int taocanneiCMbillcount = taocaninfo.Billnumber - taocanneiCTbillcount;//套餐内电信动数量

                  
                  int taocanneiCTcount = (int)taocaninfo.TCNumber / 2;
                  int taocanneiCMcount = taocaninfo.TCNumber = taocanneiCTcount;
                  int ct = mtcm.CTcount - taocanneiCTcount > 0 ? mtcm.CTcount - taocanneiCTcount : 0;//套餐外移动多出数量
                  int cm = mtcm.CMcount - taocanneiCMcount > 0 ? mtcm.CMcount - taocanneiCMcount : 0;//套餐外电信多出数量
                  int ctwaibill = (int)Math.Ceiling(ct * (dprice / 10));//移动套餐外折算后数量
                  int cmwaibill = (int)Math.Ceiling(cm * (dprice / 10));//电信动套餐外折算后数量
                   * */

                    double dprice = 10.00;
                    if (!string.IsNullOrEmpty(taocaninfo.TCOutsidePrice))
                    {
                        bool b = double.TryParse(taocaninfo.TCOutsidePrice, out dprice);
                        if (dprice <= 0.00 || !b)
                        {
                            dprice = 10.00;
                        }
                    }
                    int ctwaibill = (int)Math.Ceiling(billct * (dprice / 10));//移动套餐外折算后数量
                    int cmwaibill = 0;
                    //账单
                    MobileTypeCountModel mtcm_bill = new MobileTypeCountModel();
                    mtcm_bill.TotalCount = ctwaibill + cmwaibill + taocanneiTotalCount;//总量
                    mtcm_bill.CTcount = mtcm_bill.TotalCount;//移动
                    mtcm_bill.CMcount = 0;//电信
                    MyDelegateFunc.WriteFmLog(string.Format("CreateTaoCanCloseBillJob==>【超出套餐】企业id:{0},清单数量：{1},帐单数量：{2},套餐外单价：{3},套餐量：{4}", key, (mtcm.CMcount + mtcm.CTcount), mtcm_bill.TotalCount, taocaninfo.TCOutsidePrice, taocaninfo.TCNumber));
                    billCount.Add(key, mtcm_bill);
                }
                else
                {
                    if (mtcm.TotalCount == 0)//未发送
                    {
                        MobileTypeCountModel mbill = new MobileTypeCountModel();
                        mbill.TotalCount = taocanneiTotalCount;
                        mbill.CTcount = taocanneiTotalCount;
                       mbill.CMcount = 0;
                        MyDelegateFunc.WriteFmLog(string.Format("CreateTaoCanCloseBillJob==>【未发】送企业id:{0},清单数量：{1}", key, mbill.TotalCount));
                        billCount.Add(key, mbill);
                    }
                    else//套餐内
                    {
                        double dprice = 10.00;
                        string tprice = taocaninfo.TCPrice;
                        if (!string.IsNullOrEmpty(tprice))
                        {
                            bool b = double.TryParse(tprice, out dprice);
                            if (dprice <= 0.00 || !b)
                            {
                                dprice = 10.00;
                            }
                        }

                        MobileTypeCountModel mbill = new MobileTypeCountModel();
                        mbill.TotalCount = taocanneiTotalCount;
                        
                        //mbill.CTcount = (int)Math.Ceiling(mtcm.CTcount * (dprice / 10)); ;
                       // mbill.CMcount = taocanneiTotalCount - mbill.CTcount;
                        mbill.CTcount = taocanneiTotalCount;
                        mbill.CMcount = 0;
                        MyDelegateFunc.WriteFmLog(string.Format("CreateTaoCanCloseBillJob==>【套餐内】未发送企业id:{0},清单数量：{1},电信数量：{2}，移动数量：{3}", key, mbill.TotalCount, mbill.CMcount, mbill.CMcount));
                        billCount.Add(key, mbill);
                    }
                }


            }


            //生成账单
            foreach (int key in billCount.Keys)
            {
                EprInfoModel eprinfo = eprDic[key];//企业信息
                MobileTypeCountModel mtcm = billCount[key];//发送量
                string bnetId = eprinfo.bnetId, bnetAccount = eprinfo.bnetAccount;
                try
                {
                    //生成sql
                    object[] obj = { eprinfo.id, bnetId, bnetAccount, mtcm.CTcount, mtcm.CUcount, mtcm.CMcount, yy, mm, dd };
                    string sql = string.Format("Insert into jy15.gdkltx.dbo.t_billdaystat(EprId,BnetId,BnetAccount,CTcount,CUcount,CMcount,YY,MM,DD) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8})", obj);
                    daystatlist.Add(sql);
                }
                catch (Exception ex)
                {
                    MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>生成sqlList失败：" + ex.Message);
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

                }


            }

            try
            {
                if (daystatlist != null && daystatlist.Count > 0)
                {
                    int result = SQLbll.BatchExec(daystatlist);
                    MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>保存sql result：" + result);
                }
            }
            catch (Exception ex)
            {
                MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>保存sql失败：" + daystatlist[0]);
                MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>保存sql失败：" + ex.Message);
            }

            FtpClient ftp = new FtpClient();

            //深圳账单
            if (szbillList != null)
            {
                bool bsz = MyFileOptions.WriteFileTXT(szbillList, filePath);
                MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>生成sz套餐文件[" + filePath + "]结果：" + bsz);
                if (bsz)
                {
                    if (!isupload)
                    {
                        return;
                    }
                    bool boolf = ftp.Upload(ftpdir, szftpfilename, filePath);
                    MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>上传sz套餐账单[" + szftpfilename + "]结果：" + boolf);
                    if (boolf)
                    {
                        try
                        {
                            File.Move(filePath, dir + "\\" + szftpfilename);
                        }
                        catch (Exception ex)
                        {
                            MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>修改sz套餐文件名[" + filePath + "]to[" + szftpfilename + "]失败:" + ex.Message);
                            logger.Error("CreateTaoCanCloseBillJob==>修改sz套餐文件名[" + filePath + "]to[" + szftpfilename + "]失败:" + ex.Message);
                        }
                    }
                    else
                    {
                        MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob=FTP=>上传sz套餐文件[" + filePath + "]失败");
                        logger.Error("CreateTaoCanCloseBillJob=FTP=>上传sz套餐文件[" + filePath + "]失败");
                    }

                }
                else
                {
                    MyDelegateFunc.WriteFmLog("CreateTaoCanCloseBillJob==>生成sz文件[" + filePath + "]失败");
                    logger.Error("CreateTaoCanCloseBillJob==>生成sz文件[" + filePath + "]失败");
                }
            }

 


        }
    }//end
}//end
