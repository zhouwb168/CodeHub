using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodReport.Dals
{
    public class WoodReportDal : CommonDal
    {
        public WoodReportDal()
        { }

        public WoodReportDal(SqlTransaction transaction)
            : base(transaction)
        { }

        /// <summary>
        /// 标注水份通知单为已打印过状态
        /// </summary>
        /// <param name="arrWoodID">木材记录ID集合</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdatePrintState(string arrWoodID)
        {
            string sql = @"update [FullPound]
                           set [Printed] = 1
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                               " + string.Format(" and [WoodID] in ({0})", arrWoodID);

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取木片检验结果通知单的打印信息
        /// </summary>
        /// <param name="arrWoodID">木材记录ID集合</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitysForReportPrint(string arrWoodID)
        {
            string sql = @"with [Filtered] as
                           (
                               select [F1].[Unique]
                                      , (
                                          select distinct [Sampler] + '，'
                                          from [Factory]
                                          where [WoodID] = [F1].[WoodID] and [State] = " + StateEnum.Default.ToDatabase() + @"
                                          for xml path('')
                                      ) as [Sampler]
                                      , [F1].[License], [F1].[Tree], [F1].[Supplier], [F1].[FullVolume]
                                      , [WoodJoin].[BangID],ep.HandVolume,ep.EmptyVolume,ep.RebateVolume
                                      , [WoodLaboratory].[RebateWater], [WoodLaboratory].[RebateSkin], [WoodLaboratory].[DeductVolume]
                                      , [WoodLaboratory].[RebateScrap], [WoodLaboratory].[Bad], [WoodLaboratory].[Greater]
                                      , [WoodLaboratory].[Less], [WoodLaboratory].[Confirmor]
                                      , [WoodLaboratory].[Skin], [WoodLaboratory].[Scrap]
                               from [FullPound] as [F1]
                               left join EmptyPound ep on (ep.WoodID=[F1].WoodID and ep.[State]=0)
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left outer join [WoodLaboratory] on ([F1].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[IsConfirmed] = 1
                                   and [WoodLaboratory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [F1].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   " + string.Format(" and [F1].[WoodID] in ({0})", arrWoodID) + @"
                           )
                           SELECT [Filtered].[Unique]
                                  , left([Filtered].[Sampler], len([Filtered].[Sampler]) - 1) as [Sampler]
                                  , [Filtered].[License], [Filtered].[Tree], [Filtered].[Supplier], [Filtered].[RebateWater]
                                  , [Filtered].[RebateSkin], [Filtered].[RebateScrap], [Filtered].[Bad], [Filtered].[Greater]
                                  , [Filtered].[Less], [WoodBang].[jWeight], [WoodBang].[Bang_Time], [WoodBang].[carCID]
                                  , [WoodBang].[userXHName], [Account].[Description], [Filtered].[DeductVolume], [Filtered].[FullVolume]
                                  , [Filtered].[Skin], [Filtered].[Scrap]
                                  ,[dbo].[GetGweight]([WoodBang].[jWeight],[Filtered].RebateWater,[Filtered].RebateSkin,[Filtered].RebateScrap,[Filtered].Tree) AS GWeight
                                  ,[dbo].[GetJvolume]([WoodBang].jWeight,[Filtered].FullVolume,[Filtered].HandVolume,[Filtered].EmptyVolume,[Filtered].RebateVolume,[Filtered].RebateSkin,[Filtered].RebateScrap,[Filtered].Tree,[Filtered].DeductVolume) AS JVolume
                            from [Filtered]
                            left outer join [WoodBang] on ([Filtered].[BangID] = [WoodBang].[bangid])
                            left outer join [Account] on ([Filtered].[Confirmor] = [Account].[Unique]
                                 and [Account].[State] = " + StateEnum.Default.ToDatabase() + @")";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = results.Count;

            return results;
        }

        /// <summary>
        /// 木片检验结果通知单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="key">料厂密码</param>
        /// <param name="printed">已打印标识（-1 - 全部， 0 - 否， 1 - 是）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport05BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key, int printed)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(license))
            {
                strWhere += " AND [F1].[License] like '%" + license + "%'";
            }
            if (!string.IsNullOrWhiteSpace(supplier))
            {
                strWhere += " AND [F1].[Supplier] like '%" + supplier + "%'";
            }
            if (printed != -1)
            {
                strWhere += " AND [F1].[Printed] = " + printed + "";
            }
            string sql = @"with [Filtered] as
                           (
                               select [F1].[Unique], [F1].[WoodID]
                                      , (
                                          select distinct [Key] + '，' from [Factory]
                                          where [WoodID] = [F1].[WoodID] and [State] = 0 for xml path('')
                                      ) as [Key], (
                                          select distinct [Sampler] + '，' from [Factory]
                                          where [WoodID] = [F1].[WoodID] and [State] = 0 for xml path('')
                                      ) as [Sampler]
                                      , [F1].[License], [F1].[Tree], [F1].[Driver], [F1].[Supplier], [F1].[WeighTime]
                                      , [F1].[Printed]
                                      , [WoodLaboratory].[Water], [WoodLaboratory].[RebateWater], [WoodLaboratory].[Skin], [WoodLaboratory].[DeductVolume]
                                      , [WoodLaboratory].[RebateSkin], [WoodLaboratory].[Scrap], [WoodLaboratory].[RebateScrap]
                                      , [WoodLaboratory].[Bad], [WoodLaboratory].[Greater], [WoodLaboratory].[RebateGreater], [WoodLaboratory].[Less]
                                      , [WoodJoin].[BangID]
                               from [FullPound] as [F1]
                               inner join [WoodLaboratory] on ([F1].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[State] = 0
                                   and [WoodLaboratory].[IsConfirmed] = 1)
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] = 0) where [F1].[State] = 0
                                     and [F1].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                     and [F1].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + strWhere + @"
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [Filtered].[WoodID]
                                      , left([Filtered].[Key], len([Filtered].[Key]) - 1) as [Key]
                                      , left([Filtered].[Sampler], len([Filtered].[Sampler]) - 1) as [Sampler]
                                      , [Filtered].[License], [Filtered].[Tree], [Filtered].[Driver], [Filtered].[Supplier]
                                      , [Filtered].[WeighTime], [Filtered].[Printed], [Filtered].[Water], [Filtered].[DeductVolume]
                                      , [Filtered].[RebateWater], [Filtered].[Skin], [Filtered].[RebateSkin], [Filtered].[Scrap]
                                      , [Filtered].[RebateScrap], [Filtered].[Bad], [Filtered].[Greater], [Filtered].[RebateGreater], [Filtered].[Less]
                                      , [WoodBang].[jWeight], [WoodBang].[Bang_Time], [WoodBang].[carCID], [WoodBang].[carUser]
                                      , [WoodBang].[firstBangUser], [WoodBang].[breedName], [WoodBang].[userXHName]
                               from [Filtered]
                               left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] asc) as [Number], *
                               from [Build]
                               where 1 = 1 " + (key == "" ? "" : string.Format(" and [Key] like '%{0}%'", key)) + @"
                           )
                           SELECT * INTO #TEMP FROM [Numbered];
                           SELECT TOP " + length.ToDatabase() + @" * from #TEMP where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total] from #TEMP
                           DROP TABLE #TEMP";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 分页查询木片水份检测日分析报表
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport04ByDateAndStartAndLength(string date, int start, int length)
        {
            string sql = @"with [Filtered] as
                           (
                                select [F1].[Unique], [F1].[WoodID], (
                                        select distinct [Key] + '，'
                                        from [Factory]
                                        where [WoodID] = [F1].[WoodID]
                                           and [State] = " + StateEnum.Default.ToDatabase() + @"
                                        for xml path('')
                                      ) as [Key], (
                                        select distinct [Sampler] + '，'
                                        from [Factory]
                                        where [WoodID] = [F1].[WoodID]
                                           and [State] = " + StateEnum.Default.ToDatabase() + @"
                                        for xml path('')
                                     ) as [Sampler], (
                                        select [Remark] + '＃'
                                        from [Factory]
                                        where [WoodID] = [F1].[WoodID]
                                           and [State] = " + StateEnum.Default.ToDatabase() + @"
                                        for xml path('')
                                     ) as [Remark]
                                     , [F1].[License], [F1].[Tree], [F1].[WeighTime]
                                     , [WoodLaboratory].[Water], [WoodLaboratory].[RebateWater], [WoodLaboratory].[Skin]
                                     , [WoodLaboratory].[RebateSkin], [WoodLaboratory].[Scrap], [WoodLaboratory].[RebateScrap]
                                     , [WoodLaboratory].[Bad], [WoodLaboratory].[Greater], [WoodLaboratory].[Less]
                                     , [WoodJoin].[BangID]
                               from [FullPound] as [F1]
                               left outer join [WoodLaboratory] on ([F1].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[IsConfirmed] = 1
                                   and [WoodLaboratory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [F1].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [F1].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [F1].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @"
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [Filtered].[WoodID]
                                      , [Col1] = [Filtered].[WoodID], [Col2] = [Filtered].[WoodID], [Col3] = [Filtered].[WoodID]
                                      , [Col4] = [Filtered].[WoodID], [Col5] = [Filtered].[WoodID], [Col6] = [Filtered].[WoodID]
                                      , [Col7] = [Filtered].[WoodID], [Col8] = [Filtered].[WoodID], [Col9] = [Filtered].[WoodID]
                                      , [Co20] = [Filtered].[WoodID], [Co21] = [Filtered].[WoodID], [Co22] = [Filtered].[WoodID]
                                      , left([Filtered].[Key], len([Filtered].[Key]) - 1) as [Key]
                                      , left([Filtered].[Sampler], len([Filtered].[Sampler]) - 1) as [Sampler]
                                      , [Filtered].[Remark], [Filtered].[License], [Filtered].[Tree], [Filtered].[WeighTime]
                                      , [Filtered].[Water], [Filtered].[RebateWater], [Filtered].[Skin], [Filtered].[RebateSkin]
                                      , [Filtered].[Scrap], [Filtered].[RebateScrap], [Filtered].[Bad], [Filtered].[Greater]
                                      , [Filtered].[Less]
                                      , [WoodBang].[jWeight], [WoodBang].[Bang_Time], [WoodBang].[firstBangUser]
                                      , [User] = '" + "{\"Name\":\"实际结果\"}'" + @"
                                      , [IsMain] = 1, [IsLast] = 0
                               from [Filtered]
                               left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] asc) as [Number], *
                               from [Build]
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
                                   and [Factory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [FullPound].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @"
                           )
                           select [Filtered].*, [Account].[Description]
                           from [Filtered]
                           left outer join [Account] on ([Filtered].[Operator] = [Account].[Unique]
                                   and [Account].[State] = " + StateEnum.Default.ToDatabase() + @")
                           order by [Filtered].[WoodID] asc, [Filtered].[Operator] asc;
                           with [Filtered] as
                           (
                               select [Unique]
                               from [FullPound]
                               where [State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [WeighTime] <= " + date.ToDateEnd().ToDatabase() + @"
                           )
                           select count([Unique]) as [Total]
                           from [Filtered];";

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
            object tempWater;
            object tempRebateWater;
            object tempSkin;
            object tempRebateSkin;
            object tempScrap;
            object tempRebateScrap;
            object tempBad;
            object tempGreater;
            object tempLess;
            object tempjWeight;
            object tempBang_Time;
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
                tempWater = tempPound.GetValue("Water");
                tempRebateWater = tempPound.GetValue("RebateWater");
                tempSkin = tempPound.GetValue("Skin");
                tempRebateSkin = tempPound.GetValue("RebateSkin");
                tempScrap = tempPound.GetValue("Scrap");
                tempRebateScrap = tempPound.GetValue("RebateScrap");
                tempBad = tempPound.GetValue("Bad");
                tempGreater = tempPound.GetValue("Greater");
                tempLess = tempPound.GetValue("Less");
                tempjWeight = tempPound.GetValue("jWeight");
                tempBang_Time = tempPound.GetValue("Bang_Time");
                tempfirstBangUser = tempPound.GetValue("firstBangUser");

                /* 遍历料厂数据 */
                for (int j = 0; j < factoryItemCoun; j++)
                {
                    tempFactoryEntity = factory[j] as Entity;
                    /* 重新生成新的一行数据，含水率、树皮含量、碎料含量、取样人、备注等字段为料厂的值，其它字段使用地磅里的公共数据 */
                    if (tempWoodID.Equals(tempFactoryEntity.GetValue("WoodID")))
                    {
                        /* 两表记录的木材编号相等，则说明是同一车木材，可以组装新记录到结果表 */
                        Entity newEntity = new Entity(tempPound.PropertyCollection); // 注意！这里一定要用地磅的表的模式
                        newEntity.SetValue("Number", tempNumber);
                        newEntity.SetValue("Unique", tempUnique);
                        newEntity.SetValue("WoodID", tempWoodID);
                        newEntity.SetValue("Col1", tempWoodID);
                        newEntity.SetValue("Col2", tempWoodID);
                        newEntity.SetValue("Col3", tempWoodID);
                        newEntity.SetValue("Col4", tempWoodID);
                        newEntity.SetValue("Col5", tempWoodID);
                        newEntity.SetValue("Col6", tempWoodID);
                        newEntity.SetValue("Col7", tempWoodID);
                        newEntity.SetValue("Col8", tempWoodID);
                        newEntity.SetValue("Col9", tempWoodID);
                        newEntity.SetValue("Co20", tempWoodID);
                        newEntity.SetValue("Co21", tempWoodID);
                        newEntity.SetValue("Co22", tempWoodID);
                        newEntity.SetValue("Key", tempKey);
                        newEntity.SetValue("Sampler", tempSampler);
                        newEntity.SetValue("Remark", tempRemark);
                        newEntity.SetValue("License", tempLicense);
                        newEntity.SetValue("Tree", tempTree);
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
                        newEntity.SetValue("jWeight", tempjWeight);
                        newEntity.SetValue("Bang_Time", tempBang_Time);
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
                subEntity.SetValue("Col1", tempWoodID);
                subEntity.SetValue("Col2", tempWoodID);
                subEntity.SetValue("Col3", tempWoodID);
                subEntity.SetValue("Col4", tempWoodID);
                subEntity.SetValue("Col5", tempWoodID);
                subEntity.SetValue("Col6", tempWoodID);
                subEntity.SetValue("Col7", tempWoodID);
                subEntity.SetValue("Col8", tempWoodID);
                subEntity.SetValue("Col9", tempWoodID);
                subEntity.SetValue("Co20", tempWoodID);
                subEntity.SetValue("Co21", tempWoodID);
                subEntity.SetValue("Co22", tempWoodID);
                subEntity.SetValue("Key", tempKey);
                subEntity.SetValue("Sampler", tempSampler);
                subEntity.SetValue("Remark", tempRemark);
                subEntity.SetValue("License", tempLicense);
                subEntity.SetValue("Tree", tempTree);
                subEntity.SetValue("WeighTime", tempWeighTime);
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
                subEntity.SetValue("jWeight", tempjWeight);
                subEntity.SetValue("Bang_Time", tempBang_Time);
                subEntity.SetValue("firstBangUser", tempfirstBangUser);
                subEntity.SetValue("User", "{\"Name\":\"调整后\"}"); // 这里使用新值
                subEntity.SetValue("IsMain", 0);
                subEntity.SetValue("IsLast", 1);

                results.Add(subEntity); // 把新行添加到新表里
            }

            results.Total = (collections[2] as EntityCollection)[0].GetValue("Total").ToInt32(); // 记录总数

            return results;
        }

        /// <summary>
        /// 分页查询木片报备信息整理
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport03ByDateAndStartAndLength(string startDate, string endDate, int start, int length)
        {
            string nextEnd = endDate.ToDateEnd().ToDateTime().AddMonths(3).AddDays(-1).ToString("yyyy-MM-dd");
            string strSql = @"WITH BangAndGsmJoin AS(
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[GsmID], WJ.[WoodID], WJ.[IsRebate],
                            FP.Driver, FP.Supplier AS PoundSupplier,GI.[Supplier], GI.[Tree], GI.[Area] AS [Origin]
                            ,GI.[License], GI.[Ship], GI.[Line], GM.[Date], GM.[Text],GJ.IsAdd
                            ,EP.BackWeighTime,jVolume=((CASE ISNULL(EP.HandVolume,0) WHEN 0 THEN FP.FullVolume ELSE EP.HandVolume END)-ISNULL(EP.EmptyVolume,0)-ISNULL(EP.RebateVolume,0)) 
                            FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ ON (WB.[bangid] = WJ.[BangID] AND WJ.[State] = 0)
                            INNER JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            INNER JOIN [EmptyPound] EP ON (WJ.[WoodID] = EP.[WoodID] AND EP.[State] = 0)
                            --短信报备
                            INNER JOIN [GsmJoin] GJ ON (GJ.BangID = WB.bangid AND GJ.[State]=0)
                            INNER JOIN [GsmItem] GI ON (GI.[Unique]=GJ.GsmID AND GI.[State]=0)
                            INNER JOIN [GsmMessage] GM ON (GI.[Message] = GM.[Unique] AND GM.[State] = 0)
                            WHERE WB.[IsFiltered] = 0 AND GJ.GsmID > 0
                            AND WB.[Bang_Time] >= " + startDate.ToDateBegin().ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + endDate.ToDateEnd().ToDatabase() + @"
                            --取不需要短信报备区域和品种
                            UNION
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[GsmID], WJ.[WoodID], WJ.[IsRebate],
                            FP.Driver, FP.Supplier AS PoundSupplier,FP.[Supplier], FP.[Tree], GA.[Name] AS [Origin]
                            ,FP.[License], '' AS [Ship], '' AS [Line], '' AS [Date], '' AS [Text],0
                            ,EP.BackWeighTime,jVolume=((CASE ISNULL(EP.HandVolume,0) WHEN 0 THEN FP.FullVolume ELSE EP.HandVolume END)-ISNULL(EP.EmptyVolume,0)-ISNULL(EP.RebateVolume,0)) 
                            FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ ON (WB.[bangid] = WJ.[BangID] AND WJ.[State] = 0)
                            INNER JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            INNER JOIN [EmptyPound] EP ON (WJ.[WoodID] = EP.[WoodID] AND EP.[State] = 0)
                            INNER JOIN [GsmJoin] GJ ON (GJ.[WoodID] = FP.[WoodID] AND GJ.[State] = 0 AND GJ.[GsmID] = 0)
                            INNER JOIN [GsmArea] GA ON (GA.[Unique] = GJ.AreaID AND GA.[State]=0)
                            WHERE WB.[IsFiltered] = 0 
                            AND WB.[Bang_Time] >= " + startDate.ToDateBegin().ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + endDate.ToDateEnd().ToDatabase() + @"
                            )
                            SELECT ROW_NUMBER() OVER (ORDER BY Bang_Time ASC) AS [Number],License,Tree,jWeight,jVolume,Driver,
                            Bang_Time,Origin,BackWeighTime,PoundSupplier,Supplier,[Date],Ship,[Text],IsAdd INTO #Temp FROM BangAndGsmJoin

                            SELECT TOP " + length + @" * FROM #Temp WHERE [Number] >= " + start + @"
                            SELECT COUNT(1) AS [Total] FROM #Temp
                            DROP TABLE #Temp";

            IList collections = this.Execute.GetEntityCollections(strSql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 分页查询木片水份检验结果
        /// </summary>
        /// <param name="month">要查询的月份，格式如：2013-01</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport02ByMonthAndStartAndLength(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select [FullPound].[Unique]
                                      , substring(convert(varchar, [FullPound].[WeighTime], 120), 1, 10) as [WeighTime]
                                      , [WoodLaboratory].[Water], [WoodLaboratory].[Skin], [WoodLaboratory].[Scrap]
                               from [FullPound]
                               left outer join [WoodLaboratory] on ([FullPound].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[IsConfirmed] = 1
                                   and [WoodLaboratory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [FullPound].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + begin.ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + end.ToDatabase() + @"
                               group by [FullPound].[Unique]
                                        , substring(convert(varchar, [FullPound].[WeighTime], 120), 1, 10)
                                        , [WoodLaboratory].[Water], [WoodLaboratory].[Skin], [WoodLaboratory].[Scrap]
                           ),
                           [Build] as
                           (
                              select avg([Water]) as [AvgWater], avg([Skin]) as [AvgSkin], avg([Scrap]) as [AvgScrap]
                                     , [WeighTime] from [Filtered] group by [WeighTime]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [WeighTime] asc) as [Number], *
                               from [Build]
                           )
                           SELECT TOP " + length.ToDatabase() + @" * FROM [Numbered] WHERE [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[Unique]
                                      , substring(convert(varchar, [FullPound].[WeighTime], 120), 1, 10) as [WeighTime]
                                      , [WoodLaboratory].[Water], [WoodLaboratory].[Skin], [WoodLaboratory].[Scrap]
                               from [FullPound]
                               left outer join [WoodLaboratory] on ([FullPound].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[IsConfirmed] = 1
                                   and [WoodLaboratory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [FullPound].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + begin.ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + end.ToDatabase() + @"
                               group by [FullPound].[Unique]
                                        , substring(convert(varchar, [FullPound].[WeighTime], 120), 1, 10)
                                        , [WoodLaboratory].[Water], [WoodLaboratory].[Skin], [WoodLaboratory].[Scrap]
                           ),
                           [Merged] as
                           (
                              select sum([Unique]) as [RecordNumber] from [Filtered] group by [WeighTime]
                           )
                           SELECT COUNT(*) AS [Total] FROM [Merged];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }


        /// <summary>
        /// 各区域木料平均水份统计
        /// </summary>
        /// <param name="month">要查询的月份，格式如：2013-01</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport06ByMonthAndStartAndLength(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string strSql = @"SELECT [WeighTime],[Water],Area,Supplier,Tree,LinkMan INTO #TEMP FROM (
                            SELECT SUBSTRING(CONVERT(VARCHAR, FP.[WeighTime], 120), 1, 10) AS [WeighTime],
                            WL.[Water],GI.Area,FP.Supplier,MAX(FP.Tree) AS Tree,MAX(GS.LinkMan) AS LinkMan FROM [FullPound] FP
                            LEFT JOIN [WoodLaboratory] WL ON (FP.[WoodID] = WL.[WoodID] AND WL.[IsConfirmed] = 1 AND WL.[State] = " + StateEnum.Default.ToDatabase() + @")
                            LEFT JOIN [GsmSupplier] GS ON GS.Name=FP.Supplier AND GS.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN [GsmJoin] GJ ON GJ.WoodID=FP.WoodID AND GJ.[State]=" + StateEnum.Default.ToDatabase() + @"
                            INNER JOIN [GsmItem] GI ON GI.[Unique]=GJ.GsmID AND GI.[State]=" + StateEnum.Default.ToDatabase() + @"
                            WHERE FP.[State] = 0 AND GJ.GsmID > 0 AND FP.[WeighTime] >= " + begin.ToDatabase() + @" AND FP.[WeighTime] <= " + end.ToDatabase() + @"
                            GROUP BY  SUBSTRING(CONVERT(VARCHAR, FP.[WeighTime], 120), 1, 10),WL.[Water],GI.Area,FP.Supplier
                            --取不需要短信报备
                            UNION
                            SELECT SUBSTRING(CONVERT(VARCHAR, FP.[WeighTime], 120), 1, 10) AS [WeighTime],
                            WL.[Water],GA.Name AS Area,FP.Supplier,MAX(FP.Tree) AS Tree,MAX(GS.LinkMan) AS LinkMan FROM [FullPound] FP
                            LEFT JOIN [WoodLaboratory] WL ON (FP.[WoodID] = WL.[WoodID] AND WL.[IsConfirmed] = 1 AND WL.[State] = 0)
                            LEFT JOIN [GsmSupplier] GS ON GS.Name=FP.Supplier AND GS.[State]=0
                            INNER JOIN [GsmJoin] GJ ON (GJ.[WoodID] = FP.[WoodID] AND GJ.[State] = 0 AND GJ.[GsmID] = 0)
                            INNER JOIN [GsmArea] GA ON (GA.[Unique] = GJ.AreaID AND GA.[State]=0)
                            WHERE FP.[State] = 0 AND FP.[WeighTime] >= " + begin.ToDatabase() + @" AND FP.[WeighTime] <= " + end.ToDatabase() + @"
                            GROUP BY  SUBSTRING(CONVERT(VARCHAR, FP.[WeighTime], 120), 1, 10),WL.[Water],GA.Name,FP.Supplier
                            ) T

                            SELECT ROW_NUMBER() OVER (ORDER BY [WeighTime] ASC) AS [Number],WeighTime,AvgWater,Area,Supplier,Tree,LinkMan,TotalAvgWater,MaxWater,MinWater INTO #T FROM (
                            SELECT WeighTime,AvgWater,Area,Supplier,Tree,LinkMan,CAST(ROUND(TotalAvgWater,2) AS NUMERIC(5,2)) TotalAvgWater,MaxWater,MinWater FROM (
                            SELECT [WeighTime],AVG([Water]) AS AvgWater,MIN([Water]) AS MinWater,MAX([Water]) AS MaxWater,Area,Supplier,Tree,LinkMan,
                            (SELECT AVG(TP1.[Water]) AS [Water] FROM #TEMP TP1 WHERE TP1.Area=TP.Area AND TP1.[WeighTime] >= " + begin.ToDatabase() + @" AND TP1.[WeighTime] <= " + end.ToDatabase() + @" GROUP BY TP1.Area) AS TotalAvgWater
                            FROM #TEMP TP GROUP BY [WeighTime],Area,Supplier,Tree,LinkMan
                            ) T2) T3

                            SELECT TOP " + length + @" [Number],WeighTime,Area,AvgWater,Supplier,Tree,LinkMan,TotalAvgWater,MaxWater,MinWater FROM #T WHERE [Number] >= " + start + @" ORDER BY WeighTime ASC
                            SELECT COUNT(1) AS [Total] FROM #T

                            DROP TABLE #TEMP
                            DROP TABLE #T";
            IList collections = this.Execute.GetEntityCollections(strSql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }



        /// <summary>
        /// 分页查询木片检验结果
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport01ByDateAndStartAndLength(string startDate, string endDate, int start, int length)
        {
            string sql = @"with [Filtered] as
                           (
                               select [F1].[Unique], (
                                        select distinct [Key] + '，' from [Factory]
                                        where [WoodID] = [F1].[WoodID] and [State] = " + StateEnum.Default.ToDatabase() + @"
                                        for xml path('')
                                      ) as [Key], (
                                        select distinct [Sampler] + '，' from [Factory]
                                        where [WoodID] = [F1].[WoodID] and [State] = " + StateEnum.Default.ToDatabase() + @"
                                        for xml path('')
                                     ) as [Sampler]
                                     , [F1].[Tree], convert(varchar(12) , [F1].[WeighTime], 102 ) as [WeighTime]
                                     , [WoodLaboratory].[Water], [WoodLaboratory].[Skin], [WoodLaboratory].[Scrap]
                                     , [WoodLaboratory].[Bad], [WoodLaboratory].[Greater], [WoodLaboratory].[Less]
                               from [FullPound] as [F1]
                               left outer join [WoodLaboratory] on ([F1].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[IsConfirmed] = 1
                                   and [WoodLaboratory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [F1].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [F1].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                   and [F1].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] asc) as [Number]
                                      , left([Key], len([Key]) - 1) as [Key]
                                      , left([Sampler], len([Sampler]) - 1) as [Sampler]
                                      , [Tree], [WeighTime], [Water], [Skin], [Scrap], [Bad], [Greater], [Less]
                               from [Filtered]
                           )
                           SELECT TOP " + length.ToDatabase() + @" * from [Numbered] where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [Unique]
                               from [FullPound]
                               where [State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                   and [WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                           )
                           select count([Unique]) as [Total]
                           from [Filtered];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }



        /// <summary>
        /// 分页高级查询木质原料来源地总表 zwb add
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="License">车号</param>
        /// <param name="Driver">送货员</param>
        /// <param name="PoundSupplier">卸货员</param>
        /// <param name="Area">区域</param>
        /// <param name="statistical">统计字段</param>
        /// <returns>结果集</returns>
        public EntityCollection AdvancedSearchReport(string startDate, string endDate, int start, int length,
            string License, string Driver, string PoundSupplier, string Area, string statistical)
        {
            string nextEnd = endDate.ToDateEnd().ToDateTime().AddMonths(3).AddDays(-1).ToString("yyyy-MM-dd");
            string strWhere = string.Empty;     //查询条件
            string colomns, colomns1, order = string.Empty;      //显示列
            //转换为大写
            License = License.ToUpper();
            Driver = Driver.ToUpper();
            PoundSupplier = PoundSupplier.ToUpper();
            //车号
            if (!string.IsNullOrWhiteSpace(License))
            {
                strWhere += " AND (License IN(SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + License + "',';')) OR carCID IN(SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + License + "',';')))";
            }
            //送货员
            if (!string.IsNullOrWhiteSpace(Driver))
            {
                strWhere += " AND Driver IN(SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + Driver + "',';'))";
            }
            //卸货员
            if (!string.IsNullOrWhiteSpace(PoundSupplier))
            {
                strWhere += " AND PoundSupplier IN(SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + PoundSupplier + "',';'))";
            }
            //来源地
            if (!string.IsNullOrWhiteSpace(Area))
            {
                strWhere += " AND Origin IN(SELECT short_str FROM [dbo].[F_SQLSERVER_SPLIT]('" + Area + "',';'))";
            }

            if (!string.IsNullOrWhiteSpace(statistical))
            {
                order = "jWeight";
                colomns1 = @"" + statistical + ",SUM([jWeight]) AS jWeight,SUM(jVolume) AS jVolume";
                colomns = @"" + statistical + ",jWeight,jVolume";
                strWhere += " GROUP BY " + statistical;
            }
            else
            {
                order = "Bang_Time";
                colomns = "License,Tree,jWeight,jVolume,Driver,Bang_Time,Origin,BackWeighTime,PoundSupplier,Supplier,[Date],Ship,[Text],carCID,IsAdd";
                colomns1 = "License,Tree,jWeight,jVolume,Driver,Bang_Time,Origin,BackWeighTime,PoundSupplier,Supplier,[Date],Ship,[Text],carCID,IsAdd";
            }
            string strSql = @"WITH BangAndGsmJoin AS(
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[GsmID], WJ.[WoodID], WJ.[IsRebate],
                            FP.Driver, FP.Supplier AS PoundSupplier,GI.[Supplier], GI.[Tree], GI.[Area] AS [Origin]
                            ,GI.[License], GI.[Ship], GI.[Line], GM.[Date], GM.[Text],GJ.IsAdd
                            ,EP.BackWeighTime,jVolume=((CASE ISNULL(EP.HandVolume,0) WHEN 0 THEN FP.FullVolume ELSE EP.HandVolume END)-ISNULL(EP.EmptyVolume,0)-ISNULL(EP.RebateVolume,0)) 
                            FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ ON (WB.[bangid] = WJ.[BangID] AND WJ.[State] = 0)
                            INNER JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            INNER JOIN [EmptyPound] EP ON (WJ.[WoodID] = EP.[WoodID] AND EP.[State] = 0)
                            --短信报备
                            INNER JOIN [GsmJoin] GJ ON (GJ.BangID = WB.bangid AND GJ.[State]=0)
                            INNER JOIN [GsmItem] GI ON (GI.[Unique]=GJ.GsmID AND GI.[State]=0)
                            INNER JOIN [GsmMessage] GM ON (GI.[Message] = GM.[Unique] AND GM.[State] = 0)
                            WHERE WB.[IsFiltered] = 0 AND GJ.GsmID > 0
                            AND WB.[Bang_Time] >= " + startDate.ToDateBegin().ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + endDate.ToDateEnd().ToDatabase() + @"
                            --取不需要短信报备
                            UNION
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[GsmID], WJ.[WoodID], WJ.[IsRebate],
                            FP.Driver, FP.Supplier AS PoundSupplier,FP.[Supplier], FP.[Tree], GA.Name AS [Origin]
                            ,FP.[License], '' AS [Ship], '' AS [Line], '' AS [Date], '' AS [Text],0
                            ,EP.BackWeighTime,jVolume=((CASE ISNULL(EP.HandVolume,0) WHEN 0 THEN FP.FullVolume ELSE EP.HandVolume END)-ISNULL(EP.EmptyVolume,0)-ISNULL(EP.RebateVolume,0)) 
                            FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ ON (WB.[bangid] = WJ.[BangID] AND WJ.[State] = 0)
                            INNER JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            INNER JOIN [EmptyPound] EP ON (WJ.[WoodID] = EP.[WoodID] AND EP.[State] = 0)
                            INNER JOIN [GsmJoin] GJ ON (GJ.[WoodID] = FP.[WoodID] AND GJ.[State] = 0 AND GJ.[GsmID] = 0)
                            INNER JOIN [GsmArea] GA ON (GA.[Unique] = GJ.AreaID AND GA.[State]=0)
                            WHERE WB.[IsFiltered] = 0 
                            AND WB.[Bang_Time] >= " + startDate.ToDateBegin().ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + endDate.ToDateEnd().ToDatabase() + @"
                            )
                            SELECT ROW_NUMBER() OVER (ORDER BY " + order + " ASC) AS [Number]," + colomns + " INTO #Temp FROM (SELECT " + colomns1 + " FROM BangAndGsmJoin WHERE 1=1 " + strWhere + @") AS T

                            SELECT TOP " + length + @" * FROM #Temp WHERE [Number] >= " + start + @"
                            SELECT COUNT(1) AS [Total] FROM #Temp
                            DROP TABLE #Temp";

            IList collections = this.Execute.GetEntityCollections(strSql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }


        /// <summary>
        /// 各区域进柴统计(重量)
        /// </summary>
        /// <param name="month">要查询的月份，格式如：2013-01</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection WoodAreaInFactoryReport(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string strSql = @"WITH Bang AS (
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[WoodID], WJ.[IsRebate],GI.Area FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ on (WB.[bangid] = WJ.[BangID] AND WJ.[State] = 0)
                            INNER JOIN FullPound FP ON FP.WoodID=WJ.WoodID AND FP.[State]=0

                            INNER JOIN [GsmJoin] GJ on (WB.[bangid] = GJ.[BangID] AND GJ.[State] = 0)
                            INNER JOIN [GsmItem] GI ON (GI.[Unique]=GJ.GsmID AND GI.[State]=0)
                            INNER JOIN [GsmMessage] GS on (GI.[Message] = GS.[Unique] AND GS.[State] = 0
                            AND GS.[Date] >= " + begin.ToDatabase() + @" 
                            AND GS.[Date] <= " + end.ToDatabase() + @")

                            WHERE WB.[IsFiltered] = 0 AND GI.[State] = 0 AND GJ.GsmID > 0
                            AND WB.[Bang_Time] >= " + begin.ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + end.ToDatabase() + @"
                            --取不需要短信报备
                            UNION ALL
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[WoodID], WJ.[IsRebate],GA.Name AS Area FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ on (WB.[bangid] = WJ.[BangID] AND WJ.[State] = 0)
                            INNER JOIN FullPound FP ON FP.WoodID=WJ.WoodID AND FP.[State]=0
                            INNER JOIN [GsmJoin] GJ ON (GJ.[WoodID] = FP.[WoodID] AND GJ.[State] = 0 AND GJ.[GsmID] = 0)
                            INNER JOIN [GsmArea] GA ON (GA.[Unique] = GJ.AreaID AND GA.[State]=0)
                            WHERE WB.[IsFiltered] = 0 
                            AND WB.[Bang_Time] >= " + begin.ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + end.ToDatabase() + @"
                            )
                            SELECT ROW_NUMBER() OVER (ORDER BY [Bang_Time] ASC) AS [Number],[Bang_Time],jWeight,Area,SumjWeight,TotaljWeight INTO #Temp FROM(
                            SELECT CONVERT(VARCHAR(100),[Bang_Time],23) [Bang_Time],SUM(jWeight) jWeight ,Area,
                            (SELECT SUM(jWeight) FROM Bang l WHERE l.Area=d.Area) AS SumjWeight,
                            (SELECT SUM(jWeight) AS [Total] FROM Bang) AS TotaljWeight FROM Bang d
                            GROUP BY Area,CONVERT(VARCHAR(100),[Bang_Time],23)) T

                            SELECT TOP " + length + @" [Number],[Bang_Time],jWeight,Area,SumjWeight,TotaljWeight
                            ,scale= (CASE SumjWeight WHEN 0 THEN 0 ELSE CAST(ROUND((jWeight/SumjWeight),4) AS DECIMAL(10,4))*100 END)
                            ,Totalscale= (CASE TotaljWeight WHEN 0 THEN 0 ELSE CAST(ROUND((SumjWeight/TotaljWeight),4) AS DECIMAL(10,4))*100 END)
                            FROM #Temp WHERE [Number] >= " + start + @" ORDER BY Area DESC,[Bang_Time] ASC

                            SELECT COUNT(1) AS [Total] FROM #Temp
                            DROP TABLE #Temp";
            IList collections = this.Execute.GetEntityCollections(strSql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 各品种进柴统计(重量)
        /// </summary>
        /// <param name="month">要查询的月份，格式如：2013-01</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <returns>结果集</returns>
        public EntityCollection WoodCropInFactoryReport(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string strSql = @"WITH Bang AS (
                            SELECT WB.[Bang_Time], WB.[carCID], WB.[breedName],WB.jWeight
                            , WB.[userXHName], WJ.[GsmID], WJ.[WoodID], WJ.[IsRebate],FP.Area
                            FROM [WoodBang] WB
                            INNER JOIN [WoodJoin] WJ on (WB.[bangid] = WJ.[BangID]
                            AND WJ.[State] = 0)
                            INNER JOIN FullPound FP ON FP.WoodID=WJ.WoodID 
                            WHERE WB.[IsFiltered] = 0
                            AND WB.[Bang_Time] >= " + begin.ToDatabase() + @"
                            AND WB.[Bang_Time] <= " + end.ToDatabase() + @"
                            ),
                            Gsm AS (
                            SELECT GI.[Unique], GI.[Supplier], GI.[Tree], GI.[Origin]
                            , GI.[License], GI.[Ship], GI.[Line], GS.[Date], GS.[Text]
                            FROM [GsmItem] GI
                            INNER JOIN [GsmMessage] GS on (GI.[Message] = GS.[Unique] AND GS.[State] = 0
                            AND GS.[Date] >= " + begin.ToDatabase() + @" 
                            AND GS.[Date] <= " + end.ToDatabase() + @") WHERE GI.[State] = 0
                            ),
                            BangJoinGsm AS(
                            SELECT b.[Bang_Time], b.[carCID], b.[breedName], b.[userXHName]
                            , b.[WoodID], b.[IsRebate],b.jWeight
                            , g.[Supplier], g.[Tree], g.[Origin], g.[License]
                            , g.[Ship], g.[Line], g.[Date], g.[Text],b.Area
                            FROM Bang b LEFT JOIN Gsm g on b.[GsmID] = g.[Unique]
                            ),
                            DataList as
                            (
                            SELECT [Bang_Time],[breedName],jWeight,Area FROM BangJoinGsm
                            WHERE (([Date] IS NOT NULL) AND [Date] >= " + begin.ToDatabase() + @"
                            AND [Date] <= " + end.ToDatabase() + @")
                            OR (([Date] IS  NULL) AND [Bang_Time] >= " + begin.ToDatabase() + @"
                            AND [Bang_Time] <= " + end.ToDatabase() + @")
                            )
                            SELECT ROW_NUMBER() OVER (ORDER BY [Bang_Time] ASC) AS [Number],[Bang_Time],jWeight,Area,SumjWeight,TotaljWeight INTO #Temp FROM(
                            SELECT CONVERT(VARCHAR(100),[Bang_Time],23) [Bang_Time],SUM(jWeight) jWeight ,Area,
                            (SELECT SUM(jWeight) FROM DataList l WHERE l.Area=d.Area) AS SumjWeight,
                            (SELECT SUM(jWeight) AS [Total] FROM DataList) AS TotaljWeight FROM DataList d
                            GROUP BY Area,CONVERT(VARCHAR(100),[Bang_Time],23)) T

                            SELECT TOP " + length + @" [Number],[Bang_Time],jWeight,Area,SumjWeight,TotaljWeight,scale=CAST(ROUND((jWeight/SumjWeight),4) AS DECIMAL(10,4))*100,Totalscale=CAST(ROUND((SumjWeight/TotaljWeight),4) AS DECIMAL(10,4))*100 FROM #Temp WHERE [Number] >= " + start + @" ORDER BY Area DESC,[Bang_Time] ASC

                            SELECT COUNT(1) AS [Total] FROM #Temp
                            DROP TABLE #Temp";
            IList collections = this.Execute.GetEntityCollections(strSql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

    }
}
