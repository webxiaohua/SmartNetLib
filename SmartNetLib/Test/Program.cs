using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.NETLib;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Test
{
    class Program
    {
        /// <summary>
        /// 把对象序列化成指定编码格式的文本流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="o"></param>
        /// <param name="encoding"></param>
        private static void XmlSerialize(Stream stream, object o, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (o == null) throw new ArgumentNullException("o");
            if (encoding == null) encoding = Encoding.UTF8;
            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = encoding;
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                xmlSerializer.Serialize(writer, o);
                writer.Close();
            }
        }

        static void Main(string[] args)
        {
            #region 反射示例
            ReflectionTest.Test();
            #endregion
            #region 反射性能测试
            //EmitTest.Test();
            #endregion
            #region 委托测试
            //DelegateTest.DoGreet(DelegateTest.GreetEnglish, "Robin");
            //DelegateTest.DoGreet(DelegateTest.GreetChinest, "沈新华");
            #endregion
            //Console.WriteLine(DirFileHelper.GetMapPath(""));
            //string a = TimeHelper.GetTimeStamp();
            //Console.WriteLine(a);
            //XmlTest.Test1();
            Console.Read();
        }
    }

    sealed class DiskFullException : Exception, ISerializable
    {
        public DiskFullException() : base() { }
        public DiskFullException(string message) : base(message) { }
        public DiskFullException(string message, Exception innerException) : base(message, innerException) { }

        private string diskpath;
        public string Diskpath { get { return diskpath; } }

        public override string Message
        {
            get
            {
                string msg = base.Message;
                if (diskpath != null)
                    msg += Environment.NewLine + "Disk Path:" + diskpath;
                return msg;
            }
        }

        private DiskFullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            diskpath = info.GetString("Diskpath");
        }
    }
}
