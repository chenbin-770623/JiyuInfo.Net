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
    internal partial class frmConfigurator : Form
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public frmConfigurator()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="obj"></param>
        public frmConfigurator(object obj)
            : this()
        {
            this.grid.SelectedObject = obj;
        }
    }
}
