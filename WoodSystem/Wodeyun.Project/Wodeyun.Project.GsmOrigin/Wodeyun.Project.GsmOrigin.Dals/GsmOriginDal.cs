using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.GsmOrigin.Dals
{
    public class GsmOriginDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmOrigin";
            this.Inserts = "[Unique], [Area], [Name], [Alias], [Except], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Area], [Name], [Alias], [Except], [Remark]";
            this.Selects = "[Unique], [Area], [Name], [Alias], [Except], [Remark]";
        }

        public GsmOriginDal()
        {
            this.Init();
        }

        public GsmOriginDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        public EntityCollection GetEntitiesWithAreaNameByStartAndLength(int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by [" + this.Table + "].[Area], [" + this.Table + "].[Unique]) as [Number], " + this.GetFields(this.Selects, this.Table) + @",
                                   [GsmArea].[Name] as [AreaName]
                               from [" + this.Table + @"]
                               left join [GsmArea] on ([" + this.Table + @"].[Area] = [GsmArea].[Unique]
                                   and [GsmArea].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [GsmArea].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Numbered] as
                           (
                               select row_number() over (order by [Unique]) as [Number]
                               from [" + this.Table + @"]
                               where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           )
                           select count(*) as [Total]
                           from [Numbered]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public EntityCollection GetEntitiesWithAreaNameByAreaAndStartAndLength(int area, int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by [" + this.Table + "].[Unique]) as [Number], " + this.GetFields(this.Selects, this.Table) + @",
                                   [GsmArea].[Name] as [AreaName]
                               from [" + this.Table + @"]
                               left join [GsmArea] on ([" + this.Table + @"].[Area] = [GsmArea].[Unique]
                                   and [GsmArea].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [GsmArea].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [" + this.Table + "].[Area] = " + area.ToDatabase() + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Numbered] as
                           (
                               select row_number() over (order by [Unique]) as [Number]
                               from [" + this.Table + @"]
                               where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [Area] = " + area.ToDatabase() + @"
                           )
                           select count(*) as [Total]
                           from [Numbered]";
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }
    }
}