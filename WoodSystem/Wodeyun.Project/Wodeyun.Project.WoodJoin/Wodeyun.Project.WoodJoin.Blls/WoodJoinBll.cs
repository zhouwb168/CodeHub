using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodJoin.Dals;
using Wodeyun.Project.WoodJoin.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;
using System.Web.Configuration;

namespace Wodeyun.Project.WoodJoin.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodJoinBll : CommonBll, IWoodJoinInterface
    {
        private WoodJoinDal _Dal = new WoodJoinDal();

        public WoodJoinBll()
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

                if (aft > 0)
                {
                    SyncVolumeDataForERP(entity, "CUT");
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
        public EntityCollection GetDataOfJoinByDateAndStartAndLength(string date, int start, int length, string license)
        {
            return this._Dal.GetDataOfJoinByDateAndStartAndLength(date, start, length, license.Trim().ToUpper());
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

                if (aft > 0)
                {
                    SyncVolumeDataForERP(entity, "JOIN");
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
        /// 分页查询可对接的短信报备数据
        /// </summary>
        /// <param name="license">车牌号</param>
        /// <param name="date">过磅时间</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfGsmByDateAndStartAndLength(string license, string date, int start, int length)
        {
            return this._Dal.GetDataOfGsmByDateAndStartAndLength(license.Trim().ToUpper(), date, start, length);
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
