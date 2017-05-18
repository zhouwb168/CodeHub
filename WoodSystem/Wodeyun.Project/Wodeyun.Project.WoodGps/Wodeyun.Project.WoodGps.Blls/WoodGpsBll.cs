using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodGps.Dals;
using Wodeyun.Project.WoodGps.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.WoodGps.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodGpsBll : CommonBll, IWoodGpsInterface
    {
        private WoodGpsDal _Dal = new WoodGpsDal();

        public WoodGpsBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 修改前的验证
        /// </summary>
        /// <param name="entity"></param>
        public void CheckForSaveEntityUpdate(Entity entity)
        {
            /* 判断是否同名冲突 */
            SimpleProperty propertyOfFilter = new SimpleProperty("Unique", typeof(int));
            Entity entityOfFilter = new Entity(new PropertyCollection() { propertyOfFilter });
            entityOfFilter.SetValue("Unique", entity.GetValue("Unique"));
            entityOfFilter.Add(new SimpleProperty("StationName", typeof(string)), entity.GetValue("StationName"));

            SimpleProperty propertyOfConnector = new SimpleProperty("Unique", typeof(string));
            Entity entityOfConnector = new Entity(new PropertyCollection() { propertyOfConnector });
            entityOfConnector.SetValue("Unique", "<>");
            entityOfConnector.Add(new SimpleProperty("StationName", typeof(string)), "=");

            EntityCollection hasRecord = this._Dal.GetEntitiesByFilter(entityOfFilter, entityOfConnector);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该检查站名称已经被占用");
        }

        /// <summary>
        /// 添加前的验证
        /// </summary>
        /// <param name="stationName">检查站名称</param>
        void CheckForSaveEntityInsert(object stationName)
        {
            string fileName = "StationName";
            object fileValue = stationName;
            string connect = "=";

            /*  判断是否同名冲突 */
            EntityCollection hasRecord = this._Dal.GetEntitiesByField(fileName, fileValue, connect);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("添加失败，该检查站名称已经存在");
        }

        public Entity SaveEntityByOperator(Entity entity)
        {
            string msg = "";

            try
            {

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveEntityInsert(entity.GetValue("StationName"));

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                    entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                    this._Dal.InsertEntity(entity);

                    msg = "添加成功";
                }
                // 修改
                else
                {
                    CheckForSaveEntityUpdate(entity);

                    this._Dal.UpdateEntityByUnique(entity);

                    msg = "修改成功";
                }

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, msg, entity.GetValue("Unique").ToString());
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

    }
}
