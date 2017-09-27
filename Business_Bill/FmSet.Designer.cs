namespace Business_Bill
{
    partial class FmSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbcron1 = new System.Windows.Forms.TextBox();
            this.tbcron2 = new System.Windows.Forms.TextBox();
            this.tbsqlconnstr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbcronTaocan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbcronTaocanClose = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbFTPdir = new System.Windows.Forms.TextBox();
            this.btnCreateDIR = new System.Windows.Forms.Button();
            this.btnChk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbEpridBnetaccount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbUid = new System.Windows.Forms.TextBox();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcron1
            // 
            this.tbcron1.Location = new System.Drawing.Point(159, 20);
            this.tbcron1.Name = "tbcron1";
            this.tbcron1.Size = new System.Drawing.Size(153, 21);
            this.tbcron1.TabIndex = 0;
            // 
            // tbcron2
            // 
            this.tbcron2.Location = new System.Drawing.Point(488, 23);
            this.tbcron2.Name = "tbcron2";
            this.tbcron2.Size = new System.Drawing.Size(153, 21);
            this.tbcron2.TabIndex = 1;
            // 
            // tbsqlconnstr
            // 
            this.tbsqlconnstr.Location = new System.Drawing.Point(159, 104);
            this.tbsqlconnstr.Name = "tbsqlconnstr";
            this.tbsqlconnstr.Size = new System.Drawing.Size(482, 21);
            this.tbsqlconnstr.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "账单cronExpression：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(349, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "空帐单cronExpression：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "数据库连接串：";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(800, 353);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbcronTaocan
            // 
            this.tbcronTaocan.Location = new System.Drawing.Point(159, 62);
            this.tbcronTaocan.Name = "tbcronTaocan";
            this.tbcronTaocan.Size = new System.Drawing.Size(153, 21);
            this.tbcronTaocan.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "套餐cronExpression：";
            // 
            // tbcronTaocanClose
            // 
            this.tbcronTaocanClose.Location = new System.Drawing.Point(488, 62);
            this.tbcronTaocanClose.Name = "tbcronTaocanClose";
            this.tbcronTaocanClose.Size = new System.Drawing.Size(153, 21);
            this.tbcronTaocanClose.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(337, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "套餐结算cronExpression：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 353);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "目录：";
            // 
            // tbFTPdir
            // 
            this.tbFTPdir.Location = new System.Drawing.Point(59, 350);
            this.tbFTPdir.Name = "tbFTPdir";
            this.tbFTPdir.Size = new System.Drawing.Size(169, 21);
            this.tbFTPdir.TabIndex = 9;
            // 
            // btnCreateDIR
            // 
            this.btnCreateDIR.Location = new System.Drawing.Point(250, 350);
            this.btnCreateDIR.Name = "btnCreateDIR";
            this.btnCreateDIR.Size = new System.Drawing.Size(75, 23);
            this.btnCreateDIR.TabIndex = 10;
            this.btnCreateDIR.Text = "建目录";
            this.btnCreateDIR.UseVisualStyleBackColor = true;
            this.btnCreateDIR.Click += new System.EventHandler(this.btnCreateDIR_Click);
            // 
            // btnChk
            // 
            this.btnChk.Location = new System.Drawing.Point(331, 350);
            this.btnChk.Name = "btnChk";
            this.btnChk.Size = new System.Drawing.Size(75, 23);
            this.btnChk.TabIndex = 11;
            this.btnChk.Text = "CHK目录";
            this.btnChk.UseVisualStyleBackColor = true;
            this.btnChk.Click += new System.EventHandler(this.btnChk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPwd);
            this.groupBox1.Controls.Add(this.tbUid);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbcron1);
            this.groupBox1.Controls.Add(this.tbcron2);
            this.groupBox1.Controls.Add(this.tbcronTaocan);
            this.groupBox1.Controls.Add(this.tbcronTaocanClose);
            this.groupBox1.Controls.Add(this.tbsqlconnstr);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(865, 137);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本设置";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbEpridBnetaccount);
            this.groupBox2.Location = new System.Drawing.Point(12, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(866, 174);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "特殊设置";
            // 
            // tbEpridBnetaccount
            // 
            this.tbEpridBnetaccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEpridBnetaccount.Location = new System.Drawing.Point(3, 17);
            this.tbEpridBnetaccount.Multiline = true;
            this.tbEpridBnetaccount.Name = "tbEpridBnetaccount";
            this.tbEpridBnetaccount.Size = new System.Drawing.Size(860, 154);
            this.tbEpridBnetaccount.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(660, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "短信账号：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(662, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "短信密码：";
            // 
            // tbUid
            // 
            this.tbUid.Location = new System.Drawing.Point(731, 20);
            this.tbUid.Name = "tbUid";
            this.tbUid.Size = new System.Drawing.Size(100, 21);
            this.tbUid.TabIndex = 8;
            // 
            // tbPwd
            // 
            this.tbPwd.Location = new System.Drawing.Point(733, 62);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(100, 21);
            this.tbPwd.TabIndex = 9;
            // 
            // FmSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 385);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnChk);
            this.Controls.Add(this.btnCreateDIR);
            this.Controls.Add(this.tbFTPdir);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmSet";
            this.Text = "商务短信[设置]";
            this.Load += new System.EventHandler(this.FmSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbcron1;
        private System.Windows.Forms.TextBox tbcron2;
        private System.Windows.Forms.TextBox tbsqlconnstr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbcronTaocan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbcronTaocanClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbFTPdir;
        private System.Windows.Forms.Button btnCreateDIR;
        private System.Windows.Forms.Button btnChk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbEpridBnetaccount;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbUid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}