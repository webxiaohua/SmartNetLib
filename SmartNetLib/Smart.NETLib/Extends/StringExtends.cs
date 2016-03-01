using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;


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
    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string FormatWith(this string format, params object[] args)
    {
        return string.Format(format, args);
    }

    /// <summary>
    /// 检查是否匹配
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool IsMatch(this string s, string pattern)
    {
        if (s == null) return false;
        else return Regex.IsMatch(s, pattern);
    }

    /// <summary>
    /// 获取匹配的字符
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static string Match(this string s, string pattern)
    {
        if (s == null) return "";
        return Regex.Match(s, pattern).Value;
    }
    /// <summary>
    /// 是否为数字
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsInt(this string s)
    {
        int i;
        return int.TryParse(s, out i);
    }
    /// <summary>
    /// 不会产生异常的 int 转换
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int ToInt(this string s)
    {
        return int.Parse(s);
    }
    /// <summary>
    /// 转换成int  产生异常会抛出一个null
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int? ToInt2(this string s)
    {
        try
        {
            return int.Parse(s);
        }
        catch (ArgumentNullException ex)
        {
            return null;
            //throw ex;
        }
        catch (FormatException ex)
        {
            return null;
            //throw ex;
        }
        catch (OverflowException ex)
        {
            return null;
            //throw ex;
        }

    }

    /// <summary>
    /// Camel 规则
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToCamel(this string s)
    {
        if (s.IsNullOrEmpty()) return s;
        return s[0].ToString().ToLower() + s.Substring(1);
    }

    /// <summary>
    /// Pascal 规则
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToPascal(this string s)
    {
        if (s.IsNullOrEmpty()) return s;
        return s[0].ToString().ToUpper() + s.Substring(1);
    }
}

