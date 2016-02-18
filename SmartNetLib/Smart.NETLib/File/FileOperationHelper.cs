using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Smart.NETLib
{
    /// <summary>
    /// @Author:webxiaohua
    /// @QQ:1584004295
    /// @Date:2016-01-22
    /// @Desc:文件读写操作类
    /// </summary>
    public static class FileOperationHelper
    {
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteFile(string filePath, string content)
        {
            if (!DirFileHelper.IsExistFile(filePath))
            {

                FileStream fs = File.Create(filePath);
                fs.Close();
                fs.Dispose();
            }
            StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);
            sw.WriteLine(content);
            sw.Close();
            sw.Dispose();
        }
    }
}
