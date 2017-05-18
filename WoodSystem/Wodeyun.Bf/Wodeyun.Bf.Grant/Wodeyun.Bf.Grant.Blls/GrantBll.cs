using System;
using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Bf.Grant.Dals;
using Wodeyun.Bf.Grant.Interfaces;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Grant.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GrantBll : CommonBll, IGrantInterface
    {
        private GrantDal _Dal = new GrantDal();

        public GrantBll()
        {
            this.Dal = this._Dal;
        }

        public override void ExecuteForSaveEntities(SqlTransaction transaction, object[] entities, UniqueDal unique)
        {
            // 删除
            this._Dal.UpdateEntitiesByRole(Helper.Deserialize(entities[0].ToString()).GetValue("Role").ToInt32());

            // 新增
            for (int i = 1; i < entities.Length; i++)
            {
                Entity entity = Helper.Deserialize(entities[i].ToString());

                entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}");

                this._Dal.InsertEntity(entity);
            }
        }
    }
}