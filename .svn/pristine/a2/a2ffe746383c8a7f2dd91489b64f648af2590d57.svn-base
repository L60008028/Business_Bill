using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using bll;
using System.Threading;

namespace Business_Bill
{
    /// <summary>
    /// 处理状态
    /// </summary>
    public partial class FmSuccessRate : Form
    {
        public FmSuccessRate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 写状态
        /// </summary>
        /// <param name="txt"></param>
        private void WriteStatus(string txt)
        {
            try
            {

                if (lblStatus.InvokeRequired)
                {

                    lblStatus.Invoke(new Action<string>(x => WriteStatus(x)), new object[] { txt });

                }
                else
                {
                    lblStatus.Text = txt;
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="txt"></param>
        private void WriteLog(string txt)
        {
            try
            {
                if (rtbLog.InvokeRequired)
                {

                    rtbLog.Invoke(new Action<string>(x => WriteLog(x)), new object[] { txt });

                }
                else
                {
                    if (rtbLog.TextLength > 214748364)
                    {
                        rtbLog.Clear();
                    }
                    rtbLog.AppendText("[" + DateTime.Now.ToString() + "]==>" + txt + Environment.NewLine);
                    rtbLog.SelectionStart = rtbLog.Text.Length;
                    rtbLog.ScrollToCaret();
                }
            }
            catch (Exception)
            {

            }
        }


        private void btnBegin_Click(object sender, EventArgs e)
        {
            string eprid = this.tbEprId.Text.Trim();
            string userid = this.tbUserId.Text.Trim();
            double rate =Double.Parse( this.tbRate.Text.Trim());
            string y = this.tbYear.Text.Trim();
            string m = this.tbMonth.Text.Trim();
            string d = this.tbDay.Text.Trim();
            if (string.IsNullOrEmpty(eprid) || string.IsNullOrEmpty(userid) || rate < 0.1 || string.IsNullOrEmpty(y) || string.IsNullOrEmpty(m) || string.IsNullOrEmpty(d))
            {
                MessageBox.Show("必须输入参数");
                return;
            }
            ChangeModl cm = new ChangeModl();
            cm.EprId = eprid;
            cm.UserId = userid;
            cm.Rate = rate;
            cm.Year = y;
            cm.Month = m;
            cm.Day = d;
            cm.WriteStatus = new model.MyDelegateDefine.DelegateWriteChangeFmStatus(WriteStatus);
            cm.WriteFmLog = new model.MyDelegateDefine.DelegateWriteFmLog(WriteLog);
            ChangeReportStatus crs = new ChangeReportStatus(cm);
            Thread thread = new Thread(new ThreadStart(crs.DoWork));
            thread.IsBackground = true;
            thread.Start();
        }//end
    }//end
}//end
