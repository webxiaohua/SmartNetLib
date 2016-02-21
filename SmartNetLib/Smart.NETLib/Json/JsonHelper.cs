using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;

namespace Smart.NETLib
{
    /// <summary>
    /// @Author:webxiaohua
    /// @Date:20160220
    /// @Desc:Json处理类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 对象转化成JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("Smart.NETLib.JsonHelper.ObjectToJson():" + ex.Message);
            }
        }

        /// <summary>
        /// DataTable 转成 List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dic.Add(col.ColumnName, row[col.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// DataSet 转键值对字典
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();
            foreach (DataTable item in ds.Tables)
            {
                result.Add(item.TableName, DataTableToList(item));
            }
            return result;
        }

        /// <summary>
        /// 数据表转JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            return ObjectToJson(DataTableToList(dt));
        }
        /// <summary>
        /// JSON 转成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("Smart.NetLib.JsonHelper.JsonToObject():" + ex.Message);
            }
        }
        /// <summary>
        /// JSON 转换成 DataSet
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, List<Dictionary<string, object>>> JsonToDataSet(string json)
        {
            return JsonToObject<Dictionary<string, List<Dictionary<string, object>>>>(json);
        }
        /// <summary>
        /// Json 转 DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDataTable(string json)
        {
            return JsonToObject<Dictionary<string, object>>(json);
        }
    }
}
