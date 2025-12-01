using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiYuInfo.Security
{
    /// <summary>
    /// 加解密组件
    /// </summary>
    public partial class Factory : Component
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public Factory()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="container"></param>
        public Factory(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private Security_Algorithm m_Algorithm = Security_Algorithm.RC4;
        /// <summary>
        /// 获取或设置加解密的算法
        /// </summary>
        [DefaultValue(Security_Algorithm.RC4)]
        [Description("获取或设置加解密的算法")]
        public Security_Algorithm Algorithm
        {
            get { return m_Algorithm; }
            set { m_Algorithm = value; OnAlgorithmChanged(); }
        }

        private void OnAlgorithmChanged()
        {
            switch (Algorithm)
            {
                case Security_Algorithm.RC4:
                    m_Encrypter = new RC4();
                    break;
                case Security_Algorithm.DES:
                    m_Encrypter = new DES();
                    break;
                case Security_Algorithm.TripleDES:
                    m_Encrypter = new TripleDES();
                    break;
                case Security_Algorithm.Rijndael:
                    m_Encrypter = new Rijndael();
                    break;
            }
        }

        private SecurityBase m_Encrypter = new RC4();

        private Bytes_Display_Mode m_Display_Mode = Bytes_Display_Mode.Base64;
        /// <summary>
        /// 获取或设置字节数组显示模式
        /// </summary>
        [DefaultValue(Bytes_Display_Mode.Base64)]
        [Description("获取或设置字节数组显示模式")]
        public Bytes_Display_Mode Display_Mode
        {
            get { return m_Display_Mode; }
            set { m_Display_Mode = value; }
        }

        private Encoding m_Encoder = Encoding.Unicode;
        /// <summary>
        /// 获取或设置字符串编码方式
        /// </summary>
        [Description("获取或设置字符串编码方式")]
        public Encoding Encoder
        {
            get { return m_Encoder; }
            set { m_Encoder = value; }
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="src">字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Encrypt(string src, string key)
        {
            if (string.IsNullOrEmpty(src)) return string.Empty;
            if (string.IsNullOrEmpty(key)) return m_Encrypter.Encrypt(src, Encoder, Display_Mode);
            else return m_Encrypter.Encrypt(src, key, Encoder, Display_Mode);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public string Decrypt(string src, string key)
        {
            if (string.IsNullOrEmpty(src)) return string.Empty;
            if (string.IsNullOrEmpty(key)) return m_Encrypter.Decrypt(src, Encoder, Display_Mode);
            else return m_Encrypter.Decrypt(src, key, Encoder, Display_Mode);
        }
    }
}
