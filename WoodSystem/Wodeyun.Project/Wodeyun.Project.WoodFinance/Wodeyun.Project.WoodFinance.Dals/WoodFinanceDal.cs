using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodFinance.Dals
{
    public class WoodFinanceDal : CommonDal
    {
        public WoodFinanceDal()
        { }

        public WoodFinanceDal(SqlTransaction transaction)
            : base(transaction)
        { }

        /// <summary>
        /// 分页查询料厂民工卸木片汇总表
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="people">卸货人</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport02BySearchWithPaging(string startDate, string endDate, int start, int length, string people)
        {
            string sql = @"with [Filtered] as
                           (
                               select [WoodJoin].[BangID]
                                      , (
                                          select distinct [UnLoadPeople] + '，'
                                          from [Factory]
                                          where [WoodID] = [F1].[WoodID]
                                             and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                             and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                          for xml path('')
                                      ) as [UnLoadPeople]
                               from [FullPound] as [F1]
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                    and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                    and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [F1].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                     and [F1].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                     and ([F1].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                     and [F1].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @") 
                           ),
                           [Build] as
                           (
                              select left([Filtered].[UnLoadPeople], len([Filtered].[UnLoadPeople]) - 1) as [UnLoadPeople]
                                      , [WoodBang].[jWeight]
                              from [Filtered]
                              left outer join [WoodBang] on ([Filtered].[BangID] = [WoodBang].[bangid])
                           ),
                           [Merged] as
                           (
                               select [UnLoadPeople]
                                      , sum([jWeight]) as [TotalWeight]
                              from [Build]
                              group by [UnLoadPeople]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [TotalWeight] desc) as [Number], *
                               from [Merged]
                               where 1 = 1
                                   " + (people == "" ? "" : string.Format(" and [UnLoadPeople] like '%{0}%'", people)) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [WoodJoin].[BangID]
                                      , (
                                          select distinct [UnLoadPeople] + '，'
                                          from [Factory]
                                          where [WoodID] = [F1].[WoodID]
                                             and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                             and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                          for xml path('')
                                      ) as [UnLoadPeople]
                               from [FullPound] as [F1]
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                    and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                    and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [F1].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                     and [F1].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                     and ([F1].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                     and [F1].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @") 
                           ),
                           [Build] as
                           (
                              select left([Filtered].[UnLoadPeople], len([Filtered].[UnLoadPeople]) - 1) as [UnLoadPeople]
                              from [Filtered]
                           ),
                           [Merged] as
                           (
                               select [UnLoadPeople]
                               from [Build]
                               group by [UnLoadPeople]
                           )
                           select count(*) as [Total]
                           from [Merged]
                           where 1 = 1
                               " + (people == "" ? ";" : string.Format(" and [UnLoadPeople] like '%{0}%';", people));

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 分页查询料厂民工卸木片报表
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="people">卸货人</param>
        /// <returns>结果集</returns>
        public EntityCollection GetReport01BySearchWithPaging(string startDate, string endDate, int start, int length, string people)
        {
            string sql = @"with [Filtered] as
                           (
                               select [F1].[License], [F1].[Tree], [F1].[WeighTime], [WoodJoin].[BangID]
                                      , (
                                          select distinct [UnLoadPalce] + '，'
                                          from [Factory]
                                          where [WoodID] = [F1].[WoodID]
                                             and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                             and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                          for xml path('')
                                      ) as [UnLoadPalce]
                                      , (
                                          select distinct [UnLoadPeople] + '，'
                                          from [Factory]
                                          where [WoodID] = [F1].[WoodID]
                                             and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                             and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                          for xml path('')
                                      ) as [UnLoadPeople]
                               from [FullPound] as [F1]
                               left outer join [WoodJoin] on ([F1].[WoodID] = [WoodJoin].[WoodID]
                                   and [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [F1].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                     and [F1].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                     and ([F1].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                     and [F1].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @") 
                           ),
                           [Build] as
                           (
                              select [Filtered].[WeighTime], [Filtered].[License], [Filtered].[Tree]
                                      , left([Filtered].[UnLoadPalce], len([Filtered].[UnLoadPalce]) - 1) as [UnLoadPalce]
                                      , left([Filtered].[UnLoadPeople], len([Filtered].[UnLoadPeople]) - 1) as [UnLoadPeople]
                                      , [WoodBang].[jWeight]
                              from [Filtered]
                              left outer join [WoodBang] on ([Filtered].[BangID] = [WoodBang].[bangid])
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [WeighTime] asc) as [Number], *
                               from [Build]
                               where 1 = 1
                                   " + (people == "" ? "" : string.Format(" and [UnLoadPeople] like '%{0}%'", people)) + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [F1].[Unique]
                                      , (
                                          select distinct [UnLoadPeople] + '，'
                                          from [Factory]
                                          where [WoodID] = [F1].[WoodID]
                                             and [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                             and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                          for xml path('')
                                      ) as [UnLoadPeople]
                               from [FullPound] as [F1]
                               where [F1].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                     and [F1].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                     and ([F1].[WeighTime] >= " + startDate.ToDateBegin().ToDatabase() + @"
                                     and [F1].[WeighTime] <= " + endDate.ToDateEnd().ToDatabase() + @") 
                           ),
                           [Build] as
                           (
                              select [Unique]
                                      , left([UnLoadPeople], len([UnLoadPeople]) - 1) as [UnLoadPeople]
                              from [Filtered]
                           )
                           select count(*) as [Total]
                           from [Build]
                           where 1 = 1
                               " + (people == "" ? ";" : string.Format(" and [UnLoadPeople] like '%{0}%';", people));

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

    }
}
