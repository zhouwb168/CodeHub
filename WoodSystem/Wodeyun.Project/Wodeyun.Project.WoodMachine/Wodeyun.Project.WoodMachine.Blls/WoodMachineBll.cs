using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodMachine.Dals;
using Wodeyun.Project.WoodMachine.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.WoodMachine.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodMachineBll : CommonBll, IWoodMachineInterface
    {
        private WoodMachineDal _Dal = new WoodMachineDal();

        public WoodMachineBll()
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
            entityOfFilter.Add(new SimpleProperty("Name", typeof(string)), entity.GetValue("Name"));

            SimpleProperty propertyOfConnector = new SimpleProperty("Unique", typeof(string));
            Entity entityOfConnector = new Entity(new PropertyCollection() { propertyOfConnector });
            entityOfConnector.SetValue("Unique", "<>");
            entityOfConnector.Add(new SimpleProperty("Name", typeof(string)), "=");

            EntityCollection hasRecord = this._Dal.GetEntitiesByFilter(entityOfFilter, entityOfConnector);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该平板相机名称已经被占用");

            propertyOfFilter = new SimpleProperty("Unique", typeof(int));
            entityOfFilter = new Entity(new PropertyCollection() { propertyOfFilter });
            entityOfFilter.SetValue("Unique", entity.GetValue("Unique"));
            entityOfFilter.Add(new SimpleProperty("MachineNumber", typeof(string)), entity.GetValue("MachineNumber"));

            propertyOfConnector = new SimpleProperty("Unique", typeof(string));
            entityOfConnector = new Entity(new PropertyCollection() { propertyOfConnector });
            entityOfConnector.SetValue("Unique", "<>");
            entityOfConnector.Add(new SimpleProperty("MachineNumber", typeof(string)), "=");

            hasRecord = this._Dal.GetEntitiesByFilter(entityOfFilter, entityOfConnector);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该唯一识别码已经被占用");
        }

        /// <summary>
        /// 添加前的验证
        /// </summary>
        /// <param name="name">读卡器名称</param>
        /// <param name="machineNumber">唯一识别码</param>
        void CheckForSaveEntityInsert(object name, object machineNumber)
        {
            string fileName = "Name";
            object fileValue = name;
            string connect = "=";

            /*  判断是否同名冲突 */
            EntityCollection hasRecord = this._Dal.GetEntitiesByField(fileName, fileValue, connect);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("添加失，！该平板相机名称已经存在");
            fileName = "MachineNumber";
            fileValue = machineNumber;
            hasRecord = this._Dal.GetEntitiesByField(fileName, fileValue, connect);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("添加失败，该唯一识别码已经存在");
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
                    CheckForSaveEntityInsert(entity.GetValue("Name"), entity.GetValue("MachineNumber"));

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
