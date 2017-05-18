using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodLaboratory.Dals;
using Wodeyun.Project.WoodLaboratory.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.WoodLaboratory.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodLaboratoryBll : CommonBll, IWoodLaboratoryInterface
    {
        private WoodLaboratoryDal _Dal = new WoodLaboratoryDal();

        public WoodLaboratoryBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 根据木材记录编号，获取化验报告
        /// </summary>
        /// <param name="woodID">木材记录编号</param>
        /// <returns></returns>
        public Entity GetLaboratoryDataByWoodID(int woodID)
        {
            return this._Dal.GetLaboratoryDataByWoodID(woodID);
        }

        /// <summary>
        /// 根据木材记录编号，从地磅系统和短信报备系统中获取用于化验室填写报告时要参考的相关信息
        /// </summary>
        /// <param name="woodID">木材记录编号</param>
        /// <returns></returns>
        public Entity GetGsmInfoByWoodID(int woodID)
        {
            return this._Dal.GetGsmInfoByWoodID(woodID);
        }

        /// <summary>
        /// 根据木材记录编号，从木材收购系统中获取用于化验室填写报告时要参考的相关信息
        /// </summary>
        /// <param name="woodID">木材记录编号</param>
        /// <returns></returns>
        public Entity GetWoodInfoByWoodID(int woodID)
        {
            return this._Dal.GetWoodInfoByWoodID(woodID);
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
            string table = "WoodUnPackBox";

            /*  判断当前关联的记录在样品拆箱表里是否已被删除 */
            EntityCollection recordOfWaitCheck = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (recordOfWaitCheck.Count == 0) throw new ValueDuplicatedException("添加失败，该条样品的拆箱记录已经被拆箱者删除");

            /*  判断当前关联的记录是否已经被添加 */
            table = "WoodLaboratory";
            EntityCollection hasRecord = this._Dal.SelectRecordComeFromDataOfNextStepOperate(strFileName, objFileValue, connect, table);
            if (hasRecord.Count > 0) throw new ValueDuplicatedException("添加失败，该样品的化验报告已经被添加过，您可以选择修改");
        }

        public Entity SaveEntityByOperator(Entity entity)
        {
            string strCrateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string msg = "";

            try
            {
                //entity.SetValue("RebateWater", entity.GetValue("Water")); // 目前没有水份打折，所以折后水份等于原始水份

                /* 对于空值的字段，重新赋值，防止字符串转换为浮点数出错 */
                if (entity.GetValue("Bad").ToString() == "") entity.SetValue("Bad", null);
                if (entity.GetValue("Greater").ToString() == "") entity.SetValue("Greater", null);
                if (entity.GetValue("Less").ToString() == "") entity.SetValue("Less", null);
                //自动计算折后数据
                CalculateRebate(entity);

                this._Dal.BeginTransaction();

                UniqueDal unique = new UniqueDal(this._Dal.SqlTransaction);

                /* 新增 */
                if (entity.GetValue("Unique").TryInt32() == 0)
                {
                    CheckForSaveEntityInsert(entity.GetValue("WoodID"));

                    entity.SetValue("Unique", unique.GetValueByName(this._Dal.Table));
                    entity.Add(new SimpleProperty("CheckTime", typeof(DateTime)), strCrateDate);
                    entity.Add(new SimpleProperty("State", typeof(int)), StateEnum.Default);
                    entity.Add(new SimpleProperty("Version", typeof(int)), 1);
                    entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + strCrateDate + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");

                    this._Dal.InsertEntity(entity);

                    msg = "添加记录成功";
                }
                /* 修改 */
                else
                {
                    msg = "不允许在这里修改，需要在化验报告审核页面才能修改";

                    return Helper.GetEntity(false, msg, entity.GetValue("Unique").ToString());
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
        /// 分页查询获取化验报告记录
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="key">料厂密码</param>
        /// <param name="number">检验号</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesBySearchWithPaging(string date, int start, int length, string key, string number)
        {
            return this._Dal.GetEntitiesBySearchWithPaging(date, start, length, key.Trim(), number.Trim());
        }

        #region 计算折后数据,并赋值 zwb add 20141115
        /// <summary>
        /// 计算折后数据,并赋值
        /// </summary>
        /// <param name="entity"></param>
        public void CalculateRebate(Entity entity)
        {
            string remark = entity.GetValue("RemarkOfWood").TryString();
            string TreeOfWood = entity.GetValue("TreeOfWood").TryString();
            decimal RebateWater, RebateSkin, RebateScrap, RebateGreater;
            decimal Water = entity.GetValue("Water").TryDecimal();    //原始水份
            decimal Skin = entity.GetValue("Skin").TryDecimal();      //原始树皮
            decimal Scrap = entity.GetValue("Scrap").TryDecimal();    //原始碎屑
            decimal Greater = entity.GetValue("Greater").TryDecimal();//原始过大木片
            decimal jweight = entity.GetValue("jweight").TryDecimal();//净重(湿吨)
            //原始水分＝折后水分
            RebateWater = entity.GetValue("Water").TryDecimal();
            entity.SetValue("RebateWater", RebateWater);
            //原始过大木片-5=折后过大木片
            RebateGreater = Greater > 5 ? Greater - 5 : Greater;
            entity.SetValue("RebateGreater", RebateGreater);

            //碎单板/松片折后树皮碎料=实际树皮碎料 
            if (TreeOfWood.Contains("碎单板")
                || TreeOfWood.Contains("松片")
                || TreeOfWood.Contains("松木"))
            {
                entity.SetValue("RebateSkin", Skin);
                entity.SetValue("RebateScrap", Scrap);
            }
            else
            {
                if (remark != "" && remark.Contains("扣")
                    && (remark.Contains("树皮") || remark.Contains("碎料")))
                {
                    //料场验收有扣树皮碎料
                    if (Skin + Scrap <= 20)
                    {
                        RebateSkin = Skin;
                        RebateScrap = Scrap;
                    }
                    else
                    {
                        //折后按20%计算。折后的树皮和碎料之合为20%即可。（树皮比重80% 碎料20%）
                        RebateSkin = 20 * 0.8M;
                        RebateScrap = 20 * 0.2M;
                    }
                }
                else
                {
                    //料场验收没有扣树皮碎料
                    if (Skin + Scrap <= 20)
                    {
                        RebateSkin = Skin;
                        RebateScrap = Scrap;
                    }
                    else if (Skin + Scrap > 20 && Skin + Scrap <= 25)
                    {
                        //折后按20%计算。折后的树皮和碎料之合为20%即可。（树皮比重80% 碎料20%）
                        RebateSkin = 20 * 0.8M;
                        RebateScrap = 20 * 0.2M;
                    }
                    else
                    {
                        decimal count = (((Skin + Scrap) - 25) * 0.5M) + 20;
                        RebateSkin = count * 0.8M;
                        RebateScrap = count * 0.2M;
                    }
                }
                entity.SetValue("RebateSkin", RebateSkin);
                entity.SetValue("RebateScrap", RebateScrap);
            }

            //计算树皮碎料扣方
            if (Skin + Scrap <= 25)
            {
                entity.SetValue("DeductVolume", 0);
            }
            else
            {
                if (remark != "" && remark.Contains("扣") && (remark.Contains("方"))
                    && (remark.Contains("树皮") || remark.Contains("碎料")))
                {
                    entity.SetValue("DeductVolume", 0);
                }
                else
                {
                    decimal deductvolume = (((Skin + Scrap) - 25) * jweight) * 3 * 0.01M;
                    entity.SetValue("DeductVolume", deductvolume);
                }
            }
        }
        #endregion
    }
}
