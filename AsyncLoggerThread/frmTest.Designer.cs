namespace JiYuInfo.AsyncLoggerThread
{
    partial class frmTest
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
            this.components = new System.ComponentModel.Container();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboLevel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.loggerAsyncThread1 = new JiYuInfo.AsyncLoggerThread.LoggerAsyncThread(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(94, 72);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(328, 21);
            this.txtLog.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLog);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboLevel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(20, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 111);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日志信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "日志内容";
            // 
            // cboLevel
            // 
            this.cboLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLevel.FormattingEnabled = true;
            this.cboLevel.Location = new System.Drawing.Point(94, 30);
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.Size = new System.Drawing.Size(121, 20);
            this.cboLevel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "日志级别";
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(367, 149);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "退出";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(286, 149);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 5;
            this.btnWrite.Text = "写入";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(20, 149);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(101, 149);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // loggerAsyncThread1
            // 
            this.loggerAsyncThread1.Log_Path = ".\\log";
            this.loggerAsyncThread1.Max_File_Size = ((uint)(2u));
            this.loggerAsyncThread1.AsyncThreadStateChanged += new JiYuInfo.JYHandlerUInt(this.loggerAsyncThread1_AsyncThreadStateChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "写入千条";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 194);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnWrite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTest";
            this.Text = "日志文件异步线程记录器测试";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private LoggerAsyncThread loggerAsyncThread1;
        private System.Windows.Forms.Button button1;
    }
}