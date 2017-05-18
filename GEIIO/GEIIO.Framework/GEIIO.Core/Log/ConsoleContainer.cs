using System;

namespace GEIIO.Log
{
    public class ConsoleContainer : ILogContainer
    {
        public void ShowLog(string log)
        {
            Console.WriteLine(log);
        }
    }
}
