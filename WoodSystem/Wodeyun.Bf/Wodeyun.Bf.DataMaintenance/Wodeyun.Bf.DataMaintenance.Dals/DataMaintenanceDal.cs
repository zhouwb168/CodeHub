using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Gf.Database.SqlServer;

namespace Wodeyun.Bf.DataMaintenance.Dals
{
    public class DataMaintenanceDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "FullPound";
            this.InsertColumns = this.Inserts = "[Unique], [WoodID], [FullVolume], [License], [Area], [Tree], [Driver], [Supplier], [CardNumber], [WeighTime], [Printed], [Operator], [State], [Version], [Log],[LFUnique],[LFDate]";
            this.UpdateColumns = this.Updates = "[FullVolume], [License], [Area], [Tree], [Driver], [Supplier], [Log],[LFUnique],[LFDate],[WeighTime]";
            this.Selects = "[Unique], [WoodID], [FullVolume], [License], [Area], [Tree], [Driver], [Supplier], [CardNumber], [WeighTime],[LFUnique],[LFDate],[Operator]";
            this.Order = "[Unique] desc";
        }

        public DataMaintenanceDal()
        {
            this.Init();
        }

        public DataMaintenanceDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }


        /// <summary>
        /// 根据车牌号匹配查询出最近一次匹配的记录
        /// </summary>
        /// <param name="license">车牌号</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByFieldForMatch(object license)
        {
            string sql = @"select top 1 [Area], [Tree], [Driver], [Supplier]
                           from [FullPound]
                           where [License] = " + license.ToDatabase() + @"
                             and [State] = " + StateEnum.Default.ToDatabase() + @"
                           order by [Unique] desc";

            return this.Execute.GetEntity(sql);
        }

        /// <summary>
        /// 获取最大的woodID
        /// </summary>
        /// <returns></returns>
        public int getWoodID()
        {
            string strSql = @"UPDATE [Unique] SET Value+=1  WHERE [Name]='Wood'
                            SELECT Value FROM [Unique] WHERE [Name]='Wood'";
            Entity entity = this.Execute.GetEntity(strSql);
            return entity.GetValue("Value").TryInt32();
        }

        /// <summary>
        /// 获取等待料厂取样的数据
        /// </summary>
        /// <param name="recordId">记录编号</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByFieldWithOperator(int recordId)
        {
            string sql = @"select " + this.GetFields(this.Selects, this.Table) + @"
                           from [" + this.Table + @"]
                           where [" + this.Table + "].[WoodID] = " + recordId.ToDatabase() + @"
                               and [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase();

            return this.Execute.GetEntity(sql);
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
        /// 插入回皮数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEmptyPoundEntity(Entity entity)
        {
            string Selects = "[Unique], [WoodID], [EmptyVolume], [HandVolume], [BackWeighTime],[Operator], [State], [Version], [Log],[LFUnique],[LFDate],[RebateVolume]";
            string sql = @"insert into [EmptyPound] (" + Selects + @")
                           values (" + entity.ToInsert(Selects) + ")";
            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新回皮数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEmptyPoundEntity(Entity emptyEntity)
        {
            //回皮
            string InsertColumns = this.Inserts = "[Unique], [WoodID], [EmptyVolume], [HandVolume], [BackWeighTime], [Operator], [State], [Version], [Log],[LFUnique],[LFDate],[RebateVolume]";
            string UpdateColumns = this.Updates = "[EmptyVolume], [HandVolume], [Log],[LFUnique],[LFDate],[RebateVolume]";
            string sql = @"update [EmptyPound]
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [Unique] = " + emptyEntity.GetValue("UniqueEmpty").ToDatabase() + @"
                           and [State] = " + StateEnum.Default.ToDatabase() + @";
                           insert into [EmptyPound]
                           select top 1 " + emptyEntity.ToUpdate(InsertColumns, UpdateColumns).Replace("[State]", StateEnum.Default.ToDatabase()).Replace("[Version]", "[Version] + 1") + @"
                           from [EmptyPound]
                           where [Unique] = " + emptyEntity.GetValue("UniqueEmpty").ToDatabase() + @"
                           and [State] = " + StateEnum.Updated.ToDatabase() + @"
                           order by [Version] desc";

            return this.Execute.ExecuteNonQuery(sql);
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
                               select [" + this.Table + @"].[Unique], [Factory].[WoodID]
                               from [" + this.Table + @"]
                               left outer join [Factory] on ([" + this.Table + @"].[WoodID] = [Factory].[WoodID]
                                   and [Factory].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase() + @"
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
        /// 分页获取数据
        /// </summary>
        /// <param name="start">记录行开始索引号</param>
        /// <param name="length">记录长度</param>
        /// <param name="operatorID">操作员的身份ID</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesByStartAndLengthWithOperator(int start, int length, int operatorID, string StartTime, string EndTime, string CarID)
        {
            int kk = operatorID;
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(StartTime) && !string.IsNullOrWhiteSpace(EndTime))
            {
                strWhere += " AND [" + this.Table + "].[WeighTime]>=" + StartTime.ToDateBegin().ToDatabase() + " AND [" + this.Table + "].[WeighTime]<=" + EndTime.ToDateEnd().ToDatabase() + "";
            }
            if (!string.IsNullOrWhiteSpace(CarID))
            {
                strWhere += " AND [" + this.Table + "].[License] LIKE '%" + CarID + "%'";
            }
            string sql = @"with [Filtered] as
                           (
                               select  " + this.GetFields(this.Selects, this.Table) + @", [Wood].[CardNumber] as [CreenCardNumber]
                                       , [Wood].[ArriveDate], ep.EmptyVolume, ep.HandVolume, ep.BackWeighTime, ep.RebateVolume, ep.[Unique] as UniqueEmpty
                                       , ep.LFUnique as LFUniqueEmpty, ep.LFDate as LFDateEmpty,ep.Operator as epOperator
                               from [" + this.Table + @"]
                               left join [Wood] on ([" + this.Table + @"].[WoodID] = [Wood].[Unique] and [Wood].[State] = " + StateEnum.Default.ToDatabase() + @")
                               left join EmptyPound ep on (ep.WoodID=[" + this.Table + @"].WoodID and ep.[State]=0)
                               where [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase() + strWhere + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Unique] desc) as [Number], *
                               from [Filtered]
                           )
                           select * into #temp from [Numbered];
                           select top " + length.ToDatabase() + @" *
                           from #temp where [Number] >= " + start.ToDatabase() + @"
                           select count(DISTINCT [Unique]) as [Total] from #temp
                           drop table #temp";

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


        /// <summary>
        /// 分页获取木材收购磅单数据
        /// </summary>
        /// <param name="start">记录行开始索引号</param>
        /// <param name="length">记录长度</param>
        /// <param name="operatorID">操作员的身份ID</param>
        /// <returns>结果集</returns>
        public EntityCollection GetWoodBangPrintInfo(int start, int length, int operatorID, string startDate, string endDate, string License)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(License))
            {
                strWhere = " AND fp.[License] LIKE '%" + License + "%'";
            }
            string sql = @"WITH [Filtered] AS
                            (
                                select distinct fp.WeighTime,fp.License,fp.Tree,FullVolume=isnull(fp.FullVolume,0),EmptyVolume=isnull(ep.EmptyVolume,0),jVolume=((CASE ISNULL(ep.HandVolume,0) WHEN 0 THEN fp.FullVolume ELSE ep.HandVolume END)-ISNULL(ep.EmptyVolume,0)-ISNULL(ep.RebateVolume,0)),
                                --wb.firstBangUser,
                                fp.Supplier,fp.Driver,RebateVolume=isnull(ep.RebateVolume,0) 
                                from FullPound fp 
                                --inner join WoodJoin wj on wj.WoodID=fp.WoodID and wj.[State]=0
                                --inner join WoodBang wb on wb.bangid=wj.BangID and wj.[State]=0 and wb.IsFiltered=0
                                left join EmptyPound  ep on ep.WoodID=fp.WoodID and fp.[State]=0
                                where fp.WeighTime>=" + startDate.ToDateBegin().ToDatabase() + @"
                                and fp.WeighTime<=" + endDate.ToDateEnd().ToDatabase() + strWhere + @"
                            ),
                            [Numbered] AS
                            (
                                SELECT ROW_NUMBER() OVER (ORDER BY [WeighTime] DESC) AS [Number], * FROM [Filtered]
                            )
                            SELECT TOP " + length.ToDatabase() + @" * FROM [Numbered] WHERE [Number] >= " + start.ToDatabase() + @";
                            with [Filtered] as
                            (
                                select distinct fp.WeighTime,fp.License,fp.Tree,FullVolume=isnull(fp.FullVolume,0),EmptyVolume=isnull(ep.EmptyVolume,0),jVolume=((CASE ISNULL(ep.HandVolume,0) WHEN 0 THEN fp.FullVolume ELSE ep.HandVolume END)-ISNULL(ep.EmptyVolume,0)-ISNULL(ep.RebateVolume,0)),
                                --wb.firstBangUser,
                                fp.Supplier,fp.Driver 
                                from FullPound fp 
                                --inner join WoodJoin wj on wj.WoodID=fp.WoodID and wj.[State]=0
                                --inner join WoodBang wb on wb.bangid=wj.BangID and wj.[State]=0 and wb.IsFiltered=0
                                left join EmptyPound  ep on ep.WoodID=fp.WoodID and fp.[State]=0
                                where fp.WeighTime>=" + startDate.ToDateBegin().ToDatabase() + @"
                                and fp.WeighTime<=" + endDate.ToDateEnd().ToDatabase() + strWhere + @"
                            )
                            SELECT COUNT(*) AS [Total] FROM [Filtered];";

            IList collections = this.Execute.GetEntityCollections(sql);
            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();
            return results;
        }

        /// <summary>
        /// 量方数据同步到工业互联网
        /// </summary>
        /// <param name="entity"></param>
        public void SyncFullVolumeData(string strSql, string ConnectionStringName)
        {
            //            int WoodID = entity.GetValue("WoodID").TryInt32();
            //            string License = entity.GetValue("License").TryString();
            //            decimal FullVolume = entity.GetValue("FullVolume").TryDecimal();
            //            string WeighTime = entity.GetValue("WeighTime").TryString();
            //            string Area = entity.GetValue("Area").TryString();
            //            string Tree = entity.GetValue("Tree").TryString();
            //            int Operator = entity.GetValue("Operator").TryInt32();
            //            string Driver = entity.GetValue("Driver").TryString();
            //            string Supplier = entity.GetValue("Supplier").TryString();
            //            //回皮数据
            //            decimal EmptyVolume = entity.GetValue("EmptyVolume").TryDecimal();
            //            string BackWeighTime = entity.GetValue("BackWeighTime").TryString();
            //            decimal HandVolume = entity.GetValue("HandVolume").TryDecimal();
            //            int epOperator = entity.GetValue("epOperator").TryInt32();
            //            decimal RebateVolume = entity.GetValue("RebateVolume").TryDecimal();
            //            if (WoodID == 0) return;
            //            string strSql = @"--首磅数据
            //                            DELETE FROM [FullPound] WHERE [WoodID]=" + WoodID + @";
            //                            INSERT INTO [FullPound] ([WoodID],[License],[FullVolume],[WeighTime],[Area],[Tree],[Operator],[Driver],[Supplier],[State])
            //                            VALUES (" + WoodID + ",'" + License + "'," + FullVolume + ",'" + WeighTime + "','" + Area + "','" + Tree + "'," + Operator + ",'" + Driver + "','" + Supplier + "'," + StateEnum.Default.ToDatabase() + @");
            //                            --回皮数据
            //                            DELETE FROM [EmptyPound] WHERE [WoodID]=" + WoodID + @";
            //                            INSERT INTO [EmptyPound]([WoodID],[EmptyVolume],[BackWeighTime],[HandVolume],[Operator],[State],[RebateVolume])
            //                            VALUES (" + WoodID + "," + EmptyVolume + ",'" + BackWeighTime + "'," + HandVolume + "," + epOperator + "," + StateEnum.Default.ToDatabase() + "," + RebateVolume + ");";
            new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
        }
    }
}
