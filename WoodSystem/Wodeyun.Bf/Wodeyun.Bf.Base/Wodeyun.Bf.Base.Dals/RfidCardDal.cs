using System.Data.SqlClient;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Base.Dals
{
    /// <summary>
    /// 电子卡使用状态数据处理类
    /// </summary>
    public class RfidCardDal : BaseDal
    {
        public RfidCardDal()
        {
        }

        public RfidCardDal(SqlTransaction transaction)
            : base(transaction)
        { }

        /// <summary>
        /// 电子卡使用状态变更
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">卡颜色类型标识</param>
        /// <param name="cardState">卡的当前使用状态标识</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateState(string cardNumber, int cardType, int cardState)
        {
            string sql = @"update [WoodCardState]
                           set [CardState] = " + cardState.ToDatabase() + @"
                           where [CardNumber] = " + cardNumber.ToDatabase() + @"
                               and [CardType] = " + cardType.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 电子卡使用状态变更
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">卡颜色类型标识</param>
        /// <param name="cardState">卡的当前使用状态标识</param>
        /// <param name="recordId">关联的记录编号</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateState(string cardNumber, int cardType, int cardState, int recordId)
        {
            string sql = @"update [WoodCardState]
                           set [CardState] = " + cardState.ToDatabase() + @",
                               [RecordId] = " + recordId.ToDatabase() + @"
                           where [CardNumber] = " + cardNumber.ToDatabase() + @"
                               and [CardType] = " + cardType.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 电子卡使用状态变更
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">卡颜色类型标识</param>
        /// <param name="cardState">卡的当前使用状态标识</param>
        /// <param name="recordId">关联的记录编号</param>
        /// <param name="comeFrom">原始发卡地标识</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateState(string cardNumber, int cardType, int cardState, int recordId, int comeFrom)
        {
            string sql = @"update [WoodCardState]
                           set [CardState] = " + cardState.ToDatabase() + @",
                               [RecordId] = " + recordId.ToDatabase() + @",
                               [ComeFrom] = " + comeFrom.ToDatabase() + @"
                           where [CardNumber] = " + cardNumber.ToDatabase() + @"
                               and [CardType] = " + cardType.ToDatabase();

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 电子卡使用状态记录添加
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">卡颜色类型标识</param>
        /// <param name="cardState">卡的当前使用状态标识</param>
        /// <param name="recordId">关联的记录编号</param>
        /// <param name="comeFrom">原始发卡地标识</param>
        /// <returns>数据库表受影响的行数</returns>
        public int Insert(string cardNumber, int cardType, int cardState, int recordId, int comeFrom)
        {
            string sql = @"insert into [WoodCardState] ([CardNumber], [CardType], [CardState], [RecordId], [ComeFrom])
                           values (" + cardNumber.ToDatabase() + @", 
                                   " + cardType.ToDatabase() + @",
                                   " + cardState.ToDatabase() + @", 
                                   " + recordId.ToDatabase() + @", 
                                   " + comeFrom.ToDatabase() + ")";

            return this.Execute.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 根据卡号和颜色类型获取电子卡信息
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">颜色类型</param>
        /// <returns></returns>
        public Entity GetCardStateEntity(string cardNumber, int cardType)
        {
            string sql = @"select [CardNumber], [CardType], [CardState], [RecordId], [ComeFrom]
                           from [WoodCardState]
                           where [CardNumber] = " + cardNumber.ToDatabase() + @"
                               and [CardType] = " + cardType.ToDatabase();

            return this.Execute.GetEntity(sql);
        }
    }
}
