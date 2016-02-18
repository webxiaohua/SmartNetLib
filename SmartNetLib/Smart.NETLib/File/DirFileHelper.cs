using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Smart.NETLib
{
    /// <summary>
    /// @Author:webxiaohua
    /// @QQ:1584004295
    /// @Date:2016-01-22
    /// @Desc:目录/文件操作类
    /// </summary>
    public static class DirFileHelper
    {
        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return false;
            }
            return Directory.Exists(path);
        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="path">文件路径(相对或是绝对)</param>
        /// <returns></returns>
        public static bool IsExistFile(string path)
        {
            if (path.IsNullOrEmpty()) { return false; }
            if (path.IndexOf(":", StringComparison.Ordinal) < 0) { path = GetMapPath(path); }
            return File.Exists(path);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string dirPath)
        {
            if (dirPath.IsNullOrEmpty()) { return false; }
            DirectoryInfo dirInfo = Directory.CreateDirectory(dirPath);
            return dirInfo.Exists;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">文件绝对路径/相对路径</param>
        /// <returns></returns>
        public static bool CreateFile(string filePath)
        {
            try
            {
                if (filePath.IsNullOrEmpty()) { return false; }
                if (filePath.IndexOf(":") < 0)
                {
                    filePath = GetMapPath(filePath);
                }
                File.Create(filePath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取当前路径的绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    //Web程序
                    return HttpContext.Current.Server.MapPath(path);
                }
                else
                {
                    //Windows 程序
                    return AppDomain.CurrentDomain.BaseDirectory + path;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取指定目录下的文件名称
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static string[] GetFileListNames(string dirPath)
        {
            if (!IsExistDirectory(dirPath))
            {
                throw new DirectoryNotFoundException();
            }
            return Directory.GetFiles(dirPath);
        }



        /// <summary>
        /// 检测指定目录中是否存在指定的文件，若要搜索子目录请使用重载方法
        /// </summary>
        /// <param name="directoryPath">目录绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"表示0个或者多个字符，"?"表示1个字符  Log*.log</param>
        /// <returns></returns>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            return false;
        }
    }
}
