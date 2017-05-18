using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;
using System;

namespace Wodeyun.Project.Factory.Dals
{
    public class FactoryDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "Factory";
            this.InsertColumns = this.Inserts = "[Unique], [WoodID], [UnLoadPalce], [UnLoadPeople], [Key], [Sampler], [Water], [Skin], [Scrap], [Deduct], [Remark], [SampleTime], [Operator], [State], [Version], [Log]";
            this.UpdateColumns = this.Updates = "[UnLoadPalce], [UnLoadPeople], [Key], [Sampler], [Water], [Skin], [Scrap], [Deduct], [Remark], [Log]";
            this.Selects = "[Unique], [WoodID], [UnLoadPalce], [UnLoadPeople], [Key], [Sampler], [Water], [Skin], [Scrap], [Deduct], [Remark], [SampleTime]";
            this.Order = "[Unique] desc";
        }

        public FactoryDal()
        {
            this.Init();
        }

        public FactoryDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 获取等待地磅回皮的数据
        /// </summary>
        /// <param name="recordId">记录编号</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByFieldWithOperator(int recordId)
        {
            string sql = @"with [Filtered] as
                           (
                             select [WoodID], [FullVolume], [License], [CardNumber] as [RedCard]
                             from [FullPound]
                             where [WoodID] = " + recordId.ToDatabase() + @"
                                and [State] =0
                            ),
                           [Merged] as
                           (
                               select [F1].[WoodID], [F1].[FullVolume], [F1].[License], [F1].[RedCard]
                                       , [" + this.Table + "].[SampleTime]" + @"
                                       , (
                                           select [Deduct] + '＃' from [" + this.Table + @"]
                                           where [WoodID] = [F1].[WoodID] and [State] =0 for xml path('')
                                         ) as [Deduct]
                                       , [Wood].[CardNumber] as [GreenCard]
                               from [Filtered] as [F1]
                               inner join [" + this.Table + @"] on ([F1].[WoodID] = [" + this.Table + @"].[WoodID]
                                   and [" + this.Table + @"].[State] =0)
                               inner join [Wood] on ([F1].[WoodID] = [Wood].[Unique] and [Wood].[State] =0)
                           )
                           select top 1 * from [Merged];";

            return this.Execute.GetEntity(sql);
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
                               select [" + this.Table + @"].[Unique], [EmptyPound].[WoodID]
                               from [" + this.Table + @"]
                               left outer join [EmptyPound] on ([" + this.Table + @"].[WoodID] = [EmptyPound].[WoodID]
                                   and [EmptyPound].[State] =0)
                               where [" + this.Table + @"].[State] =0
                               and [" + this.Table + "].[Unique] = " + intUnique + @"
                               and [" + this.Table + "].[Operator] = " + operatorID + @"
                           )
                           update [" + this.Table + @"]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                           and ([Unique] in (select [Unique] from [Filtered] where ([WoodID] is null)));
                           --删除装箱记录
                           UPDATE [WoodPackBox] SET [State]=9 WHERE [WoodID]=" + intWoodID + " AND [State]=0 AND [Operator] = " + operatorID + @"";

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
                           and [Operator] = " + entity.GetValue("Operator").ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase() + @";
                           insert into [" + this.Table + @"]
                           select top 1 " + entity.ToUpdate(this.InsertColumns, this.UpdateColumns).Replace("[State]", StateEnum.Default.ToDatabase()).Replace("[Version]", "[Version] + 1") + @"
                           from [" + this.Table + @"]
                           where [Unique] = " + entity.GetValue("Unique").ToDatabase() + @"
                           and [Operator] = " + entity.GetValue("Operator").ToDatabase() + @"
                           and [State] = " + StateEnum.Updated.ToDatabase() + @"
                           order by [Version] desc";

            return this.Execute.ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 根据操作员的身份ID分页获取数据
        /// </summary>
        /// <param name="start">记录行开始索引号</param>
        /// <param name="length">记录长度</param>
        /// <param name="operatorID">操作员的身份ID</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID)
        {
            string sql = @"with [Filtered] as
                           (
                               select " + this.GetFields(this.Selects, this.Table) + @"
                                      , [FullPound].[License], [FullPound].[Tree], [FullPound].[Driver]
                                      , [FullPound].[CardNumber] as [RedCardNumber], [FullPound].[WeighTime]
                                      , WoodPackBox.Box,WoodPackBox.PackTime
                               from [" + this.Table + @"]
                               left outer join [FullPound] on ([" + this.Table + @"].[WoodID] = [FullPound].[WoodID]
                                   and [FullPound].[State]=0)
                               left outer join [WoodPackBox] on ([" + this.Table + @"].[WoodID] = [WoodPackBox].[WoodID]
                                   and [WoodPackBox].[State]=0)
                               where [" + this.Table + @"].[State]=0
                               and [" + this.Table + "].[Operator] = " + operatorID + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] desc) as [Number], *
                               from [Filtered]
                           )
                           select top " + length.ToDatabase() + @" * from [Numbered]
                           where [Number] >= " + start.ToDatabase() + @" order by [Unique] desc;
                           with [Filtered] as
                           (
                               select [" + this.Table + @"].[Unique] from [" + this.Table + @"]
                               where [" + this.Table + @"].[State] =0
                               and [" + this.Table + "].[Operator] = " + operatorID + @"
                           )
                           select count([Unique]) as [Total] from [Filtered];";

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
                           where [State] =0
                           and " + this.GetWhere(field, value, connect);

            IList collections = base.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;

            return results;
        }

        /// <summary>
        /// 判断装箱密码是否已经存在
        /// </summary>
        /// <param name="weigthtime">过磅日期</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public int IsExistsFactoryKey(string weigthtime, string key)
        {
            weigthtime = weigthtime == "" ? (System.DateTime.Now.ToString("yyyy-MM-dd")) : weigthtime.ToDateTime().ToString("yyyy-MM-dd");
            string strSql = @"SELECT COUNT(f.[Key]) AS Total FROM Factory f
                            INNER JOIN FullPound p ON p.WoodID=f.WoodID AND p.[State]=0
                            WHERE CONVERT(VARCHAR(100),WeighTime,23)='" + weigthtime + "' AND f.[Key]='" + key + "'  AND f.[State]=0";
            return base.Execute.GetEntity(strSql).GetValue("Total").TryInt32();
        }

        /// <summary>
        /// 保存装箱信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertBoxEntity(Entity entity)
        {
            string Inserts = "[Unique], [WoodID], [Box], [PackTime], [Operator], [State], [Version], [Log]";
            string sql = @"insert into [WoodPackBox] (" + Inserts + @")
                           values (" + entity.ToInsert(Inserts) + ")";
            return this.Execute.ExecuteNonQuery(sql);
        }

    }
}
