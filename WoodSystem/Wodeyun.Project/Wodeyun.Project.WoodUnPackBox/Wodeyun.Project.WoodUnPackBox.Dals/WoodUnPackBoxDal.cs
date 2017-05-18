using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodUnPackBox.Dals
{
    public class WoodUnPackBoxDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "WoodUnPackBox";
            this.InsertColumns = this.Inserts = "[Unique], [WoodID], [Number], [UnPackTime], [Operator], [State], [Version], [Log]";
            this.UpdateColumns = this.Updates = "[Number], [Log]";
            this.Selects = "[Unique], [Number], [UnPackTime], [Operator]";
            this.Order = "[Unique] desc";
        }

        public WoodUnPackBoxDal()
        {
            this.Init();
        }

        public WoodUnPackBoxDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="intUnique">要删除的记录号</param>
        /// <param name="intWoodID">关联的木材编号 (来自于Wood表）</param>
        /// <param name="operatorID">操作员的身份识别</param>
        /// <returns>数据库表受影响的行数</returns>
        public int DeleteEntityByUniqueWithOperator(int intUnique, int intWoodID, int operatorID)
        {
            string sql = @"with [Filtered] as
                           (
                               select [" + this.Table + @"].[Unique], [WoodLaboratory].[WoodID]
                               from [" + this.Table + @"]
                               left outer join [WoodLaboratory] on ([" + this.Table + @"].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodLaboratory].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [" + this.Table + "].[Unique] = " + intUnique + @"
                               and [" + this.Table + "].[Operator] = " + operatorID + @"
                           )
                           update [" + this.Table + @"]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                           and ([Unique] in (select [Unique] from [Filtered] where ([WoodID] is null)));";

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntityByUniqueWithOperator(Entity entity)
        {
            string sql = @"update [" + this.Table + @"]
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase() + @";
                           insert into [" + this.Table + @"]
                           select top 1 " + entity.ToUpdate(this.InsertColumns, this.UpdateColumns).Replace("[State]", StateEnum.Default.ToDatabase()).Replace("[Version]", "[Version] + 1") + @"
                           from [" + this.Table + @"]
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [State] = " + StateEnum.Updated.ToDatabase() + @"
                           order by [Version] desc";

            return this.Execute.ExecuteNonQuery(sql);
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

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录行开始索引号</param>
        /// <param name="length">记录长度</param>
        /// <param name="operatorID">操作员的身份ID</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesByStartAndLengthWithOperator(string date, int start, int length, int operatorID)
        {
            int kk = operatorID;
            string sql = @"with [Filtered] as
                           (
                               select [WoodPackBox].[WoodID], [WoodPackBox].[Box], [FullPound].[WeighTime], [Factory].[Key]
                                    , " + this.GetFields(this.Selects, this.Table) + @"
                               from [WoodPackBox]
                               inner join [FullPound] on ([WoodPackBox].[WoodID] = [FullPound].[WoodID]
                                   and ([FullPound].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @")
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               inner join [Factory] on ([WoodPackBox].[WoodID] = [Factory].[WoodID]
                                   and [WoodPackBox].[Operator] = [Factory].[Operator]
                                   and [Factory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Factory].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [" + this.Table + @"] on ([WoodPackBox].[WoodID] = [" + this.Table + @"].[WoodID]
                                   and [" + this.Table + @"].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [" + this.Table + @"].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [WoodPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [WoodPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [WeighTime] asc) as [RecordNumber], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [RecordNumber] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [WoodPackBox].[Unique]
                               from [WoodPackBox]
                               inner join [FullPound] on ([WoodPackBox].[WoodID] = [FullPound].[WoodID]
                                   and ([FullPound].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @")
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [WoodPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [WoodPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           )
                           select count([Unique]) as [Total]
                           from [Filtered];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

    }
}
