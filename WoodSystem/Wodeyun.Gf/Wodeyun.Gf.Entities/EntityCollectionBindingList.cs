using System;

using System.ComponentModel;

namespace Wodeyun.Gf.Entities
{
    public class EntityCollectionBindingList : BindingList<Entity>, ITypedList
    {
        private EntityCollection _EntityCollection;

        public EntityCollectionBindingList(EntityCollection entityCollection)
            : base(entityCollection)
        {
            this._EntityCollection = entityCollection;
        }

        #region ITypedList 成员

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return this._EntityCollection.PropertyCollection.GetProperties();
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
