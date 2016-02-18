using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Smart.NETLib
{
    /// <summary>
    /// @Author:webxiaohua
    /// @QQ:1584004295
    /// @Date:2016-01-22
    /// @Desc:文件读写操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        public ConfigHelper()
        {
        }
        public static bool GetConfigBool(string key)
        {
            bool flag1 = false;
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return flag1;
            }
            try
            {
                return bool.Parse(text1);
            }
            catch (FormatException)
            {
                return flag1;
            }
        }
        public static decimal GetConfigDecimal(string key)
        {
            decimal num1 = new decimal(0);
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return num1;
            }
            try
            {
                return decimal.Parse(text1);
            }
            catch (FormatException)
            {
                return num1;
            }
        }
        public static int GetConfigInt(string key)
        {
            int num1 = 0;
            string text1 = ConfigHelper.GetConfigString(key);
            if ((text1 == null) || (string.Empty == text1))
            {
                return num1;
            }
            try
            {
                return int.Parse(text1);
            }
            catch (FormatException)
            {
                return num1;
            }
        }
        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
