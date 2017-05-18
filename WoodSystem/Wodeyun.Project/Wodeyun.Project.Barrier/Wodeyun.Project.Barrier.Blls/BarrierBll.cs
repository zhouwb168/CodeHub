using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.Barrier.Dals;
using Wodeyun.Project.Barrier.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.Barrier.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BarrierBll : CommonBll, IBarrierInterface
    {
        private BarrierDal _Dal = new BarrierDal();

        public BarrierBll()
        {
            this.Dal = this._Dal;
        }

        public Entity GetEntityByFieldForWoodWhereComeFrom(string field, object value, string connect)
        {
            string table = "Wood";
            Entity entity = this._Dal.GetEntityByFieldForWoodWhereComeFrom(field, value, connect, table); // 到木材收购数据表里取木材来源地

            return entity;
        }

        /// <summary>
        /// 保存相片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity SavePhoto(Entity entity)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string msg = "";

            try
            {
                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                entity.Add(new SimpleProperty("Unique", typeof(int)), unique.GetValueByName("WoodCarPhoto"));
                entity.Add(new SimpleProperty("PhotoTime", typeof(DateTime)), strCrateDate);
                entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\"}");

               int rows = this._Dal.SavePhoto(entity);

                this._Dal.CommitTransaction();

                if (rows > 0) msg = "照片发送成功";
                else msg = "照片发送失败，可能是网络慢的原因，请再发送一次";

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
        /// <param name="greenCardNumber">绿卡号</param>
        /// <param name="cardStateBll">电子卡使用状态业务逻辑对象</param>
        /// <returns>该绿卡是否已存在使用状态记录</returns>
        bool CheckForSaveEntityInsert(object greenCardNumber, RfidCardBll cardStateBll)
        {
            /* 判断当前绿卡是否已在检查站登记，等待入厂验收 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(greenCardNumber.ToString(), (int)CardType.Green);
            if (cardStateEntity.GetValue("CardNumber") == null) return false; // 还未使用过

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.UnUse) throw new ValueDuplicatedException(string.Format("警报，该绿卡正在被使用中。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            return true;
        }

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID);
        }

        /// <summary>
        /// 这里重写了修改时的判断方法
        /// </summary>
        /// <param name="transaction">事务对象</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Entity ExecuteForSaveEntityUpdate(System.Data.SqlClient.SqlTransaction transaction, Entity entity)
        {
            string strFileName = "BarrierID";
            object objFileValue = entity.GetValue("Unique");
            string connect = "=";
            string table = "Check";

            /* 判断当前记录是否已被厂门口验收 */
            EntityCollection cardOfHasCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (cardOfHasCheck.Count != 0) throw new ValueDuplicatedException("修改失败，该绿卡已经被厂门口验收成功，不允许再修改该条记录");

            return entity;
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
                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    object greenCardNumber = entity.GetValue("CardNumber");
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    bool hasUseRecord = CheckForSaveEntityInsert(greenCardNumber, cardStateBll);

                    int newUnique = unique.GetValueByName(this._Dal.Table);
                    entity.SetValue("Unique", newUnique);
                    entity.Add(new SimpleProperty("TimeOfStation", typeof(DateTime)), strCrateDate);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);

                    affectedRows = this._Dal.InsertEntity(entity);
                    if (hasUseRecord) affectedRows = cardStateBll.UpdateState(greenCardNumber.ToString(), (int)CardType.Green, (int)CardState.Station, newUnique, (int)CardComeFrom.Station);
                    else affectedRows = cardStateBll.Insert(greenCardNumber.ToString(), (int)CardType.Green, (int)CardState.Station, newUnique, (int)CardComeFrom.Station);

                    msg = "发卡成功，请拍照并发送照片";
                }
                // 修改
                else
                {
                    entity = this.ExecuteForSaveEntityUpdate(this._Dal.SqlTransaction, entity);

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

        public Entity DeleteEntityByUniqueWithOperator(int unique, int operatorID, string greenCardNumber)
        {
            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(unique);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(unique, operatorID); // 删除还未被厂门口验收的记录
                if (affectedRows > 0)
                {
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    int affRows = cardStateBll.UpdateState(greenCardNumber, (int)CardType.Green, (int)CardState.UnUse);
                }

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows >0) operateResult = Helper.GetEntity(true, "删除成功");
                else operateResult = Helper.GetEntity(false, "删除失败，可能该绿卡已经被厂门口验收成功，不能再删除");

                return operateResult;
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 验证是否可以删除
        /// </summary>
        /// <param name="unique">要删除的记录唯一号</param>
        void ExecuteForDeleteEntity(int unique)
        {
            string strFileName = "BarrierID";
            object objFileValue = unique;
            string connect = "=";
            string table = "Check";

            /* 判断当前记录是否已被厂门口验收 */
            EntityCollection cardOfHasCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (cardOfHasCheck.Count != 0) throw new ValueDuplicatedException("删除失败，该绿卡已经被厂门口验收成功，不能再删除该记录");
        }

        public Entity GetEntityByFieldWithOperator(string greenCardNumber)
        {
            RfidCardBll cardStateBll = new RfidCardBll();
            /* 当前绿卡是否已在检查站登记，等待入厂验收 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(greenCardNumber, (int)CardType.Green);
            if (cardStateEntity.GetValue("CardNumber") == null) return Helper.GetEntity(false, "警报，该绿卡还未在移动服务站进行登记。当前状态：新卡，还未被使用");

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.Station) return Helper.GetEntity(false, string.Format("警报，该绿卡还未在移动服务站进行登记。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            Entity entity = this._Dal.GetEntityByFieldWithOperator(cardStateEntity.GetValue("RecordId").ToInt32());
            entity.Add(new SimpleProperty("Success", typeof(bool)), true);

            return entity;
        }
    }
}
