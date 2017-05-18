using System.Data.SqlClient;
using System.Collections;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.GsmJoin.Dals
{
    public class GsmJoinDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmJoin";
            this.Inserts = "[Unique], [BangID], [GsmID], [WoodID], [IsAdd], [IsGsm], [JoinTime], [Operator], [State], [Version], [Log], [AreaID]";
            this.Updates = "[BangID], [GsmID], [WoodID], [AreaID]";
            this.Selects = "[Unique], [BangID], [GsmID], [WoodID], [JoinTime], [AreaID]";
            this.Order = "[Unique] desc";
        }

        public GsmJoinDal()
        {
            this.Init();
        }

        public GsmJoinDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 修改记录的对接状态
        /// 返回数据库表受影响的行数，行数>0则为表记录修改成功
        /// </summary>
        /// <param name="bangid">地磅记录ID</param>
        /// <param name="join">已对接标识标识（0 - 否，1 - 是）</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateJoin(object bangid, object join)
        {
            string sql = @"update [WoodBang]
                           set [IsJoinGsmed] = " + join.ToDatabase() + @"
                           where [bangid] = " + bangid.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 断开对接
        /// </summary>
        /// <param name="unique">记录编号</param>
        /// <param name="log">日志</param>
        /// <returns></returns>
        public int CutOffJoin(int unique, string log)
        {
            string sql = @"update [GsmJoin]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                               , [Log] += " + log.ToDatabase() + @"
                           where [Unique] = " + unique.ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase();
            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改记录的过滤状态
        /// 返回数据库表受影响的行数，行数>0则为表记录修改成功
        /// </summary>
        /// <param name="bangid">地磅记录ID</param>
        /// <param name="filter">是否过滤标识（0 - 否，1 - 是）</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateFilter(int bangid, int filter)
        {
            string sql = @"update [WoodBang]
                           set [IsFilterGsmed] = " + filter.ToDatabase() + @"
                           where [bangid] = " + bangid.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 分页查询已经成功对接地磅数据
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="license">要查询的车牌号（不包含省份）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfJoinByDateAndStartAndLength(string date, string enddate, int start, int length, string license, string area)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(area))
            {
                strWhere += " AND [FullPound].[Area] LIKE '%" + area + "%'";
            }
            string sql = @"WITH [Filtered] AS
                           (
                               SELECT [WoodJoin].[Unique], [WoodJoin].[BangID], [WoodJoin].[WoodID], [WoodJoin].[JoinTime]
                                      , [WoodBang].[Bang_Time], [WoodBang].[carCID], [WoodBang].[carUser]
                                      , [WoodBang].[breedName], [WoodBang].[userXHName], [WoodBang].[JWeight]
                                      , [FullPound].[License], [FullPound].[Tree],[FullPound].[FullVolume]
                                      , [FullPound].[Driver], [FullPound].[Supplier], [FullPound].[WeighTime], [FullPound].[Area]
                               FROM [WoodJoin]
                               INNER JOIN [WoodBang] ON ([WoodJoin].[BangID] = [WoodBang].[bangid]
                                AND [WoodBang].[Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @" 
                                AND [WoodBang].[Bang_Time] <= " + enddate.ToDateEnd().ToDatabase() + @" 
                                AND [WoodBang].IsJoinGsmed=0 AND [WoodBang].IsFilterGsmed=0 AND [WoodBang].IsFiltered=0 AND [WoodBang].IsJoined=1 
                                   " + (license == "" ? ")" : string.Format("and [WoodBang].[carCID] like '%{0}%')", license)) + @"
                               INNER JOIN [FullPound] ON ([WoodJoin].[WoodID] = [FullPound].[WoodID] AND [FullPound].[State] = 0)
                               WHERE [WoodJoin].[State] = 0 AND [WoodJoin].[IsGsm]=0  " + strWhere + @"
                           ),
                           [Numbered] AS
                           (
                               SELECT ROW_NUMBER() OVER (ORDER BY [Bang_Time] ASC) AS [Number], * FROM [Filtered]
                           )
                           SELECT * INTO #TEMP FROM [Numbered];
                           SELECT TOP " + length.ToDatabase() + @" * FROM #TEMP WHERE [Number] >= " + start.ToDatabase() + @"
                           SELECT COUNT([Unique]) AS [Total] FROM #TEMP
                           DROP TABLE #TEMP";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }


        /// <summary>
        /// 分页查询已经成功对接报备数据
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="license">要查询的车牌号（不包含省份）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetDataOfJoinGsmByDateAndStartAndLength(string date, int start, int length, string license)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(license))
            {
                strWhere += " AND GI.License like '%" + license + @"%'";
            }
            string sql = @"WITH [Filtered] AS
                            (
                            SELECT GJ.[Unique], GJ.[BangID], GJ.[WoodID], GJ.[JoinTime], WB.Bang_Time
                            , GI.Supplier AS GISupplier, GI.Tree AS GITree, GI.Area AS GIArea,GI.Ship,GJ.IsAdd,GM.[Date]
                            , FP.[License], FP.[Tree],FP.[FullVolume],GI.License AS GILicense
                            , FP.[Driver], FP.[Supplier], FP.[WeighTime], FP.[Area],WB.jWeight
                            FROM [GsmJoin] GJ INNER JOIN GsmItem GI ON (GJ.GsmID = GI.[Unique] AND GI.[State]=0)
                            INNER JOIN [GsmMessage] GM on (GI.[Message] = GM.[Unique] AND GM.[State]=0)
                            INNER JOIN [FullPound] FP on (GJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            INNER JOIN [WoodBang] WB ON (GJ.[BangID] = WB.[bangid] AND WB.IsFiltered = 0 AND WB.IsJoinGsmed = 1)
                            WHERE GJ.[State] = 0 AND GJ.[IsGsm]=1 AND GJ.GsmID > 0 AND WB.[Bang_Time]>=" + date.ToDateBegin().ToDatabase() + strWhere + @"
                            UNION
                            SELECT GJ.[Unique], GJ.[BangID], GJ.[WoodID], GJ.[JoinTime], WB.Bang_Time
                            , NULL AS GISupplier, NULL AS GITree, GA.[Name] AS GIArea,NULL AS Ship,GJ.IsAdd,NULL AS [Date]
                            , FP.[License], FP.[Tree],FP.[FullVolume],NULL AS GILicense
                            , FP.[Driver], FP.[Supplier], FP.[WeighTime], FP.[Area],WB.jWeight
                            FROM [GsmJoin] GJ
                            INNER JOIN [FullPound] FP on (GJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            INNER JOIN [WoodBang] WB ON (GJ.[BangID] = WB.[bangid] AND WB.IsFiltered = 0 AND WB.IsJoinGsmed = 1)
                            INNER JOIN GsmArea GA ON (GA.[Unique]=GJ.AreaID AND GA.[State]=0)
                            WHERE GJ.[State] = 0 AND GJ.[IsGsm]=1 AND GJ.GsmID=0 AND WB.[Bang_Time]>=" + date.ToDateBegin().ToDatabase() + strWhere.Replace("GI.License", "FP.License") + @"
                            )

                            SELECT ROW_NUMBER() OVER (ORDER BY [Bang_Time] ASC) AS [Number], * INTO #Temp FROM [Filtered]
                            SELECT TOP " + length.ToDatabase() + @" * FROM #Temp WHERE [Number] >= " + start.ToDatabase() + @"
                            SELECT COUNT([Unique]) AS [Total] FROM #Temp;
                            DROP TABLE #Temp";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
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
            DateTime bangDate = DateTime.Parse(date);
            DateTime begin = bangDate.AddMonths(-1);
            DateTime end = bangDate.AddMonths(9);
            string strWhere = string.Empty;
            //车号
            if (!string.IsNullOrWhiteSpace(license))
            {
                strWhere += " and [GsmItem].[License] like '%" + license + @"%'";
            }
            //送货员
            if (!string.IsNullOrWhiteSpace(driver))
            {
                strWhere += " and [GsmItem].[Driver] = '" + driver.ToUpper() + @"'";
            }
            //卸货员
            if (!string.IsNullOrWhiteSpace(supplier))
            {
                strWhere += " and [GsmItem].[Supplier] = '" + supplier.ToUpper() + @"'";
            }
            //区域
            if (!string.IsNullOrWhiteSpace(area))
            {
                strWhere += " and [GsmItem].[Area] like '%" + area + @"%'";
            }
            string sql = @"with [Filtered] as
                           (
                               select [GsmItem].[Unique] from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [GsmJoin].[GsmID] from [Filtered]
                               inner join [GsmJoin] on ([Filtered].[Unique] = [GsmJoin].[GsmID]
                               and [GsmJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [GsmItem].[Unique], [GsmItem].[Supplier], [GsmItem].[Tree], [GsmItem].[Origin]
                                      , [GsmItem].[License], [GsmItem].[Ship], [GsmItem].[Line], [GsmItem].[Area], [GsmItem].[Driver]
                                      , [GsmMessage].[Date], [GsmMessage].[Text] from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + bangDate.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                                  and (not exists (select * from [Build] where [GsmID] = [GsmItem].[Unique]))
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Date] desc) as [Number], * from [Merged]
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered]  where [Number] >= " + start.ToDatabase() + @";

                           with [Filtered] as
                           (
                               select [GsmItem].[Unique] from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [GsmJoin].[GsmID] from [Filtered]
                               inner join [GsmJoin] on ([Filtered].[Unique] = [GsmJoin].[GsmID]
                               and [GsmJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [GsmItem].[Unique], [GsmItem].[Supplier], [GsmItem].[Tree], [GsmItem].[Origin]
                                      , [GsmItem].[License], [GsmItem].[Ship], [GsmItem].[Line], [GsmItem].[Area], [GsmItem].[Driver]
                                      , [GsmMessage].[Date], [GsmMessage].[Text] from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] > '" + bangDate.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                                  and (not exists (select * from [Build] where [GsmID] = [GsmItem].[Unique]))
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Date]) as [Number], * from [Merged]
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered] where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [GsmItem].[Unique] from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [GsmJoin].[GsmID] from [Filtered]
                               inner join [GsmJoin] on ([Filtered].[Unique] = [GsmJoin].[GsmID]
                               and [GsmJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [GsmItem].[Unique], [GsmMessage].[Date] from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                                  and (not exists (select * from [Build] where [GsmID] = [GsmItem].[Unique]))
                           )
                           select count([Unique]) as [Total] from [Merged];";

            IList collections = this.Execute.GetEntityCollections(sql);

            /* 根据数据重新组合结果集 */
            EntityCollection after = collections[1] as EntityCollection; // 过磅日期之后的数据
            EntityCollection before = collections[0] as EntityCollection; // 过磅日期之前的数据

            EntityCollection results = new EntityCollection(after.PropertyCollection);

            int afterItemCount = after.Count;
            int beforeItemCoun = before.Count;

            /*  先取出过磅日期之前的第一条记录，然后再取出过磅日期之后的第一条记录  */
            if (beforeItemCoun != 0) results.Add(before[0] as Entity);
            if (afterItemCount != 0) results.Add(after[0] as Entity);

            /* 再把其它的记录逐一添加，优先过磅日期之前的数据，因为一般是先短信报备后才车辆到达工厂过磅 */
            for (int j = 1; j < beforeItemCoun; j++) results.Add(before[j] as Entity);
            for (int i = 1; i < afterItemCount; i++) results.Add(after[i] as Entity);

            results.Total = (collections[2] as EntityCollection)[0].GetValue("Total").ToInt32(); // 记录总数

            return results;
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
            DateTime bangDate = DateTime.Parse(date);
            DateTime begin = bangDate.AddMonths(-1);
            DateTime end = bangDate.AddMonths(1);
            string sql = @"with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                               from [FullPound]
                               inner join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                       and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                  and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                  and [FullPound].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FullPound].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FullPound].[License] like '%" + license + @"%'
                           ),
                           [Merged] as
                           (
                               select [FP].[WoodID], [FP].[License], [FP].[Area], [FP].[Driver], [FP].[Tree], [FP].[Supplier]
                                      , [FP].[WeighTime]
                                      , [Wood].[BarrierID], [Account].[Description]
                               from [FullPound] as [FP]
                               inner join [Wood] on ([FP].[WoodID] = [Wood].[Unique]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [Account] on ([FP].[Operator] = [Account].[Unique]
                                   and [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Account].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FP].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                  and [FP].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                  and [FP].[WeighTime] >= '" + bangDate.ToString() + @"'
                                  and [FP].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FP].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Filtered] where [WoodID] = [FP].[WoodID]))
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Merged].[WeighTime]) as [Number], [Merged].[WoodID]
                                      , [Merged].[License], [Merged].[Area], [Merged].[Driver], [Merged].[Tree]
                                      , [Merged].[Supplier], [Merged].[WeighTime], [Merged].[Description]
                                      , [Barrier].[Place]
                               from [Merged]
                               left outer join [Barrier] on ([Merged].[BarrierID] = [Barrier].[Unique]
                                   and [Barrier].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Barrier].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                               from [FullPound]
                               inner join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                       and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                  and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                  and [FullPound].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FullPound].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FullPound].[License] like '%" + license + @"%'
                           ),
                           [Merged] as
                           (
                               select [FP].[WoodID], [FP].[License], [FP].[Area], [FP].[Driver], [FP].[Tree], [FP].[Supplier]
                                      , [FP].[WeighTime]
                                      , [Wood].[BarrierID], [Account].[Description]
                               from [FullPound] as [FP]
                               inner join [Wood] on ([FP].[WoodID] = [Wood].[Unique]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [Account] on ([FP].[Operator] = [Account].[Unique]
                                   and [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Account].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FP].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                  and [FP].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                  and [FP].[WeighTime] < '" + bangDate.ToString() + @"'
                                  and [FP].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FP].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Filtered] where [WoodID] = [FP].[WoodID]))
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Merged].[WeighTime]) as [Number], [Merged].[WoodID]
                                      , [Merged].[License], [Merged].[Area], [Merged].[Driver], [Merged].[Tree]
                                      , [Merged].[Supplier], [Merged].[WeighTime], [Merged].[Description]
                                      , [Barrier].[Place]
                               from [Merged]
                               left outer join [Barrier] on ([Merged].[BarrierID] = [Barrier].[Unique]
                                   and [Barrier].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Barrier].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                               from [FullPound]
                               inner join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                       and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                  and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                  and [FullPound].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FullPound].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FullPound].[License] like '%" + license + @"%'
                           ),
                           [Merged] as
                           (
                               select [FullPound].[WoodID], [Wood].[Unique]
                               from [FullPound]
                               inner join [Wood] on ([FullPound].[WoodID] = [Wood].[Unique]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                  and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                  and [FullPound].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FullPound].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FullPound].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Filtered] where [WoodID] = [FullPound].[WoodID]))
                           )
                           select count([Unique]) as [Total]
                           from [Merged];";

            IList collections = this.Execute.GetEntityCollections(sql);

            /* 根据数据重新组合结果集 */
            EntityCollection after = collections[0] as EntityCollection; // 过磅日期之后的数据
            EntityCollection before = collections[1] as EntityCollection; // 过磅日期之前的数据

            EntityCollection results = new EntityCollection(after.PropertyCollection);

            int afterItemCount = after.Count;
            int beforeItemCoun = before.Count;

            /*  先取出过磅日期之后的第一条记录，然后再取出过磅日期之前的第一条记录  */
            if (afterItemCount != 0) results.Add(after[0] as Entity);
            if (beforeItemCoun != 0) results.Add(before[0] as Entity);

            /* 再把其它的记录逐一添加，优先过磅日期之前的数据，因为数据量较之后的少 */
            for (int j = 1; j < beforeItemCoun; j++) results.Add(before[j] as Entity);
            for (int i = 1; i < afterItemCount; i++) results.Add(after[i] as Entity);

            results.Total = (collections[2] as EntityCollection)[0].GetValue("Total").ToInt32(); // 记录总数

            return results;
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
            string sql = @"WITH [Filtered] AS
                           (
                               SELECT [bangid], [jWeight], [Bang_Time], [carCID], [carUser], [firstBangUser], [breedName], [userXHName]
                               FROM [WoodBang] WHERE [IsJoinGsmed] = 0 AND [IsJoined] = 1
                                     AND [IsFilterGsmed] = " + isFiltered.ToDatabase() + @"
                                     AND [Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @"
                           ),
                           [Numbered] AS
                           (
                               SELECT ROW_NUMBER() OVER (ORDER BY [Bang_Time]) AS [Number], * FROM [Filtered]
                           )
                           SELECT TOP " + length.ToDatabase() + @" * FROM [Numbered] WHERE [Number] >= " + start.ToDatabase() + @";
                           WITH [Build] AS
                           (
                               SELECT [bangid] FROM [WoodBang] WHERE [IsJoinGsmed] = 0 AND [IsJoined] = 1
                                     AND [IsFilterGsmed] = " + isFiltered.ToDatabase() + @"
                                     AND [Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @"
                           )
                           SELECT COUNT([bangid]) AS [Total] FROM [Build];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

    }
}
