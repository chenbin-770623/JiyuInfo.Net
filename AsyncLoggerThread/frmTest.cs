using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.AsyncLoggerThread
{
    /// <summary>
    /// 测试窗口
    /// </summary>
    internal partial class frmTest : Form
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public frmTest()
        {
            InitializeComponent();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            this.cboLevel.Items.AddRange(Enum.GetNames(typeof(Logger.JY_Log_Level)));
            this.cboLevel.SelectedIndex = 0;
            loggerAsyncThread1_AsyncThreadStateChanged((uint)AsyncLib.Async_Thread_State.Stopped);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.loggerAsyncThread1.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.loggerAsyncThread1.Stop();
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtLog.Text.Trim())) return;
            this.loggerAsyncThread1.Write((Logger.JY_Log_Level)Enum.Parse(typeof(Logger.JY_Log_Level), this.cboLevel.SelectedItem.ToString()), this.txtLog.Text.Trim());
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (this.loggerAsyncThread1.Async_State != AsyncLib.Async_Thread_State.Stopped)
                this.btnStop_Click(this.btnStop, new EventArgs());
            this.Close();
        }

        private void loggerAsyncThread1_AsyncThreadStateChanged(uint arg)
        {
            this.btnStart.Enabled = arg == (uint)AsyncLib.Async_Thread_State.Stopped;
            this.btnStop.Enabled = arg == (uint)AsyncLib.Async_Thread_State.Running;
            this.btnWrite.Enabled = arg == (uint)AsyncLib.Async_Thread_State.Running;
            this.button1.Enabled = arg == (uint)AsyncLib.Async_Thread_State.Running;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < 1000; i++)
                this.loggerAsyncThread1.Write(string.Format("index = {0};", i));
            MessageBox.Show(i.ToString());
        }
    }
}
