using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Gf.Entities
{
    [Serializable]
    public class EntityCollection : IList<Entity>, IListSource, ISerializable
    {
        private IList _Items = new ArrayList();
        private PropertyCollection _PropertyCollection;

        public EntityCollection(PropertyCollection propertyCollection)
        {
            this._PropertyCollection = propertyCollection;
        }

        public EntityCollection(SerializationInfo info, StreamingContext context)
        {
            int count = info.GetInt32("Count");
            string items = info.GetString("Items");
            Entity[] collection = this.Deserialize(items, count);

            if (collection.Length > 0)
                this._PropertyCollection = collection[0].PropertyCollection;

            for (int i = 0; i < collection.Length; i++)
            {
                this.Add(collection[i]);
            }

            this.Total = info.GetInt32("Total");
        }

        public PropertyCollection PropertyCollection
        {
            get { return this._PropertyCollection; }
        }

        public int Total { get; set; }

        public Entity[] ToArray()
        {
            return this.ToArray<Entity>();
        }

        public Entity GetEntity(string propertyName, object value)
        {
            Entity result = new Entity(this._PropertyCollection);

            int property = this._PropertyCollection.IndexOf(propertyName);

            for (int i = 0; i < this._Items.Count; i++)
            {
                if ((this._Items[i] as IList)[property].TryString() == value.TryString())
                    result = this[i];
            }

            return result;
        }

        public EntityCollection GetEntityCollection(string propertyName, object value)
        {
            EntityCollection result = new EntityCollection(this._PropertyCollection);

            int property = this._PropertyCollection.IndexOf(propertyName);

            for (int i = 0; i < this._Items.Count; i++)
            {
                if ((this._Items[i] as IList)[property].TryString() == value.TryString())
                    result.Add(this[i]);
            }

            return result;
        }

        public EntityCollection Remove(string propertyName, object value)
        {
            EntityCollection result = new EntityCollection(this._PropertyCollection);

            int property = this._PropertyCollection.IndexOf(propertyName);

            for (int i = 0; i < this._Items.Count; i++)
            {
                if ((this._Items[i] as IList)[property].TryString() != value.TryString())
                    result.Add(this[i]);
            }

            return result;
        }

        public EntityCollection GetEntityCollection(IList propertyNames, IList values)
        {
            EntityCollection result = new EntityCollection(this._PropertyCollection);

            for (int i = 0; i < this._Items.Count; i++)
            {
                int match = 0;

                for (int j = 0; j < propertyNames.Count; j++)
                {
                    int property = this._PropertyCollection.IndexOf(propertyNames[j].ToString());

                    if ((this._Items[i] as IList)[property].TryString() == values[j].TryString())
                        match = match + 1;
                }

                if (match == propertyNames.Count)
                    result.Add(this[i]);
            }

            return result;
        }

        public string Serialize(Entity[] collection)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(collection.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, collection);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private Entity[] Deserialize(string json, int count)
        {
            Entity[] collection = new Entity[count];
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(collection.GetType());

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (Entity[])serializer.ReadObject(stream);
            }
        }

        #region IList<Entity> 成员

        public int IndexOf(Entity item)
        {
            return this._Items.IndexOf(item.Values);
        }

        public void Insert(int index, Entity item)
        {
            this._Items.Insert(index, item.Values);
        }

        public void RemoveAt(int index)
        {
            this._Items.RemoveAt(index);
        }

        public Entity this[int index]
        {
            get { return new Entity(this._PropertyCollection, this._Items[index] as IList); }
            set { this._Items[index] = value.Values; }
        }

        #endregion

        #region ICollection<Entity> 成员

        public void Add(Entity item)
        {
            this._Items.Add(item.Values);
        }

        public void Clear()
        {
            this._Items.Clear();
        }

        public bool Contains(Entity item)
        {
            return this._Items.Contains(item.Values);
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            for (int i = 0; i < this._Items.Count; i++)
            {
                array.SetValue(this[i], arrayIndex);
                arrayIndex = arrayIndex + 1;
            }
        }

        public int Count
        {
            get { return this._Items.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Entity item)
        {
            if (this._Items.Contains(item.Values) == true)
            {
                this._Items.Remove(item.Values);

                return true;
            }
            else return false;
        }

        #endregion

        #region IEnumerable<Entity> 成员

        public IEnumerator<Entity> GetEnumerator()
        {
            for (int i = 0; i < this._Items.Count; i++)
			{
                yield return new Entity(this._PropertyCollection, this._Items[i] as IList);
			}
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IListSource 成员

        public bool ContainsListCollection
        {
            get { return false; }
        }

        public IList GetList()
        {
            return new EntityCollectionBindingList(this);
        }

        #endregion

        #region ISerializable 成员

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            string items = this.Serialize(this.ToArray<Entity>());

            info.AddValue("Count", this._Items.Count);
            info.AddValue("Items", items);
            info.AddValue("Total", this.Total.ToString());
        }

        #endregion
    }
}
