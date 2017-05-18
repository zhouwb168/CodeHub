using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodUnPackBox.Dals;
using Wodeyun.Project.WoodUnPackBox.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.WoodUnPackBox.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodUnPackBoxBll : CommonBll, IWoodUnPackBoxInterface
    {
        private WoodUnPackBoxDal _Dal = new WoodUnPackBoxDal();

        public WoodUnPackBoxBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 删除前的验证
        /// </summary>
        /// <param name="intWoodID">关联的木材编号 (来自于Wood表）</param>
        void ExecuteForDeleteEntity(int intWoodID)
        {
            string strFileName = "WoodID";
            object objFileValue = intWoodID;
            string connect = "=";
            string table = "WoodLaboratory";

            /* 判断当前记录是否在化验室的化验报告单数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("删除失败，该样品已经化验完毕并填写好了报告单，不允许再删除该记录");
        }

        public Entity DeleteEntityByUniqueWithOperator(int intUnique, int intWoodID, int operatorID)
        {
            if (intUnique == 0) return Helper.GetEntity(false, "删除失败，该样品还没有拆箱记录");

            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(intWoodID);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(intUnique, intWoodID, operatorID); // 删除还未出化验报告单的记录

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows > 0) operateResult = Helper.GetEntity(true, "删除记录成功");
                else operateResult = Helper.GetEntity(false, "删除失败，可能该样品已经化验完毕并填写好了报告单，不能再删除");

                return operateResult;
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 修改前的验证
        /// </summary>
        /// <param name="objWoodID">关联的木材编号 (来自于Wood表）</param>
        public void CheckForSaveEntityUpdate(object objWoodID)
        {
            string strFileName = "WoodID";
            object objFileValue = objWoodID;
            string connect = "=";
            string table = "WoodLaboratory";

            /* 判断当前记录是否在化验室的化验报告单数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该样品已经化验完毕并填写好了报告单，不允许再修改");
        }

        /// <summary>
        /// 添加前的验证
        /// </summary>
        /// <param name="objWoodID">关联的木材编号 (来自于Wood表）</param>
        void CheckForSaveEntityInsert(object objWoodID)
        {
            string strFileName = "WoodID";
            object objFileValue = objWoodID;
            string connect = "=";
            string table = "WoodPackBox";

            /*  判断当前关联的记录在料厂送样表里是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("添加失败，该条送样记录已经被料厂处删除");
        }

        public Entity SaveEntityByOperator(Entity entity)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveEntityInsert(entity.GetValue("WoodID"));

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("UnPackTime", typeof(DateTime)), strCrateDate);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);

                    this._Dal.InsertEntity(entity);

                    msg = "添加记录成功";
                }
                // 修改
                else
                {
                    CheckForSaveEntityUpdate(entity.GetValue("WoodID"));

                    this._Dal.UpdateEntityByUniqueWithOperator(entity);

                    msg = "修改记录成功";
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

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(string date, int start, int length, int operatorID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(date, start, length, operatorID);
        }

    }
}
