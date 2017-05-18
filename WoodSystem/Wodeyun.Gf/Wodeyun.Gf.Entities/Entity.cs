using System;

using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Gf.Entities
{
    [Serializable]
    [KnownType(typeof(Entity[]))]
    [KnownType(typeof(EntityCollection))]
    public class Entity : DynamicObject, ICustomTypeDescriptor, ISerializable
    {
        private IList _Values = new ArrayList();
        private PropertyCollection _PropertyCollection;

        private int _Current = 0;
        private int _Next = 1;
        private IList<int> _Parents = new List<int>();

        public Entity(PropertyCollection propertyCollection)
        {
            this._PropertyCollection = propertyCollection;

            for (int i = 0; i < this._PropertyCollection.Items.Count; i++)
            {
                if (propertyCollection.Items[i] is SimpleProperty)
                    this._Values.Add(null);

                if (propertyCollection.Items[i] is ComplexProperty)
                    this._Values.Add(new Entity((propertyCollection.Items[i] as ComplexProperty).PropertyCollection));

                if (propertyCollection.Items[i] is CollectionProperty)
                    this._Values.Add(new EntityCollection((propertyCollection.Items[i] as CollectionProperty).PropertyCollection));
            }
        }

        public Entity(PropertyCollection propertyCollection, IList values)
        {
            this._PropertyCollection = propertyCollection;
            this._Values = values;
        }

        public Entity(SerializationInfo info, StreamingContext context)
        {
            this._PropertyCollection = new PropertyCollection();

            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            for (int i = 0; enumerator.MoveNext() == true; i++)
            {
                this._PropertyCollection.Add(new SimpleProperty(enumerator.Name, enumerator.ObjectType));
                this._Values.Add(enumerator.Value);
            }
        }

        public PropertyCollection PropertyCollection
        {
            get { return this._PropertyCollection; }
        }

        public IList Values
        {
            get { return this._Values; }
        }

        public bool IsEmpty
        {
            get
            {
                bool empty = true;

                for (int i = 0; i < this._Values.Count; i++)
                {
                    if (this._Values[i] != null) empty = false;
                }

                return empty;
            }
        }

        public string ToInsert(string inserts)
        {
            string result = string.Empty;
            string[] fields = inserts.Replace(" ", "").Split(", ".ToCharArray());

            for (int i = 0; i < fields.Length; i++)
            {
                string field = fields[i].TrimStart("[".ToCharArray()).TrimEnd("]".ToCharArray());
                string value = this.GetValue(field).ToDatabase();

                result = result + value;
                if (i != fields.Length - 1) result = result + ", ";
            }

            return result;
        }

        public string ToUpdate(string inserts, string updates)
        {
            string result = inserts;
            string[] fields = updates.Replace(" ", "").Split(", ".ToCharArray());

            for (int i = 0; i < fields.Length; i++)
            {
                string field = fields[i].TrimStart("[".ToCharArray()).TrimEnd("]".ToCharArray());
                string value = this.GetValue(field).ToDatabase();

                result = result.Replace(fields[i], value);
            }

            return result;
        }

        public string Serialize(Entity entity)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(entity.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, entity);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public void Add(IProperty property, object value)
        {
            this._PropertyCollection.Add(property);
            this._Values.Add(value);
        }

        public void Remove(string propertyName)
        {
            int index = this.PropertyCollection.IndexOf(propertyName);

            this._PropertyCollection.RemoveAt(index);
            this._Values.RemoveAt(index);
        }

        public object GetValue(SimpleProperty simpleProperty, params string[] parents)
        {
            return this.GetEntity(parents).GetValue(simpleProperty.Name);
        }

        public object GetValue(string propertyName, params string[] parents)
        {
            return this.GetEntity(parents).GetValue(propertyName);
        }

        public object GetValue(int propertyIndex, params string[] parents)
        {
            return this.GetEntity(parents).GetValue(propertyIndex);
        }

        private object GetValue(string propertyName)
        {
            if (this._Next < this._PropertyCollection.Items.Count && (this._PropertyCollection.Items[this._Next] as IProperty).Name == propertyName)
            {
                this._Current = this._Next;
                this._Next = this._Current + 1;
                return this._Values[this._Current];
            }

            if (this._Current < this._PropertyCollection.Items.Count && (this._PropertyCollection.Items[this._Current] as IProperty).Name == propertyName)
                return this._Values[this._Current];

            for (int i = 0; i < this._PropertyCollection.Items.Count; i++)
            {
                if (i != this._Current && i != this._Next && (this._PropertyCollection.Items[i] as IProperty).Name == propertyName)
                {
                    this._Current = i;
                    this._Next = this._Current + 1;
                    return this._Values[this._Current];
                }
            }

            throw new KeyNotFoundException();
        }

        private object GetValue(int propertyIndex)
        {
            if (propertyIndex >= 0 && propertyIndex < this._PropertyCollection.Items.Count)
            {
                this._Current = propertyIndex;
                this._Next = this._Current + 1;
                return this._Values[this._Current];
            }

            throw new IndexOutOfRangeException();
        }

        public void SetValue(SimpleProperty simpleProperty, object value, params string[] parents)
        {
            this.GetEntity(parents).SetValue(simpleProperty.Name, value);
        }

        public void SetValue(string propertyName, object value, params string[] parents)
        {
            this.GetEntity(parents).SetValue(propertyName, value);
        }

        public void SetValue(int propertyIndex, object value, params string[] parents)
        {
            this.GetEntity(parents).SetValue(propertyIndex, value);
        }

        private void SetValue(string propertyName, object value)
        {
            if (this._Next < this._PropertyCollection.Items.Count && (this._PropertyCollection.Items[this._Next] as IProperty).Name == propertyName)
            {
                this._Current = this._Next;
                this._Next = this._Current + 1;
                this._Values[this._Current] = value;

                return;
            }

            if (this._Current < this._PropertyCollection.Items.Count && (this._PropertyCollection.Items[this._Current] as IProperty).Name == propertyName)
            {
                this._Values[this._Current] = value;

                return;
            }

            for (int i = 0; i < this._PropertyCollection.Items.Count; i++)
            {
                if (i != this._Next && i != this._Current && (this._PropertyCollection.Items[i] as IProperty).Name == propertyName)
                {
                    this._Current = i;
                    this._Next = this._Current + 1;
                    this._Values[this._Current] = value;

                    return;
                }
            }

            throw new KeyNotFoundException();
        }

        private void SetValue(int propertyIndex, object value)
        {
            if (propertyIndex >= 0 && propertyIndex < this._PropertyCollection.Items.Count)
            {
                this._Current = propertyIndex;
                this._Next = this._Current + 1;
                this._Values[this._Current] = value;

                return;
            }

            throw new IndexOutOfRangeException();
        }

        private Entity GetEntity(params string[] parents)
        {
            Entity entity = this;

            if (parents != null)
            {
                for (int i = 0; i < parents.Length; i++)
                {
                    if (this._Parents.Count > i && (entity.PropertyCollection.Items[this._Parents[i]] as IProperty).Name == parents[i])
                    {
                        entity = entity.Values[this._Parents[i]] as Entity;
                    }
                    else
                    {
                        for (int j = 0; j < entity.PropertyCollection.Items.Count; j++)
                        {
                            if ((entity.PropertyCollection.Items[j] as IProperty).Name == parents[i])
                            {
                                entity = entity.Values[j] as Entity;

                                if (this._Parents.Count > i) this._Parents[i] = j;
                                else this._Parents.Add(j);
                                
                                break;
                            }
                        }
                    }
                }
            }

            return entity;
        }

        #region DynamicObject 成员

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = this.GetValue(binder.Name);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                this.SetValue(binder.Name, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region ICustomTypeDescriptor 成员

        public AttributeCollection GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string GetClassName()
        {
            throw new NotImplementedException();
        }

        public string GetComponentName()
        {
            throw new NotImplementedException();
        }

        public TypeConverter GetConverter()
        {
            throw new NotImplementedException();
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return this._PropertyCollection.GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor propertyDescriptor)
        {
            return this;
        }

        #endregion

        #region ISerializable 成员

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < this._PropertyCollection.Items.Count; i++)
                info.AddValue((this._PropertyCollection.Items[i] as IProperty).Name, this._Values[i]);
        }

        #endregion
    }
}
