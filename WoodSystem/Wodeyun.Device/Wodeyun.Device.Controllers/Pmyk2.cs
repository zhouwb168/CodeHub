using System;

using System.Net;
using System.Net.Sockets;

namespace Wodeyun.Device.Controllers
{
    public class Pmyk2 : IDisposable
    {
        private string _Address = string.Empty;
        private int _Port = 8899;
        private TcpClient _TcpClient = new TcpClient();
        private NetworkStream _NetworkStream = null;
        
        public void Connect(string address, int port)
        {
            try
            {
                this._Address = address;
                this._Port = port;

                this._TcpClient.Connect(IPAddress.Parse(this._Address), this._Port);
                this._NetworkStream = this._TcpClient.GetStream();
                this._NetworkStream.WriteTimeout = 1000;
            }
            catch
            {
                throw;
            }
        }

        public bool OpenRed()
        {
            return this.Write("31");
        }

        public bool OpenBeep()
        {
            return this.Write("32");
        }

        public bool CloseRed()
        {
            return this.Write("34");
        }

        public bool CloseBeep()
        {
            return this.Write("35");
        }

        public bool Check()
        {
            return this.Write("37");
        }

        private bool Write(string code)
        {
            byte[] bytes = new byte[1];

            bytes[0] = Convert.ToByte(code, 16);

            try
            {
                this._NetworkStream.Write(bytes, 0, 1);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (this._NetworkStream != null) this._NetworkStream.Close();
            if (this._TcpClient.Connected == true) this._TcpClient.Close();
        }
    }
}
