using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.Check.Dals;
using Wodeyun.Project.Check.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.Check.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CheckBll : CommonBll, ICheckInterface
    {
        private CheckDal _Dal = new CheckDal();

         public CheckBll()
        {
            this.Dal = this._Dal;
        }

         public Entity SaveEntityByOperator(Entity entity)
         {
             string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
             string msg = "";
             try
             {

                 CheckForSaveEntityInsert(entity.GetValue("BarrierID"));

                 object greenCardNumber = entity.GetValue("CardNumber");
                 /* 这里获取数据，为了在验收电子卡的同时，模拟新发一张工厂入门电子卡记录 */
                 SimpleProperty property = new SimpleProperty("CardNumber", typeof(string));
                 Entity entityOfWood = new Entity(new PropertyCollection() { property });
                 entityOfWood.SetValue(property, greenCardNumber);
                 entityOfWood.Add(new SimpleProperty("BarrierID", typeof(int)), entity.GetValue("BarrierID"));
                 entityOfWood.Add(new SimpleProperty("ArriveDate", typeof(DateTime)), strCrateDate);
                 entityOfWood.Add(new SimpleProperty("Operator", typeof(int)), entity.GetValue("Operator"));
                 entityOfWood.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                 entityOfWood.Add(new SimpleProperty("Version", typeof(int)), 1);
                 entityOfWood.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                 this._Dal.BeginTransaction();

                 UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                 RfidCardBll cardStateBll = new RfidCardBll(this._Dal.SqlTransaction);

                 int newUnique = unique.GetValueByName("Wood"); // 注意这里，获取木材表的唯一字段
                 if (newUnique <= 0)
                 {
                     throw new Exception("验收失败，网络原因，请重试");
                 }
                 entityOfWood.Add(new SimpleProperty("Unique", typeof(int)), newUnique);

                 /* 新增 */
                 newUnique = unique.GetValueByName(this._Dal.Table);
                 if (newUnique <= 0)
                 {
                     throw new Exception("验收失败，网络原因，请重试");
                 }
                 entity.SetValue("Unique", newUnique);

                 entity.Add(new SimpleProperty("CheckDate", typeof(DateTime)), strCrateDate);
                 entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                 entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                 entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\"}");

                 int affectedRows;
                 affectedRows = this._Dal.InsertEntity(entity); // 添加记录到电子卡验收Check
                 if (affectedRows > 0) affectedRows = this._Dal.InsertBaseDataByOperator(entityOfWood); // 添加记录到木材表Wood
                 else
                 {
                     throw new Exception("验收失败，网络原因，请重试");
                 }
                 if (affectedRows > 0) affectedRows = cardStateBll.UpdateState(greenCardNumber.ToString(), (int)CardType.Green, (int)CardState.Door, entityOfWood.GetValue("Unique").ToInt32());
                 else
                 {
                     throw new Exception("验收失败，网络原因，请重试");
                 }
                 if (affectedRows > 0) msg = "验收成功";
                 else
                 {
                     throw new Exception("验收失败，网络原因，请重试");
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
         /// <param name="barrierID">关联的关卡编号</param>
         public void CheckForSaveEntityInsert(object barrierID)
         {
             string strFileName = "Unique";
             object objFileValue = barrierID;
             string connect = "=";
             string table = "Barrier";

             /*  判断当前绿卡号关联的记录是否已被删除 */
             EntityCollection cardOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
             if (cardOfWaitCheck.Count == 0) throw new ValueDuplicatedException("验收失败，该绿卡在移动服务站发卡处的记录已经被删除");

             /* 判断当前绿卡号是否已被验收过 */
             strFileName = "BarrierID";
             objFileValue = barrierID;
             table = "Check";
             EntityCollection cardOfHasCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
             if (cardOfHasCheck.Count != 0) throw new ValueDuplicatedException("验收失败，该绿卡已经被验收过，不用再次验收");
         }

         public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
         {
             return this._Dal.GetEntitiesByStartAndLengthWithOperator(start, length, operatorID);
         }
    }
}
