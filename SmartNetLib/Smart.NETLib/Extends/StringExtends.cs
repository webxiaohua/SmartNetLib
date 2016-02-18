using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


/// <summary>
/// @Author:webxiaohua
/// @QQ:1584004295
/// @Date:2016-01-22
/// @Desc:字符串扩展函数类
/// </summary>
public static class StringExtends
{
    /// <summary>
    /// 验证字符串是否为空
    /// </summary>
    /// <param name="sourceStr">源字串</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string sourceStr)
    {
        return string.IsNullOrEmpty(sourceStr);
    }

    /// <summary>
    /// 验证是否相等，跳过单步调试
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static bool IsSameAs(this string str1, string str2)
    {
        return string.Compare(str1, str2, true) == 0;
    }


}

