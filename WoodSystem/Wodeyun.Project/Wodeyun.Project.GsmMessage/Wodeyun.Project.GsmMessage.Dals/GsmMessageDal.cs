using System.Collections;
using System.Data.SqlClient;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.GsmMessage.Dals
{
    public class GsmMessageDal : CommonDal
    {
        private void Init()
        {
            this.Table = "GsmMessage";
            this.Inserts = "[Unique], [Parent], [Mobile], [Date], [Text], [Remark], [Status], [State], [Version], [Operator], [Log]";
            this.Updates = "[Parent], [Mobile], [Date], [Text], [Remark], [Status]";
            this.Selects = "[Unique], [Parent], [Mobile], [Date], [Text], [Remark], [Status]";
            this.Order = "[Date] desc";
        }

        public GsmMessageDal()
        {
            this.Init();
        }

        public GsmMessageDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        public EntityCollection GetEntitiesWithSupplierNameByDateAndSupplierAndMobileAndStartAndLength(string date, string supplier, string mobile, int start, int length)
        {
            string sql = @"with [Filtered] as
                           (
                               select distinct top 1000 " + this.GetFields(this.Selects, this.Table) + @",
                                   [GsmItem].[Supplier] as [SupplierName] from [" + this.Table + @"]
                               left join [GsmItem] on ([" + this.Table + @"].[Unique] = [GsmItem].[Message]
                                   and [GsmItem].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase() + this.GetWhere(date, supplier, mobile) + " ORDER BY [" + this.Table + @"].[Unique] DESC
                           )
                           SELECT * INTO #T FROM Filtered;
                           WITH [Numbered] as
                           (
                               select row_number() over (order by [Date] desc) as [Number], *
                               from #T
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           select count(*) as [Total] from #T 
                           drop table #T";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        private string GetWhere(string date, string supplier, string mobile)
        {
            string result = string.Empty;

            if (date != string.Empty) result = result + " and ([" + this.Table + "].[Date] >= " + date.ToDateBegin().ToDatabase() + " and [GsmMessage].[Date] <= " + date.ToDateEnd().ToDatabase() + ")";

            if (supplier == "(无)") result = result + " and [GsmItem].[Supplier] is null";
            else if (supplier == "(全部)" || supplier == string.Empty) result = result + string.Empty;
            else result = result + " and [GsmItem].[Supplier] = " + supplier.ToDatabase();

            if (mobile != string.Empty) result = result + " and [" + this.Table + "].[Mobile] = " + mobile.ToDatabase();

            return result;
        }
    }
}