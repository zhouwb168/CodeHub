using System;
using System.Collections.Generic;

using System.Threading;
using System.Web;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;

namespace Wodeyun.Bf.Exchange.Manager
{
    public class MultiManager
    {
        private static MultiManager _Instance = new MultiManager();

        private Dictionary<string, HttpContext> _CommandDownloaders = new Dictionary<string, HttpContext>();
        private Dictionary<string, HttpContext> _ResultDownloaders = new Dictionary<string, HttpContext>();
        private Dictionary<string, Queue<Entity>> _Datas = new Dictionary<string, Queue<Entity>>();
        private Dictionary<string, Thread> _ResultThreads = new Dictionary<string, Thread>();

        private MultiManager() { }

        public static MultiManager GetInstance()
        {
            return _Instance;
        }

        public void AddCommandDownloader(string unique, HttpContext context)
        {
            if (this._CommandDownloaders.ContainsKey(unique) == true)
                this._CommandDownloaders.Remove(unique);

            this._CommandDownloaders.Add(unique, context);
        }

        public void AddResultDownloader(string unique, HttpContext context)
        {
            if (this._ResultDownloaders.ContainsKey(unique) == true)
                this._ResultDownloaders.Remove(unique);

            this._ResultDownloaders.Add(unique, context);
        }

        public void Start(string unique, Entity entity)
        {
            try
            {
                if (this._Datas.ContainsKey(unique) == true)
                    this._Datas.Remove(unique);
                if (this._ResultThreads.ContainsKey(unique) == true)
                {
                    try
                    {
                        this._ResultThreads[unique].Abort();
                    }
                    catch { }
                    this._ResultThreads.Remove(unique);
                }

                this._Datas.Add(unique, new Queue<Entity>());
                this._ResultThreads.Add(unique, new Thread(new ParameterizedThreadStart(this.SendResultThread)));
                this._ResultThreads[unique].Start(unique);

                this.SendCommand(unique, entity);
            }
            catch
            {
                throw;
            }
        }

        public void Finish(string unique, Entity entity)
        {
            try
            {
                this.ClearState(unique);

                this.SendCommand(unique, entity);
            }
            catch
            {
                throw;
            }
        }

        public bool Upload(string unique, Entity entity)
        {
            if (this._Datas.ContainsKey(unique) == false) return false;

            this.Enqueue(unique, entity);

            return true;
        }

        private void SendCommand(string unique, Entity entity)
        {
            if (this._CommandDownloaders.ContainsKey(unique) == false)
                throw new KeyNotFoundException();

            if (this._CommandDownloaders[unique].Response.IsClientConnected == false)
            {
                this._CommandDownloaders.Remove(unique);

                throw new ConnectionLostException();
            }

            this._CommandDownloaders[unique].Response.Write(entity.Serialize(entity) + Environment.NewLine);
            this._CommandDownloaders[unique].Response.Flush();
        }

        private void SendResultThread(object unique)
        {
            while (this._Datas.ContainsKey(unique.ToString()) == true)
            {
                if (this._Datas[unique.ToString()].Count != 0)
                {
                    if (this.CheckState(unique.ToString()) == true)
                    {
                        Entity entity = this.Dequeue(unique.ToString());
                        string result = string.Format("<script language='javascript'>parent.Execute('{0}');</script>", entity.Serialize(entity));

                        this._ResultDownloaders[unique.ToString()].Response.Write(result + Environment.NewLine);
                        this._ResultDownloaders[unique.ToString()].Response.Flush();
                    }
                    else this.ClearState(unique.ToString());
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void Enqueue(string unique, Entity entity)
        {
            Monitor.Enter(this);

            this._Datas[unique].Enqueue(entity);

            Monitor.Exit(this);
        }

        private Entity Dequeue(string unique)
        {
            Monitor.Enter(this);

            Entity entity = _Datas[unique.ToString()].Dequeue();

            Monitor.Exit(this);

            return entity;
        }

        private bool CheckState(string unique)
        {
            bool result = true;

            if (this._ResultDownloaders.ContainsKey(unique) == false) result = false;
            else
            {
                if (this._ResultDownloaders[unique].Response.IsClientConnected == false)
                {
                    this._ResultDownloaders.Remove(unique);

                    result = false;
                }
            }

            return result;
        }

        private void ClearState(string unique)
        {
            this._Datas.Remove(unique);
            try
            {
                this._ResultThreads[unique].Abort();
            }
            catch { }
            this._ResultThreads.Remove(unique);
        }
    }
}
