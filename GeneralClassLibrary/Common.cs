using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JiYuInfo
{
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 定义事件委托，无参数
    /// </summary>
    public delegate void JYHandlerVoid();
    /// <summary>
    /// 定义事件委托，带object参数
    /// </summary>
    public delegate void JYHandlerObject(object arg);
    /// <summary>
    /// 定义事件委托，带bool参数
    /// </summary>
    public delegate void JYHandlerBool(bool arg);
    /// <summary>
    /// 定义事件委托，带int参数
    /// </summary>
    public delegate void JYHandlerInt(int arg);
    /// <summary>
    /// 定义事件委托，带uint参数
    /// </summary>
    public delegate void JYHandlerUInt(uint arg);
    /// <summary>
    /// 定义事件委托，带long参数
    /// </summary>
    public delegate void JYHandlerLong(long arg);
    /// <summary>
    /// 定义事件委托，带ulong参数
    /// </summary>
    public delegate void JYHandlerULong(ulong arg);
    /// <summary>
    /// 定义事件委托，带string参数
    /// </summary>
    public delegate void JYHandlerString(string arg);
    /// <summary>
    /// 定义事件委托，带uint,string参数
    /// </summary>
    public delegate void JYHandlerUIntString(uint intArg, string strArg);

    /// <summary>
    /// 反射操作集合
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// 获取程序集版本号
        /// </summary>
        public static string GetVersion(string assemblyFile)
        {
            if (System.IO.File.Exists(assemblyFile))
            {
                Assembly ass = Assembly.LoadFile(assemblyFile);
                return ass.GetName().Version.ToString();
            }
            else return string.Empty;
        }
        /// <summary>
        /// 获取程序集名称
        /// </summary>
        public static string GetName(string assemblyFile)
        {
            if (System.IO.File.Exists(assemblyFile))
            {
                Assembly ass = Assembly.LoadFile(assemblyFile);
                return ass.GetName().Name;
            }
            else return string.Empty;
        }
        /// <summary>
        /// 获取程序集包含的类型
        /// </summary>
        public static Type[] GetTypes(string assemblyFile)
        {
            if (System.IO.File.Exists(assemblyFile))
            {
                Assembly ass = Assembly.LoadFile(assemblyFile);
                return ass.GetTypes();
            }
            else return new Type[0];
        }
        /// <summary>
        /// 获取程序集包含的类型名称集合
        /// </summary>
        public static string[] GetTypeNames(string assemblyFile)
        {
            Type[] types = GetTypes(assemblyFile);
            string[] ret = new string[types.Length];
            for (int i = 0; i < types.Length; i++) ret[i] = types[i].FullName;
            return ret;
        }
        /// <summary>
        /// 检测程序集是否包含有指定的类型名称
        /// </summary>
        public static bool CheckHasType(string assemblyFile, string typeName)
        {
            return GetTypeNames(assemblyFile).Contains(typeName);
        }
        /// <summary>
        /// 获取指定程序集、指定类型名称的类型
        /// </summary>
        public static Type GetTypeByName(string assemblyFile, string typeName)
        {
            Type ret = null;
            Type[] types = GetTypes(assemblyFile);
            for (int i = 0; i < types.Length; i++)
                if (types[i].FullName == typeName) { ret = types[i]; break; }
            return ret;
        }
        /// <summary>
        /// 检查一个对象是否实现指定类型接口
        /// </summary>
        public static bool CheckTypeIsInterface(object instance, Type interfaceType)
        {
            return instance != null && CheckTypeIsInterface(instance.GetType(), interfaceType);
        }
        /// <summary>
        /// 检查一个类型是否实现指定类型接口
        /// </summary>
        public static bool CheckTypeIsInterface(Type t, Type interfaceType)
        {
            return t != null && t.GetInterfaces().Contains<Type>(interfaceType);
        }
        /// <summary>
        /// 检查一个对象是否实现IList接口
        /// </summary>
        public static bool CheckTypeIsIList(object instance)
        {
            return instance != null && CheckTypeIsIList(instance.GetType());
        }
        /// <summary>
        /// 检查一个类型是否实现IList接口
        /// </summary>
        public static bool CheckTypeIsIList(Type t)
        {
            return t != null && t.GetInterfaces().Contains<Type>(typeof(System.Collections.IList));
        }
        /// <summary>
        /// 创建指定类型的IList对象
        /// </summary>
        public static object CreateIListType(Type t, int count)
        {
            if (t == null) return null;
            object ret = new object();
            ret = t.InvokeMember("Set", BindingFlags.CreateInstance, null, ret, new object[] { count });
            return ret;
        }
        /// <summary>
        /// 创建指定类型的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object CreateObject(Type t)
        {
            if (t == null) return null;
            object ret = Activator.CreateInstance(t);
            return ret;
        }
        /// <summary>
        /// 给IList对象添加成员
        /// </summary>
        public static void AddItemToIList(object instance, object item, int index)
        {
            try
            {
                if (instance == null) return;
                Type t = instance.GetType();
                if (t == null) return;
                if (t.IsGenericType)
                    t.GetMethod("Add", t.GetGenericArguments()).Invoke(instance, new object[] { item });
                if (t.IsArray)
                    ((System.Collections.IList)instance)[index] = item;
            }
            catch { }
        }
        /// <summary>
        /// 检查一个对象是否有指定类型的特征
        /// </summary>
        public static bool CheckTypeHasAttribute(object instance, Type tAttribute)
        {
            return instance != null && CheckTypeHasAttribute(instance.GetType(), tAttribute);
        }
        /// <summary>
        /// 检查一个类型是否有指定类型的特征
        /// </summary>
        public static bool CheckTypeHasAttribute(Type t, Type tAttribute)
        {
            bool ret = false;
            if (t != null)
            {
                object[] attributes = t.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (tAttribute == attributes[i].GetType())
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象的特征类型集合
        /// </summary>
        public static Type[] GetTypeAttributes(object instance)
        {
            try
            {
                return GetTypeAttributes(instance.GetType());
            }
            catch { return new Type[0]; }
        }
        /// <summary>
        /// 获取一个类型的特征类型集合
        /// </summary>
        public static Type[] GetTypeAttributes(Type t)
        {
            Type[] ret = new Type[0];
            if (t != null)
            {
                object[] attributes = t.GetCustomAttributes(true);
                ret = new Type[attributes.Length];
                for (int i = 0; i < attributes.Length; i++)
                {
                    ret[i] = attributes[i].GetType();
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象的指定类型的特征
        /// </summary>
        public static object GetTypeAttribute(object instance, Type tAttribute)
        {
            try
            {
                return GetTypeAttribute(instance.GetType(), tAttribute);
            }
            catch { return null; }
        }
        /// <summary>
        /// 获取一个类型的指定类型的特征
        /// </summary>
        public static object GetTypeAttribute(Type t, Type tAttribute)
        {
            object ret = null;
            if (t != null)
            {
                object[] attributes = t.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (tAttribute == attributes[i].GetType())
                    {
                        ret = attributes[i];
                        break;
                    }
                }
            }
            return ret;
        }

        #region Property
        /// <summary>
        /// 获取一个对象的属性名称集合
        /// </summary>
        public static string[] GetPropertyNames(object instance)
        {
            try
            {
                return GetPropertyNames(instance.GetType());
            }
            catch { return new string[0]; }
        }
        /// <summary>
        /// 获取一个类型的属性名称集合
        /// </summary>
        public static string[] GetPropertyNames(Type t)
        {
            string[] ret = new string[0];
            if (t != null)
            {
                PropertyInfo[] ps = t.GetProperties();
                ret = new string[ps.Length];
                for (int i = 0; i < ps.Length; i++)
                    ret[i] = ps[i].Name;
            }
            return ret;
        }
        /// <summary>
        /// 检查一个对象是否有指定名称的属性
        /// </summary>
        public static bool CheckHasProperty(object instance, string propertyName)
        {
            return instance != null && CheckHasProperty(instance.GetType(), propertyName);
        }
        /// <summary>
        /// 检查一个类型是否有指定名称的属性
        /// </summary>
        public static bool CheckHasProperty(Type t, string propertyName)
        {
            bool ret = false;
            string[] ps = GetPropertyNames(t);
            ret = ps.Contains<string>(propertyName);
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性
        /// </summary>
        public static PropertyInfo GetProperty(object instance, string propertyName)
        {
            try
            {
                return GetProperty(instance.GetType(), propertyName);
            }
            catch { return null; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性
        /// </summary>
        public static PropertyInfo GetProperty(Type t, string propertyName)
        {
            PropertyInfo ret = null;
            if (t != null)
            {
                ret = t.GetProperty(propertyName);
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性类型
        /// </summary>
        public static Type GetPropertyType(object instance, string propertyName)
        {
            try
            {
                return GetPropertyType(instance.GetType(), propertyName);
            }
            catch { return null; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性类型
        /// </summary>
        public static Type GetPropertyType(Type t, string propertyName)
        {
            Type ret = null;
            if (t != null)
            {
                PropertyInfo p = GetProperty(t, propertyName);
                if (p != null) ret = p.PropertyType;
            }
            return ret;
        }
        /// <summary>
        /// 检查一个对象指定名称的属性是否为只读
        /// </summary>
        public static bool CheckPropertyReadonly(object instance, string propertyName)
        {
            return instance != null && CheckPropertyReadonly(instance.GetType(), propertyName);
        }
        /// <summary>
        /// 检查一个类型指定名称的属性是否为只读
        /// </summary>
        public static bool CheckPropertyReadonly(Type t, string propertyName)
        {
            bool ret = true;
            PropertyInfo p = GetProperty(t, propertyName);
            ret = CheckPropertyReadonly(p);
            return ret;
        }
        /// <summary>
        /// 检查一个属性是否为只读
        /// </summary>
        public static bool CheckPropertyReadonly(PropertyInfo property)
        {
            bool ret = true;
            if (property != null)
            {
                // 默认为只读，当找到set访问器且属性为公共时，可写
                MethodInfo ms = property.GetSetMethod();
                ret = (ms == null);
            }
            return ret;
        }
        /// <summary>
        /// 检查一个对象指定名称的属性是否是索引属性
        /// </summary>
        public static bool CheckPropertyIndex(object instance, string propertyName)
        {
            return instance != null && CheckPropertyIndex(instance.GetType(), propertyName);
        }
        /// <summary>
        /// 检查一个类型指定名称的属性是否是索引属性
        /// </summary>
        public static bool CheckPropertyIndex(Type t, string propertyName)
        {
            bool ret = false;
            PropertyInfo p = GetProperty(t, propertyName);
            ret = CheckPropertyIndex(p);
            return ret;
        }
        /// <summary>
        /// 检查一个类型指定名称的属性是否是索引属性
        /// </summary>
        public static bool CheckPropertyIndex(PropertyInfo property)
        {
            return property != null && property.GetIndexParameters().Length > 0;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的值
        /// </summary>
        public static object GetPropertyValue(object instance, string propertyName, params object[] idx)
        {
            object ret = null;
            PropertyInfo p = GetProperty(instance, propertyName);
            if (p != null)
            {
                if (p.GetIndexParameters().Length == 0)
                    ret = p.GetValue(instance, null);
                else
                    ret = p.GetValue(instance, idx);
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的值
        /// </summary>
        public static object GetPropertyValue(object instance, PropertyInfo property)
        {
            if (instance != null && property != null)
            {
                return property.GetValue(instance, null);
            }
            else return null;
        }
        /// <summary>
        /// 设置一个对象指定名称的属性的值
        /// </summary>
        public static bool SetPropertyValue(object instance, PropertyInfo property, object value)
        {
            bool ret = false;
            if (instance != null && property != null && value != null)
            {
                if (CheckPropertyReadonly(property)) return false;
                try
                {
                    property.SetValue(instance, value, null);
                    ret = true;
                }
                catch { ret = false; }
            }
            return ret;
        }
        /// <summary>
        /// 设置一个对象指定名称的属性的值
        /// </summary>
        public static bool SetPropertyValue(object instance, string propertyName, object value, params object[] idx)
        {
            bool ret = false;
            PropertyInfo p = GetProperty(instance, propertyName);
            if (p != null && value != null)
            {
                if (p.GetSetMethod() != null)
                {
                    object setValue = value;
                    if (p.PropertyType != value.GetType())
                    {
                        try
                        {
                            if (CheckTypeIsIList(p.PropertyType))
                            {
                                System.Collections.IList objList = value as System.Collections.IList;
                                if (objList != null)
                                {
                                    System.Collections.IList newList = CreateIListType(p.PropertyType, objList.Count) as System.Collections.IList;
                                    Type elementType = null;
                                    if (p.PropertyType.IsArray) elementType = p.PropertyType.GetElementType();
                                    if (p.PropertyType.IsGenericType) elementType = p.PropertyType.GetGenericArguments()[0];
                                    for (int i = 0; i < objList.Count; i++)
                                    {
                                        object item = objList[i];
                                        try
                                        {
                                            if (elementType != null) item = Convert.ChangeType(item, elementType);
                                        }
                                        catch { }
                                        AddItemToIList(newList, item, i);
                                    }
                                    setValue = newList;
                                }
                            }
                            else
                                setValue = Convert.ChangeType(value, p.PropertyType);
                        }
                        catch { setValue = value; }
                    }
                    if (p.PropertyType == setValue.GetType())
                    {
                        try
                        {
                            if (p.GetIndexParameters().Length == 0)
                                p.SetValue(instance, setValue, null);
                            else
                                p.SetValue(instance, setValue, idx);
                            ret = true;
                        }
                        catch { ret = false; }
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 检查一个对象是否有指定名称的属性的指定类型的特征
        /// </summary>
        public static bool CheckHasPropertyAttribute(object instance, string propertyName, Type tAttribute)
        {
            return instance != null && CheckHasPropertyAttribute(instance.GetType(), propertyName, tAttribute);
        }
        /// <summary>
        /// 检查一个类型是否有指定名称的属性的指定类型的特征
        /// </summary>
        public static bool CheckHasPropertyAttribute(Type t, string propertyName, Type tAttribute)
        {
            bool ret = false;
            PropertyInfo p = GetProperty(t, propertyName);
            ret = CheckHasPropertyAttribute(p, tAttribute);
            return ret;
        }
        /// <summary>
        /// 检查一个类型是否有指定名称的属性的指定类型的特征
        /// </summary>
        public static bool CheckHasPropertyAttribute(PropertyInfo property, Type tAttribute)
        {
            bool ret = false;
            if (property != null && tAttribute != null)
            {
                object[] attributes = property.GetCustomAttributes(false);
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (tAttribute == attributes[i].GetType())
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的特征类型集合
        /// </summary>
        public static Type[] GetPropertyAttributes(object instance, string propertyName)
        {
            try
            {
                return GetPropertyAttributes(instance.GetType(), propertyName);
            }
            catch { return new Type[0]; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的特征类型集合
        /// </summary>
        public static Type[] GetPropertyAttributes(Type t, string propertyName)
        {
            Type[] ret = new Type[0];
            PropertyInfo p = GetProperty(t, propertyName);
            if (p != null)
            {
                object[] attributes = p.GetCustomAttributes(false);
                ret = new Type[attributes.Length];
                for (int i = 0; i < attributes.Length; i++)
                {
                    ret[i] = attributes[i].GetType();
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的指定类型的特征
        /// </summary>
        public static object GetPropertyAttribute(object instance, string propertyName, Type tAttribute)
        {
            try
            {
                return GetPropertyAttribute(instance.GetType(), propertyName, tAttribute);
            }
            catch { return null; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的指定类型的特征
        /// </summary>
        public static object GetPropertyAttribute(Type t, string propertyName, Type tAttribute)
        {
            object ret = null;
            PropertyInfo p = GetProperty(t, propertyName);
            ret = GetPropertyAttribute(p, tAttribute);
            return ret;
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的指定类型的特征
        /// </summary>
        public static object GetPropertyAttribute(PropertyInfo property, Type tAttribute)
        {
            object ret = null;
            if (property != null && tAttribute != null)
            {
                object[] attributes = property.GetCustomAttributes(false);
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (tAttribute == attributes[i].GetType())
                    {
                        ret = attributes[i];
                        break;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的是否显示
        /// </summary>
        public static bool GetPropertyBrowseable(object instance, string propertyName)
        {
            return instance != null && GetPropertyBrowseable(instance.GetType(), propertyName);
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的是否显示
        /// </summary>
        public static bool GetPropertyBrowseable(Type t, string propertyName)
        {
            bool ret = false;
            if (CheckHasProperty(t, propertyName))
            {
                object objBrowseable = GetPropertyAttribute(t, propertyName, typeof(BrowsableAttribute));
                if (objBrowseable != null) ret = ((BrowsableAttribute)objBrowseable).Browsable;
                else ret = BrowsableAttribute.Default.Browsable;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的是否显示
        /// </summary>
        public static bool GetPropertyBrowseable(PropertyInfo property)
        {
            bool ret = false;
            if (property != null)
            {
                object objBrowseable = GetPropertyAttribute(property, typeof(BrowsableAttribute));
                if (objBrowseable != null) ret = ((BrowsableAttribute)objBrowseable).Browsable;
                else ret = BrowsableAttribute.Default.Browsable;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象所有显示特征为true的属性名称集合
        /// </summary>
        public static string[] GetBrowseablePropertyNames(object instance)
        {
            try
            {
                return GetBrowseablePropertyNames(instance.GetType());
            }
            catch { return new string[0]; }
        }
        /// <summary>
        /// 获取一个类型所有显示特征为true的属性名称集合
        /// </summary>
        public static string[] GetBrowseablePropertyNames(Type t)
        {
            System.Collections.ArrayList alNames = new System.Collections.ArrayList();
            foreach (string propertyName in GetPropertyNames(t))
                if (GetPropertyBrowseable(t, propertyName))
                    alNames.Add(propertyName);
            string[] ret = new string[alNames.Count];
            alNames.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的分类特征
        /// </summary>
        public static string GetPropertyCategory(object instance, string propertyName)
        {
            try
            {
                return GetPropertyCategory(instance.GetType(), propertyName);
            }
            catch { return string.Empty; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的分类特征
        /// </summary>
        public static string GetPropertyCategory(Type t, string propertyName)
        {
            string ret = string.Empty;
            if (CheckHasProperty(t, propertyName))
            {
                object objCategory = GetPropertyAttribute(t, propertyName, typeof(CategoryAttribute));
                if (objCategory != null) ret = ((CategoryAttribute)objCategory).Category;
                else ret = CategoryAttribute.Default.Category;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的分类特征
        /// </summary>
        public static string GetPropertyCategory(PropertyInfo property)
        {
            string ret = string.Empty;
            if (property != null)
            {
                object objCategory = GetPropertyAttribute(property, typeof(CategoryAttribute));
                if (objCategory != null) ret = ((CategoryAttribute)objCategory).Category;
                else ret = CategoryAttribute.Default.Category;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的默认值
        /// </summary>
        public static object GetPropertyDefaultValue(object instance, string propertyName)
        {
            try
            {
                return GetPropertyDefaultValue(instance.GetType(), propertyName);
            }
            catch { return null; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的默认值
        /// </summary>
        public static object GetPropertyDefaultValue(Type t, string propertyName)
        {
            object ret = null;
            if (CheckHasProperty(t, propertyName))
            {
                object objDefaultValue = GetPropertyAttribute(t, propertyName, typeof(DefaultValueAttribute));
                if (objDefaultValue != null) ret = ((DefaultValueAttribute)objDefaultValue).Value;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的默认值
        /// </summary>
        public static object GetPropertyDefaultValue(PropertyInfo property)
        {
            object ret = null;
            if (property != null)
            {
                object objDefaultValue = GetPropertyAttribute(property, typeof(DefaultValueAttribute));
                if (objDefaultValue != null) ret = ((DefaultValueAttribute)objDefaultValue).Value;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的说明
        /// </summary>
        public static string GetPropertyDescription(object instance, string propertyName)
        {
            try
            {
                return GetPropertyDescription(instance.GetType(), propertyName);
            }
            catch { return string.Empty; }
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的说明
        /// </summary>
        public static string GetPropertyDescription(Type t, string propertyName)
        {
            string ret = string.Empty;
            if (CheckHasProperty(t, propertyName))
            {
                object objDescription = GetPropertyAttribute(t, propertyName, typeof(DescriptionAttribute));
                if (objDescription != null) ret = ((DescriptionAttribute)objDescription).Description;
                else ret = DescriptionAttribute.Default.Description;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的说明
        /// </summary>
        public static string GetPropertyDescription(PropertyInfo property)
        {
            string ret = string.Empty;
            if (property != null)
            {
                object objDescription = GetPropertyAttribute(property, typeof(DescriptionAttribute));
                if (objDescription != null) ret = ((DescriptionAttribute)objDescription).Description;
                else ret = DescriptionAttribute.Default.Description;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的属性的是否密码文本
        /// </summary>
        public static bool GetPropertyPasswordPropertyText(object instance, string propertyName)
        {
            return instance != null && GetPropertyPasswordPropertyText(instance.GetType(), propertyName);
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的是否密码文本
        /// </summary>
        public static bool GetPropertyPasswordPropertyText(Type t, string propertyName)
        {
            bool ret = false;
            if (CheckHasProperty(t, propertyName))
            {
                object objPasswordPropertyText = GetPropertyAttribute(t, propertyName, typeof(PasswordPropertyTextAttribute));
                if (objPasswordPropertyText != null) ret = ((PasswordPropertyTextAttribute)objPasswordPropertyText).Password;
                else ret = PasswordPropertyTextAttribute.Default.Password;
            }
            return ret;
        }
        /// <summary>
        /// 获取一个类型指定名称的属性的是否密码文本
        /// </summary>
        public static bool GetPropertyPasswordPropertyText(PropertyInfo property)
        {
            bool ret = false;
            if (property != null)
            {
                object objPasswordPropertyText = GetPropertyAttribute(property, typeof(PasswordPropertyTextAttribute));
                if (objPasswordPropertyText != null) ret = ((PasswordPropertyTextAttribute)objPasswordPropertyText).Password;
                else ret = PasswordPropertyTextAttribute.Default.Password;
            }
            return ret;
        }
        #endregion
        #region Method
        /// <summary>
        /// 获取一个对象的方法名称集合
        /// </summary>
        public static string[] GetMethodNames(object instance)
        {
            try
            {
                return GetMethodNames(instance.GetType());
            }
            catch { return new string[0]; }
        }
        /// <summary>
        /// 获取一个类型的方法名称集合
        /// </summary>
        public static string[] GetMethodNames(Type t)
        {
            System.Collections.ArrayList alMethods = new System.Collections.ArrayList();
            if (t != null)
            {
                foreach (MethodInfo m in t.GetMethods())
                    if (!alMethods.Contains(m.Name) && !m.IsSpecialName)
                        alMethods.Add(m.Name);
            }
            string[] ret = new string[alMethods.Count];
            alMethods.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 检查一个对象是否有指定名称的方法
        /// </summary>
        public static bool CheckHasMethod(object instance, string methodName)
        {
            return instance != null && CheckHasMethod(instance.GetType(), methodName);
        }
        /// <summary>
        /// 检查一个类型是否有指定名称的方法
        /// </summary>
        public static bool CheckHasMethod(Type t, string methodName)
        {
            bool ret = false;
            string[] ms = GetMethodNames(t);
            ret = ms.Contains<string>(methodName);
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称的方法集合
        /// </summary>
        public static MethodInfo[] GetMethod(object instance, string methodName)
        {
            try
            {
                return GetMethod(instance.GetType(), methodName);
            }
            catch { return new MethodInfo[0]; }
        }
        /// <summary>
        /// 获取一个类型指定名称的方法集合
        /// </summary>
        public static MethodInfo[] GetMethod(Type t, string methodName)
        {
            System.Collections.ArrayList alMethods = new System.Collections.ArrayList();
            if (t != null)
            {
                foreach (MethodInfo m in t.GetMethods())
                    if (!alMethods.Contains(m) && !m.IsSpecialName && m.Name == methodName)
                        alMethods.Add(m);
            }
            MethodInfo[] ret = new MethodInfo[alMethods.Count];
            alMethods.CopyTo(ret);
            return ret;
        }
        /// <summary>
        /// 获取一个对象的构造集合
        /// </summary>
        public static ConstructorInfo[] GetConstructors(object instance)
        {
            try
            {
                return GetConstructors(instance.GetType());
            }
            catch { return new ConstructorInfo[0]; }
        }
        /// <summary>
        /// 获取一个类型的构造集合
        /// </summary>
        public static ConstructorInfo[] GetConstructors(Type t)
        {
            ConstructorInfo[] ret = new ConstructorInfo[0];
            if (t != null)
            {
                ret = t.GetConstructors();
            }
            return ret;
        }
        /// <summary>
        /// 获取一个对象指定名称、指定参数的方法
        /// </summary>
        public static MethodInfo GetMethod(object instance, string methodName, object[] paras)
        {
            try
            {
                return GetMethod(instance.GetType(), methodName, paras);
            }
            catch { return null; }
        }
        /// <summary>
        /// 获取一个类型指定名称、指定参数的方法
        /// </summary>
        public static MethodInfo GetMethod(Type t, string methodName, object[] paras)
        {
            MethodInfo ret = null;
            if (t != null)
            {
                MethodInfo[] ms = GetMethod(t, methodName);
                foreach (MethodInfo m in ms)
                {
                    ParameterInfo[] ps = m.GetParameters();
                    if (paras.Length == ps.Length)
                    {
                        bool match = true;
                        for (int i = 0; i < ps.Length; i++)
                        {
                            match &= (ps[i].ParameterType == paras[i].GetType());
                        }
                        if (match) { ret = m; break; }
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 执行一个对象指定名称、指定参数的方法
        /// </summary>
        public static bool InvokeMethod(object instance, string methodName, params object[] paras)
        {
            bool ret = false;
            if (instance != null)
            {
                MethodInfo m = GetMethod(instance, methodName, paras);
                if (m != null)
                {
                    try
                    {
                        m.Invoke(instance, paras);
                        ret = true;
                    }
                    catch { ret = false; }
                }
            }
            return ret;
        }
        /// <summary>
        /// 执行一个类型指定名称、指定参数的方法。
        /// 要求类型有无参构造函数。
        /// </summary>
        public static bool InvokeMethod(Type t, string methodName, params object[] paras)
        {
            return t != null && InvokeMethod(Activator.CreateInstance(t), methodName, paras);
        }
        /// <summary>
        /// 执行一个类型指定名称、类型参数、指定参数的方法。
        /// </summary>
        public static bool InvokeMethod(Type t, string methodName, object[] tParas, params object[] paras)
        {
            return t != null && InvokeMethod(Activator.CreateInstance(t, tParas), methodName, paras);
        }
        /// <summary>
        /// 执行一个对象指定名称、指定参数的方法。
        /// 指定参数用以逗号分隔的字符串传递。
        /// 要求所有入参的类型为string。
        /// 要求重载函数的参数个数不一样。
        /// </summary>
        public static bool JYInvokeMethod(object instance, string methodName, string paras)
        {
            bool ret = false;
            MethodInfo[] ms = GetMethod(instance, methodName);
            string[] strParas = new string[0];
            if (paras.Length > 0) strParas = paras.Split(new string[] { "," }, StringSplitOptions.None);
            if (ms.Length > 0)
            {
                foreach (MethodInfo m in ms)
                {
                    if (m.GetParameters().Length == strParas.Length)
                    {
                        try
                        {
                            m.Invoke(instance, strParas);
                            ret = true;
                        }
                        catch { ret = false; }
                        break;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 执行一个类型指定名称、指定参数的方法。
        /// 要求类型有无参构造函数。
        /// 指定参数用以逗号分隔的字符串传递。
        /// 要求所有入参的类型为string。
        /// 要求重载函数的参数个数不一样。
        /// </summary>
        public static bool JYInvokeMethod(Type t, string methodName, string paras)
        {
            return t != null && JYInvokeMethod(Activator.CreateInstance(t), methodName, paras);
        }
        /// <summary>
        /// 执行一个类型指定名称、指定类型参数、指定参数的方法。
        /// 参数用以逗号分隔的字符串传递。
        /// 要求所有入参的类型为string。
        /// 要求重载函数的参数个数不一样。
        /// </summary>
        public static bool JYInvokeMethod(Type t, string methodName, string typeParas, string paras)
        {
            string[] strTypeParas = new string[0];
            if (typeParas.Length > 0) strTypeParas = typeParas.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {
                object instance = Activator.CreateInstance(t, strTypeParas);
                bool ret = false;
                if (instance != null) ret = JYInvokeMethod(instance, methodName, paras);
                return ret;
            }
            catch { return false; }
        }
        /// <summary>
        /// 执行指定程序集、类型名称、方法名称、类型参数、方法参数的方法。
        /// 参数用以逗号分隔的字符串传递。
        /// 要求所有入参的类型为string。
        /// 要求重载函数的参数个数不一样。
        /// </summary>
        public static bool JYInvokeMethod(string assemblyFile, string typeName, string methodName, string typeParas, string methodParas)
        {
            bool ret = false;
            Type t = GetTypeByName(assemblyFile, typeName);
            if (t != null)
            {
                ret = JYInvokeMethod(t, methodName, typeParas, methodParas);
            }
            return ret;
        }
        #endregion
    }
    
    /// <summary>
    /// 导入的windows api 函数
    /// </summary>
    public static class WinApi
    {
        #region Import Kernel32.dll
        /// <summary>
        /// Retrieves a string from the specified section in an initialization file.
        /// </summary>
        /// <param name="strAppName">
        /// strAppName [in] The name of the section containing the key name. 
        ///         If this parameter is NULL, the GetPrivateProfileString function copies all section names in the file to the supplied buffer. 
        /// </param>
        /// <param name="strKeyName">
        /// strKeyName [in] The name of the key whose associated string is to be retrieved. 
        ///         If this parameter is NULL, all key names in the section specified by the strAppName parameter are copied to the buffer specified by the pbtReturnedString parameter. 
        /// </param>
        /// <param name="strDefault">
        /// strDefault [in] A default string. 
        ///         If the strKeyName key cannot be found in the initialization file, GetPrivateProfileString copies the default string to the pbtReturnedString buffer. 
        ///         If this parameter is NULL, the default is an empty string, "".Avoid specifying a default string with trailing blank characters. 
        ///         The function inserts a null character in the pbtReturnedString buffer to strip any trailing blanks.
        /// </param>
        /// <param name="pbtReturnedString">
        /// pbtReturnedString [out] A pointer to the buffer that receives the retrieved string. 
        /// </param>
        /// <param name="nSize">
        /// nSize [in] The size of the buffer pointed to by the pbtReturnedString parameter, in characters.
        /// </param>
        /// <param name="strFileName">
        /// strFileName [in] The name of the initialization file. 
        ///         If this parameter does not contain a full path to the file, the system searches for the file in the Windows directory.
        /// </param>
        /// <returns>
        /// The return value is the number of characters copied to the buffer, not including the terminating null character.
        /// If neither strAppName nor strKeyName is NULL and the supplied destination buffer is too small to hold the requested string, 
        ///     the string is truncated and followed by a null character, and the return value is equal to nSize minus one.
        /// If either strAppName or strKeyName is NULL and the supplied destination buffer is too small to hold all the strings, 
        ///     the last string is truncated and followed by two null characters. 
        /// In this case, the return value is equal to nSize minus two.
        /// </returns>
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileString(string strAppName, string strKeyName, string strDefault, byte[] pbtReturnedString, int nSize, string strFileName);
        /// <summary>
        /// Retrieves an integer associated with a key in the specified section of an initialization file.
        /// </summary>
        /// <param name="strAppName">
        /// strAppName [in] The name of the section in the initialization file. 
        /// </param>
        /// <param name="strKeyName">
        /// strKeyName [in] The name of the key whose value is to be retrieved. 
        ///         This value is in the form of a string; the GetPrivateProfileInt function converts the string into an integer and returns the integer. 
        /// </param>
        /// <param name="nDefault">
        /// nDefault [in] The default value to return if the key name cannot be found in the initialization file. 
        /// </param>
        /// <param name="strFileName">
        /// strFileName [in] The name of the initialization file. 
        ///         If this parameter does not contain a full path to the file, the system searches for the file in the Windows directory. 
        /// </param>
        /// <returns>
        /// The return value is the integer equivalent of the string following the specified key name in the specified initialization file. 
        /// If the key is not found, the return value is the specified default value.
        /// </returns>
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileInt(string strAppName, string strKeyName, int nDefault, string strFileName);
        /// <summary>
        /// Copies a string into the specified section of an initialization file.
        /// </summary>
        /// <param name="strAppName">
        /// strAppName [in] The name of the section to which the string will be copied. 
        ///         If the section does not exist, it is created. The name of the section is case-independent; 
        ///         the string can be any combination of uppercase and lowercase letters. 
        /// </param>
        /// <param name="strKeyName">
        /// strKeyName [in] The name of the key to be associated with a string. If the key does not exist in the specified section, it is created. 
        ///         If this parameter is NULL, the entire section, including all entries within the section, is deleted. 
        /// </param>
        /// <param name="strString">
        /// strString [in] A null-terminated string to be written to the file. 
        ///         If this parameter is NULL, the key pointed to by the strKeyName parameter is deleted. 
        /// </param>
        /// <param name="strFileName">
        /// strFileName [in] The name of the initialization file.
        ///         If the file was created using Unicode characters, the function writes Unicode characters to the file. Otherwise, the function writes ANSI characters.
        /// </param>
        /// <returns>
        /// If the function successfully copies the string to the initialization file, the return value is nonzero.
        /// If the function fails, or if it flushes the cached version of the most recently accessed initialization file, the return value is zero. 
        /// To get extended error information, call GetLastError.
        /// </returns>
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern int WritePrivateProfileString(string strAppName, string strKeyName, string strString, string strFileName);
        #endregion
    }
}
