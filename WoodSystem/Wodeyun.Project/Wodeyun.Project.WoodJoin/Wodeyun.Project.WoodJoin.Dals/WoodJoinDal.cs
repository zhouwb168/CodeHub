using System.Data.SqlClient;
using System.Collections;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Gf.Database.SqlServer;

namespace Wodeyun.Project.WoodJoin.Dals
{
    public class WoodJoinDal : CommonDal
    {
        private void Init()
        {
            this.Table = "WoodJoin";
            this.Inserts = "[Unique], [BangID], [GsmID], [WoodID], [IsRebate], [IsGsm], [JoinTime], [Operator], [State], [Version], [Log]";
            this.Updates = "[BangID], [GsmID], [WoodID]";
            this.Selects = "[Unique], [BangID], [GsmID], [WoodID], [JoinTime]";
            this.Order = "[Unique] desc";
        }

        public WoodJoinDal()
        {
            this.Init();
        }

        public WoodJoinDal(SqlTransaction transaction)
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
                           set [IsJoined] = " + join.ToDatabase() + @"
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
            string sql = @"update [WoodJoin]
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
                           set [IsFiltered] = " + filter.ToDatabase() + @"
                           where [bangid] = " + bangid.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
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
            string sql = @"with [Filtered] as
                           (
                               select [WoodJoin].[Unique], [WoodJoin].[BangID], [WoodJoin].[JoinTime]
                                      , [WoodBang].[Bang_Time], [WoodBang].[carCID], [WoodBang].[carUser]
                                      , [WoodBang].[breedName], [WoodBang].[userXHName], [WoodBang].[bangCid]
                                      , [FullPound].[License], [FullPound].[Tree]
                                      , [FullPound].[Driver], [FullPound].[Supplier], [FullPound].[WeighTime]
                               from [WoodJoin]
                               inner join [WoodBang] on ([WoodJoin].[BangID] = [WoodBang].[bangid]
                                   and [WoodBang].[Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @"
                                   " + (license == "" ? ")" : string.Format("and [WoodBang].[carCID] like '%{0}%')", license)) + @"
                               inner join [FullPound] on ([WoodJoin].[WoodID] = [FullPound].[WoodID] and [FullPound].[State] = 0)
                               where [WoodJoin].[State] = 0  and [WoodBang].IsJoined = 1
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Bang_Time] desc) as [Number], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Build] as
                           (
                               select [WoodJoin].[Unique] from [WoodJoin]
                               inner join [WoodBang] on ([WoodJoin].[BangID] = [WoodBang].[bangid]
                                   and [WoodBang].[Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @"
                                   " + (license == "" ? ")" : string.Format("and [WoodBang].[carCID] like '%{0}%')", license)) + @"
                               inner join [FullPound] on ([WoodJoin].[WoodID] = [FullPound].[WoodID] and [FullPound].[State] = 0)
                               where [WoodJoin].[State] =0  and [WoodBang].IsJoined = 1
                           )
                           select count([Unique]) as [Total] from [Build];";

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
        public EntityCollection GetDataOfGsmByDateAndStartAndLength(string license, string date, int start, int length)
        {
            DateTime bangDate = DateTime.Parse(date);
            DateTime begin = bangDate.AddMonths(-3);
            DateTime end = bangDate.AddMonths(3);
            string sql = @"with [Filtered] as
                           (
                               select [GsmItem].[Unique]
                               from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @"
                                  and [GsmItem].[License] like '%" + license + @"%'
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [WoodJoin].[GsmID]
                               from [Filtered]
                               inner join [WoodJoin] on ([Filtered].[Unique] = [WoodJoin].[GsmID]
                                       and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [GsmItem].[Unique], [GsmItem].[Supplier], [GsmItem].[Tree], [GsmItem].[Origin]
                                      , [GsmItem].[License], [GsmItem].[Ship], [GsmItem].[Line]
                                      , [GsmMessage].[Date], [GsmMessage].[Text]
                               from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + bangDate.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @"
                                  and [GsmItem].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Build] where [GsmID] = [GsmItem].[Unique]))
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Date] desc) as [Number], *
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [GsmItem].[Unique]
                               from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @"
                                  and [GsmItem].[License] like '%" + license + @"%'
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [WoodJoin].[GsmID]
                               from [Filtered]
                               inner join [WoodJoin] on ([Filtered].[Unique] = [WoodJoin].[GsmID]
                                       and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [GsmItem].[Unique], [GsmItem].[Supplier], [GsmItem].[Tree], [GsmItem].[Origin]
                                      , [GsmItem].[License], [GsmItem].[Ship], [GsmItem].[Line]
                                      , [GsmMessage].[Date], [GsmMessage].[Text]
                               from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] > '" + bangDate.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @"
                                  and [GsmItem].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Build] where [GsmID] = [GsmItem].[Unique]))
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Date]) as [Number], *
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [GsmItem].[Unique]
                               from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @"
                                  and [GsmItem].[License] like '%" + license + @"%'
                           ),
                           [Build] as
                           (
                               select [Filtered].[Unique], [WoodJoin].[GsmID]
                               from [Filtered]
                               inner join [WoodJoin] on ([Filtered].[Unique] = [WoodJoin].[GsmID]
                                       and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [GsmItem].[Unique], [GsmMessage].[Date]
                               from [GsmItem]
                               inner join [GsmMessage] on ([GsmItem].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @"
                                       and [GsmMessage].[Date] >= '" + begin.ToString() + @"'
                                       and [GsmMessage].[Date] <= '" + end.ToString() + @"')
                               where [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @"
                                  and [GsmItem].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Build] where [GsmID] = [GsmItem].[Unique]))
                           )
                           select count([Unique]) as [Total]
                           from [Merged];";

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
                               select [FullPound].[WoodID] from [FullPound]
                               inner join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID] and [WoodJoin].[State] =0)
                               where [FullPound].[State] =0
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
                               inner join [Wood] on ([FP].[WoodID] = [Wood].[Unique] and [Wood].[State] =0)
                               left outer join [Account] on ([FP].[Operator] = [Account].[Unique]
                                   and [Account].[State] =0)
                               where [FP].[State] =0
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
                                   and [Barrier].[State] =0)
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                               from [FullPound]
                               inner join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                       and [WoodJoin].[State] =0)
                               where [FullPound].[State] =0
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
                                   and [Wood].[State] =0)
                               left outer join [Account] on ([FP].[Operator] = [Account].[Unique]
                                   and [Account].[State] =0)
                               where [FP].[State] =0
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
                                   and [Barrier].[State] =0)
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                               from [FullPound]
                               inner join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                       and [WoodJoin].[State] =0)
                               where [FullPound].[State] =0
                                  and [FullPound].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FullPound].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FullPound].[License] like '%" + license + @"%'
                           ),
                           [Merged] as
                           (
                               select [FullPound].[WoodID], [Wood].[Unique]
                               from [FullPound]
                               inner join [Wood] on ([FullPound].[WoodID] = [Wood].[Unique]
                                   and [Wood].[State] =0)
                               where [FullPound].[State] =0
                                  and [FullPound].[WeighTime] >= '" + begin.ToString() + @"'
                                  and [FullPound].[WeighTime] <= '" + end.ToString() + @"'
                                  and [FullPound].[License] like '%" + license + @"%'
                                  and (not exists (select * from [Filtered] where [WoodID] = [FullPound].[WoodID]))
                           )
                           select count([Unique]) as [Total] from [Merged];";

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
            string sql = @"with [Filtered] as
                           (
                               select [bangid], [bangCid], [jWeight], [Bang_Time], [carCID], [carUser], [firstBangUser], [breedName], [userXHName]
                               from [WoodBang]
                               where [IsJoined] = 0 and [IsFiltered] = " + isFiltered.ToDatabase() + @"
                                     and [Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Bang_Time]) as [Number], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Build] as
                           (
                               select [bangid]
                               from [WoodBang]
                               where [IsJoined] = 0 and [IsFiltered] = " + isFiltered.ToDatabase() + @"
                               and [Bang_Time] >= " + date.ToDateBegin().ToDatabase() + @"
                           )
                           select count([bangid]) as [Total] from [Build];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }


        /// <summary>
        /// 同步量方数据到ERP
        /// </summary>
        /// <param name="entity"></param>
        public void SyncVolumeDataForERP(Entity entity, string ConnectionStringName, string OrgID, string CtrlType)
        {
            string BangID = entity.GetValue("BangID").TryString();
            string BangCID = entity.GetValue("BangCID").TryString();
            if (CtrlType == "JOIN")
            {
                EntityCollection collection = getVolumeJoinData(BangID);
                foreach (Entity item in collection)
                {
                    //首磅数据
                    string bangCid = item.GetValue("bangCid").TryString();
                    decimal FullVolume = item.GetValue("FullVolume").TryDecimal();
                    string Area = item.GetValue("Area").TryString();
                    string License = item.GetValue("License").TryString();
                    string Tree = item.GetValue("Tree").TryString();
                    string Driver = item.GetValue("Driver").TryString();
                    string Supplier = item.GetValue("Supplier").TryString();
                    string WeighTime = item.GetValue("WeighTime").TryString();
                    string Description = item.GetValue("Description").TryString();
                    string FirstOptName = string.Empty;
                    if (Description != "")
                    {
                        FirstOptName = Helper.Deserialize(Description).GetValue("Name").TryString();
                    }
                    //回皮数据
                    decimal EmptyVolume = item.GetValue("EmptyVolume").TryDecimal();
                    decimal HandVolume = item.GetValue("HandVolume").TryDecimal();
                    decimal RebateVolume = item.GetValue("RebateVolume").TryDecimal();
                    string BackWeighTime = item.GetValue("BackWeighTime").TryString();
                    string EDescription = item.GetValue("EDescription").TryString();
                    string LastOptName = string.Empty;
                    if (EDescription != "")
                    {
                        LastOptName = Helper.Deserialize(EDescription).GetValue("Name").TryString();
                    }

                    string strSql = @"DECLARE @FID BIGINT
                                    SET @FID=(SELECT FID FROM t_TC_DiBangDan WHERE FSOURCENO='" + bangCid + "' AND F_ZHY_ORGID='" + OrgID + @"')
                                    UPDATE t_TC_DiBangDan SET F_LF_Area='" + Area + "',F_LF_License='" + License + @"',
                                    F_LF_Tree='" + Tree + "',F_LF_Driver='" + Driver + "',F_LF_Supplier='" + Supplier + @"',
                                    F_LF_WeighTime='" + WeighTime + "',F_LF_FirstOptName='" + FirstOptName + @"',
                                    F_LF_BackWeighTime='" + BackWeighTime + "',F_LF_LastOptName='" + LastOptName + @"'
                                    WHERE FID=@FID
                                    UPDATE t_TC_DiBangDan_Entry SET F_LF_FullVolume=" + FullVolume + ",F_LF_EmptyVolume=" + EmptyVolume + ",F_LF_HandVolume=" + HandVolume + @",
                                    F_LF_RebateVolume=" + RebateVolume + " WHERE FID=@FID";
                    new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
                }
            }
            else
            {
                //断开
                string strSql = @"DECLARE @FID BIGINT
                                    SET @FID=(SELECT FID FROM t_TC_DiBangDan WHERE FSOURCENO='" + BangCID + "' AND F_ZHY_ORGID='" + OrgID + @"')
                                    UPDATE t_TC_DiBangDan SET F_LF_Area='',F_LF_License='',
                                    F_LF_Tree='',F_LF_Driver='',F_LF_Supplier='',F_LF_WeighTime='',F_LF_FirstOptName='',
                                    F_LF_BackWeighTime='',F_LF_LastOptName='' WHERE FID=@FID
                                    UPDATE t_TC_DiBangDan_Entry SET F_LF_FullVolume=0,F_LF_EmptyVolume=0,F_LF_HandVolume=0,F_LF_RebateVolume=0 WHERE FID=@FID";
                new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 获取跟地磅关联的量方数据
        /// </summary>
        /// <param name="bangid"></param>
        /// <returns></returns>
        private EntityCollection getVolumeJoinData(string bangid)
        {
            string strSql = @"SELECT WB.[bangid],WB.[bangCid],WB.[Bang_Time], WB.[carCID], WB.[carUser], WB.[breedName]
                            , WB.[userXHName],FP.[FullVolume], FP.[Area],FP.[License], FP.[Tree], FP.[Driver]
                            , FP.[Supplier], FP.[WeighTime],FP.[Operator],AT.[Description]
                            , EP.EmptyVolume,EP.HandVolume,EP.RebateVolume,EP.BackWeighTime,AT1.[Description] AS EDescription
                            FROM [WoodJoin] WJ INNER JOIN [WoodBang] WB ON (WJ.[BangID] = WB.[bangid])
                            INNER JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            LEFT JOIN [Account] AT ON (AT.[Unique]=FP.Operator AND AT.[State]=0)
                            LEFT JOIN [EmptyPound] EP ON (EP.WoodID=FP.WoodID AND EP.[State]=0)
                            LEFT JOIN [Account] AT1 ON (AT1.[Unique]=EP.Operator AND AT1.[State]=0)
                            WHERE WJ.[State] = 0  AND WB.IsJoined = 1 AND WB.[bangid]='" + bangid + "'";
            return Execute.GetEntityCollection(strSql);
        }

    }
}
