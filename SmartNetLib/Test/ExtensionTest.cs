using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    public class ExtensionTest
    {
        public static void Test()
        {
            string today = "今天是：{0:yyyy年MM月dd日 星期ddd}".FormatWith(DateTime.Now);
            Console.WriteLine(today);
            bool b = "12345".IsMatch(@"\d+");
            string s = "12sxh11a1".Match("[a-zA-Z]+");
            if (b)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("999999999".ToInt());
            byte a = 255;
            Console.WriteLine(a.ToHex());
            byte[] key = new byte[] { 21, 35, 47, 13, 55 };
            byte[] result = key.Hash("SHA");

        }

    }
}
