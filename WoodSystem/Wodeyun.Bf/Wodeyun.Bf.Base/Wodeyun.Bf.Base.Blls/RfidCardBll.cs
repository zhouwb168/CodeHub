using System;

using System.Data.SqlClient;

using Wodeyun.Gf.Entities;
using Wodeyun.Bf.Base.Dals;
using Wodeyun.Bf.Base.Enums;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Bf.Base.Blls
{
    /// <summary>
    /// 电子卡使用状态业务逻辑类
    /// </summary>
    public class RfidCardBll
    {
        RfidCardDal dal;

        public RfidCardBll()
        {
            dal = new RfidCardDal();
        }

        public RfidCardBll(SqlTransaction transaction)
        {
            dal = new RfidCardDal(transaction);
        }

        /// <summary>
        /// 电子卡使用状态变更
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">卡颜色类型标识</param>
        /// <param name="cardState">卡的当前使用状态标识</param>
        /// <returns>数据库表受影响的行数</returns>
        public int UpdateState(string cardNumber, int cardType, int cardState)
        {
            return dal.UpdateState(cardNumber, cardType, cardState);
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
            return dal.UpdateState(cardNumber, cardType, cardState, recordId);
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
            return dal.UpdateState(cardNumber, cardType, cardState, recordId, comeFrom);
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
            return dal.Insert(cardNumber, cardType, cardState, recordId, comeFrom);
        }

        /// <summary>
        /// 根据卡号和颜色类型获取电子卡信息
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardType">颜色类型</param>
        /// <returns></returns>
        public Entity GetCardStateEntity(string cardNumber, int cardType)
        {
            Entity cardStateEntity = dal.GetCardStateEntity(cardNumber, cardType);
            if (cardStateEntity.GetValue("CardNumber") != null)
            {
                /* 把卡的当前使用状态标识，转换为用户能识别的汉字 */
                int cardState = cardStateEntity.GetValue("CardState").ToInt32();
                int comeFrom = cardStateEntity.GetValue("ComeFrom").ToInt32();
                string stateText = "";
                switch (cardState)
                {
                    case ((int)CardState.Balance):
                        {
                            stateText = cardType == (int)CardType.Green ? "已首磅登记，等待回皮" : "已首磅登记，等待取样";
                            break;
                        }
                    case ((int)CardState.Door):
                        {
                            stateText = comeFrom == (int)CardComeFrom.Factry ? "已在门口登记，等待首磅，该卡发自厂门口" : "已在门口验收登记，等待首磅，该卡发自移动服务站";
                            break;
                        }
                    case ((int)CardState.EmptyBalance):
                        {
                            stateText = "已回皮登记，等待离厂回收";
                            break;
                        }
                    case ((int)CardState.Sample):
                        {
                            stateText = "已在料厂取样登记，等待回皮";
                            break;
                        }
                    case ((int)CardState.Station):
                        {
                            stateText = "已在移动服务站登记，等待进厂验收";
                            break;
                        }
                    case ((int)CardState.UnUse):
                        {
                            stateText = "新卡，还未被使用";
                            break;
                        }
                    default:
                        {
                            stateText = "出错，请联系管理员";
                            break;
                        }
                }
                cardStateEntity.Add(new SimpleProperty("StateText", typeof(string)), stateText);
            }

            return cardStateEntity;
        }
    }
}
