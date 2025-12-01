using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiYuInfo.AsyncLib
{
    using System.Threading;
    /// <summary>
    /// 异步线程状态
    /// </summary>
    public enum Async_Thread_State : uint
    {
        /// <summary>
        /// 已停止
        /// </summary>
        Stopped = 0x00,
        /// <summary>
        /// 准备停止
        /// </summary>
        Stopping = 0x01,
        /// <summary>
        /// 运行中
        /// </summary>
        Running = 0x02,
    }
    /// <summary>
    /// 异步线程模式
    /// </summary>
    [Flags]
    public enum Async_Thread_Mode : uint
    {
        /// <summary>
        /// 循环标记
        /// </summary>
        Loop = 0x01,
        /// <summary>
        /// 按次数标记
        /// </summary>
        Times = 0x02,
        /// <summary>
        /// 按时间间隔标记
        /// </summary>
        Interval = 0x04,

        /// <summary>
        /// 循环按次数
        /// </summary>
        LoopWithTimes = Loop | Times,
        /// <summary>
        /// 循环按间隔
        /// </summary>
        LoopWithInterval = Loop | Interval,
        /// <summary>
        /// 循环按次数且带间隔
        /// </summary>
        LoopWithTimesAndInterval = Loop | Times | Interval,
    }
    /// <summary>
    /// 线程完成原因
    /// </summary>
    public enum Async_Thread_Completed_Reason : uint
    {
        /// <summary>
        /// 永不结束
        /// </summary>
        Neverend = 0x00,
        /// <summary>
        /// 手动取消
        /// </summary>
        Cancelled = 0x01,
        /// <summary>
        /// 自动完成
        /// </summary>
        Completed = 0x02,
    }

    /// <summary>
    /// 异步线程类
    /// </summary>
    [DefaultProperty("Async_Mode")]
    [DefaultEvent("Task")]
    public partial class AsyncThread : Component
    {
        #region ctor
        /// <summary>
        /// 创建类实例
        /// </summary>
        public AsyncThread()
        {
            InitializeComponent();

            InitPostEventHandler();
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="container"></param>
        public AsyncThread(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            InitPostEventHandler();
        }

        #endregion

        #region Members

        private Guid m_Thread_ID = new Guid();

        private Async_Thread_Mode m_Async_Mode = Async_Thread_Mode.LoopWithInterval;

        private Async_Thread_State m_Async_State = Async_Thread_State.Stopped;

        private Async_Thread_Completed_Reason m_Completed_Reason = Async_Thread_Completed_Reason.Neverend;

        private uint m_Max_Task_Times = 0;

        private uint m_Task_Interval = 0;

        private ulong m_Times_Summary = 0;

        private System.Collections.Specialized.HybridDictionary m_AsyncHandlerLife = new System.Collections.Specialized.HybridDictionary();

        private bool CheckIsCancelled()
        {
            return m_AsyncHandlerLife[Thread_ID] == null;
        }

        private void OnAsyncThread(object arg)
        {
            Async_State = Async_Thread_State.Running;
            try
            {
                Times_Summary = 0;
                FireThreadStart(arg);
                while (true)
                {
                    if (CheckIsCancelled()) { Completed_Reason = Async_Thread_Completed_Reason.Cancelled; break; }
                    FireTaskBegin(Times_Summary,arg);
                    Times_Summary++;
                    FireTask(Times_Summary,arg);                    
                    FireTaskEnd(Times_Summary);
                    if ((Async_Mode & Async_Thread_Mode.Times) == Async_Thread_Mode.Times)
                    {
                        if (Max_Task_Times > 0 && Times_Summary >= Max_Task_Times) { Completed_Reason = Async_Thread_Completed_Reason.Completed; break; }
                    }
                    if ((Async_Mode & Async_Thread_Mode.Interval) == Async_Thread_Mode.Interval)
                    {
                        if (Task_Interval > 0)
                            Waiting(Task_Interval, CheckIsCancelled);
                            // System.Threading.Thread.Sleep((int)Task_Interval);
                    }
                    System.Windows.Forms.Application.DoEvents();
                    if (CheckIsCancelled()) { Completed_Reason = Async_Thread_Completed_Reason.Cancelled; break; }
                }
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
            Async_State = Async_Thread_State.Stopped;
            lock (m_AsyncHandlerLife.SyncRoot)
            {
                if (m_AsyncHandlerLife.Contains(Thread_ID))
                    m_AsyncHandlerLife.Remove(Thread_ID);
            }
            FireThreadCompleted(Completed_Reason);
        }

        private delegate bool CheckBreakFlagDelegate();

        private void Waiting(uint interval, CheckBreakFlagDelegate breakFlag )
        {
            DateTime dtStart = DateTime.Now;
            while(!breakFlag())
            {
                DateTime dtCurrent = DateTime.Now;
                TimeSpan ts = dtCurrent - dtStart;
                if (ts.TotalMilliseconds >= interval) break;                
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// 异步线程标识
        /// </summary>
        [Description("异步线程标识")]
        public Guid Thread_ID
        {
            get { return m_Thread_ID; }
            private set { m_Thread_ID = value; }
        }
        /// <summary>
        /// 异步线程模式
        /// </summary>
        [DefaultValue(Async_Thread_Mode.LoopWithInterval)]
        [Description("异步线程模式")]
        public Async_Thread_Mode Async_Mode
        {
            get { return m_Async_Mode; }
            set { m_Async_Mode = value; }
        }
        /// <summary>
        /// 当前异步线程状态
        /// </summary>
        [Description("当前异步线程状态")]
        public Async_Thread_State Async_State
        {
            get { return m_Async_State; }
            private set { if (m_Async_State != value) { m_Async_State = value; FireAsyncThreadStateChanged(m_Async_State); } }
        }
        /// <summary>
        /// 当前异步线程完成原因
        /// </summary>
        [Description("当前异步线程完成原因")]
        public Async_Thread_Completed_Reason Completed_Reason
        {
            get { return m_Completed_Reason; }
            private set { m_Completed_Reason = value; }
        }
        /// <summary>
        /// 异步线程最大循环次数，0表示无限制
        /// </summary>
        [DefaultValue(0)]
        [Description("异步线程最大循环次数，0表示无限制")]
        public uint Max_Task_Times
        {
            get { return m_Max_Task_Times; }
            set { m_Max_Task_Times = value; }
        }
        /// <summary>
        /// 异步线程循环时间间隔，单位:ms，0表示无间隔
        /// </summary>
        [DefaultValue(1000)]
        [Description("异步线程循环时间间隔，单位:ms，0表示无间隔")]
        public uint Task_Interval
        {
            get { return m_Task_Interval; }
            set { m_Task_Interval = value; }
        }
        /// <summary>
        /// 异步线程已循环次数
        /// </summary>
        [Description("异步线程已循环次数")]
        public ulong Times_Summary
        {
            get { return m_Times_Summary; }
            private set { m_Times_Summary = value; if (m_Times_Summary == ulong.MaxValue - 1) m_Times_Summary = 0; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 启动异步线程
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool Start(object arg)
        {
            bool ret = false;
            InitAsyncHandler();
            try
            {
                lock (m_AsyncHandlerLife.SyncRoot)
                {
                    if (m_AsyncHandlerLife.Contains(Thread_ID))
                    {
                        FireDebugInfo("异步线程已经在运行,请核查!");
                        ret = false;
                    }
                    else
                    {
                        m_AsyncHandlerLife[Thread_ID] = Thread_ID;
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
                ret = false;
            }
            if (ret)
            {
                try
                {
                    ret = System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(OnAsyncThread), arg);
                }
                catch (Exception ex)
                {
                    FireErrorCatch(ex.Message);
                }
            }
            return ret;
        }
        /// <summary>
        /// 停止异步线程
        /// </summary>
        public void Stop()
        {
            if (m_AsyncHandler == null) return;
            try
            {
                lock (m_AsyncHandlerLife.SyncRoot)
                {
                    if (m_AsyncHandlerLife.Contains(Thread_ID))
                    {
                        Async_State = Async_Thread_State.Stopping;
                        m_AsyncHandlerLife.Remove(Thread_ID);
                    }
                    else FireDebugInfo("异步线程尚未运行,请核查!");
                }
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }
        /// <summary>
        /// 在派生类中进行处理同步回调问题
        /// </summary>
        /// <param name="callback">回调</param>
        /// <param name="arg">回调参数</param>
        protected void FireTaskCallback(SendOrPostCallback callback, object arg)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.Post(callback, arg);
        }

        #endregion

        #region Events

        private AsyncOperation m_AsyncHandler = null;

        private void InitAsyncHandler()
        {
            if (m_AsyncHandler == null) 
                m_AsyncHandler = AsyncOperationManager.CreateOperation(Thread_ID);
        }

        private void InitPostEventHandler()
        {
            m_PostErrorCatch = new SendOrPostCallback(OnPostErrorCatch);
            m_PostDebugInfo = new SendOrPostCallback(OnPostDebugInfo);
            m_PostAsyncThreadStateChanged = new SendOrPostCallback(OnPostAsyncStateChanged);
            m_PostThreadStart = new SendOrPostCallback(OnPostThreadStart);
            m_PostThreadCompleted = new SendOrPostCallback(OnPostThreadCompleted);
            m_PostTaskBegin = new SendOrPostCallback(OnPostTaskBegin);
            m_PostTaskEnd = new SendOrPostCallback(OnPostTaskEnd);
            m_PostTask = new SendOrPostCallback(OnPostTask);
        }
        /// <summary>
        /// 定义事件委托，带ulong,object参数
        /// </summary>
        public delegate void JYHandlerULongObject(ulong arg, object obj);

        private struct _Combin_ULong_Object
        {
            /// <summary>
            /// ulong参数
            /// </summary>
            public ulong _arg;
            /// <summary>
            /// object参数
            /// </summary>
            public object _obj;
            /// <summary>
            /// 创建实例
            /// </summary>
            public _Combin_ULong_Object(ulong arg, object obj)
            {
                _arg = arg;
                _obj = obj;
            }
        }

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

        private SendOrPostCallback m_PostErrorCatch;

        private void OnPostErrorCatch(object arg)
        {
            OnErrorCatch((string)arg);
        }

        private void FireErrorCatch(string info)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.Post(m_PostErrorCatch, info);
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

        private SendOrPostCallback m_PostDebugInfo;

        private void OnPostDebugInfo(object arg)
        {
            OnDebugInfo((string)arg);
        }

        private void FireDebugInfo(string info)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.Post(m_PostDebugInfo, info);
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
        
        private void OnAsyncThreadStateChanged(Async_Thread_State state)
        {
            if (m_AsyncStateChangedEvent != null)
                m_AsyncStateChangedEvent((uint)state);
        }
        
        private SendOrPostCallback m_PostAsyncThreadStateChanged;
        
        private void OnPostAsyncStateChanged(object arg)
        {
            try
            {
                OnAsyncThreadStateChanged((Async_Thread_State)arg);
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }
        
        private void FireAsyncThreadStateChanged(Async_Thread_State state)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.Post(m_PostAsyncThreadStateChanged, state);
        }

        #endregion

        #region TaskBegin

        private JYHandlerULongObject m_TaskBeginEvent;
        /// <summary>
        /// 当任务启动时触发
        /// </summary>
        [Description("当任务启动时触发")]
        public event JYHandlerULongObject TaskBegin
        {
            add { m_TaskBeginEvent += value; }
            remove { m_TaskBeginEvent -= value; }
        }

        private void OnTaskBegin(ulong arg, object obj)
        {
            if (m_TaskBeginEvent != null)
                m_TaskBeginEvent(arg,obj);
        }

        private SendOrPostCallback m_PostTaskBegin;

        private void OnPostTaskBegin(object arg)
        {
            try
            {
                _Combin_ULong_Object obj = (_Combin_ULong_Object)arg;
                OnTaskBegin(obj._arg,obj._obj);
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }

        private void FireTaskBegin(ulong arg,object obj)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.SynchronizationContext.Send(m_PostTaskBegin, new _Combin_ULong_Object(arg, obj));
        }

        #endregion

        #region TaskEnd

        private JYHandlerULong m_TaskEndEvent;
        /// <summary>
        /// 当任务结束时触发
        /// </summary>
        [Description("当任务结束时触发")]
        public event JYHandlerULong TaskEnd
        {
            add { m_TaskEndEvent += value; }
            remove { m_TaskEndEvent -= value; }
        }

        private void OnTaskEnd(ulong arg)
        {
            if (m_TaskEndEvent != null)
                m_TaskEndEvent(arg);
        }

        private SendOrPostCallback m_PostTaskEnd;

        private void OnPostTaskEnd(object arg)
        {
            try
            {
                OnTaskEnd((ulong)arg);
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }

        private void FireTaskEnd(ulong arg)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.Post(m_PostTaskEnd, arg);
        }

        #endregion

        #region Task

        private JYHandlerULongObject m_TaskEvent;
        /// <summary>
        /// 当任务执行时触发
        /// </summary>
        [Description("当任务执行时触发")]
        public event JYHandlerULongObject Task
        {
            add { m_TaskEvent += value; }
            remove { m_TaskEvent -= value; }
        }

        private void OnTask(ulong arg,object obj)
        {
            if (m_TaskEvent != null)
                m_TaskEvent(arg, obj);
        }

        private SendOrPostCallback m_PostTask;

        private void OnPostTask(object arg)
        {
            try
            {
                _Combin_ULong_Object obj = (_Combin_ULong_Object)arg;
                OnTask(obj._arg,obj._obj);
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }

        private void FireTask(ulong arg,object obj)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.SynchronizationContext.Send(m_PostTask, new _Combin_ULong_Object(arg, obj));
        }

        #endregion

        #region ThreadStart

        private JYHandlerObject m_ThreadStartEvent;
        /// <summary>
        /// 当异步线程开始时触发
        /// </summary>
        [Description("当异步线程开始时触发")]
        public event JYHandlerObject ThreadStart
        {
            add { m_ThreadStartEvent += value; }
            remove { m_ThreadStartEvent -= value; }
        }

        private void OnThreadStart(object arg)
        {
            if (m_ThreadStartEvent != null)
                m_ThreadStartEvent(arg);
        }

        private SendOrPostCallback m_PostThreadStart;

        private void OnPostThreadStart(object arg)
        {
            try
            {
                OnThreadStart(arg);
            }
            catch (Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }

        private void FireThreadStart(object arg)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.Post(m_PostThreadStart, arg);
        }

        #endregion

        #region ThreadCompleted

        private JYHandlerUInt m_ThreadCompletedEvent;
        /// <summary>
        /// 当异步线程完成时触发
        /// </summary>
        [Description("当异步线程完成时触发")]
        public event JYHandlerUInt ThreadCompleted
        {
            add { m_ThreadCompletedEvent += value; }
            remove { m_ThreadCompletedEvent -= value; }
        }

        private void OnThreadCompleted(Async_Thread_Completed_Reason reason)
        {
            if (m_ThreadCompletedEvent != null)
                m_ThreadCompletedEvent((uint)reason);
        }

        private SendOrPostCallback m_PostThreadCompleted;

        private void OnPostThreadCompleted(object arg)
        {
            try
            {
                OnThreadCompleted((Async_Thread_Completed_Reason)arg);
            }
            catch(Exception ex)
            {
                FireErrorCatch(ex.Message);
            }
        }

        private void FireThreadCompleted(Async_Thread_Completed_Reason reason)
        {
            if (m_AsyncHandler != null)
                m_AsyncHandler.SynchronizationContext.Send(m_PostThreadCompleted, reason);
        }

        #endregion

        #endregion
    }
}
