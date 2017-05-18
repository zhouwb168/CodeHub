using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Project.GsmMessage.Dals;
using Wodeyun.Project.GsmMessage.Interfaces;

namespace Wodeyun.Project.GsmMessage.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmItemBll : CommonBll, IGsmItemInterface
    {
        private GsmItemDal _Dal = new GsmItemDal();

        public GsmItemBll()
        {
            this.Dal = this._Dal;
        }

        public override Entity ExecuteForSaveEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public override Entity ExecuteForSaveEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public EntityCollection GetEntitiesWithAreaUniqueByMessage(int message)
        {
            return this._Dal.GetEntitiesWithAreaUniqueByMessage(message);
        }

        public Entity GetEntityByDateAndLicense(string date, string license)
        {
            return this._Dal.GetEntityByDateAndLicense(date, license);
        }
    }
}
