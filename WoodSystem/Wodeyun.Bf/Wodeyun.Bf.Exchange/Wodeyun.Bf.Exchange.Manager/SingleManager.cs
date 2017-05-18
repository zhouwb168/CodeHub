using System;
using System.Collections.Generic;

using System.Web;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;

namespace Wodeyun.Bf.Exchange.Manager
{
    public class SingleManager
    {
        private static SingleManager _Instance = new SingleManager();

        private Dictionary<string, HttpContext> _Downloaders = new Dictionary<string, HttpContext>();
        private Dictionary<string, Entity> _Datas = new Dictionary<string, Entity>();

        private SingleManager() { }

        public static SingleManager GetInstance()
        {
            return _Instance;
        }

        public void AddDownloader(string unique, HttpContext context)
        {
            if (this._Downloaders.ContainsKey(unique) == true)
                this._Downloaders.Remove(unique);

            this._Downloaders.Add(unique, context);
        }

        public void AddCommand(string unique, Entity entity)
        {
            try
            {
                if (this._Datas.ContainsKey(unique) == true)
                    this._Datas.Remove(unique);

                this._Datas.Add(unique, null);

                this.SendCommand(unique, entity);
            }
            catch
            {
                throw;
            }
        }

        public bool Update(string unique, Entity entity)
        {
            if (this._Datas.ContainsKey(unique) == false) return false;

            this._Datas[unique] = entity;

            return true;
        }

        private void SendCommand(string unique, Entity entity)
        {
            if (this._Downloaders.ContainsKey(unique) == false)
                throw new KeyNotFoundException();

            if (this._Downloaders[unique].Response.IsClientConnected == false)
            {
                this._Downloaders.Remove(unique);

                throw new ConnectionLostException();
            }

            this._Downloaders[unique].Response.Write(entity.Serialize(entity) + Environment.NewLine);
            this._Downloaders[unique].Response.Flush();
        }

        public Entity GetResult(string unique)
        {
            Entity result = this._Datas[unique];

            if (result != null) this._Datas.Remove(unique);

            return result;
        }
    }
}
