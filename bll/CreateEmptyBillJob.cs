﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Windows.Forms;
using System.IO;
using model;

namespace bll
{


    /// <summary>
    /// 空账单
    /// </summary>
    public class CreateEmptyBillJob : IJob
    {


        private readonly string ProductId = "100007";// 产品ID
        private readonly string FactoryNo = "11075";// 厂家编号
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CreateEmptyBillJob));

        public void Execute(IJobExecutionContext context)
        {
            DateTime dt = DateTime.Now;
            int yy = dt.Year;// 得到年
            int mm = dt.Month;// 得到月，因为从0开始的，所以要加1
            int dd = dt.Day;// 日
            int hh = dt.Hour;// 小时
            string strmm = mm > 9 ? mm + "" : "0" + mm;
            string strdd = dd > 9 ? dd + "" : "0" + dd;
            string strhh = hh > 9 ? hh + "" : "0" + hh;
            string ftpdir = yy + strmm + strdd;// ftp上的每天一个文件夹

            FtpClient ftp = new FtpClient();
            try
            {
                if (!ftp.DirectoryIsExist(ftpdir))
                {
                    bool bmd = ftp.MakeDir(ftpdir);
                    MyDelegateFunc.WriteFmLog("CreateEmptyBillJob==>创建目录[" + ftpdir + "]结果：" + bmd);
                }
            }catch(Exception ex)
            {
                MyDelegateFunc.WriteFmLog("CreateEmptyBillJob==>创建目录[" + ftpdir + "]异常：" + ex.ToString());
            }


            //20 点的生成套餐账单,19点潮洲，21，22，23
            if (hh > 18)
            {
                return;
            }
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
            string fileName = ProductId + "_" + yy + strmm + strdd + strhh + "00" + "01.txt.sending";// 文件名
            string filePath = dir + "\\" + fileName;
            string szftpfilename = ProductId + "_" + yy + strmm + strdd + strhh + "00" + "01.txt";// ftp保存文件名
            List<string> szbillList = new List<string>();// 深圳帐单
            bool bsz = MyFileOptions.WriteFileTXT(szbillList, filePath);
            MyDelegateFunc.WriteFmLog("CreateEmptyBillJob==>生成空文件[" + filePath + "]结果：" + bsz);
            if (bsz)
            {

                bool boolf = ftp.Upload(ftpdir, szftpfilename, filePath);
                MyDelegateFunc.WriteFmLog("CreateEmptyBillJob==>上传空文件[" + filePath + "]结果：" + boolf);
                if (boolf)
                {
                    File.Move(filePath, dir + "\\" + szftpfilename);
                }
                else
                {

                }
            }
        }


    }//END
}//END
