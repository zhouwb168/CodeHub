using System;
using GEIIO.Log;

namespace TestLog
{
    class Program
    {
        static void Main(string[] args)
        {
            new LogFactory().GetLog("ddd").Debug(true, "ddd");
            Console.Read();
        }
    }
}
