namespace Wodeyun.Gf.Entities
{
    public class ComplexProperty : Entity, IProperty
    {
        private string _Name;

        public ComplexProperty(string name, PropertyCollection propertyCollection)
            : base(propertyCollection)
        {
            this._Name = name;
        }

        public string Name
        {
            get { return this._Name; }
        }
    }
}
