using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.NETLib.Xml;

namespace Test
{
    public class Class1
    {
        public int IntValue { get; set; }
        public string StringValue { get; set; }
    }

    public class XmlTest
    {
        public static void Test1()
        {
            Class1 c1 = new Class1() { IntValue = 2, StringValue = "Hello" };
            string xml = XmlHelper.XmlSerialize(c1, Encoding.UTF8);
            Console.WriteLine(xml);
            Console.WriteLine("------------------------------------");
            Class1 c2 = new Class1() { IntValue = 3, StringValue = "Hello2" };
            Console.WriteLine("IntValue: " + c2.IntValue.ToString());
            Console.WriteLine("StringValue:" + c2.StringValue);
        }
    }
}
