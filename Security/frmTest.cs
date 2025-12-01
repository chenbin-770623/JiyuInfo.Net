using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.Security
{
    using Org.BouncyCastle.Utilities.Encoders;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
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
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTest_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.AddRange(Enum.GetNames(typeof(Security_Algorithm)));
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.Items.AddRange(Enum.GetNames(typeof(Bytes_Display_Mode)));
            this.comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPlain.Text.Trim())) return;
            this.factory1.Algorithm = (Security_Algorithm)Enum.Parse(typeof(Security_Algorithm), this.comboBox1.SelectedItem.ToString());
            this.factory1.Display_Mode = (Bytes_Display_Mode)Enum.Parse(typeof(Bytes_Display_Mode), this.comboBox2.SelectedItem.ToString());
            string strPwd = this.textBox1.Text.Trim();
            this.txtSecurity.Text = this.factory1.Encrypt(this.txtPlain.Text.Trim(), strPwd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSecurity.Text.Trim())) return;
            this.factory1.Algorithm = (Security_Algorithm)Enum.Parse(typeof(Security_Algorithm), this.comboBox1.SelectedItem.ToString());
            this.factory1.Display_Mode = (Bytes_Display_Mode)Enum.Parse(typeof(Bytes_Display_Mode), this.comboBox2.SelectedItem.ToString());
            string strPwd = this.textBox1.Text.Trim();
            this.txtPlain.Text = this.factory1.Decrypt(this.txtSecurity.Text.Trim(), strPwd);
        }

        private void button3_Click(object sender, EventArgs e)
        {
			string data = "{'name':'张三','phone':'13895489634','qq':'15462884'}";
			string key = "d76b58bdd5443724";

			//SM4Utils handler = new SM4Utils();
			//handler.secretKey = key;
			//handler.hexString = false;
			string rst = "d54728e7e7183e45958cfd88c800bdf1d472e51f69f280aca0acc697643ddab8a003ce06733bc6f99fb3f80e8cdec4e900223cfc3f807eb9fccfebffb9345332";
			//string erc = handler.Encrypt_CBC(data);
   //         MessageBox.Show(erc);
   //         MessageBox.Show(string.Format("{0}", erc.Equals(rst)));
   //         MessageBox.Show(handler.Decrypt_CBC(rst));
			string rr = SM4.Encrypt_Utf8(data, key);
			MessageBox.Show(rr);
			MessageBox.Show(string.Format("{0}", rr.Equals(rst)));

			string rrs = SM4.Decrypt_Utf8(rr, key);
			MessageBox.Show(rrs);

            string timestamp = "1652424055";
			string sm3data = rr + timestamp + key;
			SM3 handler3 = new SM3();

			byte[] sm3rst = new byte[32];
			byte[] sm3byte = Encoding.UTF8.GetBytes(sm3data);
			handler3.BlockUpdate(sm3byte, 0, sm3byte.Length);
			handler3.DoFinal(sm3rst, 0);
			string sm3ret = new UTF8Encoding().GetString(Hex.Encode(sm3rst));
            MessageBox.Show(sm3ret);
            MessageBox.Show(sm3ret.Equals("a1ad83a0a73f4dfd5cefa8bcb0fe5cf0b29b13a158e6f013a88bb66f80b054e9").ToString());
            //    byte[] md = new byte[32];
            //    byte[] msg1 = Encoding.Default.GetBytes("ererfeiisgod");
            //    SM3Digest sm3 = new SM3Digest();
            //    sm3.BlockUpdate(msg1, 0, msg1.Length);
            //    sm3.DoFinal(md, 0);
            //    System.String s = new UTF8Encoding().GetString(Hex.Encode(md));
            //    System.Console.Out.WriteLine(s.ToUpper());

            //SM4ServiceProvider cryp = new SM4ServiceProvider();
            //cryp.PrepareKey(Encoding.UTF8.GetBytes(key));
            //string cr = Encoding.UTF8.GetString(Hex.Encode(cryp.EcbCrypt(Encoding.UTF8.GetBytes(data))));
            //         MessageBox.Show(string.Format("{0}", cr.Equals(rst)));

            //cryp.Mode = SM4ServiceProvider.CryptModes.Decrypt;
            //cryp.PrepareKey(Encoding.UTF8.GetBytes(key));
            //string dcr = Encoding.UTF8.GetString(cryp.EcbCrypt(Hex.Decode(Encoding.UTF8.GetBytes(cr))));
            //MessageBox.Show(dcr);

            //SM4_ECB sm4 = new SM4_ECB();
        }
	}

}
