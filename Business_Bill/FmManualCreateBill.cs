﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using bll;
using System.Globalization;
using model;

namespace Business_Bill
{
    public partial class FmManualCreateBill : Form
    {
        public FmManualCreateBill()
        {
            InitializeComponent();
        }

        private void FmManualCreateBill_Load(object sender, EventArgs e)
        {
            this.dtpBegin.Format = DateTimePickerFormat.Custom;
            this.dtpBegin.CustomFormat = "yyyy-MM-dd";
            this.dtpEnd.Format = DateTimePickerFormat.Custom;
            this.dtpEnd.CustomFormat = "yyyy-MM-dd";
            
            
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
           //string str= DateTime.Now.ToString("yyyy/MM/dd H:mm:ss", DateTimeFormatInfo.InvariantInfo);
            ManualCreateBill cbj = new ManualCreateBill();
            
            DateTime dt = dtpBegin.Value;
            string begintime = dtpBegin.Value.ToShortDateString();
            string endtime = dtpEnd.Value.ToShortDateString() + " 23:59:59";
            List<int> epridLst = new List<int>();
            string eprids = this.tbEprId.Text.Trim();
            if (!string.IsNullOrEmpty(eprids))
            {
                eprids.Replace(";",",");
                eprids.Replace("，",",");
                string[] epridarrar = eprids.Split(',');
                foreach(string id in epridarrar)
                {
                    try
                    {
                        epridLst.Add(int.Parse(id));
                    }catch
                    {}
                }
            }
            bool isupload = false;
            cbj.Work(dt,begintime,endtime,epridLst,isupload);
        }

        //补单
        private void btnStart_Click(object sender, EventArgs e)
        {
            DateTime dtnow = DateTime.Now;
            int yy = dtnow.Year;
            int mm = dtnow.Month;
            int dd = dtnow.Day;
            string file = this.tbFile.Text.Trim();
            string billparam = this.tbBill.Text.Trim();
            billparam.Replace("，",",");
            billparam.Replace(";", ",");
            string[] parray=billparam.Split('\n');
            List<string> strLst = new List<string>();
            List<string> sqlLst = new List<string>();
            foreach(string s in parray)
            {
                string[] saray = s.Split(',');
                if(saray.Length>2)
                {
                    int len = int.Parse(saray[2]);
                    for (int i = 0; i < len;i++ )
                    {
                        string t = string.Format("{0}|{1}|0|1|0|0|{2}", saray[0], saray[1], dtnow.ToString("yyyy/MM/dd H:mm:ss", DateTimeFormatInfo.InvariantInfo));
                        strLst.Add(t);
                    }
                    object obj = SQLbll.ExecuteScalar(string.Format("select Id from jy15.gdkltx.dbo.t_eprinfo where BnetId='{0}'", saray[0]));
                    BnetInfoModel bimodel = SQLbll.GetBnetInfo(saray[0], saray[1]);
                    int eprid = 0;
                    if(obj!=null && bimodel!=null)
                    {
                        eprid = int.Parse(obj.ToString()); 
                        object[] objarray = { eprid, saray[0], saray[1], len, 0, 0, yy, mm, dd, 1, dtnow + ":补推", bimodel.Price, 0, bimodel.TaoCanType };
                        string sql = string.Format("Insert into jy15.gdkltx.dbo.t_billdaystat(EprId,BnetId,BnetAccount,CTcount,CUcount,CMcount,YY,MM,DD,AreaId,memo,Price,InitialNum,TaoCanType) values({0},'{1}','{2}',{3},{4},{5},{6},{7},{8},{9},'{10}',{11},{12},'{13}')", objarray);
                        sqlLst.Add(sql);
                    }


                }
            }
            if (strLst.Count > 0)
            {
                if (string.IsNullOrEmpty(file))
                {
                    file = Application.StartupPath + "\\补推.txt";
                }
                bool bsz = MyFileOptions.WriteFileTXT(strLst, file);
                if(bsz)
                {
                    if(sqlLst!=null)
                    {
                        SQLbll.BatchExec(sqlLst);
                    }
                }
                MessageBox.Show(bsz.ToString());
            }
            else
            {
                MessageBox.Show("失败");
            }
        }
    }
}
