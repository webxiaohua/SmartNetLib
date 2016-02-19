using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.NETLib;
using System.Runtime.Serialization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DirFileHelper.GetMapPath(""));
            //string a = TimeHelper.GetTimeStamp();
            //Console.WriteLine(a);
            XmlTest.Test1();
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
