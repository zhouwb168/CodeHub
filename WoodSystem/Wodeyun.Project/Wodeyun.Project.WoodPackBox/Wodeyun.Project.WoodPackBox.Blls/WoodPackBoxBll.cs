using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodPackBox.Dals;
using Wodeyun.Project.WoodPackBox.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.WoodPackBox.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodPackBoxBll : CommonBll, IWoodPackBoxInterface
    {
        private WoodPackBoxDal _Dal = new WoodPackBoxDal();

        public WoodPackBoxBll()
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
            string table = "WoodUnPackBox";

            /* 判断当前记录是否在化验室的样品拆箱数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("删除失败，该样品的箱子已经被送到化验室并拆箱完毕，不允许再删除该记录");
        }

        public Entity DeleteEntityByUniqueWithOperator(int intUnique, int intWoodID, int operatorID)
        {
            if (intUnique == 0) return Helper.GetEntity(false, "删除失败，该样品还没有装箱记录");

            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(intWoodID);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(intUnique, intWoodID, operatorID); // 删除还未被化验室拆箱的记录

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows > 0) operateResult = Helper.GetEntity(true, "删除记录成功");
                else operateResult = Helper.GetEntity(false, "删除失败，可能该样品的箱子已经被送到化验室并拆箱完毕，不能再删除");

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
            string table = "WoodUnPackBox";

            /* 判断当前记录是否在化验室的样品拆箱数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该样品的箱子已经被送到化验室并拆箱完毕，不允许再修改");
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
            string table = "Factory";

            /*  判断当前关联的记录在料厂取样表里是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("添加失败，该条取样记录已经被料厂处删除");

            /* 判断当前关联的记录是否已经添加过 */
            table = "WoodPackBox";
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("添加失败，该条取样记录已经被装过箱了，不需要多次装箱");
        }

        public Entity SaveEntityByOperator(Entity entity)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveEntityInsert(entity.GetValue("WoodID"));

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("PackTime", typeof(DateTime)), strCrateDate);
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

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID); // 根据当前操作者身份获取该操作者添加的记录
        }

    }
}
