using System;
using System.Collections.Generic;

using System.Collections;
using System.ComponentModel;

using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Gf.Entities
{
    public class PropertyCollection : IList<IProperty>
    {
        private IList _Items = new ArrayList();

        private int _Current = 0;
        private int _Next = 1;
        
        public IList Items
        {
            get { return this._Items; }
        }

        public IProperty this[string name]
        {
            get
            {
                if (this._Next < this._Items.Count && (this._Items[this._Next] as IProperty).Name == name)
                {
                    this._Current = this._Next;
                    this._Next = this._Current + 1;
                    return this._Items[this._Current] as IProperty;
                }

                if (this._Current < this._Items.Count && (this._Items[this._Current] as IProperty).Name == name)
                    return this._Items[this._Current] as IProperty;

                for (int i = 0; i < this._Items.Count; i++)
                {
                    if (i != this._Current && i != this._Next && (this._Items[i] as IProperty).Name == name)
                    {
                        this._Current = i;
                        this._Next = this._Current + 1;
                        return this._Items[this._Current] as IProperty;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public int IndexOf(string name)
        {
            for (int i = 0; i < this._Items.Count; i++)
            {
                if (name == (this._Items[i] as IProperty).Name)
                    return i;
            }

            return -1;
        }

        public string ToDatabase()
        {
            if (this._Items.Count == 0) return string.Empty;

            string result = string.Empty;

            for (int i = 0; i < this._Items.Count; i++)
                result = result + "[" + (this._Items[i] as IProperty).Name + "], ";

            return result.Remove(result.Length - ", ".Length);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(this.GetPropertyDescriptors(this).ToArray());
        }

        private List<PropertyDescriptor> GetPropertyDescriptors(PropertyCollection propertyCollection, params string[] parents)
        {
            List<PropertyDescriptor> descriptors = new List<PropertyDescriptor>();

            for (int i = 0; i < propertyCollection.Items.Count; i++)
            {
                if (propertyCollection.Items[i] is SimpleProperty)
                    descriptors.Add(new SimplePropertyDescriptor(propertyCollection.Items[i] as SimpleProperty, parents));

                if (propertyCollection.Items[i] is ComplexProperty)
                {
                    string[] parameters = parents.Add((propertyCollection.Items[i] as ComplexProperty).Name);
                    List<PropertyDescriptor> collection = this.GetPropertyDescriptors((propertyCollection.Items[i] as ComplexProperty).PropertyCollection, parameters);

                    foreach (PropertyDescriptor item in collection)
                        descriptors.Add(item);
                }
            }

            return descriptors;
        }

        #region IList<IProperty> 成员

        public int IndexOf(IProperty item)
        {
            for (int i = 0; i < this._Items.Count; i++)
            {
                if (item.Name == (this._Items[i] as IProperty).Name)
                    return i;
            }

            return -1;
        }

        public void Insert(int index, IProperty item)
        {
            foreach (IProperty property in this._Items)
            {
                if (item.Name == property.Name)
                    throw new ValueFoundException();
            }

            this._Items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._Items.RemoveAt(index);
        }

        public IProperty this[int index]
        {
            get
            {
                if (index >= 0 && index < this._Items.Count)
                {
                    this._Current = index;
                    this._Next = this._Current + 1;
                    return this._Items[this._Current] as IProperty;
                }

                throw new IndexOutOfRangeException();
            }
            set { this._Items[index] = value; }
        }

        #endregion

        #region ICollection<IProperty> 成员

        public void Add(IProperty item)
        {
            foreach (IProperty property in this._Items)
            {
                if (item.Name == property.Name)
                    throw new ValueFoundException();
            }

            this._Items.Add(item);
        }

        public void Clear()
        {
            this._Items.Clear();
        }

        public bool Contains(IProperty item)
        {
            foreach (IProperty property in this._Items)
            {
                if (item.Name == property.Name)
                    return true;
            }

            return false;
        }

        public void CopyTo(IProperty[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this._Items.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(IProperty item)
        {
            foreach (IProperty property in this._Items)
            {
                if (item.Name == property.Name)
                {
                    this._Items.Remove(property);

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region IEnumerable<IProperty> 成员

        public IEnumerator<IProperty> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
