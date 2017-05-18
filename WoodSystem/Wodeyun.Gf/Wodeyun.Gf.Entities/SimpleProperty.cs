using System;

namespace Wodeyun.Gf.Entities
{
    public class SimpleProperty : IProperty
    {
        private string _Name;
        private Type _Type;

        public SimpleProperty(string name, Type type)
        {
            this._Name = name;
            this._Type = type;
        }

        public string Name
        {
            get { return this._Name; }
        }

        public Type Type
        {
            get { return this._Type; }
        }
    }
}
