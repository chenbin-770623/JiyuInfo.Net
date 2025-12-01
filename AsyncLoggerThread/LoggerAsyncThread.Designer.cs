namespace JiYuInfo.AsyncLoggerThread
{
    partial class LoggerAsyncThread
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
            this.baseLogger = new JiYuInfo.Logger.FileLogger(this.components);
            this.baseThread = new JiYuInfo.AsyncLib.AsyncThread(this.components);
            // 
            // baseLogger
            // 
            this.baseLogger.Max_File_Size = ((uint)(2u));
            // 
            // baseThread
            // 
            this.baseThread.Max_Task_Times = ((uint)(0u));
            this.baseThread.Task_Interval = ((uint)(100u));
            this.baseThread.ErrorCatch += new JiYuInfo.JYHandlerString(this.baseThread_ErrorCatch);
            this.baseThread.DebugInfo += new JiYuInfo.JYHandlerString(this.baseThread_DebugInfo);
            this.baseThread.AsyncThreadStateChanged += new JiYuInfo.JYHandlerUInt(this.baseThread_AsyncThreadStateChanged);
            this.baseThread.Task += new JiYuInfo.AsyncLib.AsyncThread.JYHandlerULongObject(this.baseThread_Task);
            this.baseThread.ThreadStart += new JiYuInfo.JYHandlerObject(this.baseThread_ThreadStart);
            this.baseThread.ThreadCompleted += new JiYuInfo.JYHandlerUInt(this.baseThread_ThreadCompleted);

        }

        #endregion

        private AsyncLib.AsyncThread baseThread;
        private Logger.FileLogger baseLogger;
    }
}
