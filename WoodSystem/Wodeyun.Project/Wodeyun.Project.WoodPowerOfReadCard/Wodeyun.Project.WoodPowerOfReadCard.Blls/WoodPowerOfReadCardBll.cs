using System;
using System.Data.SqlClient;
using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Project.WoodPowerOfReadCard.Dals;
using Wodeyun.Project.WoodPowerOfReadCard.Interfaces;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodPowerOfReadCard.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodPowerOfReadCardBll : CommonBll, IWoodPowerOfReadCardInterface
    {
        private WoodPowerOfReadCardDal _Dal = new WoodPowerOfReadCardDal();

        public WoodPowerOfReadCardBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 通过用户ID获取该用户有权限使用的平板相机
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns></returns>
        public Entity GetEntityByAccount(int accountID)
        {
            return this._Dal.GetEntityByAccount(accountID);
        }

        public override void ExecuteForSaveEntities(SqlTransaction transaction, object[] entities, UniqueDal unique)
        {
            // 删除
            this._Dal.UpdateEntitiesByAccount(Helper.Deserialize(entities[0].ToString()).GetValue("AccountID").ToInt32());

            // 新增
            for (int i = 1; i < entities.Length; i++)
            {
                Entity entity = Helper.Deserialize(entities[i].ToString());

                entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                this._Dal.InsertEntity(entity);
            }
        }

    }
}
