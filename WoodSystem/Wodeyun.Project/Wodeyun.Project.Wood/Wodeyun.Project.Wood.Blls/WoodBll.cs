using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.Wood.Dals;
using Wodeyun.Project.Wood.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.Wood.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodBll : CommonBll, IWoodInterface
    {
        private WoodDal _Dal = new WoodDal();

        public WoodBll()
        {
            this.Dal = this._Dal;
        }

        public Entity GetEntityByFieldWithOperator(string cardNumber)
        {
            RfidCardBll cardStateBll = new RfidCardBll();
            /* 当前绿卡是否入厂已登记，等待首磅 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(cardNumber, (int)CardType.Green);
            if (cardStateEntity.GetValue("CardNumber") == null) return Helper.GetEntity(false, "警报，该绿卡还未在厂门口登记。当前状态：新卡，还未被使用");

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.Door) return Helper.GetEntity(false, string.Format("警报，该绿卡还未在厂门口登记。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            Entity entity  = this._Dal.GetEntityByFieldWithOperator(cardStateEntity.GetValue("RecordId").ToInt32());
            entity.Add(new SimpleProperty("Success", typeof(bool)), true);

            return entity;
        }

        /// <summary>
        /// 删除前的验证
        /// </summary>
        /// <param name="intUnique">要删除的记录唯一号</param>
        void ExecuteForDeleteEntity(int intUnique)
        {
            string strFileName = "WoodID";
            object objFileValue = intUnique;
            string connect = "=";
            string table = "FullPound";

            /* 判断当前记录是否在首磅数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("删除失败，该车辆已经到达地磅并首磅完毕，不允许再删除该记录");
        }

        public Entity DeleteEntityByUniqueWithOperator(int intUnique, int operatorID, string cardNumber)
        {
            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(intUnique);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(intUnique, operatorID); // 删除还未首磅的记录
                if (affectedRows > 0)
                {
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    int affRows = cardStateBll.UpdateState(cardNumber, (int)CardType.Green, (int)CardState.UnUse);
                }

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows > 0) operateResult = Helper.GetEntity(true, "删除成功");
                else operateResult = Helper.GetEntity(false, "删除失败，可能该车辆已经到达地磅并首磅完毕，不能再删除");

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
        /// <param name="objUnique">木材记录编号</param>
        public void CheckForSaveEntityUpdate(object objUnique)
        {
            string strFileName = "WoodID";
            object objFileValue = objUnique;
            string connect = "=";
            string table = "FullPound";

            /* 判断当前记录是否在首磅数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该车辆已经到达地磅并首磅完毕，不允许再修改该记录");
        }

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID);
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

                int affectedRows;
                /* 新增 */
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    object cardNumber = entity.GetValue("CardNumber");
                    bool hasUseRecord = CheckForSaveEntityInsert(cardNumber, cardStateBll);

                    int newUnique = unique.GetValueByName(this._Dal.Table);
                    entity.SetValue("Unique", newUnique);
                    entity.Add(new SimpleProperty("BarrierID", typeof(int)), null);
                    entity.Add(new SimpleProperty("ArriveDate", typeof(DateTime)), strCrateDate);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);

                    affectedRows = this._Dal.InsertEntity(entity);
                    if (hasUseRecord) affectedRows = cardStateBll.UpdateState(cardNumber.ToString(), (int)CardType.Green, (int)CardState.Door, newUnique, (int)CardComeFrom.Factry);
                    else affectedRows = cardStateBll.Insert(cardNumber.ToString(), (int)CardType.Green, (int)CardState.Door, newUnique, (int)CardComeFrom.Factry);

                    msg = "发卡成功";
                }
                // 修改
                else
                {
                    CheckForSaveEntityUpdate(entity.GetValue("Unique"));

                    affectedRows = this._Dal.UpdateEntityByUniqueWithOperator(entity);

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

        /// <summary>
        /// 添加前的验证
        /// </summary>
        /// <param name="cardNumber">绿卡号</param>
        /// <param name="cardStateBll">电子卡使用状态业务逻辑对象</param>
        /// <returns>该绿卡是否已存在使用状态记录</returns>
        public bool CheckForSaveEntityInsert(object cardNumber, RfidCardBll cardStateBll)
        {
            /* 当前绿卡是否正在被使用，还未被回收 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(cardNumber.ToString(), (int)CardType.Green);
            if (cardStateEntity.GetValue("CardNumber") == null) return false; // 还未使用过

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.UnUse) throw new ValueDuplicatedException(string.Format("警报，该绿卡正在被使用中。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            return true;
        }

    }
}
