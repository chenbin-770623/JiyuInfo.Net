using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiYuInfo.WmiLib
{
    using System.Net;
    using System.Management;

    /// <summary>
    /// 获取计算机信息
    /// </summary>
    public static class ComputerInfo
    {
        /// <summary>
        /// 获取本机名称
        /// </summary>
        /// <returns></returns>
        public static string HostName()
        {
            return Dns.GetHostName();
        }
        /// <summary>
        /// 获取网络信息
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Hashtable HostNetwork()
        {
            System.Collections.Hashtable ret = new System.Collections.Hashtable();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                System.Collections.Hashtable instance = new System.Collections.Hashtable();
                string strKey = mo["Index"].ToString();
                //instance.Add("Index", mo["Index"]);
                //instance.Add("Caption", mo["Caption"]);
                instance.Add("Description", mo["Description"]);
                instance.Add("IPEnabled", mo["IPEnabled"]);
                instance.Add("MacAddress", mo["MacAddress"]);
                if ((bool)mo["IPEnabled"] == true)
                {
                    instance.Add("DHCPEnabled", mo["DHCPEnabled"]);
                    if ((bool)mo["DHCPEnabled"] == true)
                        instance.Add("DHCPServer", mo["DHCPServer"]);                    
                    instance.Add("IPAddress", ((string[])mo["IPAddress"])[0]);
                    instance.Add("IPSubnet", ((string[])mo["IPSubnet"])[0]);
                    if (mo["DefaultIPGateway"] != null)
                        if (((string[])mo["DefaultIPGateway"]).Length > 0)
                            instance.Add("DefaultIPGateway", ((string[])mo["DefaultIPGateway"])[0]);
                    string[] dns = (string[])mo["DNSServerSearchOrder"];
                    if(dns != null)
                        for (int i = 0; i < dns.Length; i++)
                            instance.Add(string.Format("dns {0}", i + 1), dns[i]);
                }
                ret.Add(strKey, instance);
            }
            return ret;
        }
        /// <summary>
        /// 获取cpu信息
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Hashtable HostCpu()
        {
            System.Collections.Hashtable ret = new System.Collections.Hashtable();
            ManagementClass mc = new ManagementClass("Win32_Processor");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                System.Collections.Hashtable instance = new System.Collections.Hashtable();
                string strKey = mo["DeviceID"].ToString();
                System.Collections.Hashtable htArchitecture = new System.Collections.Hashtable();
                htArchitecture.Add((UInt16)0, "x86");
                htArchitecture.Add((UInt16)1, "MIPS");
                htArchitecture.Add((UInt16)2, "Alpha");
                htArchitecture.Add((UInt16)3, "PowerPC");
                htArchitecture.Add((UInt16)5, "ARM");
                htArchitecture.Add((UInt16)6, "Itanium-based systems");
                htArchitecture.Add((UInt16)9, "x64");
                if (mo["Architecture"] != null)
                    if (htArchitecture.ContainsKey((UInt16)mo["Architecture"]))
                        instance.Add("Architecture", htArchitecture[(UInt16)mo["Architecture"]]);
                //instance.Add("DeviceID", mo["DeviceID"]);
                instance.Add("Description", mo["Description"]);
                instance.Add("Manufacturer", mo["Manufacturer"]);
                instance.Add("CurrentVoltage", mo["CurrentVoltage"]);
                instance.Add("CurrentClockSpeed", mo["CurrentClockSpeed"]);
                instance.Add("DataWidth", mo["DataWidth"]);
                instance.Add("L2CacheSize", mo["L2CacheSize"]);
                //instance.Add("L2CacheSpeed", mo["L2CacheSpeed"]);
                instance.Add("L3CacheSize", mo["L3CacheSize"]);
                //instance.Add("L3CacheSpeed", mo["L3CacheSpeed"]);
                instance.Add("MaxClockSpeed", mo["MaxClockSpeed"]);
                instance.Add("Name", mo["Name"]);
                instance.Add("NumberOfCores", mo["NumberOfCores"]);
                instance.Add("NumberOfLogicalProcessors", mo["NumberOfLogicalProcessors"]);
                instance.Add("ProcessorId", mo["ProcessorId"]);
                System.Collections.Hashtable htProcessorType = new System.Collections.Hashtable();
                htProcessorType.Add((UInt16)1, "Other");
                htProcessorType.Add((UInt16)2, "Unknown");
                htProcessorType.Add((UInt16)3, "Central Processor");
                htProcessorType.Add((UInt16)4, "Math Processor");
                htProcessorType.Add((UInt16)5, "DSP Processor");
                htProcessorType.Add((UInt16)6, "Video Processor");
                if (mo["ProcessorType"] != null)
                    if (htProcessorType.ContainsKey((UInt16)mo["ProcessorType"]))
                        instance.Add("ProcessorType", htProcessorType[(UInt16)mo["ProcessorType"]]);
                instance.Add("Status", mo["Status"]);

                ret.Add(strKey, instance);
            }
            return ret;
        }
        /// <summary>
        /// 获取磁盘信息
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Hashtable HostDisk()
        {
            System.Collections.Hashtable ret = new System.Collections.Hashtable();
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                System.Collections.Hashtable instance = new System.Collections.Hashtable();
                string strKey = mo["DeviceID"].ToString();

                //instance.Add("DeviceID", strKey);
                instance.Add("Caption", mo["Caption"]);
                instance.Add("Description", mo["Description"]);
                instance.Add("FirmwareRevision", mo["FirmwareRevision"]);
                instance.Add("InterfaceType", mo["InterfaceType"]);
                instance.Add("Manufacturer", mo["Manufacturer"]);
                instance.Add("Model", mo["Model"]);
                instance.Add("Name", mo["Name"]);
                instance.Add("Partitions", mo["Partitions"]);
                instance.Add("PNPDeviceID", mo["PNPDeviceID"]);
                instance.Add("SerialNumber", mo["SerialNumber"]);
                instance.Add("Size", mo["Size"]);
                instance.Add("Status", mo["Status"]);
                instance.Add("TotalCylinders", mo["TotalCylinders"]);
                instance.Add("TotalHeads", mo["TotalHeads"]);
                instance.Add("TotalSectors", mo["TotalSectors"]);
                instance.Add("TotalTracks", mo["TotalTracks"]);
                instance.Add("TracksPerCylinder", mo["TracksPerCylinder"]);
                
                ret.Add(strKey, instance);
            }
            return ret;
        }
        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Hashtable HostOS()
        {
            System.Collections.Hashtable ret = new System.Collections.Hashtable();
            ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                System.Collections.Hashtable instance = new System.Collections.Hashtable();
                string strKey = mo["Caption"].ToString();

                instance.Add("BootDevice", mo["BootDevice"]);
                instance.Add("BuildNumber", mo["BuildNumber"]);
                instance.Add("BuildType", mo["BuildType"]);
                instance.Add("CodeSet", mo["CodeSet"]);
                instance.Add("CSName", mo["CSName"]);
                instance.Add("CountryCode", mo["CountryCode"]);
                instance.Add("Manufacturer", mo["Manufacturer"]);
                instance.Add("Name", mo["Name"]);
                instance.Add("SerialNumber", mo["SerialNumber"]);
                instance.Add("ServicePackMajorVersion", mo["ServicePackMajorVersion"]);
                instance.Add("ServicePackMinorVersion", mo["ServicePackMinorVersion"]);
                instance.Add("Status", mo["Status"]);
                instance.Add("SystemDevice", mo["SystemDevice"]);
                instance.Add("SystemDirectory", mo["SystemDirectory"]);
                instance.Add("SystemDrive", mo["SystemDrive"]);
                instance.Add("Version", mo["Version"]);
                instance.Add("WindowsDirectory", mo["WindowsDirectory"]);

                instance.Add("OSArchitecture", mo["OSArchitecture"]);

                instance.Add("TotalVirtualMemorySize", mo["TotalVirtualMemorySize"]);
                instance.Add("TotalVisibleMemorySize", mo["TotalVisibleMemorySize"]);

                ret.Add(strKey, instance);
            }
            return ret;
        }
        /// <summary>
        /// 获取内存条信息
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Hashtable HostMemory()
        {
            System.Collections.Hashtable ret = new System.Collections.Hashtable();
            ManagementClass mc = new ManagementClass("Win32_PhysicalMemory");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                System.Collections.Hashtable instance = new System.Collections.Hashtable();
                string strKey = mo["Tag"].ToString();

                instance.Add("DataWidth", mo["DataWidth"]);
                instance.Add("DeviceLocator", mo["DeviceLocator"]);
                instance.Add("Capacity", mo["Capacity"]);
                instance.Add("Manufacturer", mo["Manufacturer"]);
                instance.Add("Name", mo["Name"]);
                instance.Add("SerialNumber", mo["SerialNumber"]);
                instance.Add("TypeDetail", mo["TypeDetail"]);
                instance.Add("Speed", mo["Speed"]);
                instance.Add("TotalWidth", mo["TotalWidth"]);

                ret.Add(strKey, instance);
            }
            return ret;
        }
        /// <summary>
        /// 获取IPV4的地址
        /// </summary>
        /// <returns></returns>
        public static string[] HostIpV4()
        {
            System.Collections.ArrayList alIps = new System.Collections.ArrayList();
            System.Net.IPHostEntry localHost = System.Net.Dns.GetHostEntry(HostName());
            for (int i = 0; i < localHost.AddressList.Length; i++)
                if (localHost.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    alIps.Add(localHost.AddressList[i].ToString());
            string[] ret = new string[alIps.Count];
            alIps.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取IPV6的地址
        /// </summary>
        /// <returns></returns>
        public static string[] HostIpV6()
        {
            System.Collections.ArrayList alIps = new System.Collections.ArrayList();
            System.Net.IPHostEntry localHost = System.Net.Dns.GetHostEntry(HostName());
            for (int i = 0; i < localHost.AddressList.Length; i++)
                if (localHost.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    alIps.Add(localHost.AddressList[i].ToString());
            string[] ret = new string[alIps.Count];
            alIps.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取MAC地址，网络可用
        /// </summary>
        /// <returns></returns>
        public static string[] HostMac()
        {            
            System.Collections.ArrayList alMacs = new System.Collections.ArrayList();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    if (mo["MacAddress"] != null)
                    {
                        string mac = mo["MacAddress"].ToString();
                        alMacs.Add(string.Format("{0}", mac));
                    }
                }
            }
            string[] ret = new string[alMacs.Count];
            alMacs.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取mac地址与ipv4地址
        /// </summary>
        /// <returns></returns>
        public static string[] HostIpV4Mac()
        {
            System.Collections.ArrayList alMacs = new System.Collections.ArrayList();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    string[] ips = (string[])mo["IPAddress"];
                    if (ips.Length <= 0) continue;
                    string mac = mo["MacAddress"].ToString();
                    alMacs.Add(string.Format("{0};{1}", mac,ips[0]));
                }
            }
            string[] ret = new string[alMacs.Count];
            alMacs.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取mac地址与ipv6地址
        /// </summary>
        /// <returns></returns>
        public static string[] HostIpV6Mac()
        {
            System.Collections.ArrayList alMacs = new System.Collections.ArrayList();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    string[] ips = (string[])mo["IPAddress"];
                    if (ips.Length <= 1) continue;
                    string mac = mo["MacAddress"].ToString();
                    alMacs.Add(string.Format("{0};{1}", mac, ips[1]));
                }
            }
            string[] ret = new string[alMacs.Count];
            alMacs.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取cpuid
        /// </summary>
        /// <returns></returns>
        public static string[] HostCpuId()
        {
            System.Collections.ArrayList alCpuIds = new System.Collections.ArrayList();
            ManagementClass mc = new ManagementClass("Win32_Processor");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                string id = mo["ProcessorId"].ToString();
                alCpuIds.Add(string.Format("{0}", id));
            }
            string[] ret = new string[alCpuIds.Count];
            alCpuIds.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取disk的model
        /// </summary>
        /// <returns></returns>
        public static string[] HostDiskModel()
        {
            System.Collections.ArrayList alDiskModels = new System.Collections.ArrayList();
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                string model = mo["Model"].ToString();
                alDiskModels.Add(string.Format("{0}", model));
            }
            string[] ret = new string[alDiskModels.Count];
            alDiskModels.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取disk的serial number
        /// </summary>
        /// <returns></returns>
        public static string[] HostDiskSN()
        {
            System.Collections.ArrayList alDiskModels = new System.Collections.ArrayList();
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                string model = mo["SerialNumber"].ToString();
                alDiskModels.Add(string.Format("{0}", model));
            }
            string[] ret = new string[alDiskModels.Count];
            alDiskModels.CopyTo(ret);
            return ret;
        }
    }
}
