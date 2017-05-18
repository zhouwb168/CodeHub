using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Project.GsmMessage.Dals;
using Wodeyun.Project.GsmMessage.Helpers;
using Wodeyun.Project.GsmMessage.Interfaces;

namespace Wodeyun.Project.GsmMessage.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmMessageBll : CommonBll, IGsmMessageInterface
    {
        private GsmMessageDal _Dal = new GsmMessageDal();

        public GsmMessageBll()
        {
            this.Dal = this._Dal;
        }

        public override Entity ExecuteForSaveEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Parent").TryString() == string.Empty) entity.SetValue("Parent", null);
            if (entity.GetValue("Remark").TryString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public override Entity ExecuteForSaveEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Parent").TryString() == string.Empty) entity.SetValue("Parent", null);
            if (entity.GetValue("Remark").TryString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public EntityCollection GetEntitiesWithSupplierNameByDateAndSupplierAndMobileAndStartAndLength(string date, string supplier, string mobile, int start, int length)
        {
            return this._Dal.GetEntitiesWithSupplierNameByDateAndSupplierAndMobileAndStartAndLength(date, supplier, mobile, start, length);
        }

        public EntityCollection GetItemsByText(string text)
        {
            using (MessageHelper helper = new MessageHelper())
            {
                return helper.GetItemsByText(text);
            }
        }
    }
}
