using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiYuInfo.Logger
{
    using System.IO;

    /// <summary>
    /// 日志文件记录器
    /// </summary>
    public partial class FileLogger : Component , ILogger
    {
        #region ctor
        /// <summary>
        /// 创建类实例
        /// </summary>
        public FileLogger()
        {
            InitializeComponent();
            
            Log_Path = Default_Log_Path;
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="path"></param>
        public FileLogger(string path)
        {
            InitializeComponent();

            Log_Path = path;
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="container"></param>
        public FileLogger(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Log_Path = Default_Log_Path;
        }

        #endregion

        #region Members

        private const string Default_Log_Path = @".\log";
        private const uint Default_Max_File_Size = 2;

        private string m_Log_Path = Default_Log_Path;

        private uint m_Max_File_Size = Default_Max_File_Size;

        private void SetLogPath(string path)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                if (path != m_Log_Path) m_Log_Path = path;
            }
            catch 
            {
                if (!Directory.Exists(Default_Log_Path)) Directory.CreateDirectory(Default_Log_Path);
                m_Log_Path = Default_Log_Path; 
            }
        }

        private uint GetMaxFileSize()
        {
            return Max_File_Size * 1024 * 1024;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置日志文件存储路径
        /// </summary>
        [DefaultValue(Default_Log_Path)]
        [Description("获取或设置日志文件存储路径")]
        public string Log_Path
        {
            get { return m_Log_Path; }
            set { m_Log_Path = value; SetLogPath(m_Log_Path); }
        }
        /// <summary>
        /// 获取或设置单个日志文件的最大大小，单位为M
        /// </summary>
        [DefaultValue(Default_Max_File_Size)]
        [Description("获取或设置单个日志文件的最大大小，单位为M")]
        public uint Max_File_Size
        {
            get { return m_Max_File_Size; }
            set { m_Max_File_Size = value; if (m_Max_File_Size > 100) m_Max_File_Size = 100; if (m_Max_File_Size == 0) m_Max_File_Size = Default_Max_File_Size; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 写入日志，默认日志级别(System)和提交时间(当前)
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <returns></returns>
        public bool Write(string log)
        {
            return Write(new JYLog(JY_Log_Level.System, log));
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="log">存储有日志内容、日志级别、提交时间</param>
        /// <returns></returns>
        public bool Write(JYLog log)
        {
            string fileName = log.Commit_Time.ToString("yyyyMMdd");
            string fullFileName = string.Format("{0}\\{1}.log", Log_Path, fileName);
            System.IO.FileInfo fi = new FileInfo(fullFileName);
            try
            {
                if (fi.Exists)
                {
                    if (fi.Length > GetMaxFileSize())
                    {
                        for (int i = 0; i < int.MaxValue; i++)
                        {
                            string strNewFileName = string.Format("{0}\\{1}-{2}.log", Log_Path, fileName, i);
                            if (!System.IO.File.Exists(strNewFileName))
                            {
                                fi.MoveTo(strNewFileName);
                                break;
                            }
                        }
                    }
                }
                string logLine = log.ToString();
                using (FileStream fileHandler = File.Open(fullFileName, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    byte[] line = Encoding.Default.GetBytes(logLine);
                    fileHandler.Write(line, 0, line.Length);
                    fileHandler.Flush();
                }
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 写入日志，默认提交时间(当前)
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="log">日志内容</param>
        /// <returns></returns>
        public bool Write(JY_Log_Level level, string log)
        {
            return Write(new JYLog(level, log));
        }

        #endregion
    }
    /// <summary>
    /// 记录日志的接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 写入日志，默认日志级别和提交时间
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <returns></returns>
        bool Write(string log);
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="log">存储有日志内容、日志级别、提交时间</param>
        /// <returns></returns>
        bool Write(JYLog log);
        /// <summary>
        /// 写入日志，默认提交时间
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="log">日志内容</param>
        /// <returns></returns>
        bool Write(JY_Log_Level level, string log);
    }
    /// <summary>
    /// 日志级别
    /// </summary>
    [Flags]
    public enum JY_Log_Level : uint
    {
        /// <summary>
        /// 错误信息，必须记录
        /// </summary>
        Errors = 0x00,
        /// <summary>
        /// 系统调试信息
        /// </summary>
        System = 0x01,
        /// <summary>
        /// 明细信息
        /// </summary>
        Detail = 0x02,
    }
    /// <summary>
    /// 用于存储日志信息的结构
    /// </summary>
    public struct JYLog
    {
        /// <summary>
        /// 实例化结构体
        /// </summary>
        /// <param name="commitTime">提交时间</param>
        /// <param name="level">日志级别</param>
        /// <param name="log">日志</param>
        public JYLog(DateTime commitTime, JY_Log_Level level, string log)
        {
            m_Commit_Time = commitTime;
            m_Level = level;
            m_Log = log;
        }
        /// <summary>
        /// 实例化结构体，默认提交时间为当前
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="log">日志</param>
        public JYLog(JY_Log_Level level, string log)
            : this(DateTime.Now, level, log)
        {
        }

        private DateTime m_Commit_Time;
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime Commit_Time
        {
            get { return m_Commit_Time; }
            set { m_Commit_Time = value; }
        }

        private JY_Log_Level m_Level;
        /// <summary>
        /// 日志级别
        /// </summary>
        public JY_Log_Level Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        private string m_Log;
        /// <summary>
        /// 日志
        /// </summary>
        public string Log
        {
            get { return m_Log; }
            set { m_Log = value; }
        }

        /// <summary>
        /// 已重载。序列化日志内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}]\t[{1}]\t{2}\r\n", Commit_Time.ToString("HH:mm:ss.fff"), Level, Log);
        }
    }
}
