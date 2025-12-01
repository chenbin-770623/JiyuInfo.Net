using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JiYuInfo.WmiLib
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmTest : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public frmTest()
        {
            InitializeComponent();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {

        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            AddItem("HostName", ComputerInfo.HostName());
            System.Collections.Hashtable htList = ComputerInfo.HostNetwork();
            foreach (string key in htList.Keys)
            {
                System.Collections.Hashtable value = htList[key] as System.Collections.Hashtable;
                string strKey = string.Format("网卡 {0}", key);
                AddItem(strKey, "", "Network");
                foreach (string property in value.Keys)
                {
                    object propertyValue = value[property];
                    string strPropertyValue = propertyValue == null ? "" : propertyValue.ToString();
                    string strValue = string.Format("{0}={1}", property, strPropertyValue);
                    AddItem("", strValue, "Network");
                }
            }
            htList = ComputerInfo.HostCpu();
            foreach (string key in htList.Keys)
            {
                System.Collections.Hashtable value = htList[key] as System.Collections.Hashtable;
                string strKey = string.Format("{0}", key);
                AddItem(strKey, "", "CPU");
                foreach (string property in value.Keys)
                {
                    object propertyValue = value[property];
                    string strPropertyValue = propertyValue == null ? "" : propertyValue.ToString();
                    string strValue = string.Format("{0}={1}", property, strPropertyValue);
                    AddItem("", strValue, "CPU");
                }
            }
            htList = ComputerInfo.HostDisk();
            foreach (string key in htList.Keys)
            {
                System.Collections.Hashtable value = htList[key] as System.Collections.Hashtable;
                string strKey = string.Format("{0}", key);
                AddItem(strKey, "", "Disk");
                foreach (string property in value.Keys)
                {
                    object propertyValue = value[property];
                    string strPropertyValue = propertyValue == null ? "" : propertyValue.ToString();
                    string strValue = string.Format("{0}={1}", property, strPropertyValue);
                    AddItem("", strValue, "Disk");
                }
            }
            htList = ComputerInfo.HostOS();
            foreach (string key in htList.Keys)
            {
                System.Collections.Hashtable value = htList[key] as System.Collections.Hashtable;
                string strKey = string.Format("{0}", key);
                AddItem(strKey, "", "OS");
                foreach (string property in value.Keys)
                {
                    object propertyValue = value[property];
                    string strPropertyValue = propertyValue == null ? "" : propertyValue.ToString();
                    string strValue = string.Format("{0}={1}", property, strPropertyValue);
                    AddItem("", strValue, "OS");
                }
            }
            htList = ComputerInfo.HostMemory();
            foreach (string key in htList.Keys)
            {
                System.Collections.Hashtable value = htList[key] as System.Collections.Hashtable;
                string strKey = string.Format("{0}", key);
                AddItem(strKey, "", "Memory");
                foreach (string property in value.Keys)
                {
                    object propertyValue = value[property];
                    string strPropertyValue = propertyValue == null ? "" : propertyValue.ToString();
                    string strValue = string.Format("{0}={1}", property, strPropertyValue);
                    AddItem("", strValue, "Memory");
                }
            }
            string[] list = ComputerInfo.HostIpV4();
            for (int i = 0; i < list.Length; i++)
                AddItem(string.Format("IPV4,{0}", i + 1), list[i]);
            list = ComputerInfo.HostMac();
            for (int i = 0; i < list.Length; i++)
                AddItem(string.Format("MAC,{0}", i + 1), list[i]);
            list = ComputerInfo.HostCpuId();
            for (int i = 0; i < list.Length; i++)
                AddItem(string.Format("CPUID,{0}", i + 1), list[i]);
            list = ComputerInfo.HostDiskSN();
            for (int i = 0; i < list.Length; i++)
                AddItem(string.Format("DiskSN,{0}", i + 1), list[i]);
        }

        private void AddItem(string name, string value)
        {
            this.lvInfos.Items.Add(new ListViewItem(new string[] { name, value }));
        }

        private void AddItem(string name, string value,string group)
        {
            ListViewItem lvAdd = new ListViewItem(new string[] { name, value });
            lvAdd.Group = this.lvInfos.Groups[group];
            this.lvInfos.Items.Add(lvAdd);
        }
    }
}
