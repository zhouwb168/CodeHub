using System.Net;

namespace GEIIO.Communicate.NET
{
    public class ListenerInfo
    {
        public IPEndPoint EndPoint { get; set; }

        public int BackLog { get; set; }
    }
}
