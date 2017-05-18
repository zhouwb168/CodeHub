using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodCarPhoto.Dals
{
    public class WoodCarPhotoDal : CommonDal
    {
        public WoodCarPhotoDal()
        { }

        public WoodCarPhotoDal(SqlTransaction transaction)
            : base(transaction)
        { }

        /// <summary>
        /// 根据记录ID获取该记录关联的照片列表
        /// </summary>
        /// <param name="barrierID">关联的关卡编号(来自于Barrier表）</param>
        /// <returns>结果集</returns>
        public EntityCollection GetCarPhotoListByRecordID(int barrierID)
        {
            string sql = @"select [Unique], [GPS], [PhotoTime]
                           from [WoodCarPhoto]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                 and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                 and [BarrierID] = " + barrierID.ToDatabase() + @"
                           order by [Unique] asc";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = results.Count;

            return results;
        }

        /// <summary>
        /// 分页查询移动检查站的发卡记录
        /// </summary>
        /// <param name="startDate">查询开始日期，格式如：2013-05-23</param>
        /// <param name="endDate">查询结束日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="supplier">货主代码</param>
        /// <param name="license">车牌号</param>
        /// <param name="place">移动检查站名称</param>
        /// <returns>结果集</returns>
        public EntityCollection GetCarPhotoReportBySearchWithPaging(string startDate, string endDate, int start, int length, string supplier, string license, string place)
        {
            string sql = @"with [Filtered] as
                           (
                               select [B1].[Unique], [B1].[Place], [B1].[License], [B1].[Area], [B1].[TimeOfStation], [B1].[GPS]
                                      , [Check].[CheckDate]
                                      , [Wood].[Unique] as [WoodID]
                                      , [Account].[Description]
                                      , (
                                           select [ImageNumber] = count([Unique])
                                           from [WoodCarPhoto]
                                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                                 and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                                 and [BarrierID] = [B1].[Unique]
                                      ) as [PhotoNumber]
                               from [Barrier] as [B1]
                               left outer join [Check] on ([B1].[Unique] = [Check].[BarrierID]
                                   and [Check].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Check].[State] <> " + StateEnum.Updated.ToDatabase() + @")
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
                              select [Filtered].*
                                     , [FullPound].[Tree], [FullPound].[Supplier]
                              from [Filtered]
                              left outer join [FullPound] on ([Filtered].[WoodID] = [FullPound].[WoodID]
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] asc) as [Number], *
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
    }
}
