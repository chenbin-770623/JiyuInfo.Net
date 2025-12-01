using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiYuInfo.AsyncLoggerThread
{
    using System.Collections;
    using Logger;
    /// <summary>
    /// 日志文件异步线程记录器
    /// </summary>
    public partial class LoggerAsyncThread : Component , ILogger
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public LoggerAsyncThread()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="container"></param>
        public LoggerAsyncThread(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void baseThread_Task(ulong arg, object obj)
        {
            try
            {
                Logger.JYLog log = new Logger.JYLog();
                lock (m_Queue.SyncRoot)
                    if(m_Queue.Count > 0)
                        log = (Logger.JYLog)m_Queue.Dequeue();
                if (!string.IsNullOrEmpty(log.Log))
                    baseLogger.Write(log);
            }
            catch (Exception ex)
            {
                OnErrorCatch(ex.Message);
            }
        }

        private void baseThread_ThreadStart(object arg)
        {
            OnThreadStart();
        }

        private void baseThread_ThreadCompleted(uint arg)
        {
            try
            {
                lock (m_Queue.SyncRoot)
                {
                    Logger.JYLog log = new Logger.JYLog();
                    while (m_Queue.Count > 0)
                    {
                        log = (Logger.JYLog)m_Queue.Dequeue();
                        if (!string.IsNullOrEmpty(log.Log))
                            baseLogger.Write(log);
                    }
                }
            }
            catch (Exception ex)
            {
                OnErrorCatch(ex.Message);
            }
            OnThreadCompleted();
        }

        private void baseThread_DebugInfo(string arg)
        {
            OnDebugInfo(arg);
        }

        private void baseThread_ErrorCatch(string arg)
        {
            OnErrorCatch(arg);
        }

        private void baseThread_AsyncThreadStateChanged(uint arg)
        {
            OnAsyncThreadStateChanged(arg);
        }

        private Queue m_Queue = new Queue();

        #region Properties

        /// <summary>
        /// 获取或设置日志文件存储路径
        /// </summary>
        [Description("获取或设置日志文件存储路径")]
        public string Log_Path
        {
            get { return baseLogger.Log_Path; }
            set { baseLogger.Log_Path = value; }
        }
        /// <summary>
        /// 获取或设置单个日志文件的最大大小，单位为M
        /// </summary>
        [Description("获取或设置单个日志文件的最大大小，单位为M")]
        public uint Max_File_Size
        {
            get { return baseLogger.Max_File_Size; }
            set { baseLogger.Max_File_Size = value; }
        }
        /// <summary>
        /// 当前异步线程状态
        /// </summary>
        [Description("当前异步线程状态")]
        public AsyncLib.Async_Thread_State Async_State
        {
            get { return baseThread.Async_State; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 启动异步线程
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            bool ret = false;
            try
            {
                ret = baseThread.Start(null);
            }
            catch (Exception ex)
            {
                OnErrorCatch(ex.Message);
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 停止异步线程
        /// </summary>
        public void Stop()
        {
            try
            {
                baseThread.Stop();
            }
            catch (Exception ex)
            {
                OnErrorCatch(ex.Message);
            }
        }

        /// <summary>
        /// 写入日志，默认日志级别(System)和提交时间(当前)
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <returns></returns>
        public bool Write(string log)
        {
            return Write(new Logger.JYLog(Logger.JY_Log_Level.System, log));
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="log">存储有日志内容、日志级别、提交时间</param>
        /// <returns></returns>
        public bool Write(Logger.JYLog log)
        {
            bool ret = false;
            try
            {
                if (Async_State == AsyncLib.Async_Thread_State.Running)
                {
                    lock (m_Queue.SyncRoot)
                        m_Queue.Enqueue(log);
                    ret = true;
                }
                else OnDebugInfo("日志线程尚未运行,请核查!");
            }
            catch (Exception ex)
            {
                OnErrorCatch(ex.Message);
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 写入日志，默认提交时间(当前)
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="log">日志内容</param>
        /// <returns></returns>
        public bool Write(Logger.JY_Log_Level level, string log)
        {
            return Write(new Logger.JYLog(level, log));
        }

        #endregion

        #region Error

        private JYHandlerString m_ErrorCatchEvent;
        /// <summary>
        /// 当运行时发生可捕捉的错误时触发
        /// </summary>
        [Description("当运行时发生可捕捉的错误时触发")]
        public event JYHandlerString ErrorCatch
        {
            add { m_ErrorCatchEvent += value; }
            remove { m_ErrorCatchEvent -= value; }
        }

        private void OnErrorCatch(string info)
        {
            if (m_ErrorCatchEvent != null)
                m_ErrorCatchEvent(info);
        }

        #endregion

        #region Debug

        private JYHandlerString m_DebugInfoEvent;
        /// <summary>
        /// 当运行时有调试信息时触发
        /// </summary>
        [Description("当运行时有调试信息时触发")]
        public event JYHandlerString DebugInfo
        {
            add { m_DebugInfoEvent += value; }
            remove { m_DebugInfoEvent -= value; }
        }

        private void OnDebugInfo(string info)
        {
            if (m_DebugInfoEvent != null)
                m_DebugInfoEvent(info);
        }

        #endregion

        #region AsyncThreadStateChanged

        private JYHandlerUInt m_AsyncStateChangedEvent;
        /// <summary>
        /// 当异步线程状态发生变化时触发
        /// </summary>
        [Description("当异步线程状态发生变化时触发")]
        public event JYHandlerUInt AsyncThreadStateChanged
        {
            add { m_AsyncStateChangedEvent += value; }
            remove { m_AsyncStateChangedEvent -= value; }
        }

        private void OnAsyncThreadStateChanged(uint state)
        {
            if (m_AsyncStateChangedEvent != null)
                m_AsyncStateChangedEvent(state);
        }

        #endregion

        #region ThreadStart

        private JYHandlerVoid m_ThreadStartEvent;
        /// <summary>
        /// 当异步线程开始时触发
        /// </summary>
        [Description("当异步线程开始时触发")]
        public event JYHandlerVoid ThreadStart
        {
            add { m_ThreadStartEvent += value; }
            remove { m_ThreadStartEvent -= value; }
        }

        private void OnThreadStart()
        {
            if (m_ThreadStartEvent != null)
                m_ThreadStartEvent();
        }

        #endregion

        #region ThreadCompleted

        private JYHandlerVoid m_ThreadCompletedEvent;
        /// <summary>
        /// 当异步线程完成时触发
        /// </summary>
        [Description("当异步线程完成时触发")]
        public event JYHandlerVoid ThreadCompleted
        {
            add { m_ThreadCompletedEvent += value; }
            remove { m_ThreadCompletedEvent -= value; }
        }

        private void OnThreadCompleted()
        {
            if (m_ThreadCompletedEvent != null)
                m_ThreadCompletedEvent();
        }

        #endregion
    }
}
