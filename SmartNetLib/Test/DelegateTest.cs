using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class DelegateTest
    {
        public static void GreetEnglish(string name)
        {
            Console.WriteLine("Hello " + name);
        }

        public static void GreetChinest(string name)
        {
            Console.WriteLine("你好 " + name);
        }

        public static void DoGreet(Action<string> makeGreet, string name)
        {
            makeGreet(name);
        }

    }

    public delegate void MakeGreet(string name);

}
