using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using bll;
using System.IO;

namespace Business_Bill
{
    public partial class From1 : Form
    {
        public From1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FtpClient ftp = new FtpClient();
            string localfile = this.textBox1.Text.Trim();
            FileInfo fi = new FileInfo(localfile);
            string remotpath = this.textBox2.Text.Trim();
            bool b=ftp.Upload(remotpath, fi.FullName,localfile);
            MessageBox.Show(""+b);
            //HttpHelper hh = new HttpHelper();
            //string url = "http://61.147.77.60:8000/callback";
            //hh.SetAddrUrl("4893", "1f3cef62927b52141202cf56350e99ed", url);
        }

        private void btnCheckFile_Click(object sender, EventArgs e)
        {
            string remotpath = this.textBox2.Text.Trim();
            string ftpfile = this.textBox3.Text.Trim();
            FtpClient ftp = new FtpClient();
            ftp.WriteLog += WriteLog;
            bool b = ftp.FileCheckExist(remotpath, ftpfile);
            MessageBox.Show("" + b);
        }

        public void WriteLog(string txt)
        {
            if (this.rtbLog.InvokeRequired)
            {
                this.rtbLog.Invoke(new Action<string>(x => WriteLog(x)), new object[] { txt });
            }
            else
            {
                this.rtbLog.AppendText(txt);
                this.rtbLog.AppendText("\n\r");
                this.rtbLog.ScrollToCaret();
            }
        }

    }//end
}
