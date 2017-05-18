using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.Recover.Dals;
using Wodeyun.Project.Recover.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.Recover.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RecoverBll : CommonBll, IRecoverInterface
    {
        private RecoverDal _Dal = new RecoverDal();

        public RecoverBll()
        {
            this.Dal = this._Dal;
        }

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID);
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
            string table = "EmptyPound";

            /*  判断当前绿卡关联的记录是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("回收失败，该绿卡在地磅处的回皮记录已经被删除");

            /* 判断当前绿卡关联的记录是否已经回收 */
            EntityCollection hasRecord = this._Dal.GetEntitiesByField(strFileName, objFileValue, connect);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("回收失败，该绿卡之前已经被回收过，不用再次回收");
        }

        public Entity SaveEntityByOperator(Entity entity, string greenCardNumber)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {
                this._Dal.BeginTransaction();

                CheckForSaveEntityInsert(entity.GetValue("WoodID"));

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                // 新增
                entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                entity.Add(new SimpleProperty("RecoverTime", typeof(DateTime)), strCrateDate);
                entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");
                
                int affectedRows;
                RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                affectedRows = this._Dal.InsertEntity(entity);
                affectedRows = cardStateBll.UpdateState(greenCardNumber, (int)CardType.Green, (int)CardState.UnUse);

                this._Dal.CommitTransaction();
                
                msg = "回收成功";

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
