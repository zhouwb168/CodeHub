using System;
using System.Collections.Generic;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Device.Gsms
{
    public class Merger
    {
        private PropertyCollection _PropertyCollection = new PropertyCollection();

        public Merger()
        {
            this._PropertyCollection.Add(new SimpleProperty("Mobile", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Key", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Count", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Index", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Text", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Date", typeof(DateTime)));
        }

        public EntityCollection Merge(Dictionary<int, Entity> collection)
        {
            EntityCollection results = new EntityCollection(this._PropertyCollection);

            foreach (var item in collection)
            {
                string key = item.Value.GetValue("Key").TryString();

                if (key == string.Empty)
                {
                    Entity entity = new Entity(this._PropertyCollection);

                    entity.SetValue("Mobile", item.Value.GetValue("Mobile").ToString());
                    entity.SetValue("Text", item.Value.GetValue("Text").ToString());
                    entity.SetValue("Date", item.Value.GetValue("Date").ToString());

                    results.Add(entity);
                }
                else
                {
                    if (results.GetEntity("Key", key).IsEmpty == true)
                    {
                        EntityCollection keys = new EntityCollection(this._PropertyCollection);

                        foreach (var value in collection)
                        {
                            if (value.Value.GetValue("Key").TryString() == key) keys.Add(value.Value);
                        }

                        if (keys.Count == item.Value.GetValue("Count").ToInt32())
                        {
                            string text = string.Empty;
                            object tempMessageText = null;

                            for (int i = 0; i < item.Value.GetValue("Count").ToInt32(); i++)
                            {

                                if ((tempMessageText = keys.GetEntity("Index", (i + 1).ToString().PadLeft(2, '0')).GetValue("Text")) == null) continue;
                                text = text + tempMessageText.ToString();
                            }

                            Entity record = new Entity(this._PropertyCollection);

                            record.SetValue("Mobile", item.Value.GetValue("Mobile").ToString());
                            record.SetValue("Key", key);
                            record.SetValue("Text", text);
                            record.SetValue("Date", item.Value.GetValue("Date").ToString());

                            results.Add(record);
                        }
                    }
                }
            }

            return results;
        }
    }
}
