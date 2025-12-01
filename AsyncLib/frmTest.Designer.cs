namespace JiYuInfo.AsyncLib
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtInterval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTimes = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.asyncThread1 = new JiYuInfo.AsyncLib.AsyncThread(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimes)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtInterval);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTimes);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.cboMode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(195, 46);
            this.txtInterval.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(52, 21);
            this.txtInterval.TabIndex = 7;
            this.txtInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "循环间隔";
            // 
            // txtTimes
            // 
            this.txtTimes.Location = new System.Drawing.Point(65, 46);
            this.txtTimes.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtTimes.Name = "txtTimes";
            this.txtTimes.Size = new System.Drawing.Size(52, 21);
            this.txtTimes.TabIndex = 5;
            this.txtTimes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "循环次数";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(355, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(274, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cboMode
            // 
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.FormattingEnabled = true;
            this.cboMode.Location = new System.Drawing.Point(65, 20);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(182, 20);
            this.cboMode.TabIndex = 1;
            this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "线程模式";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtInfo);
            this.groupBox2.Location = new System.Drawing.Point(12, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 178);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Location = new System.Drawing.Point(3, 17);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(430, 158);
            this.txtInfo.TabIndex = 0;
            // 
            // asyncThread1
            // 
            this.asyncThread1.Max_Task_Times = ((uint)(10u));
            this.asyncThread1.Task_Interval = ((uint)(1000u));
            this.asyncThread1.ErrorCatch += new JiYuInfo.JYHandlerString(this.asyncThread1_ErrorCatch);
            this.asyncThread1.DebugInfo += new JiYuInfo.JYHandlerString(this.asyncThread1_DebugInfo);
            this.asyncThread1.AsyncThreadStateChanged += new JiYuInfo.JYHandlerUInt(this.asyncThread1_AsyncThreadStateChanged);
            this.asyncThread1.TaskBegin += new JiYuInfo.AsyncLib.AsyncThread.JYHandlerULongObject(this.asyncThread1_TaskBegin);
            this.asyncThread1.TaskEnd += new JiYuInfo.JYHandlerULong(this.asyncThread1_TaskEnd);
            this.asyncThread1.Task += new JiYuInfo.AsyncLib.AsyncThread.JYHandlerULongObject(this.asyncThread1_Task);
            this.asyncThread1.ThreadStart += new JiYuInfo.JYHandlerObject(this.asyncThread1_ThreadStart);
            this.asyncThread1.ThreadCompleted += new JiYuInfo.JYHandlerUInt(this.asyncThread1_ThreadCompleted);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 285);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTest";
            this.Text = "frmTest";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimes)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown txtInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown txtTimes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cboMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtInfo;
        private AsyncThread asyncThread1;
    }
}