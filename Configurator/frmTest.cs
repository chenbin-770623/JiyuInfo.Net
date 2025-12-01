using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.Configurator
{
    using System.Web.Script.Serialization;

    internal partial class frmTest : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public frmTest()
        {
            InitializeComponent();
        }

        private TestPara m_Paras = new TestPara();        

        private void frmTest_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(m_Paras.FilePath))
            {
                this.textBox1.Text = LoadFile(m_Paras.FilePath);
            }
            string strCode = Application.StartupPath + @"\code.txt";
            if (System.IO.File.Exists(strCode))
            {
                this.textBox2.Text = LoadFile(strCode);
            }

            System.Web.Script.Serialization.JavaScriptSerializer jss = new JavaScriptSerializer();
            //MessageBox.Show(jss.Serialize(100));
            //MessageBox.Show(jss.Serialize(true));
            CustPara c = new CustPara();
            c.IntItem = 1000;
            c.StrItem = "hello world";
            string ss = jss.Serialize(c);
            MessageBox.Show(ss);
            MessageBox.Show(c.ToString());
            System.Collections.Generic.Dictionary<string, string> dd = new Dictionary<string, string>();
            dd.Add("IntItem", "1000");
            dd.Add("StrItem", "hello world");
            string[] srList = new string[] {"this","is","a","test" };
            int[] intList = new int[] { 10, 20, 30, 40, 50 };
            System.Collections.IList list = srList;            
            dd.Add("StrList", jss.Serialize(list));
            list = intList;
            dd.Add("IntList", jss.Serialize(list));
            System.Collections.ArrayList al = new System.Collections.ArrayList(new object[] { 100, "2345", true });
            list = al;
            dd.Add("List", jss.Serialize(list));
            ss = jss.Serialize(dd);
            MessageBox.Show(ss);
        }

        private string LoadFile(string file)
        {
            string ret = string.Empty;
            if (System.IO.File.Exists(file))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    byte[] bzInfo = new byte[fs.Length];
                    fs.Read(bzInfo, 0, bzInfo.Length);
                    fs.Close();
                    ret = System.Text.Encoding.Default.GetString(bzInfo);
                }
            }
            return ret;
        }

        private class TestPara : FileConfigurator
        {
            private int m_IntItem = 10;
            [Browsable(true)]
            [Category("Base")]
            [Description("参数:int")]
            public int IntItem
            {
                get { return m_IntItem; }
                set { m_IntItem = value; }
            }

            private string m_StrItem = "abcd";
            [Category("Base")]
            [Description("参数:string")]
            public string StrItem
            {
                get { return m_StrItem; }
                set { m_StrItem = value; }
            }

            private string m_UnBrowseItem = "hello";
            [Browsable(false)]
            [Category("Base")]
            [Description("参数:string,不显示")]
            public string UnBrowseItem
            {
                get { return m_UnBrowseItem; }
                set { m_UnBrowseItem = value; }
            }

            private bool m_BoolItem = false;
            [Category("Base")]
            [Description("参数:bool")]
            public bool BoolItem
            {
                get { return m_BoolItem; }
                set { m_BoolItem = value; }
            }

            private DateTime m_DtItem = DateTime.Now;
            [Description("参数:datetime,按DateTimeFormatAttribute指定显示格式,按JYDateTimeEditor指定Editor,按JYDateTimeConverter指定TypeConverter")]
            [DateTimeFormat(DatetimeFormat=DateTimeFormatAttribute.Date)]
            [Editor(typeof(JYDateTimeEditor),typeof(System.Drawing.Design.UITypeEditor))]
            [TypeConverter(typeof(JYDateTimeConverter))]
            public DateTime DtItem
            {
                get { return m_DtItem; }
                set { m_DtItem = value; }
            }

            private TestEnum m_EnumItem = TestEnum.Unkown;
            [Description("参数:enum")]
            public TestEnum EnumItem
            {
                get { return m_EnumItem; }
                set { m_EnumItem = value; }
            }

            private string m_PassItem = "p@ssw0rd";
            [Description("参数:string,按PasswordPropertyText指定密码文本")]
            [PasswordPropertyText(true)]
            public string PassItem
            {
                get { return m_PassItem; }
                set { m_PassItem = value; }
            }

            private CustPara m_CustItem = new CustPara();
            [Description("参数:自定义类型,需要继承自CustomerTypeBase")]
            public CustPara CustItem
            {
                get { return m_CustItem; }
                set { m_CustItem = value; }
            }

            private string[] m_StrListItem = new string[] { "hello", "world", "!" };
            [Description("参数:数组")]
            public string[] StrListItem
            {
                get { return m_StrListItem; }
                set { m_StrListItem = value; }
            }
        }

        private class CustPara : CustomerTypeBase
        {
            private int m_IntItem = 10;
            [Browsable(true)]
            [Category("Base")]
            [Description("参数:int")]            
            public int IntItem
            {
                get { return m_IntItem; }
                set { m_IntItem = value; }
            }

            private string m_StrItem = "abcd";
            [Category("Base")]
            [Description("参数:string")]
            public string StrItem
            {
                get { return m_StrItem; }
                set { m_StrItem = value; }
            }

            private string m_UnBrowseItem = "hello";
            [Browsable(false)]
            [Category("Base")]
            [Description("参数:string,不显示")]
            public string UnBrowseItem
            {
                get { return m_UnBrowseItem; }
                set { m_UnBrowseItem = value; }
            }

            private bool m_BoolItem = false;
            [Category("Base")]
            [Description("参数:bool")]
            public bool BoolItem
            {
                get { return m_BoolItem; }
                set { m_BoolItem = value; }
            }

            private DateTime m_DtItem = DateTime.Now;
            [Description("参数:datetime,按DateTimeFormatAttribute指定显示格式,按JYDateTimeEditor指定Editor,按JYDateTimeConverter指定TypeConverter")]
            [DateTimeFormat(DatetimeFormat = DateTimeFormatAttribute.Date)]
            [Editor(typeof(JYDateTimeEditor), typeof(System.Drawing.Design.UITypeEditor))]
            [TypeConverter(typeof(JYDateTimeConverter))]
            public DateTime DtItem
            {
                get { return m_DtItem; }
                set { m_DtItem = value; }
            }

            private TestEnum m_EnumItem = TestEnum.Unkown;
            [Description("参数:enum")]
            public TestEnum EnumItem
            {
                get { return m_EnumItem; }
                set { m_EnumItem = value; }
            }

            private string m_PassItem = "p@ssw0rd";
            [Description("参数:string,按PasswordPropertyText指定密码文本")]
            [PasswordPropertyText(true)]
            public string PassItem
            {
                get { return m_PassItem; }
                set { m_PassItem = value; }
            }
        }        

        public enum TestEnum : uint
        {
            Unkown = 0x00,
            First ,
            Second,
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_Paras.Read();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_Paras.UIShow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_Paras.Write();
        }
    }
}
