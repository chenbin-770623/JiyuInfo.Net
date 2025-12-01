using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace JiYuInfo.Configurator
{
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Web.Script.Serialization;
    using System.Windows.Forms.Design;
    using System.Collections;
    using System.Reflection;
    /// <summary>
    /// 自定义类型属性显示控件
    /// </summary>
    internal partial class CustomerTypeControl : UserControl
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public CustomerTypeControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="obj"></param>
        public CustomerTypeControl(object obj)
            : this()
        {
            SelectedObject = obj;
        }

        private object m_SelectedObject = null;
        /// <summary>
        /// 显示对象
        /// </summary>
        public object SelectedObject
        {
            get { return m_SelectedObject; }
            set { m_SelectedObject = value; }
        }

        private void CustomerTypeControl_Load(object sender, EventArgs e)
        {
            this.customerTypePropertyGrid.SelectedObject = SelectedObject;
        }
    }

    /// <summary>
    /// 已重载的自定义配置类型转换器
    /// </summary>
    public class CustomerTypeConverter : TypeConverter
    {
        /// <summary>
        /// 已重载。 将给定值对象转换为指定的类型。
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                return JYConverter.ConvertJsonToObject(value.ToString(), context.PropertyDescriptor.PropertyType);
            }
            catch
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
        /// <summary>
        /// 已重载。 将给定值转换为此转换器的类型。
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            try
            {
                return JYConverter.ConvertObjectToJson(value);
            }
            catch
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }

    /// <summary>
    /// 自定义配置类型基类
    /// </summary>
    [CustomerType]
    [TypeConverter(typeof(CustomerTypeConverter))]
    [Editor(typeof(CustomerTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public class CustomerTypeBase
    {
        /// <summary>
        /// 已重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JYConverter.ConvertObjectToJson(this);
        }
    }
    /// <summary>
    /// 用于标示指定的类型是否是自定义配置类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomerTypeAttribute : System.Attribute
    {
    }

    /// <summary>
    /// 已重载的自定义配置类型设计器
    /// </summary>
    public class CustomerTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 已重载。 使用 GetEditStyle 方法所指示的编辑器样式编辑指定对象的值。
        /// </summary>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                object isConfig = Reflection.GetTypeAttribute(context.PropertyDescriptor.PropertyType, typeof(CustomerTypeAttribute));
                if (JYConverter.CheckTypeIsConfig(context.PropertyDescriptor.PropertyType))
                {
                    IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    if (edSvc != null)
                    {
                        CustomerTypeControl ctl = new CustomerTypeControl(value);
                        edSvc.DropDownControl(ctl);
                        value = ctl.SelectedObject;
                    }
                }
                return value;
            }
            catch
            {
                return base.EditValue(context, provider, value);
            }
        }
        /// <summary>
        /// 已重载。 获取由 EditValue 方法使用的编辑器样式。
        /// </summary>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
    /// <summary>
    /// 用于指定时间类型的显示格式特征
    /// </summary>
    public sealed class DateTimeFormatAttribute : System.Attribute
    {
        /// <summary>
        /// 创建类实例
        /// </summary>
        public DateTimeFormatAttribute()
            : base()
        {
        }
        /// <summary>
        /// 创建类实例
        /// </summary>
        /// <param name="datetimeFormat"></param>
        public DateTimeFormatAttribute(string datetimeFormat)
            : this()
        {
            m_DatetimeFormat = datetimeFormat;
        }

        private string m_DatetimeFormat = Long;
        /// <summary>
        /// 时间类型的显示格式
        /// </summary>
        [DefaultValue(Long)]
        public string DatetimeFormat
        {
            get { return m_DatetimeFormat; }
            set { m_DatetimeFormat = value; }
        }
        /// <summary>
        /// 检测显示格式是否有显示日期
        /// </summary>
        /// <returns></returns>
        public bool CheckHasDate()
        {
            return DatetimeFormat.Contains("yy") || DatetimeFormat.Contains("M") || DatetimeFormat.Contains("d");
        }
        /// <summary>
        /// 检测显示格式是否有显示时间
        /// </summary>
        /// <returns></returns>
        public bool CheckHasTime()
        {
            return DatetimeFormat.Contains("H") || DatetimeFormat.Contains("m") || DatetimeFormat.Contains("s");
        }
        /// <summary>
        /// 检测显示格式是否有显示毫秒
        /// </summary>
        /// <returns></returns>
        public bool CheckHasMillSecond()
        {
            return DatetimeFormat.Contains("f");
        }
        /// <summary>
        /// 完整的时间，yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        public const string Full = "yyyy-MM-dd HH:mm:ss.fff";
        /// <summary>
        /// 默认，yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string Long = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 日期，yyyy-MM-dd
        /// </summary>
        public const string Date = "yyyy-MM-dd";
        /// <summary>
        /// 时间，HH:mm:ss
        /// </summary>
        public const string Time = "HH:mm:ss";
    }
    /// <summary>
    /// 已重载的时间类型转换器
    /// </summary>
    public class JYDateTimeConverter : DateTimeConverter
    {
        /// <summary>
        /// 已重载。 将给定值对象转换为指定的类型。
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            try
            {
                return JYConverter.ConvertDatetimeToJson(context.Instance, context.PropertyDescriptor.DisplayName, (DateTime)value);
            }
            catch
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        /// <summary>
        /// 已重载。 将给定值转换为此转换器的类型。
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                return JYConverter.ConvertJsonToDatetime(context.Instance, context.PropertyDescriptor.DisplayName, value.ToString());
            }
            catch
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
    /// <summary>
    /// 已重载的时间类型设计器
    /// </summary>
    public class JYDateTimeEditor : System.Drawing.Design.UITypeEditor
    {
        /// <summary>
        /// 已重载。 使用 GetEditStyle 方法所指示的编辑器样式编辑指定对象的值。
        /// </summary>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context.PropertyDescriptor.PropertyType == typeof(System.DateTime))
            {
                string format = JYConverter.GetPropertyDatetimeFormat(context.Instance.GetType(), context.PropertyDescriptor.Name);
                // Uses the IWindowsFormsEditorService to display a 
                // drop-down UI in the Properties window.
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    // Display an angle selection control and retrieve the value.
                    JYDatetimeEditorControl ctl = new JYDatetimeEditorControl((DateTime)value, new DateTimeFormatAttribute(format));
                    edSvc.DropDownControl(ctl);

                    // Return the value in the appropraite data format.
                    value = ctl.Value;
                }
                return value;

            }
            else
                return base.EditValue(context, provider, value);
        }
        /// <summary>
        /// 已重载。 获取由 EditValue 方法使用的编辑器样式。
        /// </summary>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }
    }

    internal static class JYConverter
    {
        /// <summary>
        /// 基础类型列表
        /// </summary>
        public static readonly System.Collections.ArrayList BaseTypeList = new System.Collections.ArrayList(new Type[] {
            typeof(bool),typeof(byte),typeof(char),typeof(decimal),typeof(double),typeof(short),typeof(int),typeof(long),
            typeof(sbyte),typeof(Single),typeof(UInt16),typeof(UInt32),typeof(UInt64),typeof(string)
        });
        /// <summary>
        /// 检查一个对象是否基础类型
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool CheckIsBaseType(object instance)
        {
            return instance != null && CheckIsBaseType(instance.GetType());
        }
        /// <summary>
        /// 检查一个类型是否基础类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool CheckIsBaseType(Type t)
        {
            return t != null && BaseTypeList.Contains(t);
        }
        /// <summary>
        /// 把一个属性值转成json
        /// </summary>
        public static string ConvertPropertyValueToJson(object instance, string propertyName, object propertyValue)
        {
            string ret = "";
            if (instance == null || propertyValue == null) return ret;
            Type t = Reflection.GetPropertyType(instance, propertyName);
            if (CheckIsBaseType(t))
            {
                if (Reflection.GetPropertyPasswordPropertyText(instance, propertyName))
                    ret = ConvertPasswordToJson(instance, propertyName, propertyValue.ToString());
                else
                    ret = propertyValue.ToString();
            }
            else if (CheckTypeIsDatetime(t))
            {
                ret = ConvertDatetimeToJson(instance, propertyName, (DateTime)propertyValue);
            }
            else if (CheckTypeIsConfig(t))
            {
                ret = ConvertObjectToJson(propertyValue);
            }
            else if (Reflection.CheckTypeIsIList(propertyValue))
            {
                Type elementType = null;
                if (t.IsArray && CheckTypeIsConfig(t.GetElementType())) elementType = t.GetElementType();
                if (t.IsGenericType && CheckTypeIsConfig(t.GetGenericArguments()[0])) elementType = t.GetGenericArguments()[0];
                if (elementType == null)
                    ret = ConvertIListToJson(propertyValue as System.Collections.IList);
                else
                    ret = ConvertIListToJson(ConvertIListToStrings(propertyValue as System.Collections.IList));
            }
            else if (t.IsEnum)
            {
                ret = propertyValue.ToString();
            }
            else
            {
                ret = ConvertObjectToJson(propertyValue);
            }
            return ret;
        }
        /// <summary>
        /// 把json转成属性类型对象
        /// </summary>
        public static object ConvertJsonToPropertyValue(object instance, string propertyName, string json)
        {
            object ret = null;
            Type t = Reflection.GetPropertyType(instance, propertyName);
            if (CheckIsBaseType(t))
            {
                if (Reflection.GetPropertyPasswordPropertyText(instance, propertyName))
                    ret = ConvertJsonToPassword(instance, propertyName, json);
                else
                    if (!string.IsNullOrEmpty(json))
                    ret = Convert.ChangeType(json, t);
            }
            else if (CheckTypeIsDatetime(t))
            {
                if (!string.IsNullOrEmpty(json))
                    ret = ConvertJsonToDatetime(instance, propertyName, json);
            }
            else if (CheckTypeIsConfig(t))
            {
                ret = ConvertJsonToObject(json, t);
            }
            else if (Reflection.CheckTypeIsIList(t))
            {
                Type elementType = null;
                if (t.IsArray && CheckTypeIsConfig(t.GetElementType())) elementType = t.GetElementType();
                if (t.IsGenericType && CheckTypeIsConfig(t.GetGenericArguments()[0])) elementType = t.GetGenericArguments()[0];
                if (elementType == null)
                    ret = ConvertJsonToIList(json);
                else
                    ret = ConvertIListToIList(ConvertJsonToIList(json), elementType);
            }
            else if (t.IsEnum)
            {
                if (!string.IsNullOrEmpty(json))
                    ret = Enum.Parse(t, json);
            }
            else
            {
                ret = ConvertJsonToObject(json, t);
            }
            return ret;
        }
        /// <summary>
        /// 把密码特征属性的值转成json
        /// </summary>
        public static string ConvertPasswordToJson(object instance, string propertyName, string value)
        {
            string ret = "";
            if (Reflection.GetPropertyPasswordPropertyText(instance, propertyName))
            {
                ret = Security.RC4.Instance.Encrypt(value);
            }
            return ret;
        }
        /// <summary>
        /// 把json转成密码特征属性的值
        /// </summary>
        public static string ConvertJsonToPassword(object instance, string propertyName, string value)
        {
            string ret = "";
            if (Reflection.GetPropertyPasswordPropertyText(instance, propertyName))
            {
                ret = Security.RC4.Instance.Decrypt(value);
            }
            return ret;
        }
        /// <summary>
        /// 检查一个对象是否时间日期类型
        /// </summary>
        public static bool CheckTypeIsDatetime(object instance)
        {
            return instance != null && CheckTypeIsDatetime(instance.GetType());
        }
        /// <summary>
        /// 检查一个类型是否时间日期类型
        /// </summary>
        public static bool CheckTypeIsDatetime(Type t)
        {
            return (t != null && t == typeof(System.DateTime));
        }
        /// <summary>
        /// 把日期类型的属性值转成json
        /// </summary>
        public static string ConvertDatetimeToJson(object instance, string propertyName, DateTime objValue)
        {
            string ret = "";
            string format = GetPropertyDatetimeFormat(instance, propertyName);
            if (format != "")
                ret = objValue.ToString(format);
            else
                ret = objValue.ToString();
            return ret;
        }
        /// <summary>
        /// 把json转成日期类型的值
        /// </summary>
        public static DateTime ConvertJsonToDatetime(object instance, string propertyName, string value)
        {
            DateTime ret = MinDate;
            string format = GetPropertyDatetimeFormat(instance, propertyName);
            try
            {
                if (format != "") ret = DateTime.ParseExact(value, format, null);
                else ret = DateTime.Parse(value);
            }
            catch { ret = MinDate; }
            return ret;
        }
        /// <summary>
        /// 最小日期
        /// </summary>
        public static DateTime MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 最大日期
        /// </summary>
        public static DateTime MaxDate = new DateTime(2100, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 检查一个类型是否是有JYConfigAttribute
        /// </summary>
        public static bool CheckTypeIsConfig(Type t)
        {
            return (t != null && Reflection.CheckTypeHasAttribute(t, typeof(CustomerTypeAttribute)));
        }
        /// <summary>
        /// 把对象转成json
        /// </summary>
        public static string ConvertObjectToJson(object obj)
        {
            return ConvertDictionaryToJson(ConvertObjectToDictionary(obj));
        }
        /// <summary>
        /// 把键值对集合转成json
        /// </summary>
        public static string ConvertDictionaryToJson(Dictionary<string, string> input)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(input);
        }
        /// <summary>
        /// 把对象转成键值对集合
        /// </summary>
        public static Dictionary<string, string> ConvertObjectToDictionary(object obj)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (string propertyName in Reflection.GetBrowseablePropertyNames(obj))
            {
                object objValue = Reflection.GetPropertyValue(obj, propertyName);
                string strValue = ConvertPropertyValueToJson(obj, propertyName, objValue);
                ret.Add(propertyName, strValue);
            }
            return ret;
        }
        /// <summary>
        /// 把json转成对象
        /// </summary>
        public static object ConvertJsonToObject(string json, Type objType)
        {
            return ConvertDictionaryToObject(ConvertJsonToDictionary(json), objType);
        }
        /// <summary>
        /// 把json转成键值对集合
        /// </summary>
        public static Dictionary<string, string> ConvertJsonToDictionary(string input)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<Dictionary<string, string>>(input);
        }
        /// <summary>
        /// 把键值对集合转成指定类型的对象
        /// </summary>
        public static object ConvertDictionaryToObject(Dictionary<string, string> input, Type objType)
        {
            if (input == null) return null;
            object ret = Activator.CreateInstance(objType);
            foreach (KeyValuePair<string, string> item in input)
            {
                string propertyName = item.Key;
                string propertyValue = item.Value;
                object objValue = ConvertJsonToPropertyValue(ret, propertyName, propertyValue);
                Reflection.SetPropertyValue(ret, propertyName, objValue);
            }
            return ret;
        }
        /// <summary>
        /// 把一个IList对象转成json
        /// </summary>
        public static string ConvertIListToJson(System.Collections.IList objList)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(objList);
        }
        /// <summary>
        /// 把json转成IList对象
        /// </summary>
        public static System.Collections.IList ConvertJsonToIList(string json)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Deserialize<System.Collections.IList>(json);
            }
            catch { return new object[0]; }
        }
        /// <summary>
        /// 把一个IList对象转成字符串数组
        /// </summary>
        public static string[] ConvertIListToStrings(System.Collections.IList objList)
        {
            string[] ret = new string[objList.Count];
            for (int i = 0; i < objList.Count; i++)
            {
                ret[i] = ConvertObjectToJson(objList[i]);
            }
            return ret;
        }
        /// <summary>
        /// 把一个字符串数组转成IList对象
        /// </summary>
        public static System.Collections.IList ConvertIListToIList(System.Collections.IList jsons, Type objType)
        {
            if (jsons != null)
            {
                object[] ret = new object[jsons.Count];
                for (int i = 0; i < jsons.Count; i++)
                    ret[i] = ConvertJsonToObject(jsons[i].ToString(), objType);
                return ret;
            }
            else return new object[0];
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的时间显示格式特征
        /// </summary>
        public static string GetPropertyDatetimeFormat(object instance, string propertyName)
        {
            return GetPropertyDatetimeFormat(instance.GetType(), propertyName);
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的时间显示格式特征
        /// </summary>
        public static string GetPropertyDatetimeFormat(Type t, string propertyName)
        {
            string ret = "";
            if (Reflection.CheckHasProperty(t, propertyName))
            {
                object objFormat = Reflection.GetPropertyAttribute(t, propertyName, typeof(DateTimeFormatAttribute));
                if (objFormat != null) ret = ((DateTimeFormatAttribute)objFormat).DatetimeFormat;
                else ret = DateTimeFormatAttribute.Long;
            }
            return ret;
        }
    }
}
