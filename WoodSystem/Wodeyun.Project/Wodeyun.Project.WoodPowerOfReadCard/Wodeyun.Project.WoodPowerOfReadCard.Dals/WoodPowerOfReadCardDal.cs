using System.Collections;
using System.Data.SqlClient;
using System;

using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.WoodPowerOfReadCard.Dals
{
    public class WoodPowerOfReadCardDal : CommonDal
    {
       private void Init()
        {
            this.Table = "WoodPowerOfReadCard";
            this.Inserts = "[Unique], [AccountID], [MachineID], [Operator], [State], [Version], [Log]";
            this.Updates = "[AccountID], [MachineID], [Operator]";
            this.Selects = "[Unique], [AccountID], [MachineID]";
        }

        public WoodPowerOfReadCardDal()
        {
            this.Init();
        }

        public WoodPowerOfReadCardDal(SqlTransaction transaction)
            : base(transaction)
        {
            this.Init();
        }

        /// <summary>
        /// 通过用户ID获取该用户有权限使用的读卡器
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns></returns>
        public Entity GetEntityByAccount(int accountID)
        {
            string sql = @"select [WoodPowerOfReadCard].[Unique], [WoodMachine].[Name], [WoodMachine].[MachineNumber]
                           from [WoodPowerOfReadCard]
                           inner join [WoodMachine] on ([WoodPowerOfReadCard].[MachineID] = [WoodMachine].[Unique]
                                   and [WoodMachine].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                                   and [WoodMachine].[State] <> " + StateEnum.Updated.ToDatabase() + @")
                           where [WoodPowerOfReadCard].[State] <> " + StateEnum.Deleted.ToDatabase() + @"
                           and [WoodPowerOfReadCard].[State] <> " + StateEnum.Updated.ToDatabase() + @"
                           and [WoodPowerOfReadCard].[AccountID] = " + accountID.ToDatabase();

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
