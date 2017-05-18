using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Device.Rfids
{
    public class Hcht : IDisposable
    {
        private string _Address = string.Empty;
        private int _Port = 8899;
        private TcpClient _TcpClient = new TcpClient();
        private NetworkStream _NetworkStream = null;
        private bool _State = false;
        private Thread _ReadThread = null;
        private IList<byte> _Bytes = new List<byte>();

        private PropertyCollection _PropertyCollection = new PropertyCollection();

        public Hcht()
        {
            this._PropertyCollection.Add(new SimpleProperty("Reader", typeof(int)));
            this._PropertyCollection.Add(new SimpleProperty("Rfid", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Date", typeof(DateTime)));
        }
        
        public void Connect(string address, int port)
        {
            try
            {
                this._Address = address;
                this._Port = port;

                this._TcpClient.Connect(IPAddress.Parse(this._Address), this._Port);
                this._NetworkStream = this._TcpClient.GetStream();
                this._State = true;
                this._ReadThread = new Thread(new ThreadStart(this.ReadThread));
                this._ReadThread.Start();
            }
            catch
            {
                throw;
            }
        }

        private void ReadThread()
        {
            while (this._State == true)
            {
                if (this._TcpClient.Connected == true)
                {
                    try
                    {
                        int value = this._NetworkStream.ReadByte();

                        if (value == -1) Thread.Sleep(10);
                        else
                        {
                            Monitor.Enter(this);

                            this._Bytes.Add((byte)value);

                            Monitor.Exit(this);
                        }
                    }
                    catch { }
                }
                else this._State = false;
            }
        }

        public void Close()
        {
            this._State = false;

            try
            {
                this._ReadThread.Abort();
            }
            catch { }
        }

        public EntityCollection GetDatas()
        {
            EntityCollection results = new EntityCollection(this._PropertyCollection);

            while (this._Bytes.Count >= 17)
            {
                bool check = this.Check();

                if (check == false)
                {
                    this.Dequeue();
                    this.Dequeue();
                }
                else
                {
                    this.Dequeue();
                    string reader = this.Dequeue().ToString();
                    string rfid01 = this.Dequeue().ToString("X2");
                    string rfid02 = this.Dequeue().ToString("X2");
                    string rfid03 = this.Dequeue().ToString("X2");
                    string rfid04 = this.Dequeue().ToString("X2");
                    string rfid05 = this.Dequeue().ToString("X2");
                    string rfid06 = this.Dequeue().ToString("X2");
                    string rfid07 = this.Dequeue().ToString("X2");
                    string rfid08 = this.Dequeue().ToString("X2");
                    string rfid09 = this.Dequeue().ToString("X2");
                    string rfid10 = this.Dequeue().ToString("X2");
                    string rfid11 = this.Dequeue().ToString("X2");
                    string rfid12 = this.Dequeue().ToString("X2");
                    this.Dequeue();
                    this.Dequeue();
                    this.Dequeue();

                    string rfid = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", rfid01, rfid02, rfid03, rfid04, rfid05, rfid06, rfid07, rfid08, rfid09, rfid10, rfid11, rfid12);

                    Entity entity = new Entity(this._PropertyCollection);
                    entity.SetValue("Reader", reader.ToInt32());
                    entity.SetValue("Rfid", rfid);
                    entity.SetValue("Date", DateTime.Now);

                    results.Add(entity);
                }
            }

            return results;
        }

        private int Dequeue()
        {
            int value = this._Bytes[0];

            Monitor.Enter(this);

            this._Bytes.RemoveAt(0);

            Monitor.Exit(this);

            return value;
        }

        private bool Check()
        {
            if (this._Bytes[0] != Convert.ToByte("00", 16)) return false;
            if (this._Bytes[15] != this.Sum(0, 14)) return false;
            if (this._Bytes[16] != Convert.ToByte("FF", 16)) return false;

            return true;
        }

        private byte Sum(int from, int to)
        {
            int result = 0;

            for (int i = from; i <= to; i++)
            {
                result = result + Convert.ToByte(this._Bytes[i]);
            }

            if (result % 256 == 0) return 0;
            else return Convert.ToByte(256 - result % 256);
        }

        public void Dispose()
        {
            this.Close();

            if (this._NetworkStream != null) this._NetworkStream.Close();
            if (this._TcpClient.Connected == true) this._TcpClient.Close();
        }
    }
}
