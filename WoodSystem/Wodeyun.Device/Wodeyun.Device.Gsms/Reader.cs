using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Device.Gsms
{
    public class Reader : IDisposable
    {
        private Clienter _Clienter = null;
        private bool _State = false;
        private Thread _GetResultThread = null;
        private Nullable<bool> _Result = null;
        private Dictionary<int, string> _Values = new Dictionary<int, string>();

        public Reader(Clienter clienter)
        {
            this._Clienter = clienter;

            this._State = true;
            this._GetResultThread = new Thread(new ThreadStart(this.GetResultThread));
            this._GetResultThread.Start();
        }

        public void Clear()
        {
            this._Result = null;
            this._Values.Clear();
        }

        public Nullable<bool> GetResult()
        {
            return this._Result;
        }

        public Dictionary<int, string> GetValues()
        {
            return this._Values;
        }

        private void GetResultThread()
        {
            while (this._State == true)
            {
                // 读取结果
                byte[] bytes = new byte[this._Clienter.ReceiveBufferSize];

                try
                {
                    this._Clienter.Read(bytes, 0, this._Clienter.ReceiveBufferSize);
                }
                catch { }

                if (bytes[2] != 0)
                {
                    // 转化为字符
                    IList<byte> datas = new List<byte>();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        if (bytes[i] != 0) datas.Add(bytes[i]);
                        else break;
                    }

                    string[] values = Encoding.UTF8.GetString(datas.ToArray<byte>()).Split(Environment.NewLine.ToCharArray());

                    // 判断结果
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] == "OK")
                        {
                            Dictionary<int, string> results = this.GetPdus(values);

                            foreach (var item in results)
                            {
                                if (this._Values.ContainsKey(item.Key) == false)
                                    this._Values.Add(item.Key, item.Value);
                            }

                            this._Result = true;
                        }
                        if (values[i] == "ERROR") this._Result = false;
                    }
                }

                Thread.Sleep(1000 * 3);
            }
        }

        private Dictionary<int, string> GetPdus(string[] values)
        {
            Dictionary<int, string> results = new Dictionary<int, string>();

            int index = int.MinValue;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].StartsWith("+CMGL") == true)
                {
                    string[] x = values[i].Split(',');
                    if (x[0].Length >= 7)
                    {
                        index =x[0].Substring(7).ToInt32();
                    }
                }
                if (values[i].StartsWith("0891") == true && values[i].EndsWith(((char)26).ToString()) != true) results.Add(index, values[i]);
            }

            return results;
        }

        public void Dispose()
        {
            this._State = false;

            try
            {
                this._GetResultThread.Abort();
            }
            catch { }

            this._Clienter.Dispose();
        }
    }
}
