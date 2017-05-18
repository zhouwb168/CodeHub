using System;
using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Bf.Act.Dals;
using Wodeyun.Bf.Act.Interfaces;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Act.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ActBll : CommonBll, IActInterface
    {
        private ActDal _Dal = new ActDal();

        public ActBll()
        {
            this.Dal = this._Dal;
        }

        public override void ExecuteForSaveEntities(SqlTransaction transaction, object[] entities, UniqueDal unique)
        {
            // 删除
            this._Dal.UpdateEntitiesByAccount(Helper.Deserialize(entities[0].ToString()).GetValue("Account").ToInt32());

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

        public EntityCollection GetEntitiesWithFunctionByAccount(int account)
        {
            return this._Dal.GetEntitiesWithFunctionByAccount(account);
        }
    }
}