using System;

using System.ServiceModel.Activation;

using Wodeyun.Bf.Base.Blls;
using Wodeyun.Project.WoodLaboratoryConfirme.Dals;
using Wodeyun.Project.WoodLaboratoryConfirme.Interfaces;
using Wodeyun.Gf.System.Exceptions;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Bf.Base.Enums;

namespace Wodeyun.Project.WoodLaboratoryConfirme.Blls
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WoodLaboratoryConfirmeBll : CommonBll, IWoodLaboratoryConfirmeInterface
    {
        private WoodLaboratoryConfirmeDal _Dal = new WoodLaboratoryConfirmeDal();

        public WoodLaboratoryConfirmeBll()
        {
            this.Dal = this._Dal;
        }

        /// <summary>
        /// 批量审核通过
        /// </summary>
        /// <param name="entities">记录号集合</param>
        /// <param name="operatorID">审核人身份识别</param>
        /// <returns></returns>
        public Entity BatchConfirme(object[] entities, int operatorID)
        {
            int k = 0;
            try
            {
                this._Dal.BeginTransaction();

                for (int i = 0; i < entities.Length; i++) k = this._Dal.Confirme(Helper.Deserialize(entities[i].ToString()).GetValue("Unique").ToInt32(), operatorID);

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "批量审核成功！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 批量反审核
        /// </summary>
        /// <param name="entities">记录号集合</param>
        /// <param name="operatorID">反审核人身份识别</param>
        /// <returns></returns>
        public Entity BatchBackConfirme(object[] entities, int operatorID)
        {
            int k = 0;
            try
            {
                string log = "{\"BackConfirmeDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"BackConfirmePeople\":\"" + operatorID + "\"}";
                this._Dal.BeginTransaction();

                for (int i = 0; i < entities.Length; i++) k = this._Dal.BackConfirme(Helper.Deserialize(entities[i].ToString()).GetValue("Unique").ToInt32(), operatorID, log);

                this._Dal.CommitTransaction();

                return Helper.GetEntity(true, "批量反审核成功！");
            }
            catch (Exception exception)
            {
                this._Dal.RollbackTransaction();

                return Helper.GetEntity(false, exception.Message);
            }
        }

        public Entity SaveEntityByOperator(Entity entity)
        {
            string msg = "";

            try
            {
                if (this._Dal.GetEntityByUniqueThatNotConfirme(entity.GetValue("Unique").ToInt32()).GetValue("Unique") == null) return Helper.GetEntity(false, "修改失败，已被审核通过的记录不允许再修改");

                //entity.SetValue("RebateWater", entity.GetValue("Water")); // 目前没有水份打折，所以折后水份等于原始水份

                /* 对于空值的字段，重新赋值，防止字符串转换为浮点数出错 */
                if (entity.GetValue("Bad").ToString() == "") entity.SetValue("Bad", null);
                if (entity.GetValue("Greater").ToString() == "") entity.SetValue("Greater", null);
                if (entity.GetValue("Less").ToString() == "") entity.SetValue("Less", null);
                entity.Add(new SimpleProperty("Log", typeof(string)), "{\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"People\":\"" + entity.GetValue("Operator").ToString() + "\"}");
                //自动计算折后数据
                CalculateRebate(entity);

                this._Dal.UpdateEntityByUniqueWithOperator(entity);

                msg = "修改成功";

                return Helper.GetEntity(true, msg, entity.GetValue("Unique").ToString());
            }
            catch (Exception exception)
            {
                return Helper.GetEntity(false, exception.Message);
            }
        }

        /// <summary>
        /// 分页查询获取还未确认通过的报告记录
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="key">料厂密码</param>
        /// <param name="number">检验号</param>
        /// <param name="confirme">已审核标识（-1 - 全部， 0 - 否， 1 - 是）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesBySearchWithPaging(string date, int start, int length, string key, string number, int confirme)
        {
            return this._Dal.GetEntitiesBySearchWithPaging(date, start, length, key.Trim(), number.Trim(), confirme);
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
