namespace CopyData
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbtnCloseTask = new System.Windows.Forms.RadioButton();
            this.rdbtnOpenTask = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtImportConnStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExportConnStr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbtnCloseTask);
            this.groupBox1.Controls.Add(this.rdbtnOpenTask);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtImportConnStr);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtExportConnStr);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 185);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置参数";
            // 
            // rdbtnCloseTask
            // 
            this.rdbtnCloseTask.AutoSize = true;
            this.rdbtnCloseTask.Location = new System.Drawing.Point(215, 153);
            this.rdbtnCloseTask.Name = "rdbtnCloseTask";
            this.rdbtnCloseTask.Size = new System.Drawing.Size(35, 16);
            this.rdbtnCloseTask.TabIndex = 6;
            this.rdbtnCloseTask.TabStop = true;
            this.rdbtnCloseTask.Text = "否";
            this.rdbtnCloseTask.UseVisualStyleBackColor = true;
            // 
            // rdbtnOpenTask
            // 
            this.rdbtnOpenTask.AutoSize = true;
            this.rdbtnOpenTask.Location = new System.Drawing.Point(154, 153);
            this.rdbtnOpenTask.Name = "rdbtnOpenTask";
            this.rdbtnOpenTask.Size = new System.Drawing.Size(35, 16);
            this.rdbtnOpenTask.TabIndex = 5;
            this.rdbtnOpenTask.TabStop = true;
            this.rdbtnOpenTask.Text = "是";
            this.rdbtnOpenTask.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "开启线程";
            // 
            // txtImportConnStr
            // 
            this.txtImportConnStr.Location = new System.Drawing.Point(50, 101);
            this.txtImportConnStr.Name = "txtImportConnStr";
            this.txtImportConnStr.Size = new System.Drawing.Size(335, 21);
            this.txtImportConnStr.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "目标数据库连接字符串";
            // 
            // txtExportConnStr
            // 
            this.txtExportConnStr.Location = new System.Drawing.Point(50, 45);
            this.txtExportConnStr.Name = "txtExportConnStr";
            this.txtExportConnStr.Size = new System.Drawing.Size(335, 21);
            this.txtExportConnStr.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "源数据库连接字符串";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(166, 203);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(34, 264);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(363, 250);
            this.txtLog.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "日志";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 540);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmMain";
            this.Text = "主程序";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbtnCloseTask;
        private System.Windows.Forms.RadioButton rdbtnOpenTask;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtImportConnStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtExportConnStr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label4;
    }
}

