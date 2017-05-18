using System.Net.Sockets;

namespace GEIIO.Communicate.NET
{
    public interface ISocketAsyncEventArgsProxy
    {
        SocketAsyncEventArgsEx SocketReceiveEventArgsEx { get; set; }

        SocketAsyncEventArgs SocketSendEventArgs { get; set; }

        void Initialize(ISocketSession session);

        void Reset();
    }
}
