using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.Logger
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
            this.comboBox1.Items.AddRange(Enum.GetNames(typeof(JY_Log_Level)));
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text.Trim())) return;
            this.fileLogger1.Write((JY_Log_Level)Enum.Parse(typeof(JY_Log_Level), this.comboBox1.SelectedItem.ToString()), this.textBox1.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
