using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.NETLib.Builder;
using System.Data;

/// <summary>
/// @Author:webxiaohua
/// @QQ:1584004295
/// @Date:2016-02-18
/// @Desc:IDataReader扩展函数类
/// </summary>
public static class IDataReaderExtends
{
    /// <summary>
    /// 转换成List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader"></param>
    /// <param name="isClose"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this IDataReader reader, bool isClose)
        where T : class, new()
    {
        List<T> list = new List<T>();
        if (reader == null) return list;

        EmitEntityBuilder<T>.DynamicMethodDelegate<IDataRecord> handler
            = EmitEntityBuilder<T>.CreateHandler(reader);
        while (reader.Read())
        {
            list.Add(handler(reader));
        }
        if (isClose) { reader.Close(); reader.Dispose(); reader = null; }
        return list;
    }

    /// <summary>
    /// 转换成List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this IDataReader reader)
        where T : class, new()
    {
        return reader.ToList<T>(true);
    }
}

