using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using bll;

namespace Business_Bill
{
    /// <summary>
    /// 手动生成套餐/结算账单，套餐账单无法按日期生成
    /// </summary>
    public partial class FmManualCreateTaocanBill : Form
    {
        public FmManualCreateTaocanBill()
        {
            InitializeComponent();
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            try
            {
                string yy = this.tbyy.Text.Trim();
                string mm = this.tbmm.Text.Trim();
                DateTime dt = DateTime.Parse(yy + "-" + mm + "-01", DateTimeFormatInfo.CurrentInfo); 
                CreateTaoCanCloseBillJob job = new CreateTaoCanCloseBillJob();
                job.Work(dt,false);
                
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show(ex.ToString(),"提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }//end
}//end
