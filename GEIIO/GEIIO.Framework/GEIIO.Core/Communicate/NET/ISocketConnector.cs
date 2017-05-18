using System;
using GEIIO.Server;

namespace GEIIO.Communicate.NET
{
    internal interface ISocketConnector : IServerProvider, IDisposable
    {
        void Start();

        void Stop();

        event NewClientAcceptHandler NewClientConnected;

        event ErrorHandler Error;
    }
}
