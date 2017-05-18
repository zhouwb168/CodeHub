using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;
using System;
using Wodeyun.Gf.Database.SqlServer;

namespace Wodeyun.Project.EmptyPound.Dals
{
    public class EmptyPoundDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "EmptyPound";
            this.InsertColumns = this.Inserts = "[Unique], [WoodID], [EmptyVolume], [HandVolume], [BackWeighTime], [Operator], [State], [Version], [Log],[LFUnique],[LFDate],[RebateVolume]";
            this.UpdateColumns = this.Updates = "[EmptyVolume], [HandVolume], [Log],[LFUnique],[LFDate],[RebateVolume]";
            this.Selects = "[Unique], [WoodID], [EmptyVolume], [HandVolume], [BackWeighTime],[LFUnique],[LFDate],[RebateVolume]";
            this.Order = "[Unique] desc";
        }

        public EmptyPoundDal()
        {
            this.Init();
        }

        public EmptyPoundDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 获取等待厂门口回收电子卡的数据
        /// </summary>
        /// <param name="recordId">记录编号</param>
        /// <returns>数据对象</returns>
        public Entity GetEntityByFieldWithOperator(int recordId)
        {
            string sql = @"select " + this.GetFields(this.Selects, this.Table) + @",
                                    [FullPound].[License]
                           from [" + this.Table + @"]
                           inner join [FullPound] on ([" + this.Table + @"].[WoodID] = [FullPound].[WoodID]
                               and [FullPound].[State] = " + StateEnum.Default.ToDatabase() + @")
                           where [" + this.Table + @"].[WoodID] = " + recordId.ToDatabase() + @"
                               and [" + this.Table + @"].[State] = " + StateEnum.Default.ToDatabase();

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
                               select [" + this.Table + @"].[Unique], [Recover].[Unique] as [RecoverID]
                               from [" + this.Table + @"]
                               left outer join [Recover] on ([" + this.Table + @"].[WoodID] = [Recover].[WoodID]
                                   and [Recover].[State] = " + StateEnum.Default.ToDatabase() + @")
                               where [" + this.Table + "].[State] = " + StateEnum.Default.ToDatabase() + @"
                               and [" + this.Table + "].[Unique] = " + intUnique + @"
                               and [" + this.Table + "].[Operator] = " + operatorID + @"
                           )
                           update [" + this.Table + @"]
                           set [State] = " + StateEnum.Deleted.ToDatabase() + @"
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                           and ([Unique] in (select [Unique] from [Filtered] where ([RecoverID] is null)));";

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
            StartTime = DateTime.Now.AddMonths(0).ToString("yyyy-MM-dd");
            EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrWhiteSpace(CarID))
            {
                strWhere += " AND [FullPound].[License] LIKE '%" + CarID + "%'";
                StartTime = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrWhiteSpace(StartTime) && !string.IsNullOrWhiteSpace(EndTime))
            {
                strWhere += " AND [E1].[BackWeighTime]>=" + StartTime.ToDateBegin().ToDatabase() + " AND [E1].[BackWeighTime]<=" + EndTime.ToDateEnd().ToDatabase() + "";
            }
            string sql = @"with [Filtered] as
                           (
                               SELECT DISTINCT [E1].[Unique],[E1].[WoodID],[EmptyVolume],[HandVolume],[BackWeighTime],[E1].[LFUnique],[E1].[LFDate],[RebateVolume]
                                , [FullPound].[FullVolume], [FullPound].[License], [FullPound].[CardNumber] AS [RedCard]
                                , [Wood].[CardNumber] AS [GreenCard],WJ.BangID,WB.bangCid FROM EmptyPound AS [E1]
                                INNER JOIN [FullPound] ON ([E1].[WoodID]=[FullPound].[WoodID] AND [FullPound].[State]=0)
                                INNER JOIN [Wood] ON ([E1].[WoodID] = [Wood].[Unique] AND [Wood].[State] = 0)
                                LEFT JOIN WoodJoin WJ ON (WJ.WoodID=[E1].[WoodID] AND WJ.[State]=0)
                                LEFT JOIN WoodBang WB ON WB.bangid=WJ.BangID AND WB.[IsJoined]=1
                                WHERE [E1].[State] = 0 " + strWhere + @" 
                           ),
                           [Build] as
                           (
                               select DISTINCT [Factory].[Unique] from [Factory]
                               inner join [Filtered] on [Factory].[WoodID] = [Filtered].[WoodID]
                               where [Factory].[State] = 0
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [Filtered].[Unique] desc) as [Number]
                                    , [Filtered].*, [Factory].[SampleTime]
                               from [Filtered]
                               inner join [Factory] on ([Filtered].[WoodID] = [Factory].[WoodID]
                                   and [Factory].[State] = 0)
                               inner join [Build] on ([Factory].[Unique] = [Build].[Unique])
                           )
                           select * into #temp from [Numbered];
                           select top " + length.ToDatabase() + @" * from #temp
                           where [Number] >= " + start.ToDatabase() + @"
                           select count([Unique]) as [Total] from #temp
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
                           where [State] = " + StateEnum.Default.ToDatabase() + @"
                           and " + this.GetWhere(field, value, connect);

            IList collections = base.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;

            return results;
        }

        /// <summary>
        /// 获取当前车辆最近十车的平均首磅和回皮体积
        /// </summary>
        /// <param name="License"></param>
        /// <returns></returns>
        public Entity getAvgVolume(string License)
        {
            string strSql = @"WITH AVGVolume AS(
                            SELECT TOP 10 FP.License,EP.EmptyVolume,FP.FullVolume FROM [EmptyPound] EP
                            INNER JOIN [FullPound] FP ON FP.WoodID=EP.WoodID AND FP.[State]=0 AND EP.[State]=0
                            WHERE FP.License LIKE '%" + License + @"%' ORDER BY FP.WeighTime DESC)

                            SELECT MAX(License) AS License,ISNULL(AVG(EmptyVolume),0) AS AvgEmptyVolume,ISNULL(AVG(FullVolume),0) AS AvgFullVolume FROM AVGVolume";
            return base.Execute.GetEntity(strSql);
        }

        /// <summary>
        /// 量方数据同步到工业互联网
        /// </summary>
        /// <param name="entity"></param>
        public void SyncEmptyVolumeData(Entity entity, string ConnectionStringName)
        {
            int WoodID = entity.GetValue("WoodID").TryInt32();
            decimal EmptyVolume = entity.GetValue("EmptyVolume").TryDecimal();
            decimal HandVolume = entity.GetValue("HandVolume").TryDecimal();
            decimal RebateVolume = entity.GetValue("RebateVolume").TryDecimal();
            string BackWeighTime = entity.GetValue("BackWeighTime").TryString();
            int Operator = entity.GetValue("Operator").TryInt32();
            int LFUnique = entity.GetValue("LFUnique").TryInt32();
            string LFDate = entity.GetValue("LFDate").TryString();
            if (WoodID == 0) return;
            string strSql = @"DELETE FROM [EmptyPound] WHERE [WoodID]=" + WoodID + @";
                            INSERT INTO [EmptyPound] ([WoodID],[EmptyVolume],[HandVolume],[BackWeighTime],[Operator],[State],[LFUnique],[LFDate],[RebateVolume])
                            VALUES (" + WoodID + "," + EmptyVolume + "," + HandVolume + ",'" + BackWeighTime + "'," + Operator + "," + StateEnum.Default.ToDatabase() + "," + LFUnique + ",'" + LFDate + "'," + RebateVolume + ")";
            new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
        }

        /// <summary>
        /// 同步删除回皮数据
        /// </summary>
        /// <param name="entity"></param>
        public void SyncDeleteEmptyVolumeData(int WoodID, string ConnectionStringName)
        {
            if (WoodID == 0) return;
            string strSql = @"DELETE FROM [EmptyPound] WHERE [WoodID]=" + WoodID;
            new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
        }

        /// <summary>
        /// 同步量方数据到ERP
        /// </summary>
        /// <param name="entity"></param>
        public void SyncVolumeDataForERP(Entity entity, string ConnectionStringName, string OrgID, string CtrlType)
        {
            string BangID = entity.GetValue("BangID").TryString();
            string BangCID = entity.GetValue("BangCID").TryString();
            if (string.IsNullOrWhiteSpace(BangCID)) return;
            if (CtrlType == "ADDOREDIT")
            {
                EntityCollection collection = getVolumeJoinData(BangID);
                foreach (Entity item in collection)
                {
                    //回皮数据
                    string bangCid = item.GetValue("bangCid").TryString();
                    decimal EmptyVolume = item.GetValue("EmptyVolume").TryDecimal();
                    decimal HandVolume = item.GetValue("HandVolume").TryDecimal();
                    decimal RebateVolume = item.GetValue("RebateVolume").TryDecimal();
                    string BackWeighTime = item.GetValue("BackWeighTime").TryString();
                    string EDescription = item.GetValue("EDescription").TryString();
                    string LastOptName = string.Empty;
                    if (EDescription != "")
                    {
                        LastOptName = EDescription.Split(new char[] { ',' })[1].Split(new char[] { ':' })[1].Replace("\"", "");
                    }

                    string strSql = @"DECLARE @FID BIGINT
                                    SET @FID=(SELECT FID FROM t_TC_DiBangDan WHERE FSOURCENO='" + bangCid + "' AND F_ZHY_ORGID='" + OrgID + @"')
                                    UPDATE t_TC_DiBangDan SET F_LF_BackWeighTime='" + BackWeighTime + "',F_LF_LastOptName='" + LastOptName + @"'
                                    WHERE FID=@FID
                                    UPDATE t_TC_DiBangDan_Entry SET F_LF_EmptyVolume=" + EmptyVolume + ",F_LF_HandVolume=" + HandVolume + @",
                                    F_LF_RebateVolume=" + RebateVolume + " WHERE FID=@FID";
                    new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
                }
            }
            else
            {
                //删除
                string strSql = @"DECLARE @FID BIGINT
                                SET @FID=(SELECT FID FROM t_TC_DiBangDan WHERE FSOURCENO='" + BangCID + "' AND F_ZHY_ORGID='" + OrgID + @"')
                                UPDATE t_TC_DiBangDan SET F_LF_BackWeighTime='',F_LF_LastOptName='' WHERE FID=@FID
                                UPDATE t_TC_DiBangDan_Entry SET F_LF_EmptyVolume=0,F_LF_HandVolume=0,F_LF_RebateVolume=0 WHERE FID=@FID";
                new Execute(ConnectionStringName).ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 获取跟地磅关联的量方数据
        /// </summary>
        /// <param name="bangid"></param>
        /// <returns></returns>
        private EntityCollection getVolumeJoinData(string bangid)
        {
            string strSql = @"SELECT WB.[bangid],WB.[bangCid],WB.[Bang_Time], WB.[carCID], WB.[carUser], WB.[breedName]
                            , WB.[userXHName],FP.[FullVolume], FP.[Area],FP.[License], FP.[Tree], FP.[Driver]
                            , FP.[Supplier], FP.[WeighTime],FP.[Operator],AT.[Description]
                            , EP.EmptyVolume,EP.HandVolume,EP.RebateVolume,EP.BackWeighTime,AT1.[Description] AS EDescription
                            FROM [WoodJoin] WJ INNER JOIN [WoodBang] WB ON (WJ.[BangID] = WB.[bangid])
                            INNER JOIN [FullPound] FP ON (WJ.[WoodID] = FP.[WoodID] AND FP.[State] = 0)
                            LEFT JOIN [Account] AT ON (AT.[Unique]=FP.Operator AND AT.[State]=0)
                            LEFT JOIN [EmptyPound] EP ON (EP.WoodID=FP.WoodID AND EP.[State]=0)
                            LEFT JOIN [Account] AT1 ON (AT1.[Unique]=EP.Operator AND AT1.[State]=0)
                            WHERE WJ.[State] = 0  AND WB.IsJoined = 1 AND WB.[bangid]='" + bangid + "'";
            return Execute.GetEntityCollection(strSql);
        }

    }
}
