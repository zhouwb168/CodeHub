namespace Wodeyun.Gf.Entities
{
    public class CollectionProperty : EntityCollection, IProperty
    {
        private string _Name;

        public CollectionProperty(string name, PropertyCollection propertyCollection)
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
