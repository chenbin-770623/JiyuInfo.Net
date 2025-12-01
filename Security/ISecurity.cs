using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JiYuInfo.Security
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities.Encoders;
    using System.Text;
    /// <summary>
    /// 加解密接口
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// 用指定密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <param name="key">密码</param>
        /// <returns>加密字节流</returns>
        byte[] Encrypt(byte[] src, string key);
        /// <summary>
        /// 用指定密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <param name="key">密码</param>
        /// <returns>字节流</returns>
        byte[] Decrypt(byte[] src, string key);
    }
    /// <summary>
    /// 加解密基类
    /// </summary>
    public abstract class SecurityBase : IDisposable, ISecurity
    {
        /// <summary>
        /// 私有的公共Key
        /// </summary>
        private const string JY_Key = "密码字符串:#Pwd_13817632002@JiyuInfo.Sh.Cn#上海极昱信息科技有限公司,陈斌.";

        private string GetConstKey()
        {
            char[] czKey = JY_Key.ToCharArray();
            Array.Reverse(czKey);
            return new string(czKey);
        }
        /// <summary>
        /// 获取key的字节数组
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual byte[] GetKeyByte(string key)
        {
            byte[] bzKey = KeyEncoder.GetBytes(key);
            Array.Reverse(bzKey);
            return bzKey;
        }
        /// <summary>
        /// 密码长度
        /// </summary>
        protected abstract uint KeyLen { get; }
        /// <summary>
        /// 向量长度
        /// </summary>
        protected abstract uint IvLen { get; }
        /// <summary>
        /// 密码的编码器
        /// </summary>
        protected virtual Encoding KeyEncoder { get { return Encoding.UTF32; } }
        /// <summary>
        /// 对指定的密码获取密码和向量
        /// </summary>
        /// <param name="key">密码字符串</param>
        /// <param name="bzKey">密码字节流</param>
        /// <param name="bzIv">向量字节流</param>
        protected abstract void PrepareKey(string key, out byte[] bzKey, out byte[] bzIv);
        /// <summary>
        /// 用指定密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <param name="key">密码</param>
        /// <returns>加密字节流</returns>
        public abstract byte[] Encrypt(byte[] src, string key);
        /// <summary>
        /// 用公共密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <returns>加密字节流</returns>
        public byte[] Encrypt(byte[] src)
        {
            return Encrypt(src, GetConstKey());
        }
        /// <summary>
        /// 加密字符串。
        /// 把字符串用指定的编码转成字节数组后用指定的密码进行加密，得到的加密字节流用指定的字节流显示模式转成字符串
        /// </summary>
        /// <param name="src">字符串</param>
        /// <param name="key">密码</param>
        /// <param name="encoder">字符串编码</param>
        /// <param name="mode">字节流显示模式</param>
        /// <returns></returns>
        public string Encrypt(string src, string key, Encoding encoder, Bytes_Display_Mode mode)
        {
            byte[] bzSrc = encoder.GetBytes(src);
            byte[] bzEncrypt = Encrypt(bzSrc, key);
            return Convert(bzEncrypt, mode);
        }
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="src">字符串</param>
        /// <param name="encoder">字符串编码</param>
        /// <param name="mode">字节流显示模式</param>
        /// <returns></returns>
        public string Encrypt(string src, Encoding encoder, Bytes_Display_Mode mode)
        {
            return Encrypt(src, GetConstKey(), encoder, mode);
        }
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="src">字符串</param>
        /// <param name="encoder">字符串编码</param>
        /// <returns></returns>
        public string Encrypt(string src, Encoding encoder)
        {
            return Encrypt(src, GetConstKey(), encoder, Bytes_Display_Mode.Base64);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="src">字符串</param>
        /// <param name="mode">字节流显示模式</param>
        /// <returns></returns>
        public string Encrypt(string src, Bytes_Display_Mode mode)
        {
            return Encrypt(src, GetConstKey(), Encoding.Unicode, mode);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="src">字符串</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public string Encrypt(string src, string key)
        {
            return Encrypt(src, key, Encoding.Unicode, Bytes_Display_Mode.Base64);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="src">字符串</param>
        /// <returns></returns>
        public string Encrypt(string src)
        {
            return Encrypt(src, GetConstKey(), Encoding.Unicode, Bytes_Display_Mode.Base64);
        }
        /// <summary>
        /// 用指定密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <param name="key">密码</param>
        /// <returns>字节流</returns>
        public abstract byte[] Decrypt(byte[] src, string key);
        /// <summary>
        /// 用公共密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <returns>字节流</returns>
        public byte[] Decrypt(byte[] src)
        {
            return Decrypt(src, GetConstKey());
        }
        /// <summary>
        /// 解密字符串
        /// 把按指定字节数组显示的字符串转成的字节数组用指定的密码解密后，再用指定的编码转成字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <param name="key">密码</param>
        /// <param name="encoder">字符串编码</param>
        /// <param name="mode">字节数组显示模式</param>
        /// <returns></returns>
        public string Decrypt(string src, string key, Encoding encoder, Bytes_Display_Mode mode)
        {
            byte[] bzSrc = Convert(src, mode);
            byte[] bzDecrypt = Decrypt(bzSrc, key);
            return encoder.GetString(bzDecrypt);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <param name="encoder">字符串编码</param>
        /// <param name="mode">字节数组显示模式</param>
        /// <returns></returns>
        public string Decrypt(string src, Encoding encoder, Bytes_Display_Mode mode)
        {
            return Decrypt(src, GetConstKey(), encoder, mode);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <param name="encoder">字符串编码</param>
        /// <returns></returns>
        public string Decrypt(string src, Encoding encoder)
        {
            return Decrypt(src, GetConstKey(), encoder, Bytes_Display_Mode.Base64);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <param name="mode">字节数组显示模式</param>
        /// <returns></returns>
        public string Decrypt(string src, Bytes_Display_Mode mode)
        {
            return Decrypt(src, GetConstKey(), Encoding.Unicode, mode);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public string Decrypt(string src, string key)
        {
            return Decrypt(src, key, Encoding.Unicode, Bytes_Display_Mode.Base64);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src">加密字符串</param>
        /// <returns></returns>
        public string Decrypt(string src)
        {
            return Decrypt(src, GetConstKey(), Encoding.Unicode, Bytes_Display_Mode.Base64);
        }
        /// <summary>
        /// 把字节数组按指定的字节流显示模式转成字符串
        /// </summary>
        /// <param name="src">字节数组</param>
        /// <param name="mode">显示模式</param>
        /// <returns></returns>
        public static string Convert(byte[] src, Bytes_Display_Mode mode)
        {
            string ret = string.Empty;
            switch (mode)
            {
                case Bytes_Display_Mode.Base64:
                    {
                        ret = BytesToBase64(src);
                        break;
                    }
                case Bytes_Display_Mode.Hex:
                    {
                        ret = BytesToHex(src);
                        break;
                    }
                case Bytes_Display_Mode.Utf8:
                    {
                        ret = Encoding.UTF8.GetString(src);
                        break;
                    }
            }
            return ret;
        }
        /// <summary>
        /// 把按指定的字节流显示模式的字符串转成字节数组
        /// </summary>
        /// <param name="src">按指定的字节流显示模式的字符串</param>
        /// <param name="mode">显示模式</param>
        /// <returns></returns>
        public static byte[] Convert(string src, Bytes_Display_Mode mode)
        {
            byte[] ret = new byte[0];
            switch (mode)
            {
                case Bytes_Display_Mode.Base64:
                    {
                        ret = Base64ToBytes(src);
                        break;
                    }
                case Bytes_Display_Mode.Hex:
                    {
                        ret = HexToBytes(src);
                        break;
                    }
                case Bytes_Display_Mode.Utf8:
                    {
                        ret = Encoding.UTF8.GetBytes(src);
                        break;
                    }
            }
            return ret;
        }
        /// <summary>
        /// 把字节数组转成base64显示的字符串
        /// </summary>
        /// <param name="src">字节数组</param>
        /// <returns></returns>
        public static string BytesToBase64(byte[] src)
        {
            return System.Convert.ToBase64String(src);
        }
        /// <summary>
        /// 把base64显示的字符串转成字节数组
        /// </summary>
        /// <param name="src">base64显示的字符串</param>
        /// <returns></returns>
        public static byte[] Base64ToBytes(string src)
        {
            return System.Convert.FromBase64String(src);
        }
        /// <summary>
        /// 把字节数组转成 hex 显示的字符串
        /// </summary>
        /// <param name="src">字节数组</param>
        /// <returns></returns>
        public static string BytesToHex(byte[] src)
        {
            string ret = "";
            for (int i = 0; i < src.Length; i++)
            {
                string strByte = string.Format("{0:X2} ", src[i]);
                ret += strByte;
            }
            return ret;
        }
        /// <summary>
        /// 把 hex 显示的字符串转成字节数组
        /// </summary>
        /// <param name="src">hex 显示的字符串</param>
        /// <returns></returns>
        public static byte[] HexToBytes(string src)
        {
            string strSrc = src.Replace(" ", "");
            int len = strSrc.Length / 2;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
            {
                ret[i] = 0;
                try
                {
                    string strByte = strSrc.Substring(i * 2, 2);
                    ret[i] = System.Convert.ToByte(strByte, 16);
                }
                catch { ret[i] = 0; }
            }
            return ret;
        }
        /// <summary>
        /// 交换两个字节
        /// </summary>
        public static void Swap(ref byte a, ref byte b)
        {
            a ^= b;
            b ^= a;
            a ^= b;
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        protected abstract void Clear();
    }
    /// <summary>
    /// 字节数组显示模式
    /// </summary>
    public enum Bytes_Display_Mode : uint
    {
        /// <summary>
        /// base64
        /// </summary>
        Base64 = 0x01,
        /// <summary>
        /// 16进制字符串
        /// </summary>
        Hex = 0x02,
        /// <summary>
        /// utf8
        /// </summary>
        Utf8 = 0x03,
    }
    
    /// <summary>
    /// 用DES算法加解密
    /// </summary>
    public sealed class DES : SecurityBase
    {
        private const uint DES_LEN = 0x08;

        private DESCryptoServiceProvider m_Handler = new DESCryptoServiceProvider();
        /// <summary>
        /// 密码长度
        /// </summary>
        protected override uint KeyLen
        {
            get { return DES_LEN; }
        }
        /// <summary>
        /// 向量长度
        /// </summary>
        protected override uint IvLen
        {
            get { return DES_LEN; }
        }
        /// <summary>
        /// 对指定的密码获取密码和向量
        /// </summary>
        /// <param name="key">密码字符串</param>
        /// <param name="bzKey">密码字节流</param>
        /// <param name="bzIv">向量字节流</param>
        protected override void PrepareKey(string key, out byte[] bzKey, out byte[] bzIv)
        {
            bzKey = new byte[KeyLen];
            bzIv = new byte[IvLen];
            for (int i = 0; i < KeyLen; i++) bzKey[i] = System.Convert.ToByte(i & 0xFF);
            for (int i = 0; i < IvLen; i++) bzIv[i] = System.Convert.ToByte(i & 0xFF);
            byte[] bzSrcKey = GetKeyByte(key);
            if (bzSrcKey.Length >= KeyLen) Array.Copy(bzSrcKey, 0, bzKey, 0, KeyLen);
            else Array.Copy(bzSrcKey, 0, bzKey, 0, bzSrcKey.Length);
            if (bzSrcKey.Length > IvLen) Array.Copy(bzSrcKey, bzSrcKey.Length - IvLen, bzIv, 0, IvLen);
            else Array.Copy(bzSrcKey, 0, bzIv, 0, bzSrcKey.Length);
            Array.Reverse(bzIv);
        }
        /// <summary>
        /// 用指定密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <param name="key">密码</param>
        /// <returns>加密字节流</returns>
        public override byte[] Encrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            ICryptoTransform encryptor = m_Handler.CreateEncryptor(bzKey, bzIv);
            MemoryStream msBuffer = new MemoryStream();
            CryptoStream csStream = new CryptoStream(msBuffer, encryptor, CryptoStreamMode.Write);
            csStream.Write(src, 0, src.Length);
            csStream.FlushFinalBlock();
            csStream.Close();
            byte[] ret = msBuffer.ToArray();
            msBuffer.Close();
            return ret;
        }
        /// <summary>
        /// 用指定密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <param name="key">密码</param>
        /// <returns>字节流</returns>
        public override byte[] Decrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            ICryptoTransform decryptor = m_Handler.CreateDecryptor(bzKey, bzIv);
            MemoryStream msBuffer = new MemoryStream(src);
            CryptoStream csStream = new CryptoStream(msBuffer, decryptor, CryptoStreamMode.Read);
            byte[] bzRead = new byte[msBuffer.Length];
            int nRead = csStream.Read(bzRead, 0, bzRead.Length);
            csStream.Close();
            msBuffer.Close();
            byte[] ret = new byte[nRead];
            Array.Copy(bzRead, 0, ret, 0, nRead);
            return ret;
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        protected override void Clear()
        {
        }
        /// <summary>
        /// 获取DES的实例
        /// </summary>
        public static DES Instance
        {
            get { return new DES(); }
        }
    }
    /// <summary>
    /// 用Rijndael算法加解密
    /// </summary>
    public sealed class Rijndael : SecurityBase
    {
        private RijndaelManaged m_Handler = new RijndaelManaged();
        /// <summary>
        /// 密码长度
        /// </summary>
        protected override uint KeyLen
        {
            get { return 32; }
        }
        /// <summary>
        /// 向量长度
        /// </summary>
        protected override uint IvLen
        {
            get { return 16; }
        }
        /// <summary>
        /// 对指定的密码获取密码和向量
        /// </summary>
        /// <param name="key">密码字符串</param>
        /// <param name="bzKey">密码字节流</param>
        /// <param name="bzIv">向量字节流</param>
        protected override void PrepareKey(string key, out byte[] bzKey, out byte[] bzIv)
        {
            bzKey = new byte[KeyLen];
            bzIv = new byte[IvLen];
            for (int i = 0; i < KeyLen; i++) bzKey[i] = System.Convert.ToByte(i & 0xFF);
            for (int i = 0; i < IvLen; i++) bzIv[i] = System.Convert.ToByte(i & 0xFF);
            byte[] bzSrcKey = GetKeyByte(key);
            if (bzSrcKey.Length >= KeyLen) Array.Copy(bzSrcKey, 0, bzKey, 0, KeyLen);
            else Array.Copy(bzSrcKey, 0, bzKey, 0, bzSrcKey.Length);
            if (bzSrcKey.Length > IvLen) Array.Copy(bzSrcKey, bzSrcKey.Length - IvLen, bzIv, 0, IvLen);
            else Array.Copy(bzSrcKey, 0, bzIv, 0, bzSrcKey.Length);
            Array.Reverse(bzIv);
        }
        /// <summary>
        /// 用指定密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <param name="key">密码</param>
        /// <returns>加密字节流</returns>
        public override byte[] Encrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            ICryptoTransform encryptor = m_Handler.CreateEncryptor(bzKey, bzIv);
            MemoryStream msBuffer = new MemoryStream();
            CryptoStream csStream = new CryptoStream(msBuffer, encryptor, CryptoStreamMode.Write);
            csStream.Write(src, 0, src.Length);
            csStream.FlushFinalBlock();
            csStream.Close();
            byte[] ret = msBuffer.ToArray();
            msBuffer.Close();
            return ret;
        }
        /// <summary>
        /// 用指定密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <param name="key">密码</param>
        /// <returns>字节流</returns>
        public override byte[] Decrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            ICryptoTransform decryptor = m_Handler.CreateDecryptor(bzKey, bzIv);
            MemoryStream msBuffer = new MemoryStream(src);
            CryptoStream csStream = new CryptoStream(msBuffer, decryptor, CryptoStreamMode.Read);
            byte[] bzRead = new byte[msBuffer.Length];
            int nRead = csStream.Read(bzRead, 0, bzRead.Length);
            csStream.Close();
            msBuffer.Close();
            byte[] ret = new byte[nRead];
            Array.Copy(bzRead, 0, ret, 0, nRead);
            return ret;
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        protected override void Clear()
        {
        }
        /// <summary>
        /// 获取Rijndael的实例
        /// </summary>
        public static Rijndael Instance
        {
            get { return new Rijndael(); }
        }
    }
    /// <summary>
    /// 用TripleDES算法加解密
    /// </summary>
    public sealed class TripleDES : SecurityBase
    {
        private TripleDESCryptoServiceProvider m_Handler = new TripleDESCryptoServiceProvider();
        /// <summary>
        /// 密码长度
        /// </summary>
        protected override uint KeyLen
        {
            get { return 24; }
        }
        /// <summary>
        /// 向量长度
        /// </summary>
        protected override uint IvLen
        {
            get { return 8; }
        }
        /// <summary>
        /// 对指定的密码获取密码和向量
        /// </summary>
        /// <param name="key">密码字符串</param>
        /// <param name="bzKey">密码字节流</param>
        /// <param name="bzIv">向量字节流</param>
        protected override void PrepareKey(string key, out byte[] bzKey, out byte[] bzIv)
        {
            bzKey = new byte[KeyLen];
            bzIv = new byte[IvLen];
            for (int i = 0; i < KeyLen; i++) bzKey[i] = System.Convert.ToByte(i & 0xFF);
            for (int i = 0; i < IvLen; i++) bzIv[i] = System.Convert.ToByte(i & 0xFF);
            byte[] bzSrcKey = GetKeyByte(key);
            if (bzSrcKey.Length >= KeyLen) Array.Copy(bzSrcKey, 0, bzKey, 0, KeyLen);
            else Array.Copy(bzSrcKey, 0, bzKey, 0, bzSrcKey.Length);
            if (bzSrcKey.Length > IvLen) Array.Copy(bzSrcKey, bzSrcKey.Length - IvLen, bzIv, 0, IvLen);
            else Array.Copy(bzSrcKey, 0, bzIv, 0, bzSrcKey.Length);
            Array.Reverse(bzIv);
        }
        /// <summary>
        /// 用指定密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <param name="key">密码</param>
        /// <returns>加密字节流</returns>
        public override byte[] Encrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            ICryptoTransform encryptor = m_Handler.CreateEncryptor(bzKey, bzIv);
            MemoryStream msBuffer = new MemoryStream();
            CryptoStream csStream = new CryptoStream(msBuffer, encryptor, CryptoStreamMode.Write);
            csStream.Write(src, 0, src.Length);
            csStream.FlushFinalBlock();
            csStream.Close();
            byte[] ret = msBuffer.ToArray();
            msBuffer.Close();
            return ret;
        }
        /// <summary>
        /// 用指定密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <param name="key">密码</param>
        /// <returns>字节流</returns>
        public override byte[] Decrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            ICryptoTransform decryptor = m_Handler.CreateDecryptor(bzKey, bzIv);
            MemoryStream msBuffer = new MemoryStream(src);
            CryptoStream csStream = new CryptoStream(msBuffer, decryptor, CryptoStreamMode.Read);
            byte[] bzRead = new byte[msBuffer.Length];
            int nRead = csStream.Read(bzRead, 0, bzRead.Length);
            csStream.Close();
            msBuffer.Close();
            byte[] ret = new byte[nRead];
            Array.Copy(bzRead, 0, ret, 0, nRead);
            return ret;
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        protected override void Clear()
        {
        }
        /// <summary>
        /// 获取TripleDES的实例
        /// </summary>
        public static TripleDES Instance
        {
            get { return new TripleDES(); }
        }
    }
    /// <summary>
    /// 用RC4算法加解密
    /// </summary>
    public sealed class RC4 : SecurityBase
    {
        /// <summary>
        /// 对指定的密码获取密码和向量
        /// </summary>
        /// <param name="key">密码字符串</param>
        /// <param name="bzKey">密码字节流</param>
        /// <param name="bzIv">向量字节流</param>
        protected override void PrepareKey(string key, out byte[] bzKey, out byte[] bzIv)
        {
            bzKey = new byte[KeyLen];
            for (int i = 0; i < KeyLen; i++) bzKey[i] = 0;
            bzIv = new byte[IvLen];
            for (int i = 0; i < IvLen; i++) bzIv[i] = 0;
            byte[] bzSrcKey = GetKeyByte(key);
            int nSrcKeyLen = bzSrcKey.Length;
            for (int i = 0; i < RC4_LEN; i++)
            {
                bzKey[i] = bzSrcKey[i % nSrcKeyLen];
                bzIv[i] = System.Convert.ToByte(i & 0xFF);
            }
            byte swapIndex = 0;
            for (int cnt = 0; cnt < RC4_LEN; cnt++)
            {
                swapIndex = System.Convert.ToByte((swapIndex + bzIv[cnt] + bzKey[cnt]) & 0xff);
                Swap(ref bzIv[cnt], ref bzIv[swapIndex]);
            }
        }
        /// <summary>
        /// 用指定密码加密
        /// </summary>
        /// <param name="src">字节流</param>
        /// <param name="key">密码</param>
        /// <returns>加密字节流</returns>
        public override byte[] Encrypt(byte[] src, string key)
        {
            byte[] bzKey, bzIv;
            PrepareKey(key, out bzKey, out bzIv);
            int len = src.Length;
            byte[] ret = new byte[len];
            Array.Copy(src, 0, ret, 0, len);
            byte swapIndex = 0;
            int i = 0, j = 0;
            for (int cnt = 0; cnt < len; cnt++)
            {
                i = (i + 1) & 0xff;
                j = (j + bzIv[i]) & 0xff;
                Swap(ref bzIv[i], ref bzIv[j]);
                swapIndex = bzIv[(bzIv[i] + bzIv[j]) & 0xff];
                ret[cnt] ^= bzIv[swapIndex];
            }
            return ret;
        }
        /// <summary>
        /// 用指定密码解密
        /// </summary>
        /// <param name="src">加密字节流</param>
        /// <param name="key">密码</param>
        /// <returns>字节流</returns>
        public override byte[] Decrypt(byte[] src, string key)
        {
            return Encrypt(src, key);
        }

        private const uint RC4_LEN = 0x0100;
        /// <summary>
        /// 密码长度
        /// </summary>
        protected override uint KeyLen
        {
            get { return RC4_LEN; }
        }
        /// <summary>
        /// 向量长度
        /// </summary>
        protected override uint IvLen
        {
            get { return RC4_LEN; }
        }
        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        protected override void Clear()
        {
        }
        /// <summary>
        /// 获取RC4的实例
        /// </summary>
        public static RC4 Instance
        {
            get { return new RC4(); }
        }
    }
    /// <summary>
    /// 加解密的算法
    /// </summary>
    public enum Security_Algorithm : uint
    {
        /// <summary>
        /// rc4算法
        /// </summary>
        RC4 = 0x01,
        /// <summary>
        /// des算法
        /// </summary>
        DES = 0x02,
        /// <summary>
        /// 3des算法
        /// </summary>
        TripleDES = 0x03,
        /// <summary>
        /// rijndael算法
        /// </summary>
        Rijndael = 0x04,
    }

    /*
	class SM4
		{
			public const int SM4_ENCRYPT = 1;
			public const int SM4_DECRYPT = 0;

			private long GET_ULONG_BE(byte[] b, int i)
			{
				long n = (long)(b[i] & 0xff) << 24 | (long)((b[i + 1] & 0xff) << 16) | (long)((b[i + 2] & 0xff) << 8) | (long)(b[i + 3] & 0xff) & 0xffffffffL;
				return n;
			}

			private void PUT_ULONG_BE(long n, byte[] b, int i)
			{
				b[i] = (byte)(int)(0xFF & n >> 24);
				b[i + 1] = (byte)(int)(0xFF & n >> 16);
				b[i + 2] = (byte)(int)(0xFF & n >> 8);
				b[i + 3] = (byte)(int)(0xFF & n);
			}

			private long SHL(long x, int n)
			{
				return (x & 0xFFFFFFFF) << n;
			}

			private long ROTL(long x, int n)
			{
				return SHL(x, n) | x >> (32 - n);
			}

			private void SWAP(long[] sk, int i)
			{
				long t = sk[i];
				sk[i] = sk[(31 - i)];
				sk[(31 - i)] = t;
			}

			public byte[] SboxTable = new byte[] { (byte) 0xd6, (byte) 0x90, (byte) 0xe9, (byte) 0xfe,
			(byte) 0xcc, (byte) 0xe1, 0x3d, (byte) 0xb7, 0x16, (byte) 0xb6,
			0x14, (byte) 0xc2, 0x28, (byte) 0xfb, 0x2c, 0x05, 0x2b, 0x67,
			(byte) 0x9a, 0x76, 0x2a, (byte) 0xbe, 0x04, (byte) 0xc3,
			(byte) 0xaa, 0x44, 0x13, 0x26, 0x49, (byte) 0x86, 0x06,
			(byte) 0x99, (byte) 0x9c, 0x42, 0x50, (byte) 0xf4, (byte) 0x91,
			(byte) 0xef, (byte) 0x98, 0x7a, 0x33, 0x54, 0x0b, 0x43,
			(byte) 0xed, (byte) 0xcf, (byte) 0xac, 0x62, (byte) 0xe4,
			(byte) 0xb3, 0x1c, (byte) 0xa9, (byte) 0xc9, 0x08, (byte) 0xe8,
			(byte) 0x95, (byte) 0x80, (byte) 0xdf, (byte) 0x94, (byte) 0xfa,
			0x75, (byte) 0x8f, 0x3f, (byte) 0xa6, 0x47, 0x07, (byte) 0xa7,
			(byte) 0xfc, (byte) 0xf3, 0x73, 0x17, (byte) 0xba, (byte) 0x83,
			0x59, 0x3c, 0x19, (byte) 0xe6, (byte) 0x85, 0x4f, (byte) 0xa8,
			0x68, 0x6b, (byte) 0x81, (byte) 0xb2, 0x71, 0x64, (byte) 0xda,
			(byte) 0x8b, (byte) 0xf8, (byte) 0xeb, 0x0f, 0x4b, 0x70, 0x56,
			(byte) 0x9d, 0x35, 0x1e, 0x24, 0x0e, 0x5e, 0x63, 0x58, (byte) 0xd1,
			(byte) 0xa2, 0x25, 0x22, 0x7c, 0x3b, 0x01, 0x21, 0x78, (byte) 0x87,
			(byte) 0xd4, 0x00, 0x46, 0x57, (byte) 0x9f, (byte) 0xd3, 0x27,
			0x52, 0x4c, 0x36, 0x02, (byte) 0xe7, (byte) 0xa0, (byte) 0xc4,
			(byte) 0xc8, (byte) 0x9e, (byte) 0xea, (byte) 0xbf, (byte) 0x8a,
			(byte) 0xd2, 0x40, (byte) 0xc7, 0x38, (byte) 0xb5, (byte) 0xa3,
			(byte) 0xf7, (byte) 0xf2, (byte) 0xce, (byte) 0xf9, 0x61, 0x15,
			(byte) 0xa1, (byte) 0xe0, (byte) 0xae, 0x5d, (byte) 0xa4,
			(byte) 0x9b, 0x34, 0x1a, 0x55, (byte) 0xad, (byte) 0x93, 0x32,
			0x30, (byte) 0xf5, (byte) 0x8c, (byte) 0xb1, (byte) 0xe3, 0x1d,
			(byte) 0xf6, (byte) 0xe2, 0x2e, (byte) 0x82, 0x66, (byte) 0xca,
			0x60, (byte) 0xc0, 0x29, 0x23, (byte) 0xab, 0x0d, 0x53, 0x4e, 0x6f,
			(byte) 0xd5, (byte) 0xdb, 0x37, 0x45, (byte) 0xde, (byte) 0xfd,
			(byte) 0x8e, 0x2f, 0x03, (byte) 0xff, 0x6a, 0x72, 0x6d, 0x6c, 0x5b,
			0x51, (byte) 0x8d, 0x1b, (byte) 0xaf, (byte) 0x92, (byte) 0xbb,
			(byte) 0xdd, (byte) 0xbc, 0x7f, 0x11, (byte) 0xd9, 0x5c, 0x41,
			0x1f, 0x10, 0x5a, (byte) 0xd8, 0x0a, (byte) 0xc1, 0x31,
			(byte) 0x88, (byte) 0xa5, (byte) 0xcd, 0x7b, (byte) 0xbd, 0x2d,
			0x74, (byte) 0xd0, 0x12, (byte) 0xb8, (byte) 0xe5, (byte) 0xb4,
			(byte) 0xb0, (byte) 0x89, 0x69, (byte) 0x97, 0x4a, 0x0c,
			(byte) 0x96, 0x77, 0x7e, 0x65, (byte) 0xb9, (byte) 0xf1, 0x09,
			(byte) 0xc5, 0x6e, (byte) 0xc6, (byte) 0x84, 0x18, (byte) 0xf0,
			0x7d, (byte) 0xec, 0x3a, (byte) 0xdc, 0x4d, 0x20, 0x79,
			(byte) 0xee, 0x5f, 0x3e, (byte) 0xd7, (byte) 0xcb, 0x39, 0x48 };

			public uint[] FK = { 0xa3b1bac6, 0x56aa3350, 0x677d9197, 0xb27022dc };

			public uint[] CK = { 0x00070e15,0x1c232a31,0x383f464d,0x545b6269,
										0x70777e85,0x8c939aa1,0xa8afb6bd,0xc4cbd2d9,
										0xe0e7eef5,0xfc030a11,0x181f262d,0x343b4249,
										0x50575e65,0x6c737a81,0x888f969d,0xa4abb2b9,
										0xc0c7ced5,0xdce3eaf1,0xf8ff060d,0x141b2229,
										0x30373e45,0x4c535a61,0x686f767d,0x848b9299,
										0xa0a7aeb5,0xbcc3cad1,0xd8dfe6ed,0xf4fb0209,
										0x10171e25,0x2c333a41,0x484f565d,0x646b7279 };

			private byte sm4Sbox(byte inch)
			{
				int i = inch & 0xFF;
				byte retVal = SboxTable[i];
				return retVal;
			}

			private long sm4Lt(long ka)
			{
				long bb = 0L;
				long c = 0L;
				byte[] a = new byte[4];
				byte[] b = new byte[4];
				PUT_ULONG_BE(ka, a, 0);
				b[0] = sm4Sbox(a[0]);
				b[1] = sm4Sbox(a[1]);
				b[2] = sm4Sbox(a[2]);
				b[3] = sm4Sbox(a[3]);
				bb = GET_ULONG_BE(b, 0);
				c = bb ^ ROTL(bb, 2) ^ ROTL(bb, 10) ^ ROTL(bb, 18) ^ ROTL(bb, 24);
				return c;
			}

			private long sm4F(long x0, long x1, long x2, long x3, long rk)
			{
				return x0 ^ sm4Lt(x1 ^ x2 ^ x3 ^ rk);
			}

			private long sm4CalciRK(long ka)
			{
				long bb = 0L;
				long rk = 0L;
				byte[] a = new byte[4];
				byte[] b = new byte[4];
				PUT_ULONG_BE(ka, a, 0);
				b[0] = sm4Sbox(a[0]);
				b[1] = sm4Sbox(a[1]);
				b[2] = sm4Sbox(a[2]);
				b[3] = sm4Sbox(a[3]);
				bb = GET_ULONG_BE(b, 0);
				rk = bb ^ ROTL(bb, 13) ^ ROTL(bb, 23);
				return rk;
			}

			private void sm4_setkey(long[] SK, byte[] key)
			{
				long[] MK = new long[4];
				long[] k = new long[36];
				int i = 0;
				MK[0] = GET_ULONG_BE(key, 0);
				MK[1] = GET_ULONG_BE(key, 4);
				MK[2] = GET_ULONG_BE(key, 8);
				MK[3] = GET_ULONG_BE(key, 12);
				k[0] = MK[0] ^ (long)FK[0];
				k[1] = MK[1] ^ (long)FK[1];
				k[2] = MK[2] ^ (long)FK[2];
				k[3] = MK[3] ^ (long)FK[3];
				for (; i < 32; i++)
				{
					k[(i + 4)] = (k[i] ^ sm4CalciRK(k[(i + 1)] ^ k[(i + 2)] ^ k[(i + 3)] ^ (long)CK[i]));
					SK[i] = k[(i + 4)];
				}
			}

			private void sm4_one_round(long[] sk, byte[] input, byte[] output)
			{
				int i = 0;
				long[] ulbuf = new long[36];
				ulbuf[0] = GET_ULONG_BE(input, 0);
				ulbuf[1] = GET_ULONG_BE(input, 4);
				ulbuf[2] = GET_ULONG_BE(input, 8);
				ulbuf[3] = GET_ULONG_BE(input, 12);
				while (i < 32)
				{
					ulbuf[(i + 4)] = sm4F(ulbuf[i], ulbuf[(i + 1)], ulbuf[(i + 2)], ulbuf[(i + 3)], sk[i]);
					i++;
				}
				PUT_ULONG_BE(ulbuf[35], output, 0);
				PUT_ULONG_BE(ulbuf[34], output, 4);
				PUT_ULONG_BE(ulbuf[33], output, 8);
				PUT_ULONG_BE(ulbuf[32], output, 12);
			}

			private byte[] padding(byte[] input, int mode)
			{
				if (input == null)
				{
					return null;
				}

				byte[] ret = (byte[])null;
				if (mode == SM4_ENCRYPT)
				{
					int p = 16 - input.Length % 16;
					ret = new byte[input.Length + p];
					Array.Copy(input, 0, ret, 0, input.Length);
					for (int i = 0; i < p; i++)
					{
						ret[input.Length + i] = (byte)p;
					}
				}
				else
				{
					int p = input[input.Length - 1];
					ret = new byte[input.Length - p];
					Array.Copy(input, 0, ret, 0, input.Length - p);
				}
				return ret;
			}

			public void sm4_setkey_enc(SM4_Context ctx, byte[] key)
			{
				ctx.mode = SM4_ENCRYPT;
				sm4_setkey(ctx.sk, key);
			}

			public void sm4_setkey_dec(SM4_Context ctx, byte[] key)
			{
				int i = 0;
				ctx.mode = SM4_DECRYPT;
				sm4_setkey(ctx.sk, key);
				for (i = 0; i < 16; i++)
				{
					SWAP(ctx.sk, i);
				}
			}

			public byte[] sm4_crypt_ecb(SM4_Context ctx, byte[] input)
			{
				if ((ctx.isPadding) && (ctx.mode == SM4_ENCRYPT))
				{
					input = padding(input, SM4_ENCRYPT);
				}

				int length = input.Length;
				byte[] bins = new byte[length];
				Array.Copy(input, 0, bins, 0, length);
				byte[] bous = new byte[length];
				for (int i = 0; length > 0; length -= 16, i++)
				{
					byte[] inBytes = new byte[16];
					byte[] outBytes = new byte[16];
					Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
					sm4_one_round(ctx.sk, inBytes, outBytes);
					Array.Copy(outBytes, 0, bous, i * 16, length > 16 ? 16 : length);
				}

				if (ctx.isPadding && ctx.mode == SM4_DECRYPT)
				{
					bous = padding(bous, SM4_DECRYPT);
				}
				return bous;
			}

			public byte[] sm4_crypt_cbc(SM4_Context ctx, byte[] iv, byte[] input)
			{
				if (ctx.isPadding && ctx.mode == SM4_ENCRYPT)
				{
					input = padding(input, SM4_ENCRYPT);
				}

				int i = 0;
				int length = input.Length;
				byte[] bins = new byte[length];
				Array.Copy(input, 0, bins, 0, length);
				byte[] bous = null;
				List<byte> bousList = new List<byte>();
				if (ctx.mode == SM4_ENCRYPT)
				{
					for (int j = 0; length > 0; length -= 16, j++)
					{
						byte[] inBytes = new byte[16];
						byte[] outBytes = new byte[16];
						byte[] out1 = new byte[16];

						Array.Copy(bins, j * 16, inBytes, 0, length > 16 ? 16 : length);
						for (i = 0; i < 16; i++)
						{
							outBytes[i] = ((byte)(inBytes[i] ^ iv[i]));
						}
						sm4_one_round(ctx.sk, outBytes, out1);
						Array.Copy(out1, 0, iv, 0, 16);
						for (int k = 0; k < 16; k++)
						{
							bousList.Add(out1[k]);
						}
					}
				}
				else
				{
					byte[] temp = new byte[16];
					for (int j = 0; length > 0; length -= 16, j++)
					{
						byte[] inBytes = new byte[16];
						byte[] outBytes = new byte[16];
						byte[] out1 = new byte[16];

						Array.Copy(bins, j * 16, inBytes, 0, length > 16 ? 16 : length);
						Array.Copy(inBytes, 0, temp, 0, 16);
						sm4_one_round(ctx.sk, inBytes, outBytes);
						for (i = 0; i < 16; i++)
						{
							out1[i] = ((byte)(outBytes[i] ^ iv[i]));
						}
						Array.Copy(temp, 0, iv, 0, 16);
						for (int k = 0; k < 16; k++)
						{
							bousList.Add(out1[k]);
						}
					}

				}

				if (ctx.isPadding && ctx.mode == SM4_DECRYPT)
				{
					bous = padding(bousList.ToArray(), SM4_DECRYPT);
					return bous;
				}
				else
				{
					return bousList.ToArray();
				}
			}
	}
	class SM4_Context
	{
		public int mode;

		public long[] sk;

		public bool isPadding;

		public SM4_Context()
		{
			this.mode = 1;
			this.isPadding = true;
			this.sk = new long[32];
		}
}
	class SM4Utils
	{
		public String secretKey = "";
		public String iv = "this is iv bytes";
		public bool hexString = false;

		public String Encrypt_ECB(String plainText)
		{
			SM4_Context ctx = new SM4_Context();
			ctx.isPadding = true;
			ctx.mode = SM4.SM4_ENCRYPT;

			byte[] keyBytes;
			if (hexString)
			{
				keyBytes = Hex.Decode(secretKey);
			}
			else
			{
                //keyBytes = Encoding.Default.GetBytes(secretKey);
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
            }

			SM4 sm4 = new SM4();
			sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.UTF8.GetBytes(plainText));
            //byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.Default.GetBytes(plainText));

            String cipherText = Encoding.UTF8.GetString(Hex.Encode(encrypted));
            //String cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
            return cipherText;
		}

		public String Decrypt_ECB(String cipherText)
		{
			SM4_Context ctx = new SM4_Context();
			ctx.isPadding = true;
			ctx.mode = SM4.SM4_DECRYPT;

			byte[] keyBytes;
			if (hexString)
			{
				keyBytes = Hex.Decode(secretKey);
			}
			else
			{
				keyBytes = Encoding.UTF8.GetBytes(secretKey);
				//keyBytes = Encoding.Default.GetBytes(secretKey);
			}

			SM4 sm4 = new SM4();
			sm4.sm4_setkey_dec(ctx, keyBytes);
			byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
			return Encoding.UTF8.GetString(decrypted);
			//return Encoding.Default.GetString(decrypted);
		}
		public String Encrypt_CBC(String plainText)
		{
			SM4_Context ctx = new SM4_Context();
			ctx.isPadding = true;
			ctx.mode = SM4.SM4_ENCRYPT;

			byte[] keyBytes;
			byte[] ivBytes;
			if (hexString)
			{
				keyBytes = Hex.Decode(secretKey);
				ivBytes = Hex.Decode(iv);
			}
			else
			{
				keyBytes = Encoding.Default.GetBytes(secretKey);
				ivBytes = Encoding.Default.GetBytes(iv);
			}

			SM4 sm4 = new SM4();
			sm4.sm4_setkey_enc(ctx, keyBytes);
			byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.Default.GetBytes(plainText));

			String cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
			return cipherText;
		}

		public String Decrypt_CBC(String cipherText)
		{
			SM4_Context ctx = new SM4_Context();
			ctx.isPadding = true;
			ctx.mode = SM4.SM4_DECRYPT;

			byte[] keyBytes;
			byte[] ivBytes;
			if (hexString)
			{
				keyBytes = Hex.Decode(secretKey);
				ivBytes = Hex.Decode(iv);
			}
			else
			{
				keyBytes = Encoding.Default.GetBytes(secretKey);
				ivBytes = Encoding.Default.GetBytes(iv);
			}

			SM4 sm4 = new SM4();
			sm4.sm4_setkey_dec(ctx, keyBytes);
			byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
			return Encoding.Default.GetString(decrypted);
		}

		//[STAThread]
		//public static void Main()
		//{
		//    String plainText = "ererfeiisgod";  

		//    SM4Utils sm4 = new SM4Utils();  
		//    sm4.secretKey = "JeF8U9wHFOMfs2Y8";  
		//    sm4.hexString = false;  

		//    System.Console.Out.WriteLine("ECB模式");  
		//    String cipherText = sm4.Encrypt_ECB(plainText);  
		//    System.Console.Out.WriteLine("密文: " + cipherText);  
		//    System.Console.Out.WriteLine("");  

		//    plainText = sm4.Decrypt_ECB(cipherText);  
		//    System.Console.Out.WriteLine("明文: " + plainText);  
		//    System.Console.Out.WriteLine("");  

		//    System.Console.Out.WriteLine("CBC模式");  
		//    sm4.iv = "UISwD9fW6cFh9SNS";  
		//    cipherText = sm4.Encrypt_CBC(plainText);  
		//    System.Console.Out.WriteLine("密文: " + cipherText);  
		//    System.Console.Out.WriteLine("");  

		//    plainText = sm4.Decrypt_CBC(cipherText);  
		//    System.Console.Out.WriteLine("明文: " + plainText);

		//    Console.ReadLine();
		//}
	}
	*/
    public abstract class GeneralDigest : IDigest
    {
        private const int BYTE_LENGTH = 64;

        private byte[] xBuf;
        private int xBufOff;

        private long byteCount;

        internal GeneralDigest()
        {
            xBuf = new byte[4];
        }

        internal GeneralDigest(GeneralDigest t)
        {
            xBuf = new byte[t.xBuf.Length];
            Array.Copy(t.xBuf, 0, xBuf, 0, t.xBuf.Length);

            xBufOff = t.xBufOff;
            byteCount = t.byteCount;
        }

        public void Update(byte input)
        {
            xBuf[xBufOff++] = input;

            if (xBufOff == xBuf.Length)
            {
                ProcessWord(xBuf, 0);
                xBufOff = 0;
            }

            byteCount++;
        }

        public void BlockUpdate(
            byte[] input,
            int inOff,
            int length)
        {
            //
            // fill the current word
            //
            while ((xBufOff != 0) && (length > 0))
            {
                Update(input[inOff]);
                inOff++;
                length--;
            }

            //
            // process whole words.
            //
            while (length > xBuf.Length)
            {
                ProcessWord(input, inOff);

                inOff += xBuf.Length;
                length -= xBuf.Length;
                byteCount += xBuf.Length;
            }

            //
            // load in the remainder.
            //
            while (length > 0)
            {
                Update(input[inOff]);

                inOff++;
                length--;
            }
        }

        public void Finish()
        {
            long bitLength = (byteCount << 3);

            //
            // add the pad bytes.
            //
            Update(unchecked((byte)128));

            while (xBufOff != 0) Update(unchecked((byte)0));
            ProcessLength(bitLength);
            ProcessBlock();
        }

        public virtual void Reset()
        {
            byteCount = 0;
            xBufOff = 0;
            Array.Clear(xBuf, 0, xBuf.Length);
        }

        public int GetByteLength()
        {
            return BYTE_LENGTH;
        }

        internal abstract void ProcessWord(byte[] input, int inOff);
        internal abstract void ProcessLength(long bitLength);
        internal abstract void ProcessBlock();
        public abstract string AlgorithmName { get; }
        public abstract int GetDigestSize();
        public abstract int DoFinal(byte[] output, int outOff);
    }
    public class SM3 : GeneralDigest
    {
        public class SupportClass
        {
            /// <summary>
            /// Performs an unsigned bitwise right shift with the specified number
            /// </summary>
            /// <param name="number">Number to operate on</param>
            /// <param name="bits">Ammount of bits to shift</param>
            /// <returns>The resulting number from the shift operation</returns>
            public static int URShift(int number, int bits)
            {
                if (number >= 0)
                    return number >> bits;
                else
                    return (number >> bits) + (2 << ~bits);
            }

            /// <summary>
            /// Performs an unsigned bitwise right shift with the specified number
            /// </summary>
            /// <param name="number">Number to operate on</param>
            /// <param name="bits">Ammount of bits to shift</param>
            /// <returns>The resulting number from the shift operation</returns>
            public static int URShift(int number, long bits)
            {
                return URShift(number, (int)bits);
            }

            /// <summary>
            /// Performs an unsigned bitwise right shift with the specified number
            /// </summary>
            /// <param name="number">Number to operate on</param>
            /// <param name="bits">Ammount of bits to shift</param>
            /// <returns>The resulting number from the shift operation</returns>
            public static long URShift(long number, int bits)
            {
                if (number >= 0)
                    return number >> bits;
                else
                    return (number >> bits) + (2L << ~bits);
            }

            /// <summary>
            /// Performs an unsigned bitwise right shift with the specified number
            /// </summary>
            /// <param name="number">Number to operate on</param>
            /// <param name="bits">Ammount of bits to shift</param>
            /// <returns>The resulting number from the shift operation</returns>
            public static long URShift(long number, long bits)
            {
                return URShift(number, (int)bits);
            }


        }
        public override string AlgorithmName
        {
            get
            {
                return "SM3";
            }

        }
        public override int GetDigestSize()
        {
            return DIGEST_LENGTH;
        }

        private const int DIGEST_LENGTH = 32;

        private static readonly int[] v0 = new int[] { 0x7380166f, 0x4914b2b9, 0x172442d7, unchecked((int)0xda8a0600), unchecked((int)0xa96f30bc), 0x163138aa, unchecked((int)0xe38dee4d), unchecked((int)0xb0fb0e4e) };

        private int[] v = new int[8];
        private int[] v_ = new int[8];

        private static readonly int[] X0 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private int[] X = new int[68];
        private int xOff;

        private int T_00_15 = 0x79cc4519;
        private int T_16_63 = 0x7a879d8a;

        public SM3()
        {
            Reset();
        }

        public SM3(SM3 t) : base(t)
        {

            Array.Copy(t.X, 0, X, 0, t.X.Length);
            xOff = t.xOff;

            Array.Copy(t.v, 0, v, 0, t.v.Length);
        }

        public override void Reset()
        {
            base.Reset();

            Array.Copy(v0, 0, v, 0, v0.Length);

            xOff = 0;
            Array.Copy(X0, 0, X, 0, X0.Length);
        }

        internal override void ProcessBlock()
        {
            int i;

            int[] ww = X;
            int[] ww_ = new int[64];

            for (i = 16; i < 68; i++)
            {
                ww[i] = P1(ww[i - 16] ^ ww[i - 9] ^ (ROTATE(ww[i - 3], 15))) ^ (ROTATE(ww[i - 13], 7)) ^ ww[i - 6];
            }

            for (i = 0; i < 64; i++)
            {
                ww_[i] = ww[i] ^ ww[i + 4];
            }

            int[] vv = v;
            int[] vv_ = v_;

            Array.Copy(vv, 0, vv_, 0, v0.Length);

            int SS1, SS2, TT1, TT2, aaa;
            for (i = 0; i < 16; i++)
            {
                aaa = ROTATE(vv_[0], 12);
                SS1 = aaa + vv_[4] + ROTATE(T_00_15, i);
                SS1 = ROTATE(SS1, 7);
                SS2 = SS1 ^ aaa;

                TT1 = FF_00_15(vv_[0], vv_[1], vv_[2]) + vv_[3] + SS2 + ww_[i];
                TT2 = GG_00_15(vv_[4], vv_[5], vv_[6]) + vv_[7] + SS1 + ww[i];
                vv_[3] = vv_[2];
                vv_[2] = ROTATE(vv_[1], 9);
                vv_[1] = vv_[0];
                vv_[0] = TT1;
                vv_[7] = vv_[6];
                vv_[6] = ROTATE(vv_[5], 19);
                vv_[5] = vv_[4];
                vv_[4] = P0(TT2);
            }
            for (i = 16; i < 64; i++)
            {
                aaa = ROTATE(vv_[0], 12);
                SS1 = aaa + vv_[4] + ROTATE(T_16_63, i);
                SS1 = ROTATE(SS1, 7);
                SS2 = SS1 ^ aaa;

                TT1 = FF_16_63(vv_[0], vv_[1], vv_[2]) + vv_[3] + SS2 + ww_[i];
                TT2 = GG_16_63(vv_[4], vv_[5], vv_[6]) + vv_[7] + SS1 + ww[i];
                vv_[3] = vv_[2];
                vv_[2] = ROTATE(vv_[1], 9);
                vv_[1] = vv_[0];
                vv_[0] = TT1;
                vv_[7] = vv_[6];
                vv_[6] = ROTATE(vv_[5], 19);
                vv_[5] = vv_[4];
                vv_[4] = P0(TT2);
            }
            for (i = 0; i < 8; i++)
            {
                vv[i] ^= vv_[i];
            }

            // Reset
            xOff = 0;
            Array.Copy(X0, 0, X, 0, X0.Length);
        }

        internal override void ProcessWord(byte[] in_Renamed, int inOff)
        {
            int n = in_Renamed[inOff] << 24;
            n |= (in_Renamed[++inOff] & 0xff) << 16;
            n |= (in_Renamed[++inOff] & 0xff) << 8;
            n |= (in_Renamed[++inOff] & 0xff);
            X[xOff] = n;

            if (++xOff == 16)
            {
                ProcessBlock();
            }
        }

        internal override void ProcessLength(long bitLength)
        {
            if (xOff > 14)
            {
                ProcessBlock();
            }

            X[14] = (int)(SupportClass.URShift(bitLength, 32));
            X[15] = (int)(bitLength & unchecked((int)0xffffffff));
        }

        public static void IntToBigEndian(int n, byte[] bs, int off)
        {
            bs[off] = (byte)(SupportClass.URShift(n, 24));
            bs[++off] = (byte)(SupportClass.URShift(n, 16));
            bs[++off] = (byte)(SupportClass.URShift(n, 8));
            bs[++off] = (byte)(n);
        }

        public override int DoFinal(byte[] out_Renamed, int outOff)
        {
            Finish();

            for (int i = 0; i < 8; i++)
            {
                IntToBigEndian(v[i], out_Renamed, outOff + i * 4);
            }

            Reset();

            return DIGEST_LENGTH;
        }

        private int ROTATE(int x, int n)
        {
            return (x << n) | (SupportClass.URShift(x, (32 - n)));
        }

        private int P0(int X)
        {
            return ((X) ^ ROTATE((X), 9) ^ ROTATE((X), 17));
        }

        private int P1(int X)
        {
            return ((X) ^ ROTATE((X), 15) ^ ROTATE((X), 23));
        }

        private int FF_00_15(int X, int Y, int Z)
        {
            return (X ^ Y ^ Z);
        }

        private int FF_16_63(int X, int Y, int Z)
        {
            return ((X & Y) | (X & Z) | (Y & Z));
        }

        private int GG_00_15(int X, int Y, int Z)
        {
            return (X ^ Y ^ Z);
        }

        private int GG_16_63(int X, int Y, int Z)
        {
            return ((X & Y) | (~X & Z));
        }

        //[STAThread]
        //public static void  Main()
        //{
        //    byte[] md = new byte[32];
        //    byte[] msg1 = Encoding.Default.GetBytes("ererfeiisgod");
        //    SM3Digest sm3 = new SM3Digest();
        //    sm3.BlockUpdate(msg1, 0, msg1.Length);
        //    sm3.DoFinal(md, 0);
        //    System.String s = new UTF8Encoding().GetString(Hex.Encode(md));
        //    System.Console.Out.WriteLine(s.ToUpper());

        //    Console.ReadLine();
        //}
    }
    public sealed class SM4ServiceProvider
    {
        private long GetUlongFromBuffer(byte[] buffer, int pos)
        {
            //long n = (long)(buffer[pos] & 0xff) << 24 |
            //    (long)((buffer[pos + 1] & 0xff) << 16) |
            //    (long)((buffer[pos + 2] & 0xff) << 8) |
            //    (long)(buffer[pos + 3] & 0xff) &
            //    0xffffffffL;
            //return n;
            long ret = 0L;
            for (int i = 0; i < wordSize; i++)
                ret |= (long)((buffer[pos + i] & 0xFF) << (wordLength - ((i + 1) * byteLength)));
            ret &= 0xFFFFFFFFL;
            return ret;
        }

        private void PutUlongToBuffer(long num, byte[] buffer, int pos)
        {
            //buffer[pos] = (byte)(int)(0xFF & num >> 24);
            //buffer[pos + 1] = (byte)(int)(0xFF & num >> 16);
            //buffer[pos + 2] = (byte)(int)(0xFF & num >> 8);
            //buffer[pos + 3] = (byte)(int)(0xFF & num);
            //byte[] bb = new byte[4];
            for (int i = 0; i < wordSize; i++)
                buffer[pos + i] = (byte)(num >> (wordLength - ((i + 1) * byteLength)) & 0xFF);
        }

        private long GetUlongShiftLeft(long num, int pos) => (num & 0xFFFFFFFF) << pos;

        private long ExchangeByBit(long num, int pos) => GetUlongShiftLeft(num, pos) | num >> (wordLength - pos);

        private void SWAP(long[] keyBuffer, int idx)
        {
            long t = keyBuffer[idx];
            keyBuffer[idx] = keyBuffer[(KeyLength - 1 - idx)];
            keyBuffer[(KeyLength - 1 - idx)] = t;
        }
        /// <summary>
        /// Sbox，由256个8 bit的数字组成
        /// </summary>
        private static readonly byte[] SboxTable = new byte[] { (byte) 0xd6, (byte) 0x90, (byte) 0xe9, (byte) 0xfe,
            (byte) 0xcc, (byte) 0xe1, 0x3d, (byte) 0xb7, 0x16, (byte) 0xb6,
            0x14, (byte) 0xc2, 0x28, (byte) 0xfb, 0x2c, 0x05, 0x2b, 0x67,
            (byte) 0x9a, 0x76, 0x2a, (byte) 0xbe, 0x04, (byte) 0xc3,
            (byte) 0xaa, 0x44, 0x13, 0x26, 0x49, (byte) 0x86, 0x06,
            (byte) 0x99, (byte) 0x9c, 0x42, 0x50, (byte) 0xf4, (byte) 0x91,
            (byte) 0xef, (byte) 0x98, 0x7a, 0x33, 0x54, 0x0b, 0x43,
            (byte) 0xed, (byte) 0xcf, (byte) 0xac, 0x62, (byte) 0xe4,
            (byte) 0xb3, 0x1c, (byte) 0xa9, (byte) 0xc9, 0x08, (byte) 0xe8,
            (byte) 0x95, (byte) 0x80, (byte) 0xdf, (byte) 0x94, (byte) 0xfa,
            0x75, (byte) 0x8f, 0x3f, (byte) 0xa6, 0x47, 0x07, (byte) 0xa7,
            (byte) 0xfc, (byte) 0xf3, 0x73, 0x17, (byte) 0xba, (byte) 0x83,
            0x59, 0x3c, 0x19, (byte) 0xe6, (byte) 0x85, 0x4f, (byte) 0xa8,
            0x68, 0x6b, (byte) 0x81, (byte) 0xb2, 0x71, 0x64, (byte) 0xda,
            (byte) 0x8b, (byte) 0xf8, (byte) 0xeb, 0x0f, 0x4b, 0x70, 0x56,
            (byte) 0x9d, 0x35, 0x1e, 0x24, 0x0e, 0x5e, 0x63, 0x58, (byte) 0xd1,
            (byte) 0xa2, 0x25, 0x22, 0x7c, 0x3b, 0x01, 0x21, 0x78, (byte) 0x87,
            (byte) 0xd4, 0x00, 0x46, 0x57, (byte) 0x9f, (byte) 0xd3, 0x27,
            0x52, 0x4c, 0x36, 0x02, (byte) 0xe7, (byte) 0xa0, (byte) 0xc4,
            (byte) 0xc8, (byte) 0x9e, (byte) 0xea, (byte) 0xbf, (byte) 0x8a,
            (byte) 0xd2, 0x40, (byte) 0xc7, 0x38, (byte) 0xb5, (byte) 0xa3,
            (byte) 0xf7, (byte) 0xf2, (byte) 0xce, (byte) 0xf9, 0x61, 0x15,
            (byte) 0xa1, (byte) 0xe0, (byte) 0xae, 0x5d, (byte) 0xa4,
            (byte) 0x9b, 0x34, 0x1a, 0x55, (byte) 0xad, (byte) 0x93, 0x32,
            0x30, (byte) 0xf5, (byte) 0x8c, (byte) 0xb1, (byte) 0xe3, 0x1d,
            (byte) 0xf6, (byte) 0xe2, 0x2e, (byte) 0x82, 0x66, (byte) 0xca,
            0x60, (byte) 0xc0, 0x29, 0x23, (byte) 0xab, 0x0d, 0x53, 0x4e, 0x6f,
            (byte) 0xd5, (byte) 0xdb, 0x37, 0x45, (byte) 0xde, (byte) 0xfd,
            (byte) 0x8e, 0x2f, 0x03, (byte) 0xff, 0x6a, 0x72, 0x6d, 0x6c, 0x5b,
            0x51, (byte) 0x8d, 0x1b, (byte) 0xaf, (byte) 0x92, (byte) 0xbb,
            (byte) 0xdd, (byte) 0xbc, 0x7f, 0x11, (byte) 0xd9, 0x5c, 0x41,
            0x1f, 0x10, 0x5a, (byte) 0xd8, 0x0a, (byte) 0xc1, 0x31,
            (byte) 0x88, (byte) 0xa5, (byte) 0xcd, 0x7b, (byte) 0xbd, 0x2d,
            0x74, (byte) 0xd0, 0x12, (byte) 0xb8, (byte) 0xe5, (byte) 0xb4,
            (byte) 0xb0, (byte) 0x89, 0x69, (byte) 0x97, 0x4a, 0x0c,
            (byte) 0x96, 0x77, 0x7e, 0x65, (byte) 0xb9, (byte) 0xf1, 0x09,
            (byte) 0xc5, 0x6e, (byte) 0xc6, (byte) 0x84, 0x18, (byte) 0xf0,
            0x7d, (byte) 0xec, 0x3a, (byte) 0xdc, 0x4d, 0x20, 0x79,
            (byte) 0xee, 0x5f, 0x3e, (byte) 0xd7, (byte) 0xcb, 0x39, 0x48 };
        /// <summary>
        /// 系统参数，由4个32 bit的数字组成
        /// </summary>
        private static readonly uint[] FK = { 0xa3b1bac6, 0x56aa3350, 0x677d9197, 0xb27022dc };
        /// <summary>
        /// 固定参数，由32个32 bit的数字组成
        /// </summary>
        private static readonly uint[] CK = { 0x00070e15,0x1c232a31,0x383f464d,0x545b6269,
                                        0x70777e85,0x8c939aa1,0xa8afb6bd,0xc4cbd2d9,
                                        0xe0e7eef5,0xfc030a11,0x181f262d,0x343b4249,
                                        0x50575e65,0x6c737a81,0x888f969d,0xa4abb2b9,
                                        0xc0c7ced5,0xdce3eaf1,0xf8ff060d,0x141b2229,
                                        0x30373e45,0x4c535a61,0x686f767d,0x848b9299,
                                        0xa0a7aeb5,0xbcc3cad1,0xd8dfe6ed,0xf4fb0209,
                                        0x10171e25,0x2c333a41,0x484f565d,0x646b7279 };

        private const int LoopTimes = 32;

        private const int KeySize = 128; // 128 bit
        private const int KeyLength = 32;// 
        private const int BlockSize = 128; // 128 bit

        private const int byteLength = 8; // 一个字节表示8 bit
        private const int wordLength = 32;// 一个word表示32 bit
        private const int wordSize = 4;// 一个word表示4个字节

        private byte GetSboxItem(byte inch) => SboxTable[inch & 0xFF];

        private long CalculateRK(long ka)
        {
            long bb = 0L;
            long rk = 0L;
            byte[] a = new byte[wordSize];
            byte[] b = new byte[wordSize];
            PutUlongToBuffer(ka, a, 0);
            for (int i = 0; i < wordSize; i++)
                b[i] = GetSboxItem(a[i]);
            //b[0] = GetSboxItem(a[0]);
            //b[1] = GetSboxItem(a[1]);
            //b[2] = GetSboxItem(a[2]);
            //b[3] = GetSboxItem(a[3]);
            bb = GetUlongFromBuffer(b, 0);
            rk = bb ^ ExchangeByBit(bb, 13) ^ ExchangeByBit(bb, 23);
            return rk;
        }

        private void GenerateKey(long[] keyBuffer, byte[] key)
        {
            long[] MK = new long[wordSize];
            long[] k = new long[KeyLength + 4];
            int i = 0;
            // 第一步：密钥与系统参数的异或：
            for (i = 0; i < wordSize; i++)
            {
                MK[i] = GetUlongFromBuffer(key, i * 4);
                k[i] = MK[i] ^ (long)FK[i];
            }
            //MK[0] = GetUlongFromBuffer(key, 0);
            //MK[1] = GetUlongFromBuffer(key, 4);
            //MK[2] = GetUlongFromBuffer(key, 8);
            //MK[3] = GetUlongFromBuffer(key, 12);
            //k[0] = MK[0] ^ (long)FK[0];
            //k[1] = MK[1] ^ (long)FK[1];
            //k[2] = MK[2] ^ (long)FK[2];
            //k[3] = MK[3] ^ (long)FK[3];
            // 第二步：获取子密钥：
            for (i = 0; i < KeyLength; i++)
            {
                k[(i + 4)] = (k[i] ^ CalculateRK(k[(i + 1)] ^ k[(i + 2)] ^ k[(i + 3)] ^ (long)CK[i]));
                keyBuffer[i] = k[(i + 4)];
            }
        }

        private long LeftExchange(long ka)
        {
            long bb = 0L;
            long c = 0L;
            byte[] a = new byte[wordSize];
            byte[] b = new byte[wordSize];
            PutUlongToBuffer(ka, a, 0);
            for (int i = 0; i < wordSize; i++)
                b[i] = GetSboxItem(a[i]);
            //b[0] = GetSboxItem(a[0]);
            //b[1] = GetSboxItem(a[1]);
            //b[2] = GetSboxItem(a[2]);
            //b[3] = GetSboxItem(a[3]);
            bb = GetUlongFromBuffer(b, 0);
            c = bb ^ ExchangeByBit(bb, 2) ^ ExchangeByBit(bb, 10) ^ ExchangeByBit(bb, 18) ^ ExchangeByBit(bb, 24);
            return c;
        }

        private long XorFunction(long x0, long x1, long x2, long x3, long rk)
        {
            return x0 ^ LeftExchange(x1 ^ x2 ^ x3 ^ rk);
        }

        private void EncryptOneRound(long[] sk, byte[] input, byte[] output)
        {
            int i = 0;
            long[] ulbuf = new long[36];
            ulbuf[0] = GetUlongFromBuffer(input, 0);
            ulbuf[1] = GetUlongFromBuffer(input, 4);
            ulbuf[2] = GetUlongFromBuffer(input, 8);
            ulbuf[3] = GetUlongFromBuffer(input, 12);
            while (i < LoopTimes)
            {
                ulbuf[(i + 4)] = XorFunction(ulbuf[i], ulbuf[(i + 1)], ulbuf[(i + 2)], ulbuf[(i + 3)], sk[i]);
                i++;
            }
            PutUlongToBuffer(ulbuf[35], output, 0);
            PutUlongToBuffer(ulbuf[34], output, 4);
            PutUlongToBuffer(ulbuf[33], output, 8);
            PutUlongToBuffer(ulbuf[32], output, 12);
        }

        private byte[] Padding(byte[] input)
        {
            if (input == null)
            {
                return null;
            }

            byte[] ret = (byte[])null;
            if (Mode == CryptModes.Encrypt)
            {
                int p = 16 - input.Length % 16;
                ret = new byte[input.Length + p];
                Array.Copy(input, 0, ret, 0, input.Length);
                for (int i = 0; i < p; i++)
                {
                    ret[input.Length + i] = (byte)p;
                }
            }
            else
            {
                int p = input[input.Length - 1];
                ret = new byte[input.Length - p];
                Array.Copy(input, 0, ret, 0, input.Length - p);
            }
            return ret;
        }

        public byte[] EcbCrypt(byte[] input)
        {
            if (IsPadding && (Mode == CryptModes.Encrypt))
            {
                input = Padding(input);
            }

            int length = input.Length;
            byte[] bins = new byte[length];
            Array.Copy(input, 0, bins, 0, length);
            byte[] bous = new byte[length];
            for (int i = 0; length > 0; length -= 16, i++)
            {
                byte[] inBytes = new byte[16];
                byte[] outBytes = new byte[16];
                Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
                EncryptOneRound(KeyBuffer, inBytes, outBytes);
                Array.Copy(outBytes, 0, bous, i * 16, length > 16 ? 16 : length);
            }

            if (IsPadding && Mode == CryptModes.Decrypt)
            {
                bous = Padding(bous);
            }
            return bous;
        }

        //public byte[] CbcCrypt(byte[] iv, byte[] input)
        //{
        //	if (IsPadding && (Mode == CryptModes.Encrypt))
        //	{
        //		input = Padding(input);
        //	}

        //	int i = 0;
        //	int length = input.Length;
        //	byte[] bins = new byte[length];
        //	Array.Copy(input, 0, bins, 0, length);
        //	byte[] bous = null;
        //	List<byte> bousList = new List<byte>();
        //	if (Mode == CryptModes.Encrypt)
        //	{
        //		for (int j = 0; length > 0; length -= 16, j++)
        //		{
        //			byte[] inBytes = new byte[16];
        //			byte[] outBytes = new byte[16];
        //			byte[] out1 = new byte[16];

        //			Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
        //			for (i = 0; i < 16; i++)
        //			{
        //				outBytes[i] = ((byte)(inBytes[i] ^ iv[i]));
        //			}
        //			EncryptOneRound(KeyBuffer, outBytes, out1);
        //			Array.Copy(out1, 0, iv, 0, 16);
        //			for (int k = 0; k < 16; k++)
        //			{
        //				bousList.Add(out1[k]);
        //			}
        //		}
        //	}
        //	else
        //	{
        //		byte[] temp = new byte[16];
        //		for (int j = 0; length > 0; length -= 16, j++)
        //		{
        //			byte[] inBytes = new byte[16];
        //			byte[] outBytes = new byte[16];
        //			byte[] out1 = new byte[16];

        //			Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
        //			Array.Copy(inBytes, 0, temp, 0, 16);
        //			EncryptOneRound(KeyBuffer, inBytes, outBytes);
        //			for (i = 0; i < 16; i++)
        //			{
        //				out1[i] = ((byte)(outBytes[i] ^ iv[i]));
        //			}
        //			Array.Copy(temp, 0, iv, 0, 16);
        //			for (int k = 0; k < 16; k++)
        //			{
        //				bousList.Add(out1[k]);
        //			}
        //		}

        //	}

        //	if (IsPadding && Mode == CryptModes.Decrypt)
        //	{
        //		bous = Padding(bousList.ToArray());
        //		return bous;
        //	}
        //	else
        //	{
        //		return bousList.ToArray();
        //	}
        //}

        public enum CryptTypes : uint
        {
            ECB = 0x01,
            CBC = 0x02,
        }

        public enum CryptModes : uint
        {
            Encrypt = 0x01,
            Decrypt = 0x00,
        }

        private CryptTypes m_CryptType = CryptTypes.ECB;

        private CryptModes m_Mode = CryptModes.Encrypt;

        private bool m_IsPadding = true;

        private long[] m_KeyBuffer = new long[32];

        internal CryptModes Mode { get => m_Mode; set => m_Mode = value; }

        public bool IsPadding { get => m_IsPadding; private set => m_IsPadding = value; }

        public long[] KeyBuffer { get => m_KeyBuffer; private set => m_KeyBuffer = value; }

        internal CryptTypes CryptType { get => m_CryptType; private set => m_CryptType = value; }

        public SM4ServiceProvider(CryptModes mode = CryptModes.Encrypt, bool isPadding = true)
        {
            CryptType = CryptTypes.ECB;
            Mode = mode;
            IsPadding = isPadding;
        }

        public void PrepareKey(byte[] key)
        {
            KeyBuffer = new long[32];
            GenerateKey(KeyBuffer, key);
            if (Mode == CryptModes.Decrypt)
            {
                for (int i = 0; i < 16; i++)
                {
                    SWAP(KeyBuffer, i);
                }
            }
        }
    }
    public sealed class SM4 : SecurityBase
    {
        protected override uint KeyLen => 128;

        protected override uint IvLen => 0;

        protected override Encoding KeyEncoder => Encoding.UTF8;

        protected override byte[] GetKeyByte(string key)
        {
            return KeyEncoder.GetBytes(key);
        }

        private SM4ServiceProvider m_Provider = new SM4ServiceProvider();

        public override byte[] Decrypt(byte[] src, string key)
        {
            m_Provider.Mode = SM4ServiceProvider.CryptModes.Decrypt;
            m_Provider.PrepareKey(GetKeyByte(key));
            return m_Provider.EcbCrypt(Hex.Decode(src));
        }

        public override byte[] Encrypt(byte[] src, string key)
        {
            m_Provider.Mode = SM4ServiceProvider.CryptModes.Encrypt;
            m_Provider.PrepareKey(GetKeyByte(key));
            return Hex.Encode(m_Provider.EcbCrypt(src));
        }

        protected override void Clear()
        {

        }

        protected override void PrepareKey(string key, out byte[] bzKey, out byte[] bzIv)
        {
            bzKey = GetKeyByte(key);
            bzIv = null;
            m_Provider.PrepareKey(bzKey);
        }

        public static SM4 Instance { get { return new SM4(); } }

        public static string Encrypt_Utf8(string data, string key)
            => Instance.Encrypt(data, key, Encoding.UTF8, Bytes_Display_Mode.Utf8);
        public static string Decrypt_Utf8(string data, string key)
            => Instance.Decrypt(data, key, Encoding.UTF8, Bytes_Display_Mode.Utf8);
    }
}
