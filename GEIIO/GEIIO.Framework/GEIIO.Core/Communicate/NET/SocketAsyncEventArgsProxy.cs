using System.Net.Sockets;

namespace GEIIO.Communicate.NET
{
    public class SocketAsyncEventArgsProxy : ISocketAsyncEventArgsProxy
    {
        public SocketAsyncEventArgsProxy(SocketAsyncEventArgsEx saea)
        {
            SocketReceiveEventArgsEx = saea;
            SocketSendEventArgs = new SocketAsyncEventArgs();
        }

        public SocketAsyncEventArgsEx SocketReceiveEventArgsEx { get; set; }

        public SocketAsyncEventArgs SocketSendEventArgs { get; set; }

        public void Initialize(ISocketSession session)
        {
            SocketReceiveEventArgsEx.UserToken = session;
            SocketSendEventArgs.UserToken = session;
        }

        public void Reset()
        {
            SocketReceiveEventArgsEx.UserToken = null;
            SocketSendEventArgs.UserToken = null;
        }
    }
}
