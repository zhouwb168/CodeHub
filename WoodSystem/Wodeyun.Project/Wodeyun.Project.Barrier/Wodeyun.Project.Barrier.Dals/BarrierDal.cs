using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.Barrier.Dals
{
    public class BarrierDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "Barrier";
            this.InsertColumns = this.Inserts = "[Unique], [Place], [CardNumber], [License], [Area], [TimeOfStation], [GPS], [Operator], [State], [Version], [Log]";
            this.UpdateColumns = this.Updates = "[License], [Area], [Log]";
            this.Selects = "[Unique], [Place], [CardNumber], [License], [Area], [TimeOfStation], [GPS]";
            this.Order = "[Unique] desc";
        }

        public BarrierDal()
        {
            this.Init();
        }

        public BarrierDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 根据车牌号匹配查询出最近一次记录的木材来源地
        /// </summary>
        /// <param name="field">要查询的字段名</param>
        /// <param name="value">要查询的字段值</param>
        /// <param name="connect">字段名和字段值的逻辑关系</param>
        /// <param name="table">要查询的数据表</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByFieldForWoodWhereComeFrom(string field, object value, string connect, string table)
        {
            string sql = @"with [Filtered] as
                           (
                             select [Unique], [Area]
                             from [" + table + @"]
                             where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                             and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                             and " + this.GetWhere(field, value, connect) + @"
                            )
                           select top 1 *
                           from [Filtered] order by [Unique] desc;";

            return this.Execute.GetEntity(sql);
        }

        /// <summary>
        /// 保存照片
        /// 返回数据库表受影响的行数，行数>0则为操作成功
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>数据库表受影响的行数</returns>
        public int SavePhoto(Entity entity)
        {
            string sql = @"insert into [WoodCarPhoto] ([Unique], [BarrierID], [GPS], [Photo], [PhotoTime], [Operator]
                                , [State], [Version], [Log])
                           values (" + entity.GetValue("Unique").ToDatabase() + @"
                                    , " + entity.GetValue("BarrierID").ToDatabase() + @"
                                    , " + entity.GetValue("GPS").ToDatabase() + @"
                                    , (select top 1 [Photo] from [WoodTempPhoto] where [PhotoName] = " + entity.GetValue("ImageFileName").ToDatabase() + @")
                                    , " + entity.GetValue("PhotoTime").ToDatabase() + @"
                                    , " + entity.GetValue("Operator").ToDatabase() + @"
                                    , " + entity.GetValue("State").ToDatabase() + @"
                                    , " + entity.GetValue("Version").ToDatabase() + @"
                                    , " + entity.GetValue("Log").ToDatabase() + ")";

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 修改数据（适用于已登录状态当前操作者）
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
        /// <param name="start">记录行开始索引号</param>
        /// <param name="length">记录长度</param>
        /// <param name="operatorID">操作员的身份ID</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            int kk = operatorID;
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this.Selects, "B1") + @"
                                      , (
                                           select [ImageNumber] = count([Unique])
                                           from [WoodCarPhoto]
                                           where [State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                                 and [State] <> " + StateEnum.Updated.ToDatabase() + @"
                                                 and [BarrierID] = [B1].[Unique]
                                      ) as [PhotoNumber]
                               from [" + this.Table + @"] as [B1]
                               where [B1].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                     and [B1].[State] <> " + StateEnum.Updated.ToDatabase() + @"
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
                               select [" + this.Table + @"].[Unique]" + @"
                               from [" + this.Table + @"]
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
        /// 删除记录
        /// </summary>
        /// <param name="unique">要删除的记录号</param>
        /// <param name="operatorID">操作员的身份识别</param>
        /// <returns>数据库表受影响的行数</returns>
        public int DeleteEntityByUniqueWithOperator(int unique, int operatorID)
        {
            string sql = @"with [Filtered] as
                           (
                               select [" + this.Table + @"].[Unique], [Check].[BarrierID]
                               from [" + this.Table + @"]
                               left outer join [Check] on ([" + this.Table + @"].[Unique] = [Check].[BarrierID]
                                   and [Check].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Check].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [" + this.Table + "].[Unique] = " + unique + @"
                               and [" + this.Table + "].[Operator] = " + operatorID + @"
                           )
                           update [" + this.Table + @"]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                           and ([Unique] in (select [Unique] from [Filtered] where ([BarrierID] is null)));";

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取等待验收的数据
        /// </summary>
        /// <param name="recordId">记录编号</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByFieldWithOperator(int recordId)
        {
            string sql = @"select " + this.GetFields(this.Selects, this.Table) + @"
                           from [" + this.Table + @"]
                           where [" + this.Table + "].[Unique] = " + recordId.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [" + this.Table + "].[State] <> " + StateEnum.Deleted.ToDatabase();

            return this.Execute.GetEntity(sql);
        }

    }
}
