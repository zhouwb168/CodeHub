using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.GsmJoin.Dals;
using Wodeyun.Project.GsmJoin.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.GsmJoin.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class GsmJoinBll : CommonBll, IGsmJoinInterface
    {
        private GsmJoinDal _Dal = new GsmJoinDal();

        public GsmJoinBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 断开对接
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity CutOffJoin(Entity entity)
        {
            string msg = "";

            try
            {
                string log = "{\"CutOffDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"CutOffPeople\":\"" + entity.GetValue("Operator").ToString() + "\"}";

                this._Dal.BeginTransaction();

                this._Dal.CutOffJoin(entity.GetValue("Unique").ToInt32(), log);
                int aft = this._Dal.UpdateJoin(entity.GetValue("BangID"), (object)0);

                msg = "断开成功";

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
        /// 修改记录的过滤状态
        /// </summary>
        /// <param name="bangid">地磅记录ID</param>
        /// <param name="filter">是否过滤标识（0 - 否，1 - 是）</param>
        /// <param name="operatorID">操作员的身份识别</param>
        /// <returns></returns>
        public Entity UpdateFilter(int bangid, int filter)
        {
            string msg = "";

            try
            {
                int aft = this._Dal.UpdateFilter(bangid, filter);
                if (aft > 0) msg = filter == 1 ? "过滤成功" : "恢复成功";
                else msg = filter == 1 ? "过滤失败" : "恢复失败";

                return Helper.GetEntity(true, msg);
            }
            catch (Exception exception)
            {
                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 分页查询已经成功对接的数据
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="license">要查询的车牌号（不包含省份）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfJoinByDateAndStartAndLength(string date, string enddate, int start, int length, string license, string area)
        {
            return this._Dal.GetDataOfJoinByDateAndStartAndLength(date, enddate, start, length, license.Trim().ToUpper(), area);
        }

        /// <summary>
        /// 分页查询已经成功对接的报备数据
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="license">要查询的车牌号（不包含省份）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfJoinGsmByDateAndStartAndLength(string date, int start, int length, string license)
        {
            return this._Dal.GetDataOfJoinGsmByDateAndStartAndLength(date, start, length, license.Trim().ToUpper());
        }

        /// <summary>
        /// 添加前的验证
        /// </summary>
        /// <param name="bangID">关联的地磅货重编号（来自于WoodBang表）</param>
        /// <param name="gsmID">关联的短信报备编号（来自于GsmItem表）</param>
        /// <param name="woodID">关联的木材编号 (来自于Wood表）</param>
        void CheckForSaveEntityInsert(object bangID, object gsmID, object woodID)
        {
            /* 判断当前各表数据是否已经对接过 */
            SimpleProperty propertyOfFilter = new SimpleProperty("BangID", typeof(int));
            Entity entityOfFilter = new Entity(new PropertyCollection() { propertyOfFilter });
            entityOfFilter.SetValue("BangID", bangID);
            entityOfFilter.Add(new SimpleProperty("GsmID", typeof(int)), gsmID);
            entityOfFilter.Add(new SimpleProperty("WoodID", typeof(int)), woodID);

            SimpleProperty propertyOfConnector = new SimpleProperty("BangID", typeof(string));
            Entity entityOfConnector = new Entity(new PropertyCollection() { propertyOfConnector });
            entityOfConnector.SetValue("BangID", "=");
            entityOfConnector.Add(new SimpleProperty("GsmID", typeof(string)), "=");
            entityOfConnector.Add(new SimpleProperty("WoodID", typeof(string)), "=");

            EntityCollection hasAdd = this._Dal.GetEntitiesByFilter(entityOfFilter, entityOfConnector);
            if (hasAdd.Count != 0) throw new ValueDuplicatedException("操作失败，该地磅数据记录之前已经成功对接过");
        }

        public Entity SaveEntityByOperator(Entity entity)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {
                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                CheckForSaveEntityInsert(entity.GetValue("BangID"), entity.GetValue("GsmID"), entity.GetValue("WoodID"));

                entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                entity.Add(new SimpleProperty("JoinTime", typeof(DateTime)), strCrateDate);
                entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"JoinDate\":\"" + strCrateDate + "\",\"JoinPeople\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                this._Dal.InsertEntity(entity);
                int aft = this._Dal.UpdateJoin(entity.GetValue("BangID"), (object)1);

                msg = "数据对接成功";

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
        /// 分页查询可对接的短信报备数据
        /// </summary>
        /// <param name="license">车牌号</param>
        /// <param name="date">过磅时间</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfGsmByDateAndStartAndLength(string license, string driver, string supplier, string area, string date, int start, int length)
        {
            return this._Dal.GetDataOfGsmByDateAndStartAndLength(license.Trim().ToUpper(), driver, supplier, area, date, start, length);
        }

        /// <summary>
        /// 分页查询可对接的电子卡系统数据
        /// </summary>
        /// <param name="license">车牌号</param>
        /// <param name="date">过磅时间</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfWoodByDateAndStartAndLength(string license, string date, int start, int length)
        {
            return this._Dal.GetDataOfWoodByDateAndStartAndLength(license.Trim().ToUpper(), date, start, length);
        }

        /// <summary>
        /// 分页查询等待对接的地磅货重数据
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="isFiltered">已过滤标识（0 - 否，1 - 是）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfBangByDateAndStartAndLength(string date, int start, int length, int isFiltered)
        {
            return this._Dal.GetDataOfBangByDateAndStartAndLength(date, start, length, isFiltered);
        }

        /// <summary>
        /// 批量对接
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string BatchJoinData(Entity entity)
        {
            string strRes = string.Empty;
            string BangIDs = entity.GetValue("BangIDs").TryString();
            string WoodIDs = entity.GetValue("WoodIDs").TryString();
            int AreaID = entity.GetValue("AreaID").TryInt32();
            int Account = entity.GetValue("Account").TryInt32();
            int intRes = 0;
            try
            {
                this._Dal.BeginTransaction();
                if (BangIDs != "")
                {
                    string[] BangID = BangIDs.Split(new char[] { ',' });
                    string[] WoodID = WoodIDs.Split(new char[] { ',' });
                    for (int i = 0; i < BangID.Length; i++)
                    {
                        string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);
                        CheckForSaveEntityInsert(BangID[i], 0, WoodID[i]);
                        SimpleProperty propertyOfConnector = new SimpleProperty("BangID", typeof(string));
                        Entity entity1 = new Entity(new PropertyCollection() { propertyOfConnector });
                        entity1.SetValue("BangID", BangID[i]);
                        entity1.Add(new SimpleProperty("GsmID", typeof(int)), 0);
                        entity1.Add(new SimpleProperty("WoodID", typeof(int)), WoodID[i].TryInt32());
                        entity1.Add(new SimpleProperty("Unique", typeof(int)), unique.GetValueByName(this._Dal.Table));
                        entity1.Add(new SimpleProperty("JoinTime", typeof(DateTime)), strCrateDate);
                        entity1.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                        entity1.Add(new SimpleProperty("Version", typeof(int)), 1);
                        entity1.Add(new SimpleProperty("Operator", typeof(int)), Account);
                        entity1.Add(new SimpleProperty("IsGsm", typeof(int)), 1);
                        entity1.Add(new SimpleProperty("IsAdd", typeof(int)), 0);
                        entity1.Add(new SimpleProperty("AreaID", typeof(int)), AreaID);
                        entity1.Add(new SimpleProperty("Log", typeof(string)), "{\"JoinDate\":\"" + strCrateDate + "\",\"JoinPeople\":\"" + Account + "\"}");

                        this._Dal.InsertEntity(entity1);
                        intRes += this._Dal.UpdateJoin(BangID[i], (object)1);
                    }
                }

                this._Dal.CommitTransaction();

                strRes = intRes > 0 ? "SUCCESS" : "FAIL";
            }
            catch (System.Exception ex)
            {
                strRes = "EXCEPTION";
                this._Dal.RollbackTransaction();
            }
            return strRes;

        }

    }
}
