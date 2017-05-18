using System;

using System.ServiceModel.Description;

using Wodeyun.Bf.Execute.Wrappers;
using Wodeyun.Device.Gsms;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.Win.Modules
{
    public class ZhhwyModule : IDisposable
    {
        public event EventHandler DataReceived;

        private Zhhwy _Zhhwy = new Zhhwy();
        private PropertyCollection _PropertyCollection = new PropertyCollection();

        public ZhhwyModule()
        {
            this._PropertyCollection.Add(new SimpleProperty("Unique", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Parent", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Mobile", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Text", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Date", typeof(DateTime)));
            this._PropertyCollection.Add(new SimpleProperty("Remark", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Status", typeof(int)));
            this._PropertyCollection.Add(new SimpleProperty("Operator", typeof(int)));
        }

        public void Connect(string zhhwy, string sca, string reply, string forward)
        {
            try
            {
                string[] zhhwys = zhhwy.Split(":".ToCharArray());
                this._Zhhwy.Connect(zhhwys[0], zhhwys[1].ToInt32(), sca, reply, forward);

                this._Zhhwy.DataReceived += Zhhwy_DataReceived;
            }
            catch
            {
                throw;
            }
        }

        public bool SendMessage(string mobile, string text)
        {
            return this._Zhhwy.SendMessage(mobile, text);
        }

        public void Update(string unique, Entity entity)
        {
            ExecutorClient client = new ExecutorClient();
            client.Endpoint.Behaviors.Add(new WebHttpBehavior());
            client.Execute("Exchange.Single", "Update", new object[] { unique, entity });
            client.Close();
        }

        private void Zhhwy_DataReceived(object sender, EventArgs e)
        {
            Entity result = (Entity)sender;
            string date = result.GetValue("Date").ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            string mobile = result.GetValue("Mobile").ToString();
            string text = result.GetValue("Text").ToString();

            Entity entity = new Entity(this._PropertyCollection);
            entity.SetValue("Unique", string.Empty);
            entity.SetValue("Parent", string.Empty);
            entity.SetValue("Mobile", mobile);
            entity.SetValue("Text", text);
            entity.SetValue("Date", date);
            entity.SetValue("Remark", string.Empty);
            entity.SetValue("Status", 0);
            entity.SetValue("Operator", Program.Account);

            ExecutorClient client = new ExecutorClient();
            client.Endpoint.Behaviors.Add(new WebHttpBehavior());

            int length = text.Length;
            for (int i = 0; i < length - 1; i++)
            {
                try
                {
                    client.Execute("GsmMessage", "SaveEntity", new object[] { entity });
                    break;
                }
                catch
                {
                    if (i == length - 1) entity.SetValue("Text", "（全部是乱码）");
                    else entity.SetValue("Text", text.Substring(0, length - i - 1));
                }
            }
            client.Close();

            if (this.DataReceived != null) this.DataReceived(sender, e);
        }

        public void Dispose()
        {
            this._Zhhwy.Dispose();
        }
    }
}
