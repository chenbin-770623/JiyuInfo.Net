using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiYuInfo.Configurator
{
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    /// <summary>
    /// 配置器的接口
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// 初始化配置器
        /// </summary>
        void Initialize();
        /// <summary>
        /// 读取配置信息
        /// </summary>
        void Read();
        /// <summary>
        /// 写入配置信息
        /// </summary>
        void Write();
        /// <summary>
        /// 显示配置界面
        /// </summary>
        void UIShow();
    }

    /// <summary>
    /// 配置类的基类
    /// </summary>
    public class FileConfigurator : IConfigurator
    {
        /// <summary>
        /// 初始化配置器
        /// </summary>
        public void Initialize()
        {
        }
        /// <summary>
        /// 读取配置信息
        /// </summary>
        public void Read()
        {
            foreach (string propertyName in Reflection.GetBrowseablePropertyNames(this))
            {
                if (Reflection.CheckPropertyReadonly(this, propertyName)) continue;
                PropertyInfo property = Reflection.GetProperty(this, propertyName);
                string strApp = GetAppName(propertyName);
                string strKey = GetKeyName(propertyName);
                string strValue = FileHandler.Read(strApp, strKey);
                Reflection.SetPropertyValue(this, propertyName, JYConverter.ConvertJsonToPropertyValue(this, propertyName, strValue));
            }
        }
        /// <summary>
        /// 写入配置信息
        /// </summary>
        public void Write()
        {
            foreach (string propertyName in Reflection.GetBrowseablePropertyNames(this))
            {
                if (Reflection.CheckPropertyReadonly(this, propertyName)) continue;
                PropertyInfo property = Reflection.GetProperty(this, propertyName);
                string strApp = GetAppName(propertyName);
                string strKey = GetKeyName(propertyName);
                object objValue = GetValue(propertyName);
                string strValue = JYConverter.ConvertPropertyValueToJson(this, propertyName, objValue);                
                FileHandler.Write(strApp, strKey, strValue);
            }
        }
        /// <summary>
        /// 显示配置界面
        /// </summary>
        public void UIShow()
        {
            frmConfigurator pDlg = new frmConfigurator(this);
            pDlg.ShowDialog();
        }

        private ReadWriteConfigFile m_FileHandler = new ReadWriteConfigFile();
        /// <summary>
        /// 配置文件操作句柄
        /// </summary>
        private ReadWriteConfigFile FileHandler
        {
            get { return m_FileHandler; }
            set { m_FileHandler = value; }
        }
        /// <summary>
        /// 获取或设置配置文件路径
        /// </summary>
        [Description("获取或设置配置文件路径")]
        [Browsable(false)]
        public string FilePath
        {
            get { return FileHandler.FilePath; }
            set { FileHandler.FilePath = value; }
        }

        private string m_DefaultAppName = "System";
        /// <summary>
        /// 默认App名称
        /// </summary>
        [Browsable(false)]
        private string DefaultAppName
        {
            get { return m_DefaultAppName; }
            set { m_DefaultAppName = value; }
        }

        private string GetAppName(string propertyName)
        {
            string ret = Reflection.GetPropertyCategory(this, propertyName);
            if (ret == "" || ret == CategoryAttribute.Default.Category) ret = DefaultAppName;
            return ret;
        }

        private string GetKeyName(string propertyName)
        {
            return propertyName;
        }

        private object GetValue(string propertyName)
        {
            return Reflection.GetPropertyValue(this, propertyName);
        }
    }

    internal sealed class ReadWriteConfigFile
    {
        #region ctor
        /// <summary>
        /// 创建类实例
        /// </summary>
        public ReadWriteConfigFile()
        {
            SetFilePath(DefaultFilePath);
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="file"></param>
        public ReadWriteConfigFile(string file)
        {
            SetFilePath(file);
        }
        #endregion

        #region Private Member

        private const string DefaultFilePath = @".\bin\config.ini";

        private const int AllocBufferSize = 256;

        private string m_FilePath = DefaultFilePath;
        /// <summary>
        /// 设置文件路径，如果文件路径不存在，则创建此文件路径
        /// </summary>
        private void SetFilePath(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            try
            {
                if (!fi.Directory.Exists) fi.Directory.Create();
                if (m_FilePath != filePath) m_FilePath = filePath;
            }
            catch { }
        }
        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置配置文件路径
        /// </summary>
        [Description("获取或设置配置文件路径")]
        public string FilePath
        {
            get { return m_FilePath; }
            set { SetFilePath(value); }
        }
        #endregion

        #region Method
        /// <summary>
        /// 写入配置信息
        /// </summary>
        public bool Write(string appName, string keyName, string value)
        {
            return (WinApi.WritePrivateProfileString(appName, keyName, value, FilePath) != 0);
        }
        /// <summary>
        /// 读取配置信息，如果读取失败，返回传入的缺省值
        /// </summary>
        public string Read(string appName, string keyName, string strDefault)
        {
            int nSetBufferSize = 0;
            byte[] pbtBuffer = new byte[nSetBufferSize];
            int nRead = 0;
            do
            {
                nSetBufferSize += AllocBufferSize;
                pbtBuffer = new byte[nSetBufferSize];
                nRead = WinApi.GetPrivateProfileString(appName, keyName, strDefault, pbtBuffer, nSetBufferSize, FilePath);
            }
            while (nRead == nSetBufferSize - 1 || nRead == nSetBufferSize - 2);
            string strValue = Encoding.Default.GetString(pbtBuffer, 0, nRead);
            return strValue;
        }
        /// <summary>
        /// 读取配置信息，如果读取失败，返回""
        /// </summary>
        public string Read(string appName, string keyName)
        {
            return Read(appName, keyName, "");
        }
        #endregion

        #region Static Member
        private static ReadWriteConfigFile m_DefaultInstance = new ReadWriteConfigFile();
        /// <summary>
        /// 获取缺省路径的类实例
        /// </summary>
        [Browsable(false)]
        public static ReadWriteConfigFile Default
        {
            get { return m_DefaultInstance; }
        }
        #endregion
    }

}
