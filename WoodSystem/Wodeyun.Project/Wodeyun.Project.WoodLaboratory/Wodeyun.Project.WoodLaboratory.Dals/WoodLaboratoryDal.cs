using System.Data.SqlClient;
using System.Collections;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodLaboratory.Dals
{
    public class WoodLaboratoryDal : CommonDal
    {
        string UpdateColumns;
        string InsertColumns;

        private void Init()
        {
            this.Table = "WoodLaboratory";
            this.InsertColumns = this.Inserts = "[Unique], [WoodID], [Water], [RebateWater], [Skin], [RebateSkin], [Scrap], [RebateScrap], [Bad], [Greater], [Less], [CheckTime], [Operator], [State], [Version], [Log], [RebateGreater], [DeductVolume]";
            this.UpdateColumns = this.Updates = "[Water], [RebateWater], [Skin], [RebateSkin], [Scrap], [RebateScrap], [Bad], [Greater], [Less], [RebateGreater], [DeductVolume]";
            this.Selects = "[Unique], [WoodID], [Water], [RebateWater], [Skin], [RebateSkin], [Scrap], [RebateScrap], [Bad], [Greater], [Less], [CheckTime], [RebateGreater], [DeductVolume]";
            this.Order = "[Unique] desc";
        }

        public WoodLaboratoryDal()
        {
            this.Init();
        }

        public WoodLaboratoryDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 根据木材记录编号，获取化验报告
        /// </summary>
        /// <param name="woodID">木材记录编号</param>
        /// <returns></returns>
        public Entity GetLaboratoryDataByWoodID(int woodID)
        {
            string sql = @"select " + this.GetFields(this.Selects, this.Table) + @"
                           from [" + this.Table + @"]
                           where [" + this.Table + @"].[State] =0
                              and [" + this.Table + "].[WoodID] = " + woodID.ToDatabase();

            return this.Execute.GetEntity(sql);
        }

        /// <summary>
        /// 根据木材记录编号，从地磅系统和短信报备系统中获取用于化验室填写报告时要参考的相关信息
        /// </summary>
        /// <param name="woodID">木材记录编号</param>
        /// <returns></returns>
        public Entity GetGsmInfoByWoodID(int woodID)
        {
            string sql = @"select [WoodJoin].[IsRebate], [WoodBang].[Bang_Time], [WoodBang].[breedName]
                           from [WoodJoin]
                           left outer join [WoodBang] on ([WoodJoin].[BangID] = [WoodBang].[bangid])
                           where [WoodJoin].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                               and [WoodJoin].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                               and [WoodJoin].[WoodID] = " + woodID.ToDatabase() + @"";

            return this.Execute.GetEntity(sql);
        }

        /// <summary>
        /// 根据木材记录编号，从木材收购系统中获取用于化验室填写报告时要参考的相关信息
        /// </summary>
        /// <param name="woodID">木材记录编号</param>
        /// <returns></returns>
        public Entity GetWoodInfoByWoodID(int woodID)
        {
            string sql = @"with [Filtered] as
                           (
                               select (
                                        select [Remark] + '＃'
                                        from [Factory] where [WoodID] = [F1].[WoodID] and [State] =0 for xml path('')
                                      ) as [Remark]
                                      , [FullPound].[Tree], [FullPound].[WeighTime],wb.jWeight
                               from [Factory] as [F1]
                               left outer join [FullPound] on ([F1].[WoodID] = [FullPound].[WoodID]
                                  and [FullPound].[State] =0)
                               left join WoodJoin wj on wj.WoodID=[FullPound].WoodID and wj.[State]=0
                               left join WoodBang wb on wb.bangid=wj.BangID and wb.IsFiltered=0
                               where [F1].[State] =0 and [F1].[WoodID] = " + woodID.ToDatabase() + @"
                            )
                           select [Tree], [WeighTime], left([Remark], len([Remark]) - 1) as [Remark],jWeight
                           from [Filtered];";

            return this.Execute.GetEntity(sql);
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
        /// 分页查询获取化验报告记录
        /// </summary>
        /// <param name="date">查询日期，格式如：2013-05-23</param>
        /// <param name="start">记录开始索引</param>
        /// <param name="length">记录数</param>
        /// <param name="key">料厂密码</param>
        /// <param name="number">检验号</param>
        /// <returns>结果集</returns>
        public EntityCollection GetEntitiesBySearchWithPaging(string date, int start, int length, string key, string number)
        {
            string sql = @"with [Filtered] as
                           (
                               select [WoodUnPackBox].[WoodID], [WoodUnPackBox].[Number]
                                      , [WoodPackBox].[Operator]
                                      , [FullPound].[WeighTime]
                                      , [WoodLaboratory].[Unique], [WoodLaboratory].[CheckTime]
                               from [WoodUnPackBox]
                               inner join [WoodPackBox] on ([WoodUnPackBox].[WoodID] = [WoodPackBox].[WoodID]
                                   and [WoodPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               inner join [FullPound] on ([WoodUnPackBox].[WoodID] = [FullPound].[WoodID]
                                   and ([FullPound].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @")
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               left outer join [WoodLaboratory] on ([WoodUnPackBox].[WoodID] = [WoodLaboratory].[WoodID]
                                   and [WoodLaboratory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodLaboratory].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [WoodUnPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodUnPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   " + (number == "" ? "" : string.Format(" and [WoodUnPackBox].[Number] = {0}", number)) + @"
                           ),
                           [Build] as
                           (
                              select [Filtered].*
                                     , [Factory].[Key]
                              from [Filtered]
                              inner join [Factory] on ([Filtered].[WoodID] = [Factory].[WoodID]
                                   and [Filtered].[Operator] = [Factory].[Operator]
                                   and [Factory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Factory].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   " + (key == "" ? ")" : string.Format(" and [Factory].[Key] = '{0}')", key)) + @"
                           ),
                           [Numbered] as
                           (
                               select row_number() over (order by [WeighTime] asc) as [RecordNumber], *
                               from [Build]
                           )
                           select top " + length.ToDatabase() + @" *
                           from [Numbered]
                           where [RecordNumber] >= " + start.ToDatabase() + @";
                           with [Filtered] as
                           (
                               select [WoodUnPackBox].[WoodID]
                                      , [WoodPackBox].[Operator]
                               from [WoodUnPackBox]
                               inner join [WoodPackBox] on ([WoodUnPackBox].[WoodID] = [WoodPackBox].[WoodID]
                                   and [WoodPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               inner join [FullPound] on ([WoodUnPackBox].[WoodID] = [FullPound].[WoodID]
                                   and ([FullPound].[WeighTime] >= " + date.ToDateBegin().ToDatabase() + @"
                                   and [FullPound].[WeighTime] <= " + date.ToDateEnd().ToDatabase() + @")
                                   and [FullPound].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [FullPound].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                               where [WoodUnPackBox].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodUnPackBox].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   " + (number == "" ? "" : string.Format(" and [WoodUnPackBox].[Number] = {0}", number)) + @"
                           ),
                           [Build] as
                           (
                              select [Filtered].*
                              from [Filtered]
                              inner join [Factory] on ([Filtered].[WoodID] = [Factory].[WoodID]
                                   and [Filtered].[Operator] = [Factory].[Operator]
                                   and [Factory].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [Factory].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                                   " + (key == "" ? ")" : string.Format(" and [Factory].[Key] = '{0}')", key)) + @"
                           )
                           select count(*) as [Total]
                           from [Build];";

            IList collections = this.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;
            results.Total = (collections[1] as EntityCollection)[0].GetValue("Total").ToInt32();

            return results;
        }

    }
}
