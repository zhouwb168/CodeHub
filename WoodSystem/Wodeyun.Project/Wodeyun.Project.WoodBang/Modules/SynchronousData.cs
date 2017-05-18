using System.Data.SqlClient;
using System.Collections;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System;

using System.Text;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodBang.Modules
{
    /// <summary>
    /// 同步地磅系统的数据到木材收购系统
    /// </summary>
    public class SynchronousData : CommonDal
    {
        private static LogHelper helper = new LogHelper(ConfigurationManager.AppSettings["LogFilePath"].ToString());

        private void Init()
        {
            this.Table = "WoodBang";
            this.Inserts = "[bangid], [bangCid], [jWeight], [Bang_Time], [carCID], [carUser], [firstBangUser], [breedName], [userXHName]";
            this.Selects = "[bangid], [bangCid], [jWeight], [Bang_Time], [carCID], [carUser], [firstBangUser], [breedName], [userXHName]";
        }

        public SynchronousData()
        {
            this.Init();
        }

        public SynchronousData(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        #region 木材收购系统

        /// <summary>
        /// 检查是否存在指定的记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否存在</returns>
        public bool Exits(Entity entity)
        {
            string sql = @"select " + this.GetFields(this.Selects, this.Table) + @"
                           from [" + this.Table + @"]
                           where " + this.GetWhere("bangid", entity.GetValue("bangid"), "=");

            Entity tempEntity = this.Execute.GetEntity(sql);

            return (tempEntity.GetValue("bangid") != null);
        }

        /// <summary>
        /// 再次同步近3个月以来车辆过磅的数据
        /// 说明：之所以要再次同步，是为了应付地磅系统中途修改（地磅系统允许数据修改一次）或后期修改的情况，还有就是因停电等原因，后期使用超级用户权限人工添加的记录
        /// </summary>
        public void UpdateDataAgainForChange()
        {
            try
            {
                DateTime end = DateTime.Now;
                DateTime begin = end.AddMonths(-3);

                /* 注意！因为原地磅系统的ACCESS数据库版本太低了，所以日期查询字段时，值不能用单引号，而是用井号 */
                string sql = @"select " + this.GetFields(this.Selects, "t_bang") + @"
                               from [t_bang]
                               where ([Bang_Time] >= #" + begin.ToString() + @"#
                                     and [Bang_Time] <= #" + end.ToString() + @"#)
                                     and [jWeight] > 0";

                EntityCollection oldDataCollection = this.GetEntityCollectionFromAccess(sql);
                if (oldDataCollection == null) return;

                int entityCount = oldDataCollection.Count;
                int act = 0;
                for (int i = 0; i < entityCount; i++)
                {
                    if (Exits(oldDataCollection[i])) UpdateOldData(oldDataCollection[i]);
                    else act = this.InsertEntity(oldDataCollection[i]);
                }
            }
            catch (Exception ex)
            {
                helper.WriteLine("UpdateDataAgainForChange:" + ex.Message);
            }
        }

        /// <summary>
        /// 把数据从地磅系统更新到木材收购系统
        /// </summary>
        /// <param name="entity"></param>
        void UpdateOldData(Entity entity)
        {
            string sql = @"update [WoodBang] set [bangCid] = " + entity.GetValue("bangCid").ToDatabase() + @"
                                  , [jWeight] = " + entity.GetValue("jWeight").ToDatabase() + @"
                                  , [Bang_Time] = " + entity.GetValue("Bang_Time").ToDatabase() + @"
                                  , [carCID] = " + entity.GetValue("carCID").ToDatabase() + @"
                                  , [carUser] = " + entity.GetValue("carUser").ToDatabase() + @"
                                  , [firstBangUser] = " + entity.GetValue("firstBangUser").ToDatabase() + @"
                                  , [breedName] = " + entity.GetValue("breedName").ToDatabase() + @"
                                  , [userXHName] = " + entity.GetValue("userXHName").ToDatabase() + @"
                           where [bangid] = " + entity.GetValue("bangid").ToDatabase();

            int act = this.Execute.ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 同步车辆回皮的数据
        /// 说明：只同步当前日期之前3个月以来的车辆回皮数据
        /// </summary>
        public void UpdateDataForSecondWeigh()
        {
            try
            {
                DateTime end = DateTime.Now;
                DateTime begin = end.AddMonths(-3);

                string sql = @"select [bangid]
                           from [WoodBang]
                           where ([Bang_Time] >= " + begin.ToDatabase() + @"
                                 and [Bang_Time] <= " + end.ToDatabase() + @")
                                 and [jWeight] = 0";

                IList collections = this.Execute.GetEntityCollections(sql);
                EntityCollection results = collections[0] as EntityCollection;

                StringBuilder sqlBuild = new StringBuilder("select ");
                sqlBuild.Append(this.GetFields(this.Selects, "t_bang"));
                sqlBuild.Append(" from [t_bang]");
                sqlBuild.Append(" where [jWeight] > 0 and [bangid] in (0");
                int itemCount = results.Count;
                for (int i = 0; i < itemCount; i++) sqlBuild.AppendFormat(", {0}", results[i].GetValue("bangid"));
                sqlBuild.Append(")");

                EntityCollection oldDataCollection = this.GetEntityCollectionFromAccess(sqlBuild.ToString());
                if (oldDataCollection == null) return;

                int entityCount = oldDataCollection.Count;
                for (int i = 0; i < entityCount; i++) UpdateOldData(oldDataCollection[i]);
            }
            catch (Exception ex)
            {
                helper.WriteLine("UpdateDataForSecondWeigh:" + ex.Message);
            }
        }

        /// <summary>
        /// 删除木材收购系统里地磅表数据
        /// 返回数据库表受影响的行数，行数>0则为执行成功
        /// </summary>
        /// <param name="startDate">要删除的开始日期（首磅日期）</param>
        /// <returns>数据库表受影响的行数</returns>
        public int Delete(DateTime startDate)
        {
            string date = string.Format("{0}-{1}-{2} 00:00:00", startDate.Year, startDate.Month, startDate.Day);
            string sql = @"delete from [" + this.Table + @"]
                           where [Bang_Time] >= '" + date + "'";
            int act = 0;
            try
            {
                act = this.Execute.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                helper.WriteLine("删除木材收购系统里地磅表数据:" + ex.Message);
            }

            return act;
        }

        /// <summary>
        /// 把从地磅系统获取到的新数据添加到木材系统
        /// </summary>
        /// <param name="collections">新数据集合</param>
        void AddNewData(EntityCollection collections)
        {
            int act = 0;
            int itemCount = collections.Count;
            for (int i = 0; i < itemCount; i++)
            {
                act = this.InsertEntity(collections[i]);
            }
        }

        /// <summary>
        /// 获取木材系统里的地磅表里的最大记录号
        /// </summary>
        /// <returns>结果集</returns>
        EntityCollection GetTheMaxUnique()
        {
            string sql = @"select max([bangid]) as [bangid]" + @"
                           from [" + this.Table + "]";

            IList collections = base.Execute.GetEntityCollections(sql);

            EntityCollection results = collections[0] as EntityCollection;

            return results;
        }

        /// <summary>
        /// 同步车辆首磅的数据
        /// 说明：只取2013年开始的数据；每次只同步100条记录，防止长期占用Access数据库库文件而影响旧地磅系统的工作
        /// </summary>
        public void SynchronousDataFromBangToWood()
        {
            try
            {
                EntityCollection uniqueCollection = this.GetTheMaxUnique();
                int maxUnique = 0;
                if (uniqueCollection[0].GetValue("bangid") != null) maxUnique = uniqueCollection[0].GetValue("bangid").ToInt32();

                string sql = @"select top 100 " + this.GetFields(this.Selects, "t_bang") + @"
                           from [t_bang]
                           where [bangid] > " + maxUnique.ToDatabase() + @"
                               and year([Bang_Time]) > 2012
                           order by [bangid] asc";
                EntityCollection newDataCollection = this.GetEntityCollectionFromAccess(sql);
                if (newDataCollection == null) return;

                this.AddNewData(newDataCollection);
            }
            catch (Exception exception)
            {
                helper.WriteLine("同步车辆首磅的数据:" + exception.Message);
            }
        }

        #endregion


        #region  地磅系统

        /// <summary>
        /// 从地磅系统里获取地磅表的数据
        /// 使用返回的值之前先判断是否为null
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>结果集</returns>
        EntityCollection GetEntityCollectionFromAccess(string sql)
        {
            EntityCollection entityCollection = null;

            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;data source={0};", ConfigurationManager.AppSettings["dbConnectionString"]);
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    using (OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        Wodeyun.Gf.Entities.PropertyCollection propertyCollection = new Wodeyun.Gf.Entities.PropertyCollection();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            propertyCollection.Add(new SimpleProperty(reader.GetName(i), reader.GetFieldType(i)));
                        }

                        entityCollection = new EntityCollection(propertyCollection);
                        while (reader.Read())
                        {
                            Entity entity = new Entity(propertyCollection);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                entity.SetValue(i, reader.GetValue(i).ToVariable());
                            }

                            entityCollection.Add(entity);
                        }
                    }
                }
            }

            return entityCollection;
        }

        #endregion
    }
}
