using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TC.Tools
{
    public class BLL
    {
        /// <summary>
        /// 生成收益计划
        /// </summary>
        /// <param name="BorrowID">借款ID</param>
        /// <param name="BidRecordID">投标ID</param>
        /// <param name="BidAmount">投标金额</param>
        /// <param name="BorrowAmount">借款金额</param>
        public void InsertIncomePlan(int BorrowID, int BidRecordID, decimal BidAmount, decimal BorrowAmount)
        {
            decimal Percentage = BidAmount / BorrowAmount; //投标金额占借款金额的百分比
            DA.InsertIncomePlan(BorrowID, BidRecordID, Percentage);//放款时添加收款计划表
        }

        /// <summary>
        /// 获取收益列表
        /// </summary>
        /// <param name="BorrowID"></param>
        /// <returns></returns>
        public DataTable getInComeInfo(int BorrowID)
        {
            return DA.getInComeInfo(BorrowID);
        }
    }
}
