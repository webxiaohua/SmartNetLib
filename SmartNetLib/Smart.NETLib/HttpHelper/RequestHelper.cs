using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Smart.NETLib.HttpHelper
{
    /// <summary>
    /// Http 请求类
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// Http get 请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="getdataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string getdataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (getdataStr == "" ? "" : "?") + getdataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 提交POST请求 
        /// </summary>
        /// <param name="Url">POST 地址</param>
        /// <param name="postdataStr">POST 数据</param>
        /// <returns>获取响应值</returns>
        public static string HttpPost(string Url, string postdataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            byte[] bs = Encoding.UTF8.GetBytes(postdataStr);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bs.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;

        }
    }
}
