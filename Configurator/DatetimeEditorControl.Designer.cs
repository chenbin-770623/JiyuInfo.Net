namespace JiYuInfo.Configurator
{
    partial class JYDatetimeEditorControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dtDate = new System.Windows.Forms.MonthCalendar();
            this.dtMillSecond = new System.Windows.Forms.NumericUpDown();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.dtTime = new System.Windows.Forms.DateTimePicker();
            this.getTime = new System.Windows.Forms.Timer(this.components);
            this.lblSetCurrent = new System.Windows.Forms.LinkLabel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtMillSecond)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dtDate);
            this.splitContainer1.Panel1MinSize = 183;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblSetCurrent);
            this.splitContainer1.Panel2.Controls.Add(this.dtMillSecond);
            this.splitContainer1.Panel2.Controls.Add(this.lblCurrentTime);
            this.splitContainer1.Panel2.Controls.Add(this.dtTime);
            this.splitContainer1.Panel2MinSize = 28;
            this.splitContainer1.Size = new System.Drawing.Size(245, 215);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 5;
            // 
            // dtDate
            // 
            this.dtDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtDate.Location = new System.Drawing.Point(0, 0);
            this.dtDate.MaxSelectionCount = 1;
            this.dtDate.Name = "dtDate";
            this.dtDate.ShowWeekNumbers = true;
            this.dtDate.TabIndex = 1;
            this.dtDate.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.dtDate_DateSelected);
            // 
            // dtMillSecond
            // 
            this.dtMillSecond.Location = new System.Drawing.Point(81, 3);
            this.dtMillSecond.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.dtMillSecond.Name = "dtMillSecond";
            this.dtMillSecond.Size = new System.Drawing.Size(39, 21);
            this.dtMillSecond.TabIndex = 8;
            this.dtMillSecond.ValueChanged += new System.EventHandler(this.dtMillSecond_ValueChanged);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(168, 9);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(77, 12);
            this.lblCurrentTime.TabIndex = 7;
            this.lblCurrentTime.Text = "10:55:27.000";
            // 
            // dtTime
            // 
            this.dtTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtTime.Location = new System.Drawing.Point(3, 3);
            this.dtTime.Name = "dtTime";
            this.dtTime.ShowUpDown = true;
            this.dtTime.Size = new System.Drawing.Size(72, 21);
            this.dtTime.TabIndex = 5;
            this.dtTime.ValueChanged += new System.EventHandler(this.dtTime_ValueChanged);
            // 
            // getTime
            // 
            this.getTime.Interval = 50;
            this.getTime.Tick += new System.EventHandler(this.getTime_Tick);
            // 
            // lblSetCurrent
            // 
            this.lblSetCurrent.AutoSize = true;
            this.lblSetCurrent.Location = new System.Drawing.Point(133, 9);
            this.lblSetCurrent.Name = "lblSetCurrent";
            this.lblSetCurrent.Size = new System.Drawing.Size(29, 12);
            this.lblSetCurrent.TabIndex = 9;
            this.lblSetCurrent.TabStop = true;
            this.lblSetCurrent.Text = "当前";
            this.lblSetCurrent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSetCurrent_LinkClicked);
            // 
            // JYDatetimeEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "JYDatetimeEditorControl";
            this.Size = new System.Drawing.Size(245, 215);
            this.Load += new System.EventHandler(this.DatetimeEditorControl_Load);
            this.Resize += new System.EventHandler(this.DatetimeEditorControl_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtMillSecond)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MonthCalendar dtDate;
        private System.Windows.Forms.NumericUpDown dtMillSecond;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.DateTimePicker dtTime;
        private System.Windows.Forms.Timer getTime;
        private System.Windows.Forms.LinkLabel lblSetCurrent;


    }
}
