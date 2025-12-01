using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.AsyncLib
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
            this.cboMode.Items.AddRange(Enum.GetNames(typeof(Async_Thread_Mode)));
            this.txtTimes.Value = this.asyncThread1.Max_Task_Times;
            this.txtInterval.Value = this.asyncThread1.Task_Interval;
            this.cboMode.SelectedIndex = 3;
            this.asyncThread1_AsyncThreadStateChanged((uint)this.asyncThread1.Async_State);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.asyncThread1.Async_Mode = (Async_Thread_Mode)Enum.Parse(typeof(Async_Thread_Mode), this.cboMode.SelectedItem.ToString());
            this.asyncThread1.Task_Interval = uint.Parse(this.txtInterval.Value.ToString());
            this.asyncThread1.Max_Task_Times = uint.Parse(this.txtTimes.Value.ToString());
            this.asyncThread1.Start(10);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.asyncThread1.Stop();
        }

        private void cboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMode = this.cboMode.SelectedItem.ToString();
            Async_Thread_Mode eMode = (Async_Thread_Mode)Enum.Parse(typeof(Async_Thread_Mode), strMode);
            this.txtTimes.Enabled = (eMode & Async_Thread_Mode.Times) == Async_Thread_Mode.Times;
            this.txtInterval.Enabled = (eMode & Async_Thread_Mode.Interval) == Async_Thread_Mode.Interval;
        }

        private void asyncThread1_AsyncThreadStateChanged(uint arg)
        {
            ShowInfo(string.Format("Thread State Changed,Current State is {0}", (Async_Thread_State)Enum.Parse(typeof(Async_Thread_State), arg.ToString())));
            this.btnStart.Enabled = (arg == (uint)Async_Thread_State.Stopped);
            this.btnStop.Enabled = (arg == (uint)Async_Thread_State.Running);
        }

        private const int MaxInfoLength = 4096;

        private void ShowInfo(string info)
        {
            string strLine = string.Format("{0}\t{1}\r\n", DateTime.Now.ToString("HH:mm:ss.fff"), info);
            this.txtInfo.Text = strLine + this.txtInfo.Text;
            if (this.txtInfo.Text.Length > MaxInfoLength)
                this.txtInfo.Text = this.txtInfo.Text.Substring(0, MaxInfoLength);
        }

        private void asyncThread1_ErrorCatch(string arg)
        {
            ShowInfo(string.Format("Error:{0}", arg));
        }

        private void asyncThread1_DebugInfo(string arg)
        {
            ShowInfo(string.Format("Debgu:{0}", arg));
        }

        private void asyncThread1_Task(ulong arg, object obj)
        {
            ShowInfo(string.Format("Task,Input Argument is {1},Current Times is {0}", arg,obj));
            int nMax = (int)(arg) * (int.Parse(obj.ToString()));
            for (int i = 0; i < nMax; i++)
            {
                ShowInfo(string.Format("current percent is {0}", (Single)((Single)i / (Single)nMax)));
                Application.DoEvents();
            }
        }

        private void asyncThread1_ThreadCompleted(uint arg)
        {
            ShowInfo(string.Format("Thread Completed ,Reason is {0}", (Async_Thread_Completed_Reason)Enum.Parse(typeof(Async_Thread_Completed_Reason), arg.ToString())));
        }

        private void asyncThread1_ThreadStart(object arg)
        {
            ShowInfo(string.Format("Thread Start,Input Argument is {0}", arg));
        }

        private void asyncThread1_TaskBegin(ulong arg,object obj)
        {
            ShowInfo(string.Format("Task Begin,Last Times Summary is {0},Input Argument is {1}", arg, obj));
        }

        private void asyncThread1_TaskEnd(ulong arg)
        {
            ShowInfo(string.Format("Task End,Current Times Summary is {0}", arg));
        }
    }
}
