using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace webDmsApi.Controllers
{
    public class ExcelHelper
    {

        //DataTable转List<T>
        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {

            if (dt == null) return null;

            List<T> list = new List<T>();

            //遍历DataTable中所有的数据行 
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();

                PropertyInfo[] propertys = t.GetType().GetProperties();

                foreach (PropertyInfo pro in propertys)
                {
                    //检查DataTable是否包含此列（列名==对象的属性名）   
                    if (dt.Columns.Contains(pro.Name))
                    {
                        object value = dr[pro.Name];
                        //if(pro.PropertyType.)
                        value = Convert.ChangeType(value, pro.PropertyType);//强制转换类型

                        //如果非空，则赋给对象的属性  PropertyInfo
                        if (value != DBNull.Value)
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中 
                list.Add(t);
            }

            return list;

        }

        public static List<T> DataTableToListByDisplayName<T>(DataTable dt) where T : class, new()
        {
            if (dt == null) return null;

            List<T> list = new List<T>();

            //遍历DataTable中所有的数据行 
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                bool IsNullRow = true;
                foreach (DataColumn col in dt.Columns)
                {
                    if (dr[col.ColumnName] != null && !string.IsNullOrEmpty(dr[col.ColumnName].ToString()))
                    {
                        IsNullRow = false;
                    }
                }
                //如果是空行跳过
                if (IsNullRow)
                {
                    continue;
                }
                PropertyInfo[] propertys = t.GetType().GetProperties();

                foreach (PropertyInfo pro in propertys)
                {
                    //检查DataTable是否包含此列（列名==对象的属性名）   
                    var displayName = pro.GetCustomAttribute<DisplayNameAttribute>();

                    if (dt.Columns.Contains(displayName.DisplayName))
                    {
                        object value = dr[displayName.DisplayName];
                        //if(pro.PropertyType.)
                        //如果非空，则赋给对象的属性  PropertyInfo
                        if (value != DBNull.Value && (value != null && !string.IsNullOrEmpty(value.ToString()))) 
                        {
                            value = ChangeType(value, pro.PropertyType);//强制转换类型
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中 
                list.Add(t);
            }

            return list;

        }

        /// <summary>
        /// 根据类型转换成响应的对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        } 
    }
}
