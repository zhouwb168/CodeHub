using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.Recover.Dals
{
    public class RecoverDal : CommonDal
    {
        private void Init()
        {
            this.Table = "Recover";
            this.Inserts = "[Unique], [WoodID], [RecoverTime], [Operator], [State], [Version], [Log]";
            this.Updates = "";
            this.Selects = "[Unique], [WoodID], [RecoverTime]";
            this.Order = "[Unique] desc";
        }

        public RecoverDal()
        {
            this.Init();
        }

        public RecoverDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="start">记录行开始索引号</param>
        /// <param name="length">记录长度</param>
        /// <param name="operatorID">操作员的身份ID</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            int kk = operatorID;
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this.Selects, this.Table) + @"
                                  , [Wood].[CardNumber], [FullPound].[License]
                               from [" + this.Table + @"]
                               inner join [Wood] on ([" + this.Table + @"].[WoodID] = [Wood].[Unique]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               inner join [FullPound] on ([" + this.Table + @"].[WoodID] = [FullPound].[WoodID]
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] desc) as [Number], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [" + this.Table + @"].[Unique]
                               from [" + this.Table + @"]
                               inner join [Wood] on ([" + this.Table + @"].[WoodID] = [Wood].[Unique]
                                   and [Wood].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Wood].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               inner join [FullPound] on ([" + this.Table + @"].[WoodID] = [FullPound].[WoodID]
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           )
                           select count([Unique]) as [Total]
                           from [Filtered];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

        /// <summary>
        /// 查询下一步操作的数据表里的记录
        /// </summary>
        /// <param name="field">要查询的字段名</param>
        /// <param name="value">要查询的字段值</param>
        /// <param name="connect">字段名和字段值的逻辑关系</param>
        /// <param name="table">数据库表名</param>
        /// <returns>结果集</returns>
        public EntityCollection SelectRecordComeFromDataOfNextStepOperate(string field, object value, string connect, string table)
        {
            string sql = @"select [Unique]" + @"
                           from [" + table + @"]
                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and " + this.GetWhere(field, value, connect);

            IList collections = base.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;

            return results;
        }

    }
}
