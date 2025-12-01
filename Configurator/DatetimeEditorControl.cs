using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.Configurator
{
    /// <summary>
    /// 自定义的时间编辑控件
    /// </summary>
    public partial class JYDatetimeEditorControl : UserControl
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public JYDatetimeEditorControl()
        {
            InitializeComponent();
            SetValue(DateTime.Now);
            this.Size = LongSize;
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="val"></param>
        public JYDatetimeEditorControl(DateTime val)
            : this()
        {
            SetValue(val);
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="val"></param>
        /// <param name="format"></param>
        public JYDatetimeEditorControl(DateTime val, DateTimeFormatAttribute format)
            : this(val)
        {
            Format = format;
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="format"></param>
        public JYDatetimeEditorControl(DateTimeFormatAttribute format)
            : this()
        {
            Format = format;
        }

        private DateTime m_Value = DateTimePicker.MinimumDateTime;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Value
        {
            get { return m_Value; }
            //set { if (m_Value != value) { m_Value = value; SetValue(); } }
        }

        private DateTimeFormatAttribute m_Format = new DateTimeFormatAttribute(DateTimeFormatAttribute.Long);
        /// <summary>
        /// 显示格式
        /// </summary>
        public DateTimeFormatAttribute Format
        {
            get { return m_Format; }
            set { m_Format = value; ShowByFormat(); GetValue(); }
        }

        private Size LongSize = new Size(245, 215);
        private Size DateSize = new Size(245, 183);
        private Size TimeSize = new Size(245, 28);

        private string GetCurrentTimeFormat()
        {
            if (Format.CheckHasMillSecond()) return "HH:mm:ss.fff";
            else return "HH:mm:ss";
        }
        /// <summary>
        /// 按显示格式显示
        /// </summary>
        private void ShowByFormat()
        {
            if (Format.CheckHasDate())
            {
                if (Format.CheckHasTime())
                {
                    this.Size = LongSize;
                    this.splitContainer1.Panel1Collapsed = false;
                    this.splitContainer1.Panel2Collapsed = false;
                }
                else
                {
                    this.Size = DateSize;
                    this.splitContainer1.Panel1Collapsed = false;
                    this.splitContainer1.Panel2Collapsed = true;
                }
            }
            else
            {
                if (Format.CheckHasTime())
                {
                    this.Size = TimeSize;
                    this.splitContainer1.Panel1Collapsed = true;
                    this.splitContainer1.Panel2Collapsed = false;
                }
                else
                {
                    this.Size = LongSize;
                    this.splitContainer1.Panel1Collapsed = false;
                    this.splitContainer1.Panel2Collapsed = false;
                }
            }
            this.dtMillSecond.Visible = Format.CheckHasMillSecond();
        }
        /// <summary>
        /// 设置时间
        /// </summary>
        public void SetValue(DateTime val)
        {
            IsSetValueOver = false;
            this.dtDate.SetDate(val);
            this.dtTime.Value = val;
            this.dtMillSecond.Value = val.Millisecond;
            this.dtDate.BoldedDates = new DateTime[] { val };
            ResetValue(val);
            IsSetValueOver = true;
        }

        private void DatetimeEditorControl_Load(object sender, EventArgs e)
        {
            this.getTime.Interval = 50;
            this.getTime.Enabled = true;
        }

        private bool m_IsSetValueOver = false;

        private bool IsSetValueOver
        {
            get { return m_IsSetValueOver; }
            set { m_IsSetValueOver = value; }
        }

        //public static DateTime JYConverter.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //public static DateTime MaxValue = new DateTime(2050, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private void ResetValue(DateTime val)
        {
            int year = Format.CheckHasDate() ? val.Year : JYConverter.MinDate.Year;
            int month = Format.CheckHasDate() ? val.Month : JYConverter.MinDate.Month;
            int day = Format.CheckHasDate() ? val.Day : JYConverter.MinDate.Day;
            int hour = Format.CheckHasTime() ? val.Hour : JYConverter.MinDate.Hour;
            int minute = Format.CheckHasTime() ? val.Minute : JYConverter.MinDate.Minute;
            int second = Format.CheckHasTime() ? val.Second: JYConverter.MinDate.Second;
            int millsec = Format.CheckHasMillSecond() ? val.Millisecond : JYConverter.MinDate.Millisecond;
            this.m_Value = new DateTime(year, month, day, hour, minute, second, millsec);
        }

        private void GetValue()
        {
            if (IsSetValueOver)
            {
                int year = Format.CheckHasDate() ? this.dtDate.SelectionStart.Year : JYConverter.MinDate.Year;
                int month = Format.CheckHasDate() ? this.dtDate.SelectionStart.Month : JYConverter.MinDate.Month;
                int day = Format.CheckHasDate() ? this.dtDate.SelectionStart.Day : JYConverter.MinDate.Day;
                int hour = Format.CheckHasTime() ? this.dtTime.Value.Hour : JYConverter.MinDate.Hour;
                int minute = Format.CheckHasTime() ? this.dtTime.Value.Minute  : JYConverter.MinDate.Minute;
                int second = Format.CheckHasTime() ? this.dtTime.Value.Second : JYConverter.MinDate.Second;
                int millsec = Format.CheckHasMillSecond() ? int.Parse(this.dtMillSecond.Value.ToString()) : JYConverter.MinDate.Millisecond;
                m_Value = new DateTime(year, month, day, hour, minute, second, millsec);
            }
        }

        private void dtDate_DateSelected(object sender, DateRangeEventArgs e)
        {
            GetValue();
        }

        private void dtTime_ValueChanged(object sender, EventArgs e)
        {
            GetValue();
        }

        private void dtMillSecond_ValueChanged(object sender, EventArgs e)
        {
            GetValue();
        }

        private void getTime_Tick(object sender, EventArgs e)
        {
            this.lblCurrentTime.Text = DateTime.Now.ToString(GetCurrentTimeFormat());
        }

        private void DatetimeEditorControl_Resize(object sender, EventArgs e)
        {
            ShowByFormat();
        }

        private void lblSetCurrent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.dtTime.Value = DateTime.Now;
            this.dtMillSecond.Value = DateTime.Now.Millisecond;
        }
    }
}
