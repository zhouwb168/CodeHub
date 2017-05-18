using System;

using System.ComponentModel;

namespace Wodeyun.Gf.Entities
{
    public class SimplePropertyDescriptor : PropertyDescriptor
    {
        private string[] _Parents = null;
        private SimpleProperty _SimpleProperty;

        public SimplePropertyDescriptor(SimpleProperty simpleProperty, params string[] parents)
            : base(simpleProperty.Name, null)
        {
            this._Parents = parents;
            this._SimpleProperty = simpleProperty;
        }

        #region PropertyDescriptor 成员

        public override bool CanResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override Type ComponentType
        {
            get { return typeof(Entity); }
        }

        public override object GetValue(object component)
        {
            return (component as Entity).GetValue(this._SimpleProperty, this._Parents);
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public override Type PropertyType
        {
            get { return this._SimpleProperty.Type; }
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            (component as Entity).SetValue(this._SimpleProperty, value, this._Parents);
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
