using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// IsXXX 扩展函数
/// </summary>
public static class IsExtends
{
    /// <summary>
    /// 验证某个对象是否在数组中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="thisValue"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool IsIn<T>(this T thisValue, params T[] values) {
        return values.Contains(thisValue);
    }
}

