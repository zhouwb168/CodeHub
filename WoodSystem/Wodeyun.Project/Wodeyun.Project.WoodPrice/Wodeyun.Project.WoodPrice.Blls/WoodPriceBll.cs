using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using Wodeyun.Bf.Base.Blls;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Project.WoodPrice.Dals;
using Wodeyun.Project.WoodPrice.Interfaces;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Dals;
using System.Collections;

namespace Wodeyun.Project.WoodPrice.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodPriceBll : CommonBll, IWoodPriceInterface
    {
        private WoodPriceDal _Dal = new WoodPriceDal();
        public WoodPriceBll()
        {
            this.Dal = this._Dal;
        }

        public Entity SaveWoodPriceEntityInsert(SqlTransaction transaction, Entity entity)
        {
            if (this._Dal.isExists(entity.GetValue("AreaID").TryInt32(), entity.GetValue("TreeID").TryInt32(), entity.GetValue("ExeDate").TryString()))
                throw new ValueDuplicatedException("该记录已存在，请重新输入！");

            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        public Entity SaveWoodPriceEntityUpdate(SqlTransaction transaction, Entity entity)
        {
            if (this._Dal.isExists(entity.GetValue("AreaID").TryInt32(), entity.GetValue("TreeID").TryInt32(), entity.GetValue("ExeDate").TryString(), entity.GetValue("Price").TryDecimal(), entity.GetValue("WetPrice").TryDecimal(), entity.GetValue("CubePrice").TryDecimal()))
                throw new ValueDuplicatedException("该记录已存在，请重新输入！");

            if (entity.GetValue("Remark").ToString() == string.Empty) entity.SetValue("Remark", null);

            return entity;
        }

        /// <summary>
        /// 保存和修改价格体系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity SaveWoodPriceEntity(Entity entity)
        {
            try
            {
                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                // 新增
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    entity = this.SaveWoodPriceEntityInsert(this._Dal.SqlTransaction, entity);

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("State", typeof(int)), 0);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                    entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}");

                    this._Dal.InsertEntity(entity);
                }
                // 修改
                else
                {
                    entity = this.SaveWoodPriceEntityUpdate(this._Dal.SqlTransaction, entity);

                    this._Dal.UpdateWoodPrice(entity);
                }

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "保存记录成功！", entity.GetValue("Unique").ToString());
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 分页查询价格体系
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetWoodPriceData(string startDate, string endDate, int start, int length, string area, string tree)
        {
            return this._Dal.GetWoodPriceData(startDate, endDate, start, length, area, tree);
        }

        /// <summary>
        /// 木材收购价格统计明细表
        /// </summary>
        /// <param name="month"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="area"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public EntityCollection getPriceDataList(string startDate, string endDate, int start, int length, string area, string tree, string IsCreate, string Supplier)
        {
            return this._Dal.getPriceDataList(startDate, endDate, start, length, area, tree, IsCreate, Supplier);
        }

        /// <summary>
        /// 生成结算单
        /// </summary>
        /// <param name="Unique"></param>
        /// <param name="License">车号</param>
        /// <param name="LinkMan">联系人</param>
        /// <param name="GWeight">干吨重量</param>
        /// <param name="GPrice">干吨单价</param>
        /// <param name="Amount">干吨金额</param>
        /// <param name="account">操作人员</param>
        /// <returns></returns>
        public EntityCollection InsertCostStatement(object[] entities)
        {
            string ordernos = string.Empty;
            try
            {
                this._Dal.BeginTransaction();
                int k = 0;
                string groupid = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                for (int i = 0; i < entities.Length; i++)
                {
                    int Unique = Helper.Deserialize(entities[i].ToString()).GetValue("Unique").TryInt32();
                    string License = Helper.Deserialize(entities[i].ToString()).GetValue("License").TryString();
                    string LinkMan = Helper.Deserialize(entities[i].ToString()).GetValue("LinkMan").TryString();
                    decimal GWeight = Helper.Deserialize(entities[i].ToString()).GetValue("GWeight").TryDecimal();
                    decimal GPrice = Helper.Deserialize(entities[i].ToString()).GetValue("GPrice").TryDecimal();
                    decimal Amount = Helper.Deserialize(entities[i].ToString()).GetValue("Amount").TryDecimal();
                    int Account = Helper.Deserialize(entities[i].ToString()).GetValue("Account").TryInt32();
                    decimal JWeight = Helper.Deserialize(entities[i].ToString()).GetValue("JWeight").TryDecimal();
                    string Tree = Helper.Deserialize(entities[i].ToString()).GetValue("Tree").TryString();
                    string Area = Helper.Deserialize(entities[i].ToString()).GetValue("Area").TryString();
                    string Bang_Time = Helper.Deserialize(entities[i].ToString()).GetValue("Bang_Time").TryString();
                    decimal JVolume = Helper.Deserialize(entities[i].ToString()).GetValue("JVolume").TryDecimal();
                    decimal FullVolume = Helper.Deserialize(entities[i].ToString()).GetValue("FullVolume").TryDecimal();
                    decimal CubePrice = Helper.Deserialize(entities[i].ToString()).GetValue("CubePrice").TryDecimal();
                    string orderno = this._Dal.InsertCostStatement(Unique, License, LinkMan, JWeight, Tree, Area, GWeight, GPrice, Amount, Account, Bang_Time, JVolume, FullVolume, CubePrice, groupid);
                    if (orderno != "")
                    {
                        if (k == 0)
                            ordernos = "'" + orderno + "'";
                        else
                            ordernos += ",'" + orderno + "'";
                        k++;
                    }
                }

                EntityCollection collection = this._Dal.getCurrentCostDataList(ordernos);

                this._Dal.CommitTransaction();

                return collection;//Helper.GetEntity(true, "生成结算单成功！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();
                return null;//Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="license"></param>
        /// <param name="printed"></param>
        /// <returns></returns>
        public EntityCollection getCostDataList(string startDate, string endDate, string bangstartDate, string bangendDate, int start, int length, string license, int printed, string groupid, string Supplier)
        {
            return this._Dal.getCostDataList(startDate, endDate, bangstartDate, bangendDate, start, length, license, printed, groupid, Supplier);
        }


        /// <summary>
        /// 获取木片结算单打印信息
        /// </summary>
        /// <param name="ordernos">单号集合</param>
        /// <returns>结果集</returns>
        public Entity GetEntitysForCostPrint(Entity entity)
        {
            string ordernos = entity.GetValue("OrderNo").TryString();
            int Account = entity.GetValue("Account").TryInt32();
            int Ismegre = entity.GetValue("Ismegre").TryInt32();
            Entity listCostObj = null;
            try
            {
                this._Dal.BeginTransaction();

                int acts = this._Dal.UpdatePrintState(ordernos, Account, Ismegre);
                ArrayList list = this._Dal.GetEntitysForCostPrint(ordernos) as ArrayList;
                SimpleProperty Weights = new SimpleProperty("Weights", typeof(EntityCollection));
                SimpleProperty Volumes = new SimpleProperty("Volumes", typeof(EntityCollection));
                SimpleProperty CarCounts = new SimpleProperty("CarCounts", typeof(EntityCollection));
                PropertyCollection propertyCollectiongControl = new PropertyCollection() { 
                Weights,Volumes,CarCounts
                };
                listCostObj = new Entity(propertyCollectiongControl, new ArrayList() { list[0], list[1], list[2] });

                this._Dal.CommitTransaction();
            }
            catch (System.Exception exception)
            {
                this._Dal.RollbackTransaction();

                throw new System.Exception("出错", exception);
            }

            return listCostObj;
        }

        /// <summary>
        /// 修改区域
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string OnChangeArea(Entity entity)
        {
            string strRes = string.Empty;
            string OrderNo = entity.GetValue("OrderNo").TryString();
            string Area = entity.GetValue("Area").TryString();
            string Bang_Time = entity.GetValue("Bang_Time").TryString();
            string Tree = entity.GetValue("Tree").TryString();
            int Account = entity.GetValue("Account").TryInt32();
            int Unique = entity.GetValue("Unique").TryInt32();
            string Supplier = entity.GetValue("Supplier").TryString();
            try
            {
                this._Dal.BeginTransaction();
                //获取木片单价
                Entity entityprice = this._Dal.getWoodPrice(Area, Bang_Time, Tree);
                decimal price = entityprice.GetValue("Price").TryDecimal();
                decimal wetPrice = entityprice.GetValue("WetPrice").TryDecimal();
                decimal cubePrice = entityprice.GetValue("CubePrice").TryDecimal();//体质单价
                int res = this._Dal.UpdateCostOrder(OrderNo, Area, Account, price, cubePrice);
                int res1 = this._Dal.UpdateSupplier(Unique, Account, Supplier);
                this._Dal.CommitTransaction();
                strRes = res > 0 ? "SUCCESS" : "FAIL";
            }
            catch (System.Exception ex)
            {
                strRes = "EXCEPTION";
                this._Dal.RollbackTransaction();
            }
            return strRes;
        }

        /// <summary>
        /// 批量审核结算单状态
        /// </summary>
        /// <param name="ordernos">单号集合</param>
        /// <returns>结果集</returns>
        public string BatchCheckCostState(Entity entity)
        {
            string strRes = string.Empty;
            string ordernos = entity.GetValue("OrderNo").TryString();
            int Account = entity.GetValue("Account").TryInt32();
            int CheckType = entity.GetValue("CheckType").TryInt32();
            try
            {
                this._Dal.BeginTransaction();

                int acts = CheckType == 0 ? this._Dal.UpdateIsConfirmedState(ordernos, Account) : this._Dal.BackCheckCostState(ordernos, Account);
                this._Dal.CommitTransaction();

                strRes = acts > 0 ? "SUCCESS" : "FAIL";
            }
            catch (System.Exception ex)
            {
                strRes = "EXCEPTION";
                this._Dal.RollbackTransaction();
            }
            return strRes;
        }

        /// <summary>
        /// 批量删除结算单
        /// </summary>
        /// <param name="ordernos">单号集合</param>
        /// <returns>结果集</returns>
        public string BatchDeleteCostState(Entity entity)
        {
            string strRes = string.Empty;
            string ordernos = entity.GetValue("OrderNo").TryString();
            int Account = entity.GetValue("Account").TryInt32();
            try
            {
                this._Dal.BeginTransaction();

                int acts = this._Dal.DeleteCostList(ordernos, Account);
                this._Dal.CommitTransaction();

                strRes = acts > 0 ? "SUCCESS" : "FAIL";
            }
            catch (System.Exception ex)
            {
                strRes = "EXCEPTION";
                this._Dal.RollbackTransaction();
            }
            return strRes;
        }

        /// <summary>
        /// 获取用户成本结算的审核权限
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public int ReviewPermissionsbyAccount(Entity entity)
        {
            int Account = entity.GetValue("Account").TryInt32();
            int role = entity.GetValue("Role").TryInt32();
            return this._Dal.ReviewPermissionsbyAccount(Account, role);
        }

        /// <summary>
        /// 删除价格体系
        /// </summary>
        /// <param name="unique"></param>
        /// <returns></returns>
        public Entity DeleteWoodPriceByUnique(Entity entity)
        {
            try
            {
                int Unique = entity.GetValue("Unique").TryInt32();
                int Version = entity.GetValue("Version1").TryInt32();
                this._Dal.BeginTransaction();
                int res = this._Dal.DeleteWoodPriceByUnique(Unique, Version);
                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, res > 0 ? "删除记录成功！" : "删除记录失败！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();
                return Helper.GetEntity(false, exception.Message);
            }
        }


        /// <summary>
        /// 批量审核/反审价格体系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string BatchCheckWoodPrice(Entity entity)
        {
            string strRes = string.Empty;
            string uniques = entity.GetValue("Unique").TryString();
            string versions = entity.GetValue("Version").TryString();
            int Account = entity.GetValue("Account").TryInt32();
            int CheckType = entity.GetValue("CheckType").TryInt32();
            int intRes = 0;
            try
            {
                this._Dal.BeginTransaction();
                if (uniques != "")
                {
                    string[] unique = uniques.Split(new char[] { ',' });
                    string[] version = versions.Split(new char[] { ',' });
                    for (int i = 0; i < unique.Length; i++)
                    {
                        intRes += this._Dal.UpdateWoodPriceState(unique[i].TryInt32(), version[i].TryInt32(), Account, CheckType);
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
