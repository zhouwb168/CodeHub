using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodDataList.Dals
{
    public class WoodDataListDal : CommonDal
    {
        public WoodDataListDal()
        { }

        public WoodDataListDal(SqlTransaction transaction)
            : base(transaction)
        { }

        /// <summary>
        /// 分页获取化验室原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="key">料厂密码</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport04BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key)
        {
            string sql = @"with [Filtered] as
                           (
                               select distinct [FullPound].[WoodID], [FullPound].[License], [FullPound].[Tree], [FullPound].[Supplier]
                                      , [FullPound].[WeighTime]
                                      , [WoodLaboratory].[Water], [WoodLaboratory].[RebateWater], [WoodLaboratory].[Skin]
                                      , [WoodLaboratory].[RebateSkin], [WoodLaboratory].[Scrap], [WoodLaboratory].[RebateScrap]
                                      , [WoodLaboratory].[Bad], [WoodLaboratory].[Greater], [WoodLaboratory].[Less]
                                      , [WoodLaboratory].[CheckTime], [WoodLaboratory].[Operator]
                                      , [WoodLaboratory].[IsConfirmed], [WoodLaboratory].[Confirmor]
                                      , [WoodUnPackBox].[Number] as [CheckNumber], [WoodLaboratory].[DeductVolume]
                                      , [WoodPackBox].[Operator] as [PackBoxOperator]
                                      , [WoodJoin].[BangID]
                               from [FullPound]
                               left outer join [WoodLaboratory] on ([FullPound].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[State] =0)
                               left outer join [WoodUnPackBox] on ([FullPound].[WoodID] = [WoodUnPackBox].[WoodID]
                                   and [WoodUnPackBox].[State] =0)
                               left outer join [WoodPackBox] on ([FullPound].[WoodID] = [WoodPackBox].[WoodID]
                                   and [WoodPackBox].[State] =0)
                               left outer join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] =0)
                               where [FullPound].[State] =0
                                   and [FullPound].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [FullPound].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [FullPound].[License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [FullPound].[Supplier] = '{0}'", supplier)) + @"
                           ),
                           [Build] as
                           (
                              select distinct [Filtered].*
                                     , [Account].[Description] as [LaboratoryPeople]
                                     , [Factory].[Key], [Factory].[Sampler], [Factory].[Deduct], [Factory].[Remark]
                                     , [WoodBang].[jWeight], [WoodBang].[Bang_Time]
                              from [Filtered]
                              left outer join [Account] on ([Filtered].[Operator] = [Account].[Unique]
                                   and [Account].[State] =0)
                              left outer join [Factory] on ([Filtered].[WoodID] = [Factory].[WoodID]
                                   and [Filtered].[PackBoxOperator] = [Factory].[Operator]
                                   and [Factory].[State] =0)
                              left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                           ),
                           [Merged] as
                           (
                               select distinct [Build].* , [Account].[Description] as [ConfirmePeople]
                               from [Build]
                               left outer join [Account] on ([Build].[Confirmor] = [Account].[Unique]
                                   and [Account].[State] =0)
                              where 1 = 1
                                   " + (key == "" ? "" : string.Format(" and [Build].[Key] = '{0}'", key)) + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [WeighTime] asc) as [Number], *
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[WoodID]
                                      , [WoodPackBox].[Operator] as [PackBoxOperator]
                               from [FullPound]
                               left outer join [WoodPackBox] on ([FullPound].[WoodID] = [WoodPackBox].[WoodID]
                                   and [WoodPackBox].[State] =0)
                               where [FullPound].[State] =0
                                   and [FullPound].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [FullPound].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [FullPound].[License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [FullPound].[Supplier] = '{0}'", supplier)) + @"
                           ),
                           [Build] as
                           (
                              select [Filtered].* , [Factory].[Key] from [Filtered]
                              left outer join [Factory] on ([Filtered].[WoodID] = [Factory].[WoodID]
                                   and [Filtered].[PackBoxOperator] = [Factory].[Operator]
                                   and [Factory].[State] =0)
                           ),
                           [Merged] as
                           (
                               select *
                               from [Build]
                               where 1 = 1
                                   " + (key == "" ? "" : string.Format(" and [Key] = '{0}'", key)) + @"
                           )
                           select count(*) as [Total]
                           from [Merged];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 分页获取料厂原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="key">料厂密码</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport03BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string key)
        {
            string sql = @"with [Filtered] as
                           (
                               select [FullPound].[WoodID], [FullPound].[License], [FullPound].[Tree], [FullPound].[Supplier]
                                      , [FullPound].[WeighTime]
                                      , [Factory].[Unique], [Factory].[UnLoadPalce], [Factory].[UnLoadPeople], [Factory].[Key]
                                      , [Factory].[Sampler], [Factory].[Water], [Factory].[Skin], [Factory].[Scrap]
                                      , [Factory].[Deduct], [Factory].[Remark], [Factory].[SampleTime], [Factory].[Operator]
                                      , [WoodJoin].[BangID]
                               from [FullPound]
                               left outer join [Factory] on ([FullPound].[WoodID] = [Factory].[WoodID]
                                   and [Factory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Factory].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [FullPound].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [FullPound].[License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [FullPound].[Supplier] = '{0}'", supplier)) + @"
                           ),
                           [Build] as
                           (
                              select distinct [Filtered].*
                                     , [WoodBang].[jWeight], [WoodBang].[Bang_Time]
                                     , [WoodPackBox].[Box]
                                     , [Account].[Description]
                              from [Filtered]
                              left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                              left outer join [WoodPackBox] on ([Filtered].[WoodID] = [WoodPackBox].[WoodID]
                                   and [Filtered].[Operator] = [WoodPackBox].[Operator]
                                   and [WoodPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [Account] on ([Filtered].[Operator] = [Account].[Unique]
                                   and [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Account].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                              where 1 = 1
                                   " + (key == "" ? "" : string.Format(" and [Filtered].[Key] = '{0}'", key)) + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [WeighTime] asc) as [Number], *
                               from [Build]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [FullPound].[Unique]
                                      , [Factory].[Key]
                               from [FullPound]
                               left outer join [Factory] on ([FullPound].[WoodID] = [Factory].[WoodID]
                                   and [Factory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Factory].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [FullPound].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [FullPound].[License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [FullPound].[Supplier] = '{0}'", supplier)) + @"
                           ),
                           [Build] as
                           (
                               select *
                               from [Filtered]
                               where 1 = 1
                                   " + (key == "" ? "" : string.Format(" and [Key] = '{0}'", key)) + @"
                           )
                           select count(*) as [Total]
                           from [Build];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 分页获取地磅原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport02BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license)
        {
            string sql = @"with [Filtered] as
                           (
                               select [FullPound].[Unique], ISNULL([FullPound].[FullVolume], 0) as [FullVolume]
                                      , [FullPound].[License], [FullPound].[Area], [FullPound].[Tree], [FullPound].[Driver]
                                      , [FullPound].[Supplier], [FullPound].[WeighTime]
                                      , [Account].[Description] as [PeopleOfFullPound]
                                      , ISNULL([EmptyPound].[EmptyVolume], 0) as [EmptyVolume]
                                      , ISNULL([EmptyPound].[RebateVolume], 0) as [RebateVolume]
                                      , ISNULL([EmptyPound].[HandVolume], 0) as [HandVolume], [EmptyPound].[BackWeighTime]
                                      , [EmptyPound].[Operator], [WoodJoin].[BangID]
                               from [FullPound]
                               left outer join [Account] on ([FullPound].[Operator] = [Account].[Unique]
                                   and [Account].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left outer join [EmptyPound] on ([FullPound].[WoodID] = [EmptyPound].[WoodID]
                                   and [EmptyPound].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left outer join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [FullPound].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [FullPound].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [FullPound].[License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [FullPound].[Supplier] = '{0}'", supplier)) + @"
                           ),
                           [Build] as
                           (
                              select distinct [Filtered].[Unique], [Filtered].[FullVolume], [Filtered].[License], [Filtered].[Area]
                                     , [Filtered].[Tree], [Filtered].[Driver], [Filtered].[Supplier], [Filtered].[WeighTime]
                                     , [Filtered].[PeopleOfFullPound], [Filtered].[EmptyVolume], [Filtered].[HandVolume], [Filtered].[RebateVolume]
                                     , [Filtered].[BackWeighTime], [DiscVolume] = ((CASE [Filtered].[HandVolume] WHEN 0 THEN [Filtered].[FullVolume] ELSE [Filtered].[HandVolume] END)-[Filtered].[EmptyVolume]-[Filtered].[RebateVolume])
                                     , [MoreTanVolume] = ([Filtered].[HandVolume] - ([Filtered].[FullVolume] - [Filtered].[EmptyVolume]))
                                     , [Account].[Description] as [PeopleOfEmptyPound]
                                     , [WoodBang].[jWeight], [WoodBang].[Bang_Time]
                              from [Filtered]
                              left outer join [Account] on ([Filtered].[Operator] = [Account].[Unique]
                                   and [Account].[State] = " + StateEnum.Default.ToDatabase() + @")
                              left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Bang_Time] asc) as [Number], *
                               from [Build]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [Unique]
                               from [FullPound]
                               where [State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [Supplier] = '{0}'", supplier)) + @"
                           )
                           select count(*) as [Total] from [Filtered];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 分页获取移动检查站原始数据清单
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="place">移动检查站名称</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport01BySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string place)
        {
            string sql = @"with [Filtered] as
                           (
                               select [B1].[Unique]
                                      , (
                                           select [ImageNumber] = count([Unique])
                                           from [WoodCarPhoto]
                                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                                 and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                                 and [BarrierID] = [B1].[Unique]
                                      ) as [PhotoNumber]
                                      , [B1].[Place], [B1].[License], [B1].[Area], [B1].[TimeOfStation], [B1].[GPS]
                                      , [Wood].[Unique] as [WoodID], [Wood].[ArriveDate]
                                      , [Account].[Description]
                               from [Barrier] as [B1]
                               left outer join [Wood] on ([B1].[Unique] = [Wood].[BarrierID]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [Account] on ([B1].[Operator] = [Account].[Unique]
                                   and [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Account].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [B1].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [B1].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   and [B1].[TimeOfStation] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [B1].[TimeOfStation] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (place == "" ? "" : string.Format(" and [B1].[Place] like '%{0}%'", place)) + @"
                                   " + (license == "" ? "" : string.Format(" and [B1].[License] like '%{0}%'", license)) + @"
                           ),
                           [Build] as
                           (
                              select [Filtered].*, [FullPound].[Tree], [FullPound].[Supplier]
                              from [Filtered]
                              left outer join [FullPound] on ([Filtered].[WoodID] = [FullPound].[WoodID]
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [TimeOfStation] asc) as [Number], *
                               from [Build]
                               where 1 = 1
                                   " + (supplier == "" ? "" : string.Format(" and [Supplier] = '{0}'", supplier)) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [Barrier].[Unique]
                                      , [Wood].[Unique] as [WoodID]
                               from [Barrier]
                               left outer join [Wood] on ([Barrier].[Unique] = [Wood].[BarrierID]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [Barrier].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Barrier].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   and [Barrier].[TimeOfStation] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [Barrier].[TimeOfStation] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (place == "" ? "" : string.Format(" and [Barrier].[Place] like '%{0}%'", place)) + @"
                                   " + (license == "" ? "" : string.Format(" and [Barrier].[License] like '%{0}%'", license)) + @"
                           ),
                           [Build] as
                           (
                              select [Filtered].*, [FullPound].[Supplier]
                              from [Filtered]
                              left outer join [FullPound] on ([Filtered].[WoodID] = [FullPound].[WoodID]
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           )
                           select count(*) as [Total]
                           from [Build]
                           where 1 = 1
                                 " + (supplier == "" ? "" : string.Format(" and [Supplier] = '{0}'", supplier)) + ";";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 获取数据统计
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplier"></param>
        /// <param name="license"></param>
        /// <returns></returns>
        public Entity getDataStatistics(string startDate, string endDate, string supplier, string license)
        {
            string sql = @"with [Filtered] as
                           (
                               select [FullPound].[Unique], ISNULL([FullPound].[FullVolume], 0) as [FullVolume]
                                      , [FullPound].[License], [FullPound].[Area], [FullPound].[Tree], [FullPound].[Driver]
                                      , [FullPound].[Supplier], [FullPound].[WeighTime]
                                      , [Account].[Description] as [PeopleOfFullPound]
                                      , ISNULL([EmptyPound].[EmptyVolume], 0) as [EmptyVolume]
                                      , ISNULL([EmptyPound].[RebateVolume], 0) as [RebateVolume]
                                      , ISNULL([EmptyPound].[HandVolume], 0) as [HandVolume], [EmptyPound].[BackWeighTime]
                                      , [EmptyPound].[Operator], [WoodJoin].[BangID]
                               from [FullPound]
                               left outer join [Account] on ([FullPound].[Operator] = [Account].[Unique]
                                   and [Account].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left outer join [EmptyPound] on ([FullPound].[WoodID] = [EmptyPound].[WoodID]
                                   and [EmptyPound].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left outer join [WoodJoin] on ([FullPound].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [FullPound].[State] = " + StateEnum.Default.ToDatabase() + @"
                                   and [FullPound].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @" 
                                   and [FullPound].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @"
                                   " + (license == "" ? "" : string.Format(" and [FullPound].[License] like '%{0}%'", license)) + @"
                                   " + (supplier == "" ? "" : string.Format(" and [FullPound].[Supplier] = '{0}'", supplier)) + @"
                           ),
                           [Build] as
                           (
                              select distinct [Filtered].[Unique], [Filtered].[FullVolume], [Filtered].[License], [Filtered].[Area]
                                     , [Filtered].[Tree], [Filtered].[Driver], [Filtered].[Supplier], [Filtered].[WeighTime]
                                     , [Filtered].[PeopleOfFullPound], [Filtered].[EmptyVolume], [Filtered].[HandVolume], [Filtered].[RebateVolume]
                                     , [Filtered].[BackWeighTime], [DiscVolume] = ((CASE [Filtered].[HandVolume] WHEN 0 THEN [Filtered].[FullVolume] ELSE [Filtered].[HandVolume] END)-[Filtered].[EmptyVolume]-[Filtered].[RebateVolume])
                                     , [MoreTanVolume] = ([Filtered].[HandVolume] - ([Filtered].[FullVolume] - [Filtered].[EmptyVolume]))
                                     , [Account].[Description] as [PeopleOfEmptyPound]
                                     , [WoodBang].[jWeight], [WoodBang].[Bang_Time]
                              from [Filtered]
                              left outer join [Account] on ([Filtered].[Operator] = [Account].[Unique]
                                   and [Account].[State] = " + StateEnum.Default.ToDatabase() + @")
                              left outer join [WoodBang] on [Filtered].[BangID] = [WoodBang].[bangid]
                           )
                           select SUMFullVolume=SUM(FullVolume),SUMEmptyVolume=SUM(EmptyVolume)
                           ,SUMDiscVolume=SUM(DiscVolume),SUMRebateVolume=SUM(RebateVolume) from [Build]";
            return this.Execute.GetEntity(sql);
        }
    }
}
