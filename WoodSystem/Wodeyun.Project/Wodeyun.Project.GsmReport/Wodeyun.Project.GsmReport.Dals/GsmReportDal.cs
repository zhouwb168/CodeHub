using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.GsmReport.Dals
{
    public class GsmReportDal : CommonDal
    {
        private string _MessageTable = "GsmMessage";
        private string _ItemTable = "GsmItem";
        private string _MessageSelects = "[Unique], [Mobile], [Date], [Text], [Remark]";
        private string _ItemSelects = "[Unique], [Message], [Supplier], [Tree], [Make], [Area], [Origin], [License], [Driver], [Ship], [Line], [Remark]";

        public GsmReportDal()
        { }

        public GsmReportDal(SqlTransaction transaction)
            : base(transaction)
        { }

        public EntityCollection GetReport01WithSupplierNameByDateAndStartAndLength(string date, int start, int length)
        {
            string sql = @"with [Filtered] as
                           (
                               select distinct " + this.GetFields(this._MessageSelects, this._MessageTable) + @",
                                   [GsmItem].[Supplier] as [SupplierName]
                               from [" + this._MessageTable + @"]
                               left join [GsmItem] on ([" + this._MessageTable + @"].[Unique] = [GsmItem].[Message]
                                   and [GsmItem].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [GsmItem].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._MessageTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._MessageTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [" + this._MessageTable + @"].[Status] = 0
                               and ([" + this._MessageTable + "].[Date] >= " + date.ToDateBegin().ToDatabase() + " and [" + this._MessageTable + "].[Date] <= " + date.ToDateEnd().ToDatabase() + @")
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Date]) as [Number], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._MessageTable + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and [" + this._MessageTable + @"].[Status] = 0
                           and ([" + this._MessageTable + "].[Date] >= " + date.ToDateBegin().ToDatabase() + " and [" + this._MessageTable + "].[Date] <= " + date.ToDateEnd().ToDatabase() + ")";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetReport02WithMessageByMonthAndStartAndLength(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @",
                                   [GsmMessage].[Date] as [MessageDate],
                                   [GsmMessage].[Remark] as [MessageRemark]
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [MessageDate] desc) as [Number], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this._ItemTable + @"]
                           left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                   and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetReport03ByMonthAndSupplierAndStartAndLength(string month, string supplier, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                               and [" + this._ItemTable + "].[Supplier] = " + supplier.ToDatabase() + @"
                           ),
                           [Merged] as
                           (
                               select count(*) as [Count], (
                                       select distinct [Tree] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Origin] = [Filtered].[Origin]
                                       and [License] = [Filtered].[License]
                                       for xml path('')
                                   ) as [Trees], [Origin], [License], (
                                       select distinct [Driver] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Origin] = [Filtered].[Origin]
                                       and [License] = [Filtered].[License]
                                       for xml path('')
                                   ) as [Drivers], (
                                       select distinct [Line] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Origin] = [Filtered].[Origin]
                                       and [License] = [Filtered].[License]
                                       for xml path('')
                                   ) as [Lines]
                                from [Filtered]
                                group by [Origin], [License]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Origin], [Count] desc) as [Number],
                                   row_number() over (partition by [Origin] order by [Count] desc) as [No],
                                   [Count],
                                   left([Trees], len([Trees]) - 1) as [Trees],
                                   [Origin],
                                   [License],
                                   left([Drivers], len([Drivers]) - 1) as [Drivers],
                                   left([Lines], len([Lines]) - 1) as [Lines]
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                               and [" + this._ItemTable + "].[Supplier] = " + supplier.ToDatabase() + @"
                           ),
                           [Merged] as
                           (
                               select count(*) as [Count]
                               from [Filtered]
                               group by [Origin], [License]
                           )
                           select count(*) as [Total]
                           from [Merged]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetReport04ByMonthAndStartAndLength(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [Supplier], (
                                       select distinct [Tree] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Supplier] = [Filtered].[Supplier]
                                       and [Origin] = [Filtered].[Origin]
                                       for xml path('')
                                   ) as [Trees], [Origin], count(*) as [Count]
                                from [Filtered]
                                group by [Supplier], [Origin]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Supplier], [Count] desc) as [Number],
                                   [Supplier],
                                   row_number() over (partition by [Supplier] order by [Count] desc) as [No],
                                   left([Trees], len([Trees]) - 1) as [Trees],
                                   [Origin],
                                   [Count]
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select count(*) as [Count]
                               from [Filtered]
                               group by [Supplier], [Origin]
                           )
                           select count(*) as [Total]
                           from [Merged]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetReport05ByMonthAndStartAndLength(string month, int start, int length)
        {
            string begin = (month + "-01").ToDateBegin();
            string end = (month + "-01").ToDateTime().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [Area], (
                                       select distinct [Tree] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Area] = [Filtered].[Area]
                                       for xml path('')
                                   ) as [Trees], count(*) as [Count]
                                from [Filtered]
                                group by [Area]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Count] desc) as [Number],
                                   [Area],
                                   left([Trees], len([Trees]) - 1) as [Trees],
                                   [Count]
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select count(*) as [Count]
                               from [Filtered]
                               group by [Area]
                           )
                           select count(*) as [Total]
                           from [Merged]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetReport06ByYearAndStartAndLength(string year, int start, int length)
        {
            string begin = (year + "-01-01").ToDateBegin();
            string end = (year + "-01-01").ToDateTime().AddYears(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [Supplier], (
                                       select distinct [Tree] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Supplier] = [Filtered].[Supplier]
                                       for xml path('')
                                   ) as [Trees], count(*) as [Count]
                                from [Filtered]
                                group by [Supplier]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Count] desc) as [Number],
                                   [Supplier],
                                   left([Trees], len([Trees]) - 1) as [Trees],
                                   [Count], (
                                       select sum([Count]) from [Merged]
                                   ) as [Sum]
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select count(*) as [Count]
                               from [Filtered]
                               group by [Supplier]
                           )
                           select count(*) as [Total]
                           from [Merged]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetReport07ByYearAndStartAndLength(string year, int start, int length)
        {
            string begin = (year + "-01-01").ToDateBegin();
            string end = (year + "-01-01").ToDateTime().AddYears(1).AddDays(-1).ToString("yyyy-MM-dd").ToDateEnd();
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select [Area], (
                                       select distinct [Tree] + ','
                                       from (select * from [Filtered]) [Table]
                                       where [Area] = [Filtered].[Area]
                                       for xml path('')
                                   ) as [Trees], count(*) as [Count]
                                from [Filtered]
                                group by [Area]
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Count] desc) as [Number],
                                   [Area],
                                   left([Trees], len([Trees]) - 1) as [Trees],
                                   [Count],(
                                       select sum([Count]) from [Merged]
                                   ) as [Sum]
                               from [Merged]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select " + this.GetFields(this._ItemSelects, this._ItemTable) + @"
                               from [" + this._ItemTable + @"]
                               left join [GsmMessage] on ([" + this._ItemTable + @"].[Message] = [GsmMessage].[Unique]
                                       and [GsmMessage].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                       and [GsmMessage].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this._ItemTable + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this._ItemTable + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and ([GsmMessage].[Date] >= " + begin.ToDatabase() + " and [GsmMessage].[Date] <= " + end.ToDatabase() + @")
                           ),
                           [Merged] as
                           (
                               select count(*) as [Count]
                               from [Filtered]
                               group by [Area]
                           )
                           select count(*) as [Total]
                           from [Merged]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }
    }
}
