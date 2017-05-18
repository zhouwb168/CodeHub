using System;
using System.Collections.Generic;

using System.Collections;
using System.Threading;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Device.Rfids
{
    public class HchtCollection : IDisposable
    {
        private IList<string> _Addresses = new List<string>();
        private IList<int> _Ports = new List<int>();
        private IList<Hcht> _Hchts = new List<Hcht>();
        private bool _State = true;
        private IList<Thread> _Threads = new List<Thread>();
        private Dictionary<string, Queue<Entity>> _Datas = new Dictionary<string,Queue<Entity>>();

        public void Connect(string address, int port)
        {
            try
            {
                int index = this._Addresses.IndexOf(address);

                if (index == -1)
                {
                    this.AddAddressAndPort(address, port);
                    index = this._Addresses.Count - 1;
                }

                this._Hchts[index].Connect(this._Addresses[index], this._Ports[index]);

                this._Threads[index] = new Thread(new ParameterizedThreadStart(this.GetDatasThread));
                this._Threads[index].Start(index);
            }
            catch
            {
                throw;
            }
        }

        private void AddAddressAndPort(string address, int port)
        {
            this._Addresses.Add(address);
            this._Ports.Add(port);
            this._Hchts.Add(new Hcht());
            this._Threads.Add(null);
            this._Datas.Add(string.Format("{0}:{1}", address, port.ToString()), new Queue<Entity>());
        }

        private void GetDatasThread(object index)
        {
            PropertyCollection properties = new PropertyCollection();
            properties.Add(new SimpleProperty("Reader", typeof(int)));
            properties.Add(new SimpleProperty("Rfid", typeof(string)));
            properties.Add(new SimpleProperty("Date", typeof(DateTime)));
            properties.Add(new SimpleProperty("Address", typeof(string)));
            properties.Add(new SimpleProperty("Port", typeof(int)));

            while (this._State == true)
            {
                EntityCollection datas = this._Hchts[index.ToInt32()].GetDatas();

                if (datas.Count == 0) Thread.Sleep(10);
                else
                {
                    for (int i = 0; i < datas.Count; i++)
                    {
                        Entity entity = new Entity(properties);
                        entity.SetValue("Reader", datas[i].GetValue("Reader"));
                        entity.SetValue("Rfid", datas[i].GetValue("Rfid"));
                        entity.SetValue("Date", datas[i].GetValue("Date"));
                        entity.SetValue("Address", this._Addresses[index.ToInt32()]);
                        entity.SetValue("Port", this._Ports[index.ToInt32()]);

                        Monitor.Enter(this);

                        this._Datas[string.Format("{0}:{1}", this._Addresses[index.ToInt32()], this._Ports[index.ToInt32()].ToString())].Enqueue(entity);

                        Monitor.Exit(this);
                    }
                }
            }
        }

        public void Close()
        {
            for (int i = 0; i < this._Hchts.Count; i++)
            {
                this._Hchts[i].Close();
            }

            this._State = false;

            try
            {
                for (int i = 0; i < this._Threads.Count; i++)
                {
                    this._Threads[i].Abort();
                }
            }
            catch { }
        }

        public IList<Entity> GetDatas(string ip)
        {
            IList<Entity> results = new List<Entity>();

            Monitor.Enter(this);

            for (int i = 0; i < this._Datas[ip].Count; i++)
            {
                results.Add(this._Datas[ip].Dequeue());
            }

            Monitor.Exit(this);

            return results;
        }

        public void Dispose()
        {
            this.Close();

            for (int i = 0; i < this._Hchts.Count; i++)
            {
                this._Hchts[i].Dispose();
            }
        }
    }
}
