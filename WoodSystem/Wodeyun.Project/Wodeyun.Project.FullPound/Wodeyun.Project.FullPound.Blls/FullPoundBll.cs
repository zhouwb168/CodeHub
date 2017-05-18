using System;
using System.ServiceModel.Activation;
using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.FullPound.Dals;
using Wodeyun.Project.FullPound.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Wodeyun.Project.FullPound.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FullPoundBll : CommonBll, IFullPoundInterface
    {
        private FullPoundDal _Dal = new FullPoundDal();

        public FullPoundBll()
        {
            this.Dal = this._Dal;
        }

        public Entity GetEntityByFieldForMatch(object license)
        {
            return this._Dal.GetEntityByFieldForMatch(license);
        }

        public Entity GetEntityByFieldWithOperator(string cardNumber)
        {
            RfidCardBll cardStateBll = new RfidCardBll();
            /* 当前红卡是否首磅已登记，但还未回皮，等待取样 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(cardNumber, (int)CardType.Red);
            if (cardStateEntity.GetValue("CardNumber") == null) return Helper.GetEntity(false, "警报，该红卡还未进行首磅登记。当前状态：新卡，还未被使用");

            int cardState = cardStateEntity.GetValue("CardState").ToInt32();
            if (cardState != (int)CardState.Balance && cardState != (int)CardState.Sample) return Helper.GetEntity(false, string.Format("警报，该红卡还未进行首磅登记。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            Entity entity = this._Dal.GetEntityByFieldWithOperator(cardStateEntity.GetValue("RecordId").ToInt32());
            entity.Add(new SimpleProperty("Success", typeof(bool)), true);

            return entity;
        }

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID, string StartTime, string EndTime, string CarID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID, StartTime, EndTime, CarID);
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
            string table = "Factory";

            /* 判断当前记录是否在料厂数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("删除失败，该车辆已经到达料厂卸货并取样完毕，不允许再删除该记录");
        }

        public Entity DeleteEntityByUniqueWithOperator(int intUnique, int intWoodID, int operatorID, string greenCardNumber, string redCardNumber)
        {
            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(intWoodID);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(intUnique, intWoodID, operatorID); // 删除还未被料厂取样的记录
                if (affectedRows > 0)
                {
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    int affRows = cardStateBll.UpdateState(greenCardNumber, (int)CardType.Green, (int)CardState.Door);
                    affRows = cardStateBll.UpdateState(redCardNumber, (int)CardType.Red, (int)CardState.UnUse);
                }

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows > 0)
                {
                    operateResult = Helper.GetEntity(true, "删除记录成功");
                    SyncDeleteFullVolumeData(intWoodID);
                }
                else
                {
                    operateResult = Helper.GetEntity(false, "删除失败，可能该车辆已经到达料厂卸货并取样完毕，不能再删除");
                }

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
            string table = "Factory";

            /* 判断当前记录是否在料厂数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该车辆已经到达料厂卸货并取样完毕，不允许再修改该记录");
        }

        public Entity SaveEntityByOperator(Entity entity, string greenCardNumber)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";
            string CtrlType = "NEW";
            try
            {
                if (entity.GetValue("FullVolume") != null && entity.GetValue("FullVolume").ToString().Trim() == "") entity.SetValue("FullVolume", null);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                int affectedRows;
                /* 新增 */
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    object woodID = entity.GetValue("WoodID");
                    object cardNumber = entity.GetValue("CardNumber");
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    bool hasUseRecord = CheckForSaveEntityInsert(woodID, cardNumber, cardStateBll);

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("WeighTime", typeof(DateTime)), strCrateDate);
                    entity.Add(new SimpleProperty("Printed", typeof(bool)), 0);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);

                    affectedRows = this._Dal.InsertEntity(entity);
                    affectedRows = cardStateBll.UpdateState(greenCardNumber, (int)CardType.Green, (int)CardState.Balance);
                    if (hasUseRecord) affectedRows = cardStateBll.UpdateState(cardNumber.ToString(), (int)CardType.Red, (int)CardState.Balance, woodID.ToInt32());
                    else affectedRows = cardStateBll.Insert(cardNumber.ToString(), (int)CardType.Red, (int)CardState.Balance, woodID.ToInt32(), (int)CardComeFrom.Weighbridge);

                    msg = "添加记录成功#" + strCrateDate;
                }
                /* 修改 */
                else
                {
                    CheckForSaveEntityUpdate(entity.GetValue("WoodID"));

                    affectedRows = this._Dal.UpdateEntityByUniqueWithOperator(entity);

                    msg = "修改记录成功";

                    CtrlType = "EDIT";
                }

                this._Dal.CommitTransaction();

                SyncFullVolumeData(entity, CtrlType);

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
        /// <param name="woodID">关联的木材编号 (来自于Wood表）</param>
        /// <param name="cardNumber">红卡号</param>
        /// <param name="cardStateBll">电子卡使用状态业务逻辑对象</param>
        /// <returns>该红卡是否已存在使用状态记录</returns>
        bool CheckForSaveEntityInsert(object woodID, object cardNumber, RfidCardBll cardStateBll)
        {
            string strFileName = "Unique";
            object objFileValue = woodID;
            string connect = "=";
            string table = "Wood";

            /*  判断当前绿卡号关联的记录是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("添加失败，该绿卡在厂门口的登记记录已经被删除");

            /* 判断当前红卡是否正在被使用，还未被回收 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(cardNumber.ToString(), (int)CardType.Red);
            if (cardStateEntity.GetValue("CardNumber") == null) return false; // 还未使用过

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.UnUse) throw new ValueDuplicatedException(string.Format("警报，该红卡正在被使用中。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            /* 判断当前绿卡关联的记录是否已添加过 */
            strFileName = "WoodID";
            table = "FullPound";
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("添加失败，这张绿卡的记录之前已经被添加过，不允许再次添加");

            return true;
        }

        public EntityCollection GetWoodBangPrintInfo(int start, int length, int operatorID, string startDate, string endDate, string License)
        {
            return this._Dal.GetWoodBangPrintInfo(start, length, operatorID, startDate, endDate, License);
        }

        /// <summary>
        /// 量方数据同步到工业互联网
        /// </summary>
        private void SyncFullVolumeData(Entity entity, string CtrlType)
        {
            try
            {
                string IsSync = WebConfigurationManager.AppSettings["IsSync"].ToString();
                string Factory = WebConfigurationManager.AppSettings["Factory"].ToString();
                string ConnectionStringName = "ConnectionString" + Factory;
                if (IsSync.ToLower() == "true")
                {
                    this._Dal.SyncFullVolumeData(entity, ConnectionStringName, CtrlType);
                }
            }
            catch { }
        }

        /// <summary>
        /// 同步删除首磅数据
        /// </summary>
        private void SyncDeleteFullVolumeData(int WoodID)
        {
            try
            {
                string IsSync = WebConfigurationManager.AppSettings["IsSync"].ToString();
                string Factory = WebConfigurationManager.AppSettings["Factory"].ToString();
                string ConnectionStringName = "ConnectionString" + Factory;
                if (IsSync.ToLower() == "true")
                {
                    this._Dal.SyncDeleteFullVolumeData(WoodID, ConnectionStringName);
                }
            }
            catch { }
        }
    }
}
