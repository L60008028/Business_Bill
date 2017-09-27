using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using model;
using bll;

namespace Business_Bill
{
    public partial class FmSet : Form
    {
        public FmSet()
        {
            InitializeComponent();
        }
        LocalParams lp;
        private void FmSet_Load(object sender, EventArgs e)
        {
             lp = new LocalParams();
             if(lp!=null)
             {
                 this.tbcron1.Text = lp.CronExpression1;
                 this.tbcron2.Text = lp.CronExpression2;
                 this.tbsqlconnstr.Text = lp.SqlConnStr;
                 this.tbcronTaocan.Text = lp.CronExpressionTaocan;
                 this.tbcronTaocanClose.Text = lp.CronExpressionTaocanClose;
                 this.tbEpridBnetaccount.Text = lp.EprIdBnetAccount;
                 this.tbUid.Text = lp.UID;
                 this.tbPwd.Text = lp.PWD;
             }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lp.CronExpression1 = this.tbcron1.Text.Trim();
                lp.CronExpression2 = this.tbcron2.Text.Trim();
                lp.SqlConnStr = this.tbsqlconnstr.Text.Trim();
                if (!SQLbll.IsConn(lp.SqlConnStr))
                {
                    MessageBox.Show("数据库连接失败：" + lp.SqlConnStr);
                    return;
                }
                lp.CronExpressionTaocan = this.tbcronTaocan.Text.Trim();
                lp.CronExpressionTaocanClose = this.tbcronTaocanClose.Text.Trim();
                lp.EprIdBnetAccount = this.tbEpridBnetaccount.Text.Trim();
                lp.UID = this.tbUid.Text.Trim();
                lp.PWD = this.tbPwd.Text.Trim();
                lp.Save();
                lp.Reload();
                MessageBox.Show("成功！");
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show("失败" + ex);
            }
           
        }


        private void btnCreateDIR_Click(object sender, EventArgs e)
        {
            string dirname = this.tbFTPdir.Text.Trim();
            if(!string.IsNullOrEmpty(dirname))
            {
                FtpClient ftp = new FtpClient();
                bool b=ftp.MakeDir(dirname);
                MessageBox.Show(b.ToString());
            }
        }

        private void btnChk_Click(object sender, EventArgs e)
        {
            string dirname = this.tbFTPdir.Text.Trim();
            if (!string.IsNullOrEmpty(dirname))
            {
                FtpClient ftp = new FtpClient();
                bool b = ftp.DirectoryIsExist(dirname);
                MessageBox.Show(b.ToString());
            }
        }


    }//end
}
