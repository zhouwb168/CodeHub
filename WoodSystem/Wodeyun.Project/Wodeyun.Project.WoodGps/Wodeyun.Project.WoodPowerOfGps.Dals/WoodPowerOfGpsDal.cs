using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodPowerOfGps.Dals
{
    public class WoodPowerOfGpsDal : CommonDal
    {
        private void Init()
        {
            this.Table = "WoodPowerOfGps";
            this.Inserts = "[Unique], [AccountID], [GpsID], [Operator], [State], [Version], [Log]";
            this.Updates = "[AccountID], [GpsID], [Operator]";
            this.Selects = "[Unique], [AccountID], [GpsID]";
        }

        public WoodPowerOfGpsDal()
        {
            this.Init();
        }

        public WoodPowerOfGpsDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 通过用户ID获取该用户有权限使用的检查站的GPS坐标和检查站名称
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns></returns>
        public Entity GetEntityByAccount(int accountID)
        {
            string sql = @"select [WoodPowerOfGps].[Unique], [WoodGps].[StationName], [WoodGps].[Lat], [WoodGps].[Lng]
                           from [WoodPowerOfGps]
                           inner join [WoodGps] on ([WoodPowerOfGps].[GpsID] = [WoodGps].[Unique]
                                   and [WoodGps].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodGps].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           where [WoodPowerOfGps].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [WoodPowerOfGps].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and [WoodPowerOfGps].[AccountID] = " + accountID.ToDatabase();

            return this.Execute.GetEntity(sql);
        }

        /// <summary>
        /// 修改数据通过用户ID
        /// 返回数据库受影响的行数
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns>数据库受影响的行数</returns>
        public int UpdateEntitiesByAccount(int accountID)
        {
            string sql = @"update " + this.Table + @"
                           set [State] = " + StateEnum.Updated.ToDatabase() + @"
                           where [AccountID] = " + accountID.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }
           

    }
}
