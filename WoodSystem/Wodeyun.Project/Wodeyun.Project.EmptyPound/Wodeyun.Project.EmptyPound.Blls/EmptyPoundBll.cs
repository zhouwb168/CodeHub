using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.EmptyPound.Dals;
using Wodeyun.Project.EmptyPound.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;
using System.Web.Configuration;

namespace Wodeyun.Project.EmptyPound.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EmptyPoundBll : CommonBll, IEmptyPoundInterface
    {
        private EmptyPoundDal _Dal = new EmptyPoundDal();

        public EmptyPoundBll()
        {
            this.Dal = this._Dal;
        }

        public Entity GetEntityByFieldWithOperator(string greenCardNumber)
        {
            RfidCardBll cardStateBll = new RfidCardBll();
            /* 当前绿卡是否回皮已登记，等待离厂回收 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(greenCardNumber, (int)CardType.Green);
            if (cardStateEntity.GetValue("CardNumber") == null) return Helper.GetEntity(false, "警报，该绿卡还未在地磅进行回皮登记。当前状态：新卡，还未被使用");

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.EmptyBalance) return Helper.GetEntity(false, string.Format("警报，该绿卡还未在地磅进行回皮登记。当前状态：{0}", cardStateEntity.GetValue("StateText")));

            Entity entity = this._Dal.GetEntityByFieldWithOperator(cardStateEntity.GetValue("RecordId").ToInt32());
            entity.Add(new SimpleProperty("Success", typeof(bool)), true);

            return entity;
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
            string table = "Recover";

            /* 判断当前记录是否在厂门口电子卡回收数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("删除失败，该车辆已经驶出厂区大门，不允许再删除该记录");
        }

        public Entity DeleteEntityByUniqueWithOperator(int intUnique, int intWoodID, int operatorID, string greenCardNumber, string redCardNumber, string bangCid)
        {
            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(intWoodID);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(intUnique, intWoodID, operatorID); // 删除车辆还未离厂的记录
                if (affectedRows > 0)
                {
                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    int affRows = cardStateBll.UpdateState(greenCardNumber, (int)CardType.Green, (int)CardState.Balance);
                    affRows = cardStateBll.UpdateState(redCardNumber, (int)CardType.Red, (int)CardState.Sample);
                }

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows > 0)
                {
                    operateResult = Helper.GetEntity(true, "删除记录成功");
                    SyncDeleteEmptyVolumeData(intWoodID);

                    SimpleProperty BangCID = new SimpleProperty("BangCID", typeof(string));
                    SimpleProperty BangID = new SimpleProperty("BangID", typeof(string));
                    Entity entity = new Entity(new PropertyCollection() { BangCID, BangID });
                    entity.SetValue(BangCID, bangCid);
                    entity.SetValue(BangID, "");
                    SyncVolumeDataForERP(entity, "DEL");
                }
                else
                {
                    operateResult = Helper.GetEntity(false, "删除失败，可能该车辆已经驶出厂区大门，不能再删除");
                }

                return operateResult;
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID, string StartTime, string EndTime, string CarID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID, StartTime, EndTime, CarID);
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
            string table = "Recover";

            /* 判断当前记录是否在门口电子卡回收数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该车辆已经驶出厂区大门，不允许再修改该记录");
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

            /*  判断当前红卡关联的记录是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("添加失败，该红卡在料厂的取样记录已经被删除");

            /* 判断当前红卡关联的记录是否已经回皮 */
            EntityCollection hasRecord = this._Dal.GetEntitiesByField(strFileName, objFileValue, connect);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("添加失败，该红卡的记录之前已经被添加过，请询问该车辆之前是否有回皮过");
        }

        public Entity SaveEntityByOperator(Entity entity, string greenCardNumber, string redCardNumber)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {
                if (entity.GetValue("EmptyVolume") != null && entity.GetValue("EmptyVolume").ToString().Trim() == "") entity.SetValue("EmptyVolume", null);
                if (entity.GetValue("HandVolume") != null && entity.GetValue("HandVolume").ToString().Trim() == "") entity.SetValue("HandVolume", null);
                if (entity.GetValue("RebateVolume") != null && entity.GetValue("RebateVolume").ToString().Trim() == "") entity.SetValue("RebateVolume", null);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);
                int affectedRows;
                /* 新增 */
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveEntityInsert(entity.GetValue("WoodID"));

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    //entity.Add(new SimpleProperty("BackWeighTime", typeof(DateTime)), strCrateDate);
                    entity.SetValue("BackWeighTime", strCrateDate);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);

                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    affectedRows = this._Dal.InsertEntity(entity);
                    affectedRows = cardStateBll.UpdateState(redCardNumber, (int)CardType.Red, (int)CardState.UnUse);
                    affectedRows = cardStateBll.UpdateState(greenCardNumber, (int)CardType.Green, (int)CardState.EmptyBalance);

                    msg = "添加记录成功";
                }
                /* 修改 */
                else
                {
                    CheckForSaveEntityUpdate(entity.GetValue("WoodID"));

                    affectedRows = this._Dal.UpdateEntityByUniqueWithOperator(entity);

                    msg = "修改记录成功";
                }

                this._Dal.CommitTransaction();

                if (affectedRows > 0)
                {
                    SyncEmptyVolumeData(entity);
                    SyncVolumeDataForERP(entity, "ADDOREDIT");
                }

                return Helper.GetEntity(true, msg, entity.GetValue("Unique").ToString());
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 获取当前车辆最近十车的平均首磅和回皮体积
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity getAvgVolume(Entity entity)
        {
            try
            {
                string License = entity.GetValue("License").TryString();
                if (!string.IsNullOrWhiteSpace(License))
                {
                    return this._Dal.getAvgVolume(License);
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 量方数据同步到工业互联网
        /// </summary>
        private void SyncEmptyVolumeData(Entity entity)
        {
            try
            {
                string IsSync = WebConfigurationManager.AppSettings["IsSync"].ToString();
                string Factory = WebConfigurationManager.AppSettings["Factory"].ToString();
                string ConnectionStringName = "ConnectionString" + Factory;
                if (IsSync.ToLower() == "true")
                {
                    this._Dal.SyncEmptyVolumeData(entity, ConnectionStringName);
                }
            }
            catch { }
        }

        /// <summary>
        /// 同步删除回皮数据
        /// </summary>
        private void SyncDeleteEmptyVolumeData(int WoodID)
        {
            try
            {
                string IsSync = WebConfigurationManager.AppSettings["IsSync"].ToString();
                string Factory = WebConfigurationManager.AppSettings["Factory"].ToString();
                string ConnectionStringName = "ConnectionString" + Factory;
                if (IsSync.ToLower() == "true")
                {
                    this._Dal.SyncDeleteEmptyVolumeData(WoodID, ConnectionStringName);
                }
            }
            catch { }
        }

        /// <summary>
        /// 同步量方数据到ERP
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="CtrlType"></param>
        private void SyncVolumeDataForERP(Entity entity, string CtrlType)
        {
            try
            {
                string IsSyncERP = WebConfigurationManager.AppSettings["IsSyncERP"].ToString();
                if (IsSyncERP.ToLower() == "true")
                {
                    string Factory = WebConfigurationManager.AppSettings["Factory"].ToString();
                    string OrgID = string.Empty;
                    switch (Factory)
                    {
                        case "FLMY"://丰林明阳
                            OrgID = "100008";
                            break;
                        case "FLBS"://丰林百色
                            OrgID = "100006";
                            break;
                        case "FLNN"://丰林南宁
                            OrgID = "100003";
                            break;
                    }
                    string ConnectionStringName = "ConnectionStringERP";
                    this._Dal.SyncVolumeDataForERP(entity, ConnectionStringName, OrgID, CtrlType);
                }
            }
            catch { }
        }
    }
}
