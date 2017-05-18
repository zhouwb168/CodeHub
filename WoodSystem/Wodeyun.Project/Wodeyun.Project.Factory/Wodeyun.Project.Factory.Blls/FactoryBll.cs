using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.Factory.Dals;
using Wodeyun.Project.Factory.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.Factory.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FactorydBll : CommonBll, IFactoryInterface
    {
        private FactoryDal _Dal = new FactoryDal();

        public FactorydBll()
        {
            this.Dal = this._Dal;
        }

        public Entity GetEntityByFieldWithOperator(string redCardNumber)
        {
            RfidCardBll cardStateBll = new RfidCardBll();
            /* 当前红卡是否首磅已登记，但还未回皮，等待取样 */
            Entity cardStateEntity = cardStateBll.GetCardStateEntity(redCardNumber, (int)CardType.Red);
            if (cardStateEntity.GetValue("CardNumber") == null) return Helper.GetEntity(false, "警报，该红卡还未在料厂取样登记。当前状态：新卡，还未被使用");

            if (cardStateEntity.GetValue("CardState").ToInt32() != (int)CardState.Sample) return Helper.GetEntity(false, string.Format("警报，该红卡还未在料厂取样登记。当前状态：{0}", cardStateEntity.GetValue("StateText")));

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
            string table = "EmptyPound";

            /* 判断当前记录是否在回皮数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("删除失败，该车辆已经到地磅并回皮完毕，不允许再删除该记录");
        }

        public Entity DeleteEntityByUniqueWithOperator(int intUnique, int intWoodID, int operatorID, string redCardNumber)
        {
            try
            {
                this._Dal.BeginTransaction();

                ExecuteForDeleteEntity(intWoodID);

                int affectedRows = this._Dal.DeleteEntityByUniqueWithOperator(intUnique, intWoodID, operatorID); // 删除还未地磅回皮的记录
                if (affectedRows > 0)
                {
                    string strFileName = "WoodID";
                    object objFileValue = intWoodID;
                    string connect = "=";
                    string table = "Factory";

                    /* 判断当前记录在料厂处的取样记录是否已删除完 */
                    EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
                    if (hasRecord.Count == 0)
                    {
                        /* 料厂处的取样记录已删除完，则需要变更该红卡的状态 */
                        RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                        int affRows = cardStateBll.UpdateState(redCardNumber, (int)CardType.Red, (int)CardState.Balance);
                    }
                }

                this._Dal.CommitTransaction();

                Entity operateResult;
                if (affectedRows > 0) operateResult = Helper.GetEntity(true, "删除记录成功");
                else operateResult = Helper.GetEntity(false, "删除失败，可能该车辆已经到地磅并回皮完毕，不能再删除");

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
            string table = "EmptyPound";

            /* 判断当前记录是否在地磅回皮数据表里有关联的记录 */
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count != 0) throw new ValueDuplicatedException("修改失败，该车辆已经到地磅并回皮完毕，不允许再修改该记录");
        }

        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID); // 根据当前操作者身份获取该操作者添加的记录
        }

        /// <summary>
        /// 添加前的验证
        /// </summary>
        /// <param name="objWoodID">关联的木材编号 (来自于Wood表）</param>
        /// <param name="objOperator">操作员的身份识别</param>
        void CheckForSaveEntityInsert(object objWoodID, object objOperator)
        {
            string strFileName = "WoodID";
            object objFileValue = objWoodID;
            string connect = "=";
            string table = "FullPound";

            /*  判断当前红卡关联的记录是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("添加失败，该红卡在地磅的首磅记录已经被删除");

            /* 判断当前红卡关联的记录被添加的次数是否超过了3次 */
            EntityCollection hasRecord = this._Dal.GetEntitiesByField(strFileName, objFileValue, connect);
            if (hasRecord.Count > 3) throw new ValueDuplicatedException("添加失败，这张卡的记录已经被添了4次，每车木料最多只允许4个人取样");

            /* 判断当前红卡关联的记录是否已经被当前操作员添加过 */
            SimpleProperty propertyOfFilter = new SimpleProperty("WoodID", typeof(int));
            Entity entityOfFilter = new Entity(new PropertyCollection() { propertyOfFilter });
            entityOfFilter.SetValue("WoodID", objWoodID);
            entityOfFilter.Add(new SimpleProperty("Operator", typeof(int)), objOperator);

            SimpleProperty propertyOfConnector = new SimpleProperty("WoodID", typeof(string));
            Entity entityOfConnector = new Entity(new PropertyCollection() { propertyOfConnector });
            entityOfConnector.SetValue("WoodID", "=");
            entityOfConnector.Add(new SimpleProperty("Operator", typeof(string)), "=");

            EntityCollection hasAdd = this._Dal.GetEntitiesByFilter(entityOfFilter, entityOfConnector);
            if (hasAdd.Count != 0) throw new ValueDuplicatedException("添加失败，这张卡的记录之前已经被您添加过，不允许再次添加");
        }

        public Entity SaveEntityByOperator(Entity entity, string redCardNumber)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            string msg = "";

            try
            {
                string weightime = entity.GetValue("WeighTime").TryString();
                string Key = entity.GetValue("Key").TryString();

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                int affectedRows;
                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveEntityInsert(entity.GetValue("WoodID"), entity.GetValue("Operator"));
                    if (this._Dal.IsExistsFactoryKey(weightime, Key) > 0)
                    {
                        throw new ValueDuplicatedException("添加失败，密码出现重复。");
                    }
                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("SampleTime", typeof(DateTime)), strCrateDate);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);

                    RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);
                    affectedRows = this._Dal.InsertEntity(entity);
                    affectedRows = cardStateBll.UpdateState(redCardNumber, (int)CardType.Red, (int)CardState.Sample);

                    msg = "添加记录成功";
                }
                // 修改
                else
                {
                    //CheckForSaveEntityUpdate(entity.GetValue("WoodID"));

                    string OldKey = entity.GetValue("OldKey").TryString();
                    if (Key != OldKey)
                    {
                        if (this._Dal.IsExistsFactoryKey(weightime, Key) > 0)
                        {
                            throw new ValueDuplicatedException("修改失败，密码出现重复。");
                        }
                    }
                    affectedRows = this._Dal.UpdateEntityByUniqueWithOperator(entity);

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

        //================================装箱操作==============================================================
        /// <summary>
        /// 修改前的验证(装箱)
        /// </summary>
        /// <param name="objWoodID">关联的木材编号 (来自于Wood表）</param>
        public void CheckForSaveBoxEntityUpdate(object objWoodID)
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
        /// 添加前的验证(装箱)
        /// </summary>
        /// <param name="objWoodID">关联的木材编号 (来自于Wood表）</param>
        void CheckForSaveBoxEntityInsert(object objWoodID)
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

        /// <summary>
        /// 装箱操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity SaveBoxEntity(Entity entity)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveBoxEntityInsert(entity.GetValue("WoodID"));
                    Entity entityBox = new Entity(new PropertyCollection() { });
                    entityBox.Add(new SimpleProperty("Unique", typeof(int)), unique.GetValueByName("WoodPackBox"));
                    entityBox.Add(new SimpleProperty("WoodID", typeof(int)), entity.GetValue("WoodID").TryInt32());
                    entityBox.Add(new SimpleProperty("Box", typeof(int)), entity.GetValue("Box").TryInt32());
                    entityBox.Add(new SimpleProperty("PackTime", typeof(DateTime)), strCrateDate);
                    entityBox.Add(new SimpleProperty("Operator", typeof(int)), entity.GetValue("Operator").TryInt32());
                    entityBox.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entityBox.Add(new SimpleProperty("Version", typeof(int)), 1);
                    entityBox.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");
                    this._Dal.InsertBoxEntity(entityBox);

                    msg = "添加记录成功";
                }
                // 修改
                //else
                //{
                //    CheckForSaveBoxEntityUpdate(entity.GetValue("WoodID"));

                //    this._Dal.UpdateEntityByUniqueWithOperator(entity);

                //    msg = "修改记录成功";
                //}

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
