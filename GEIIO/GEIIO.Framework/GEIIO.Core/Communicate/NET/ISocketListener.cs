using System;
using System.Net;
using GEIIO.Config;

namespace GEIIO.Communicate.NET
{
    public interface ISocketListener
    {
        IPEndPoint EndPoint { get; }

        ListenerInfo ListenerInfo { get; }

        bool Start(IServerConfig config);

        void Stop();

        event NewClientAcceptHandler NewClientAccepted;

        event ErrorHandler Error;

        event EventHandler Stopped;
    }
}
