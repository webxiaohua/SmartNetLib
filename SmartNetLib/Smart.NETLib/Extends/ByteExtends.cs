using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// byte 扩展函数
/// </summary>
public static class ByteExtends
{
    /// <summary>
    /// 转16进制
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public static string ToHex(this byte b)
    {
        return b.ToString("X2");  //0x0A  
    }
    /// <summary>
    /// bytes 集合 转16进制
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHex(this IEnumerable<byte> bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("X2"));
        }
        return sb.ToString();
    }
    /// <summary>
    /// 转成base64字符编码
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToBase64String(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }
    /// <summary>
    /// 转int
    /// </summary>
    /// <param name="value"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public static int ToInt(this byte[] value, int startIndex)
    {
        return BitConverter.ToInt32(value, startIndex);
    }
    /// <summary>
    /// 转big int
    /// </summary>
    /// <param name="value"></param>
    /// <param name="startIndex"></param>
    /// <returns></returns>
    public static long ToInt64(this byte[] value, int startIndex)
    {
        return BitConverter.ToInt64(value, startIndex);
    }
    /// <summary>
    /// 转换成指定编码的字符串
    /// </summary>
    /// <param name="data"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Decode(this byte[] data, Encoding encoding)
    {
        return encoding.GetString(data);
    }

    /// <summary>
    /// 使用指定算法Hash
    /// </summary>
    /// <param name="data"></param>
    /// <param name="hashName"></param>
    /// <returns></returns>
    public static byte[] Hash(this byte[] data, string hashName = null)
    {
        HashAlgorithm algorithm;
        if (string.IsNullOrEmpty(hashName)) algorithm = HashAlgorithm.Create();
        else algorithm = HashAlgorithm.Create(hashName);
        return algorithm.ComputeHash(data);
    }

    /// <summary>
    /// 位运算 index从0开始  获取第inde是否为1
    /// </summary>
    /// <param name="b"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool GetBit(this byte b, int index)
    {
        return (b & (1 << index)) > 0;
    }
    /// <summary>
    /// 将 index 位设为1
    /// </summary>
    /// <param name="b"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static byte SetBit(this byte b, int index)
    {
        b |= (byte)(1 << index); //将 1 左移 index 位
        return b;
    }
}

