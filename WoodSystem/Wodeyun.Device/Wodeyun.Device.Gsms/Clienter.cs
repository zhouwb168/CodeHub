using System;

using System.Net;
using System.Net.Sockets;

namespace Wodeyun.Device.Gsms
{
    public class Clienter : IDisposable
    {
        private TcpClient _TcpClient = new TcpClient();
        private NetworkStream _NetworkStream = null;
        private string _Address = string.Empty;
        private int _Port = 8899;

        public Clienter(string address, int port)
        {
            this._Address = address;
            this._Port = port;
        }
        
        public void Connect()
        {
            try
            {
                _TcpClient = new TcpClient();
                this._TcpClient.Connect(IPAddress.Parse(this._Address), this._Port);
                this._NetworkStream = this._TcpClient.GetStream();
                this._NetworkStream.ReadTimeout = 1000 * 10;
            }
            catch
            {
                throw;
            }
        }

        public int ReceiveBufferSize
        {
            get
            {
                try
                {
                    return this._TcpClient.ReceiveBufferSize;
                }
                catch
                {
                    try
                    {
                        this.Connect();

                        return this._TcpClient.ReceiveBufferSize;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public void Write(byte[] buffer, int offset, int size)
        {
            try
            {
                this._NetworkStream.Write(buffer, offset, size);
            }
            catch
            {
                try
                {
                    this.Connect();
                    this._NetworkStream.Write(buffer, offset, size);
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Read(byte[] buffer, int offset, int size)
        {
            try
            {
                this._NetworkStream.Read(buffer, offset, size);
            }
            catch
            {
                try
                {
                    this.Connect();
                    this._NetworkStream.Read(buffer, offset, size);
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            if (this._NetworkStream != null) this._NetworkStream.Close();
            if (this._TcpClient.Connected == true) this._TcpClient.Close();
        }
    }
}
