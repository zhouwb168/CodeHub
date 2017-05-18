using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Act.Dals
{
    public class ActDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Act";
            this.Inserts = "[Unique], [Account], [Role], [State], [Version], [Operator], [Log]";
            this.Updates = "[Account], [Role]";
            this.Selects = "[Unique], [Account], [Role]";
        }

        public ActDal()
        {
            this.Init();
        }

        public ActDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        public int UpdateEntitiesByAccount(int account)
        {
            string sql = @"update " + this.Table + @"
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Account] = " + account.ToDatabase();
            return this.Execute.ExecuteNonQuery(sql);
        }

        public EntityCollection GetEntitiesWithFunctionByAccount(int account)
        {
            string sql = @"select distinct [Function].[Unique],
                                  [Function].[Parent],
                                  [Function].[Order],
                                  [Function].[No],
                                  [Function].[Name],
                                  [Function].[Url]
                           from [" + this.Table + @"]
                           left join [Grant] on ([" + this.Table + @"].[Role] = [Grant].[Role]
                               and [Grant].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [Grant].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           left join [Function] on ([Grant].[Function] = [Function].[Unique]
                               and [Function].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [Function].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and [" + this.Table + "].[Account] = " + account.ToDatabase() + @"
                           order by [Function].[Parent], [Function].[Order];
                           select count(distinct [Function].[Unique]) as [Total]
                           from [" + this.Table + @"]
                           left join [Grant] on ([" + this.Table + @"].[Role] = [Grant].[Role]
                               and [Grant].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [Grant].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           left join [Function] on ([Grant].[Function] = [Function].[Unique]
                               and [Function].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [Function].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and [" + this.Table + "].[Account] = " + account.ToDatabase();
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }
    }
}