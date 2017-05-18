using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Verificate.Dals
{
    public class VerificateDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Verificate";
            this.Inserts = "[Unique], [Account], [Link], [Value], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Account], [Link], [Value], [Remark]";
            this.Selects = "[Unique], [Account], [Link], [Value], [Remark]";
        }

        public VerificateDal()
        {
            this.Init();
        }

        public VerificateDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        public Entity GetEntityByIdAndLink(string id, int link)
        {
            string sql = @"select [Account].[Unique], [" + this.Table + @"].[Value]
                           from [Account]
                           left join [Verificate] on ([Account].[Unique] = [" + this.Table + @"].[Account]
                               and [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           where [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [Account].[Id] = " + id.ToDatabase() + @"
                           and [Verificate].[Link] = " + link.ToDatabase();
            return this.Execute.GetEntity(sql);
        }

        public EntityCollection GetEntitiesWithAccountAndLinkNameByStartAndLength(int start, int length)
        {
            string sql = @"with [Numbered] as
                           (
                               select row_number() over (order by [Account].[Id]) as [Number],
                                   [Account].[Unique] as [AccountUnique],
                                   [Account].[Id] as [AccountId],
                                   [Account].[Description] as [AccountDescription],
                                   [Link].[Name] as [LinkName],
                                   [Verificate].*
                               from [Account]
                               left join [Verificate] on ([Account].[Unique] = [" + this.Table + @"].[Account]
                                   and [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left join [Link] on ([Verificate].[Link] = [Link].[Unique]
                                   and [Link].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Link].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [Account].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [Account].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total]
                           from [Account]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase();
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }
    }
}