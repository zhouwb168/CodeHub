using System;
using System.Collections.Generic;

using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Device.Gsms
{
    public class Zhhwy : IDisposable
    {
        public event EventHandler DataReceived;

        private string _Sca = string.Empty;
        private string _Reply = string.Empty;
        private string _Forward = string.Empty;
        private Clienter _Clienter = null;
        private Reader _Reader = null;
        private Commander _Commander = null;
        private bool _State = false;
        private Thread _ListThread = null;
        private Nullable<bool> _SendResult = null;
        private PropertyCollection _PropertyCollection = new PropertyCollection();

        public Zhhwy()
        {
            this._PropertyCollection.Add(new SimpleProperty("Mobile", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Key", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Count", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Index", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Text", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Date", typeof(DateTime)));
        }

        public void Connect(string address, int port, string sca, string reply, string forward)
        {
            try
            {
                this._Sca = sca;
                this._Reply = reply;
                this._Forward = forward;

                this._Clienter = new Clienter(address, port);
                this._Clienter.Connect();

                this._Reader = new Reader(this._Clienter);

                this._Commander = new Commander(this._Clienter, this._Reader, this._Sca);
                this._Commander.DataReceived += Commander_DataReceived;
                this._Commander.SendResult += Commander_SendResult;

                this._Commander.AddCommand("SetFormat", new object[] { });

                this._State = true;
                this._ListThread = new Thread(new ThreadStart(this.ListThread));
                this._ListThread.Start();
            }
            catch
            {
                throw;
            }
        }

        private void ListThread()
        {
            while (this._State == true)
            {
                if (this.DataReceived != null)
                {
                    this._Commander.AddCommand("ListMessages", new object[] { });

                    Thread.Sleep(1000 * 60);
                }
            }
        }

        public bool SendMessage(string mobile, string text)
        {
            this._SendResult = null;

            this._Commander.AddCommand("SendMessage", new object[] { mobile, text, true });

            while (this._State == true)
            {
                if (this._SendResult == null) Thread.Sleep(100);
                else break;
            }

            return this._SendResult.Value;
        }

        private void Commander_SendResult(object sender, EventArgs e)
        {
            this._SendResult = (bool)sender;
        }

        private void Commander_DataReceived(object sender, EventArgs e)
        {
            Dictionary<int, Entity> collection = this.GetCollection((Dictionary<int, string>)sender);

            // 合并短信
            EntityCollection records = (new Merger()).Merge(collection);

            // 保存短信
            for (int i = 0; i < records.Count; i++)
            {
                this.DataReceived(records[i], new EventArgs());
            }

            string tempMobile;
            // 发送回复
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].GetValue("Text").ToString().StartsWith("089168") == false)
                {
                    /* 判断是否有效的手机号码，无效时，则结束此次循环，继续下一次循环 */
                    tempMobile = records[i].GetValue("Mobile").ToString();
                    if (!Regex.IsMatch(tempMobile, @"(^13[0-9]{9}$)|(^15[012356789][0-9]{8}$)|(^18[02356789][0-9]{8}$)|(^147[0-9]{8}$)")) continue;

                    this._Commander.AddCommand("SendMessage", new object[] { tempMobile, this._Reply + DateTime.Now.ToString("MM-dd HH:mm"), false });

                    string text = records[i].GetValue("Text").ToString();
                    int length = text.Length / 67 + 1;

                    for (int j = 0; j < length; j++)
                    {
                        if (j == length - 1) this._Commander.AddCommand("SendMessage", new object[] { this._Forward, text.Substring(j * 67), false });
                        else this._Commander.AddCommand("SendMessage", new object[] { this._Forward, text.Substring(j * 67, 67), false });
                    }
                }
            }

            // 删除短信
            foreach (var item in collection)
            {
                string mobile = item.Value.GetValue("Mobile").ToString();
                string key = item.Value.GetValue("Key").TryString();

                IList properties = new ArrayList() { "Mobile", "Key" };
                IList values = new ArrayList() { mobile, key };

                if (records.GetEntityCollection(properties, values).Count > 0)
                    this._Commander.AddCommand("DeleteMessage", new object[] { item.Key });
            }
        }

        private Dictionary<int, Entity> GetCollection(Dictionary<int, string> pdus)
        {
            Dictionary<int, Entity> results = new Dictionary<int, Entity>();

            foreach (var item in pdus)
            {
                if (item.Value != "0891683108701705F004"
                 && item.Value != "0891683108700705F7240D916881"
                 && item.Value != "0891683108701705F0440D91685199342372F8000831703111039123120500037D020"
                 && item.Value != "0891683108"
                 && item.Value != "089168310870"
                 && item.Value != "0891683108701705F0440D91685199342372F8"
                 && item.Value != "0891683108706705F0440D91683137677691F900083190400"
                 && item.Value != "08916831")
                {
                    Entity entity = new Entity(this._PropertyCollection);
                    Entity ud = (new Decoder()).GetUd(item.Value);

                    entity.SetValue("Mobile", (new Decoder()).GetOa(item.Value));
                    entity.SetValue("Key", ud.GetValue("Key"));
                    entity.SetValue("Count", ud.GetValue("Count"));
                    entity.SetValue("Index", ud.GetValue("Index"));
                    entity.SetValue("Text", ud.GetValue("Text"));
                    entity.SetValue("Date", (new Decoder()).GetScts(item.Value).ToDateTime());

                    results.Add(item.Key, entity);
                }
            }

            return results;
        }

        public void Dispose()
        {
            this._State = false;

            try
            {
                this._ListThread.Abort();
            }
            catch { }

            if (this._Clienter != null) this._Clienter.Dispose();
            if (this._Reader != null) this._Reader.Dispose();
            if (this._Commander != null) this._Commander.Dispose();
        }
    }
}
