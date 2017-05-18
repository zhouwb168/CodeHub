using System.Data.SqlClient;
using System.Collections;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodLaboratoryConfirme.Dals
{
    public class WoodLaboratoryConfirmeDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "WoodLaboratory";
            this.InsertColumns = this.Inserts = "[Unique], [WoodID], [Water], [RebateWater], [Skin], [RebateSkin], [Scrap], [RebateScrap], [Bad], [Greater], [Less], [CheckTime], [Operator], [IsConfirmed], [Confirmor], [ConfirmTime], [State], [Version], [Log], [RebateGreater], [DeductVolume]";
            this.UpdateColumns = this.Updates = "[Water], [RebateWater], [Skin], [RebateSkin], [Scrap], [RebateScrap], [Bad], [Greater], [Less], [Log], [RebateGreater], [DeductVolume]";
            this.Selects = "[Unique], [WoodID], [Water], [RebateWater], [Skin], [RebateSkin], [Scrap], [RebateScrap], [Bad], [Greater], [Less], [CheckTime], [RebateGreater], [DeductVolume]";
            this.Order = "[Unique] desc";
        }

        public WoodLaboratoryConfirmeDal()
        {
            this.Init();
        }

        public WoodLaboratoryConfirmeDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 获取还未审核的验报告
        /// </summary>
        /// <param name="unique">记录编号</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByUniqueThatNotConfirme(int unique)
        {
            string sql = @"select [Unique]
                           from [WoodLaboratory]
                           where [Unique] = " + unique.ToDatabase() + @"
                               and [IsConfirmed] = 0
                               and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [State] <> " + StateEnum.Updated.ToDatabase();

            return this.Execute.GetEntity(sql);
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="unique">记录号</param>
        /// <param name="operatorID">审核人身份识别</param>
        /// <returns>数据库表受影响的行数</returns>
        public int Confirme(int unique, int operatorID)
        {
            string sql = @"update [" + this.Table + @"]
                           set [IsConfirmed] = 1, [Confirmor] = " + operatorID.ToDatabase() + @"
                               , [ConfirmTime] = getdate()
                           where [Unique] = " + unique.ToDatabase() + @"
                               and [IsConfirmed] = 0
                               and [State] = " + StateEnum.Default.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="unique">记录号</param>
        /// <param name="operatorID">审核人身份识别</param>
        /// <returns>数据库表受影响的行数</returns>
        public int BackConfirme(int unique, int operatorID, string log)
        {
            string sql = @"update [" + this.Table + @"]
                           set [IsConfirmed] = 0, [Log] += " + log.ToDatabase() + @"
                           where [Unique] = " + unique.ToDatabase() + @"
                               and [IsConfirmed] = 1
                               and [State] = " + StateEnum.Default.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改数据（适用于已登录状态当前操作者）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntityByUniqueWithOperator(Entity entity)
        {
            string sql = @"update [" + this.Table + @"]
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase() + @";
                           insert into [" + this.Table + @"]
                           select top 1 " + entity.ToUpdate(this.InsertColumns, this.UpdateColumns).Replace("[State]", StateEnum.Default.ToDatabase()).Replace("[Version]", "[Version] + 1") + @"
                           from [" + this.Table + @"]
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [State] = " + StateEnum.Updated.ToDatabase() + @"
                           order by [Version] desc";

            return this.Execute.ExecuteNonQuery(sql);
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

            string sql = @"with [Filtered] as
                           (
                                select [F1].[Unique], [F1].[WoodID], (
                                        select distinct [Key] + '，'
                                        from [Factory] where [WoodID] = [F1].[WoodID] and [State] =0
                                        for xml path('')
                                      ) as [Key], (
                                        select distinct [Sampler] + '，'
                                        from [Factory] where [WoodID] = [F1].[WoodID] and [State] =0
                                        for xml path('')
                                     ) as [Sampler], (
                                        select [Remark] + '＃'
                                        from [Factory] where [WoodID] = [F1].[WoodID] and [State] =0
                                        for xml path('')
                                     ) as [Remark]
                                     , [F1].[License], [F1].[Tree], [F1].[WeighTime]
                                     , [WoodLaboratory].[Unique] as [LaboratoryID], [WoodLaboratory].[Water], [WoodLaboratory].[DeductVolume]
                                     , [WoodLaboratory].[RebateWater], [WoodLaboratory].[Skin], [WoodLaboratory].[RebateSkin]
                                     , [WoodLaboratory].[Scrap], [WoodLaboratory].[RebateScrap], [WoodLaboratory].[Bad]
                                     , [WoodLaboratory].[Greater], [WoodLaboratory].[Less], [WoodLaboratory].[IsConfirmed]
                                     , [WoodUnPackBox].[Number] as [CheckNumber]
                                     , [WoodJoin].[BangID]
                               from [FullPound] as [F1]
                               left outer join [WoodLaboratory] on ([F1].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[State] =0)
                               left outer join [WoodUnPackBox] on ([F1].[WoodID] = [WoodUnPackBox].[WoodID]
                                   and [WoodUnPackBox].[State] =0)
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] =0)
                               where [F1].[State] =0
                                   and [F1].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [F1].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @"
                           ),
                           [Build] as
                           (
                               select distinct [Filtered].[Unique], [Filtered].[WoodID]
                                      , [Co20] = [Filtered].[WoodID], [Col1] = [Filtered].[WoodID], [Col2] = [Filtered].[WoodID]
                                      , [Col3] = [Filtered].[WoodID], [Col4] = [Filtered].[WoodID], [Col5] = [Filtered].[WoodID]
                                      , [Col6] = [Filtered].[WoodID], [Col7] = [Filtered].[WoodID], [Col8] = [Filtered].[WoodID]
                                      , [Col9] = [Filtered].[WoodID], [Co21] = [Filtered].[WoodID], [Co22] = [Filtered].[WoodID]
                                      , [Co23] = [Filtered].[WoodID], [Co24] = [Filtered].[WoodID], [Co25] = [Filtered].[WoodID]
                                      , left([Filtered].[Key], len([Filtered].[Key]) - 1) as [Key]
                                      , left([Filtered].[Sampler], len([Filtered].[Sampler]) - 1) as [Sampler]
                                      , [Filtered].[Remark], [Filtered].[License], [Filtered].[Tree], [Filtered].[WeighTime]
                                      , [Filtered].[LaboratoryID], [Filtered].[Water], [Filtered].[RebateWater], [Filtered].[DeductVolume]
                                      , [Filtered].[Skin], [Filtered].[RebateSkin], [Filtered].[Scrap], [Filtered].[RebateScrap]
                                      , [Filtered].[Bad], [Filtered].[Greater], [Filtered].[Less], [Filtered].[IsConfirmed]
                                      , [Filtered].[CheckNumber]
                                      , [WoodBang].[jWeight], [WoodBang].[firstBangUser]
                                      , [User] = '" + "{\"Name\":\"实际结果\"}'" + @"
                                      , [IsMain] = 1, [IsLast] = 0
                               from [Filtered]
                               left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                               where 1 = 1
                                   " + (number == "" ? "" : string.Format(" and [Filtered].[CheckNumber] = {0}", number)) + @"
                                   " + (confirme == -1 ? "" : string.Format(" and [Filtered].[IsConfirmed] = {0}", confirme)) + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] asc) as [Number], *
                               from [Build]
                               where 1 = 1
                                   " + (key == "" ? "" : string.Format(" and [Key] like '%{0}%'", key)) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                                      , [Factory].[Water], [Factory].[Skin], [Factory].[Scrap], [Factory].[Remark]
                                      , [Factory].[Operator]
                               from [FullPound]
                               left outer join [Factory] on ([FullPound].[WoodID] = [Factory].[WoodID]
                                   and [Factory].[State] =0)
                               where [FullPound].[State] =0
                                   and [FullPound].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @"
                           )
                           select [Filtered].*, [Account].[Description]
                           from [Filtered]
                           left outer join [Account] on ([Filtered].[Operator] = [Account].[Unique]
                                   and [Account].[State] =0)
                           order by [Filtered].[WoodID] asc, [Filtered].[Operator] asc;
                           with [Filtered] as
                           (
                               select [F1].[Unique], (
                                        select distinct [Key] + '，'
                                        from [Factory]
                                        where [WoodID] = [F1].[WoodID]
                                           and [State] =0
                                        for xml path('')
                                      ) as [Key]
                                     , [WoodLaboratory].[IsConfirmed]
                                     , [WoodUnPackBox].[Number] as [CheckNumber]
                               from [FullPound] as [F1]
                               left outer join [WoodLaboratory] on ([F1].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[State] =0)
                               left outer join [WoodUnPackBox] on ([F1].[WoodID] = [WoodUnPackBox].[WoodID]
                                   and [WoodUnPackBox].[State] =0)
                               where [F1].[State] =0
                                   and [F1].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [F1].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @"
                           ),
                           [Build] as
                           (
                               select distinct [Unique] , left([Key], len([Key]) - 1) as [Key]
                               from [Filtered]
                               where 1 = 1
                                   " + (number == "" ? "" : string.Format(" and [CheckNumber] = {0}", number)) + @"
                                   " + (confirme == -1 ? "" : string.Format(" and [IsConfirmed] = {0}", confirme)) + @"
                           ),
                           [Numbered] as
                           (
                               select * from [Build]
                               where 1 = 1
                                   " + (key == "" ? "" : string.Format(" and [Key] like '%{0}%'", key)) + @"
                           )
                           select count(*) as [Total] from [Numbered];";

            IList collections = this.Execute.GetEntityCollections(sql);

            /* 根据数据重新组合结果集 */
            EntityCollection pound = collections[0] as EntityCollection; // 来地磅的数据
            EntityCollection factory = collections[1] as EntityCollection; // 来自料厂的数据

            EntityCollection results = new EntityCollection(pound.PropertyCollection); // 这个结果集是一个新的结果集，模式和地磅的结果集一到致

            /* 各缓存变量 */
            Entity tempPound;
            Entity tempFactoryEntity;

            object tempNumber;
            object tempUnique;
            object tempWoodID;
            object tempKey;
            object tempSampler;
            object tempRemark;
            object tempLicense;
            object tempTree;
            object tempWeighTime;
            object tempLaboratoryID;
            object tempWater;
            object tempDeductVolume;
            object tempRebateWater;
            object tempSkin;
            object tempRebateSkin;
            object tempScrap;
            object tempRebateScrap;
            object tempBad;
            object tempGreater;
            object tempLess;
            object tempIsConfirmed;
            object tempCheckNumber;
            object tempjWeight;
            object tempfirstBangUser;
            DateTime tempDate;

            int factoryItemCoun = factory.Count;

            /* 遍历地磅数据 */
            int laboratoryItemCount = pound.Count;
            for (int i = 0; i < laboratoryItemCount; i++)
            {
                /* 获取公共部分的数据 */
                tempPound = pound[i] as Entity;
                tempNumber = tempPound.GetValue("Number");
                tempUnique = tempPound.GetValue("Unique");
                tempWoodID = tempPound.GetValue("WoodID");
                tempKey = tempPound.GetValue("Key");
                tempSampler = tempPound.GetValue("Sampler");
                tempRemark = tempPound.GetValue("Remark");
                tempLicense = tempPound.GetValue("License");
                tempTree = tempPound.GetValue("Tree");
                tempDate = Convert.ToDateTime(tempPound.GetValue("WeighTime"));
                tempWeighTime = (object)(string.Format("{0}月{1}日", tempDate.Month, tempDate.Day));
                tempLaboratoryID = tempPound.GetValue("LaboratoryID");
                tempWater = tempPound.GetValue("Water");
                tempDeductVolume = tempPound.GetValue("DeductVolume");
                tempRebateWater = tempPound.GetValue("RebateWater");
                tempSkin = tempPound.GetValue("Skin");
                tempRebateSkin = tempPound.GetValue("RebateSkin");
                tempScrap = tempPound.GetValue("Scrap");
                tempRebateScrap = tempPound.GetValue("RebateScrap");
                tempBad = tempPound.GetValue("Bad");
                tempGreater = tempPound.GetValue("Greater");
                tempLess = tempPound.GetValue("Less");
                tempIsConfirmed = tempPound.GetValue("IsConfirmed");
                tempCheckNumber = tempPound.GetValue("CheckNumber");
                tempjWeight = tempPound.GetValue("jWeight");
                tempfirstBangUser = tempPound.GetValue("firstBangUser");

                /* 遍历料厂数据 */
                for (int j = 0; j < factoryItemCoun; j++)
                {
                    tempFactoryEntity = factory[j] as Entity;
                    /* 重新生成新的一行数据，含水率、树皮含量、碎料含量、取样人、备注等字段为料厂的值，其它字段使用地磅的公共数据 */
                    if (tempWoodID.Equals(tempFactoryEntity.GetValue("WoodID")))
                    {
                        /* 两表记录的木材编号相等，则说明是同一车木材，可以组装新记录到结果表 */
                        Entity newEntity = new Entity(tempPound.PropertyCollection); // 注意！这里一定要用地磅的表的模式
                        newEntity.SetValue("Number", tempNumber);
                        newEntity.SetValue("Unique", tempUnique);
                        newEntity.SetValue("WoodID", tempWoodID);
                        newEntity.SetValue("Co20", tempWoodID);
                        newEntity.SetValue("Col1", tempWoodID);
                        newEntity.SetValue("Col2", tempWoodID);
                        newEntity.SetValue("Col3", tempWoodID);
                        newEntity.SetValue("Col4", tempWoodID);
                        newEntity.SetValue("Col5", tempWoodID);
                        newEntity.SetValue("Col6", tempWoodID);
                        newEntity.SetValue("Col7", tempWoodID);
                        newEntity.SetValue("Col8", tempWoodID);
                        newEntity.SetValue("Col9", tempWoodID);
                        newEntity.SetValue("Co21", tempWoodID);
                        newEntity.SetValue("Co22", tempWoodID);
                        newEntity.SetValue("Co23", tempWoodID);
                        newEntity.SetValue("Co24", tempWoodID);
                        newEntity.SetValue("Co25", tempWoodID);
                        newEntity.SetValue("Key", tempKey);
                        newEntity.SetValue("Sampler", tempSampler);
                        newEntity.SetValue("Remark", tempRemark);
                        newEntity.SetValue("License", tempLicense);
                        newEntity.SetValue("Tree", tempTree);
                        newEntity.SetValue("LaboratoryID", tempLaboratoryID);
                        newEntity.SetValue("WeighTime", tempWeighTime);
                        newEntity.SetValue("Water", tempFactoryEntity.GetValue("Water")); // 这里使用料厂的值
                        newEntity.SetValue("RebateWater", null); // 这里使用null值
                        newEntity.SetValue("Skin", tempFactoryEntity.GetValue("Skin")); // 这里使用料厂的值
                        newEntity.SetValue("RebateSkin", null); // 这里使用null值
                        newEntity.SetValue("Scrap", tempFactoryEntity.GetValue("Scrap")); // 这里使用料厂的值
                        newEntity.SetValue("RebateScrap", null); // 这里使用null值
                        newEntity.SetValue("Bad", tempBad);
                        newEntity.SetValue("Greater", tempGreater);
                        newEntity.SetValue("Less", tempLess);
                        newEntity.SetValue("IsConfirmed", tempIsConfirmed);
                        newEntity.SetValue("CheckNumber", tempCheckNumber);
                        newEntity.SetValue("jWeight", tempjWeight);
                        newEntity.SetValue("DeductVolume", null);
                        newEntity.SetValue("firstBangUser", tempfirstBangUser);
                        newEntity.SetValue("User", tempFactoryEntity.GetValue("Description")); // 这里使用料厂的值
                        newEntity.SetValue("IsMain", 0);
                        newEntity.SetValue("IsLast", 0);

                        results.Add(newEntity); // 把新行添加到新表里
                    }
                }

                /* 添加完料厂的一组数据后，最后得重新添加地磅自己的记录 */
                tempPound.SetValue("WeighTime", tempWeighTime);
                results.Add(tempPound);

                /* 最后增加一行，作为调整后（打折）显示 */
                Entity subEntity = new Entity(tempPound.PropertyCollection); // 注意！这里一定要用地磅的表的模式
                subEntity.SetValue("Number", tempNumber);
                subEntity.SetValue("Unique", tempUnique);
                subEntity.SetValue("WoodID", tempWoodID);
                subEntity.SetValue("Co20", tempWoodID);
                subEntity.SetValue("Col1", tempWoodID);
                subEntity.SetValue("Col2", tempWoodID);
                subEntity.SetValue("Col3", tempWoodID);
                subEntity.SetValue("Col4", tempWoodID);
                subEntity.SetValue("Col5", tempWoodID);
                subEntity.SetValue("Col6", tempWoodID);
                subEntity.SetValue("Col7", tempWoodID);
                subEntity.SetValue("Col8", tempWoodID);
                subEntity.SetValue("Col9", tempWoodID);
                subEntity.SetValue("Co21", tempWoodID);
                subEntity.SetValue("Co22", tempWoodID);
                subEntity.SetValue("Co23", tempWoodID);
                subEntity.SetValue("Co24", tempWoodID);
                subEntity.SetValue("Co25", tempWoodID);
                subEntity.SetValue("Key", tempKey);
                subEntity.SetValue("Sampler", tempSampler);
                subEntity.SetValue("Remark", tempRemark);
                subEntity.SetValue("License", tempLicense);
                subEntity.SetValue("Tree", tempTree);
                subEntity.SetValue("WeighTime", tempWeighTime);
                subEntity.SetValue("LaboratoryID", tempLaboratoryID);
                /* 开始构造调整后的值，有打折的，才显示，不打折的和没有值的则不显示 */
                if (tempWater != null && tempRebateWater != null && Convert.ToDecimal(tempWater) != Convert.ToDecimal(tempRebateWater)) subEntity.SetValue("Water", tempRebateWater);
                else subEntity.SetValue("Water", null);
                if (tempSkin != null && tempRebateSkin != null && Convert.ToDecimal(tempSkin) != Convert.ToDecimal(tempRebateSkin)) subEntity.SetValue("Skin", tempRebateSkin);
                else subEntity.SetValue("Skin", null);
                if (tempScrap != null && tempRebateScrap != null && Convert.ToDecimal(tempScrap) != Convert.ToDecimal(tempRebateScrap)) subEntity.SetValue("Scrap", tempRebateScrap);
                else subEntity.SetValue("Scrap", null);
                subEntity.SetValue("Bad", tempBad);
                subEntity.SetValue("Greater", tempGreater);
                subEntity.SetValue("Less", tempLess);
                subEntity.SetValue("IsConfirmed", tempIsConfirmed);
                subEntity.SetValue("CheckNumber", tempCheckNumber);
                subEntity.SetValue("jWeight", tempjWeight);
                subEntity.SetValue("DeductVolume", null);
                subEntity.SetValue("firstBangUser", tempfirstBangUser);
                subEntity.SetValue("User", "{\"Name\":\"调整后\"}"); // 这里使用新值
                subEntity.SetValue("IsMain", 0);
                subEntity.SetValue("IsLast", 1);

                results.Add(subEntity); // 把新行添加到新表里
            }

            results.Total = (collections[2] as EntityCollection)[0].GetValue("Total").ToInt32(); // 记录总数

            return results;
        }
    }
}
