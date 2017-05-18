using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.GsmMessage.Dals
{
    public class GsmItemDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmItem";
            this.Inserts = "[Unique], [Message], [Supplier], [Tree], [Make], [Area], [Origin], [License], [Driver], [Ship], [Line], [Remark], [State], [Version], [Operator], [Log]";
            this.Updates = "[Message], [Supplier], [Tree], [Make], [Area], [Origin], [License], [Driver], [Ship], [Line], [Remark]";
            this.Selects = "[Unique], [Message], [Supplier], [Tree], [Make], [Area], [Origin], [License], [Driver], [Ship], [Line], [Remark]";
        }

        public GsmItemDal()
        {
            this.Init();
        }

        public GsmItemDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        public EntityCollection GetEntitiesWithAreaUniqueByMessage(int message)
        {
            string sql = @"select " + this.GetFields(this.Selects, this.Table) + @",
                               [GsmArea].[Unique] as [AreaUnique]
                           from [" + this.Table + @"]
                           left join [GsmArea] on ([" + this.Table + @"].[Area] = [GsmArea].[Name]
                                   and [GsmArea].[State] = " + StateEnum.Default.ToDatabase() + @")
                           where [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase() + @"
                           and [" + this.Table + "].[Message] = " + message.ToDatabase() + @";
                           select count(*) as [Total]
                           from [" + this.Table + @"]
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                           and [Message] = " + message.ToDatabase();
            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        public Entity GetEntityByDateAndLicense(string date, string license)
        {
            string sql = @"select [GsmMessage].[Date]
                           from [" + this.Table + @"]
                           left join [GsmMessage] on ([" + this.Table + @"].[Message] = [GsmMessage].[Unique]
                                   and [GsmMessage].[State] = " + StateEnum.Default.ToDatabase() + @")
                           where [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase() + @"
                           and ([GsmMessage].[Date] >= " + date.ToDateBegin().ToDatabase() + " and [GsmMessage].[Date] <= " + date.ToDateEnd().ToDatabase() + @")
                           and [" + this.Table + "].[License] = " + license.ToDatabase();
            return this.Execute.GetEntity(sql);
        }
    }
}