using System;

using System.Net;
using System.Net.Sockets;

namespace Wodeyun.Device.Controllers
{
    public class Pmyk8 : IDisposable
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

                this.OpenGreen();
            }
            catch
            {
                throw;
            }
        }

        public void OpenRed()
        {
            this.Write("04");
        }

        public void OpenOrange()
        {
            this.Write("03");
        }

        private void OpenGreen()
        {
            this.Write("02");
        }

        public void OpenBeep()
        {
            this.Write("05");
        }

        public void CloseRed()
        {
            this.Write("22");
        }

        public void CloseOrange()
        {
            this.Write("21");
        }

        public void CloseGreen()
        {
            this.Write("20");
        }

        public void CloseBeep()
        {
            this.Write("23");
        }

        private void Write(string code)
        {
            byte[] bytes = new byte[8];

            bytes[0] = Convert.ToByte("BB", 16);
            bytes[1] = Convert.ToByte("BB", 16);
            bytes[2] = Convert.ToByte("03", 16);
            bytes[3] = Convert.ToByte("03", 16);
            bytes[4] = Convert.ToByte(code, 16);
            bytes[5] = Convert.ToByte(code, 16);
            bytes[6] = Convert.ToByte("FF", 16);
            bytes[7] = Convert.ToByte("FF", 16);

            this._NetworkStream.Write(bytes, 0, 8);
        }

        public void Dispose()
        {
            if (this._NetworkStream != null) this.CloseGreen();

            if (this._NetworkStream != null) this._NetworkStream.Close();
            if (this._TcpClient.Connected == true) this._TcpClient.Close();
        }
    }
}
